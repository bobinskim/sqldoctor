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
    }
}
