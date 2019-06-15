using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Schema
{
    public class TableInfo
    {
        public TableInfo(string name)
        {
            this.Columns = new Dictionary<string, ColumnInfo>();
            this.Name = name;
            this.Description = string.Empty;
        }

        public Dictionary<string, ColumnInfo> Columns { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
