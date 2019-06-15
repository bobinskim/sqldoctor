using System.Collections.Generic;

namespace SqlDoctor.Schema
{
    public class SchemaInfo
    {
        public SchemaInfo()
        {
            this.Tables = new List<TableInfo>();
        }

        public ICollection<TableInfo> Tables { get; private set; }
    }
}