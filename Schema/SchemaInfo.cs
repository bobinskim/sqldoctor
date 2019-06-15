using System.Collections.Generic;

namespace SqlDoctor.Schema
{
    public class SchemaInfo
    {
        public SchemaInfo()
        {
            this.Tables = new Dictionary<string, TableInfo>();
        }

        public IDictionary<string, TableInfo> Tables { get; private set; }
    }
}