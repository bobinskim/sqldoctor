using Microsoft.SqlServer.TransactSql.ScriptDom;
using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Parser
{
    abstract public class SchemaVisitorBase : TSqlConcreteFragmentVisitor
    {
        public SchemaInfo Schema { get; set; }

        protected SchemaVisitorBase()
        {
            this.Schema = new SchemaInfo();
        }
    }
}
