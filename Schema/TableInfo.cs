using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Schema
{
    public class TableInfo
    {
        public TableInfo()
        {
            this.Columns = new Dictionary<string, ColumnInfo>();
        }

        public Dictionary<string, ColumnInfo> Columns { get; private set; }
    }
}
