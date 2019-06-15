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
            //string tableName = string.Format(
            //    "[{0}].[{1}]",
            //    node.SchemaObjectName.Identifiers[0].Value,
            //    node.SchemaObjectName.Identifiers[1].Value
            //    );

            //node.SchemaObjectName.Identifiers.Aggregate(
            //    string.Empty,
            //    func: (s, i) => string.IsNullOrEmpty(s) ? string.Format("[{0}]", i.Value) : string.Format("{0}.[{1}]", s, i.Value)
            //);

            var tableName = string.Join(".", node.SchemaObjectName.Identifiers.Select(i => string.Format("[{0}]", i.Value)));

            this.logger.Debug("Parsing CREATE TABLE statement for table {0}", tableName);

            var table = new TableInfo(tableName);
            
            foreach(var cd in node.Definition.ColumnDefinitions)
            {
                var ci = new ColumnInfo();
                ci.ColumnName = cd.ColumnIdentifier.Value;
                ci.DataType = cd.DataType.Name.BaseIdentifier.Value;
            }

            this.Schema.Tables.Add(tableName, table);
        }
    }
}
