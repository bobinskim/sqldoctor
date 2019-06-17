using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using NLog;
using SqlDoctor.Schema;

namespace SqlDoctor.Parser
{
    public class SchemaVisitor : SchemaVisitorBase
    {
        private readonly ILogger logger;

        public SchemaVisitor(ILogger logger) : base()
        {
            this.logger = logger;
        }

        public override void Visit(CreateTableStatement node)
        {
            var tableName = string.Join(".", node.SchemaObjectName.Identifiers.Select(i => string.Format("[{0}]", i.Value)));

            if (!tableName.Contains(".") && this.Options != null)
            {
                tableName = string.Format("[{0}].{1}", this.Options.Schema, tableName);
            }

            this.logger.Debug("Parsing CREATE TABLE statement for table {0}", tableName);

            var table = new TableInfo(tableName);
            
            foreach(var cd in node.Definition.ColumnDefinitions)
            {
                var ci = new ColumnInfo();
                ci.Name = cd.ColumnIdentifier.Value;
                ci.DataType = cd.DataType.Name.BaseIdentifier.Value;

                if (cd.DataType is ParameterizedDataTypeReference dt)
                {
                    ci.Size = string.Join(", ", dt.Parameters.Select(l => l.Value));
                }
                else
                {
                    ci.Size = string.Empty;
                }

                ci.Identity = (cd.IdentityOptions != null);

                ci.Nullable = false;
                ci.Unique = false;
                ci.PrimaryKey = false;

                foreach (var con in cd.Constraints)
                {
                    switch(con)
                    {
                        case NullableConstraintDefinition n:
                            ci.Nullable = n.Nullable;
                            break;
                        case UniqueConstraintDefinition u:
                            ci.Unique = true;
                            ci.PrimaryKey = u.IsPrimaryKey;
                            break;
                    }
                }

                table.Columns.Add(ci.Name, ci);
            }

            this.Schema.Tables.Add(tableName, table);
        }

        public override void Visit(ExecuteSpecification node)
        {
            var proc = node.ExecutableEntity as ExecutableProcedureReference;

            if (proc == null)
            {
                base.Visit(node);
                return;
            }

            var ids = proc.ProcedureReference.ProcedureReference.Name.Identifiers;
            var par = proc.Parameters;

            if ((ids.Count == 1 && ids[0].Value == "sp_addextendedproperty") || (ids.Count == 2 && ids[0].Value == "sys" && ids[1].Value == "sp_addextendedproperty"))
            {
                var parDic = new Dictionary<string, string>();

                foreach (var p in par)
                {
                    var v = p.ParameterValue as StringLiteral;

                    if (v != null)
                    {
                        parDic[p.Variable.Name.Replace("@", "")] = v.Value;
                    }
                }

                if (!new string[] { "level0type", "level0name", "level1type", "level1name", "level2type", "level2name", "name", "value" }.Except(parDic.Keys).Any())
                {
                    if (parDic["name"].ToUpper() == "MS_DESCRIPTION" && parDic["level0type"].ToUpper() == "SCHEMA" && parDic["level1type"].ToUpper() == "TABLE" && parDic["level2type"].ToUpper() == "COLUMN")
                    {
                        this.AddColumnDescription(parDic["level0name"], parDic["level1name"], parDic["level2name"], parDic["value"]);
                    }
                    else
                    {
                        this.logger.Warn("Invalid level 2 EXEC sp_addextendedproperty: {0}", node);
                    }
                }
                else if (!new string[] { "level0type", "level0name", "level1type", "level1name", "name", "value" }.Except(parDic.Keys).Any())
                {
                    if (parDic["name"].ToUpper() == "MS_DESCRIPTION" && parDic["level0type"].ToUpper() == "SCHEMA" && parDic["level1type"].ToUpper() == "TABLE" )
                    {
                        this.AddTableDescription(parDic["level0name"], parDic["level1name"], parDic["value"]);
                    }
                    else
                    {
                        this.logger.Warn("Invalid level 1 EXEC sp_addextendedproperty: {0}", node);
                    }
                }
            }
        }

        private void AddColumnDescription(string schema, string table, string column, string description)
        {
            var tableName = string.Format("[{0}].[{1}]", schema, table);
            if (this.Schema.Tables.ContainsKey(tableName))
            {
                if (this.Schema.Tables[tableName].Columns.ContainsKey(column))
                {
                    this.Schema.Tables[tableName].Columns[column].Description = description;
                }
                else
                {
                    this.logger.Warn("Attempt to add description for non existent column {0} of table {1}", column, tableName);
                }
            }
            else
            {
                this.logger.Warn("Attempt to add description for column of table {0}, which does not exist", tableName);
            }
        }

        private void AddTableDescription(string schema, string table, string description)
        {
            var tableName = string.Format("[{0}].[{1}]", schema, table);
            if (this.Schema.Tables.ContainsKey(tableName))
            {
                this.Schema.Tables[tableName].Description = description;
            }
            else
            {
                this.logger.Warn("Attempt to add description for table {0}, which does not exist", tableName);
            }
        }
    }
}
