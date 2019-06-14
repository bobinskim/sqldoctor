using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Schema
{
    public class ColumnInfo
    {
        public string ColumnName { get; private set; }
        public SqlDbType DataType { get; private set; }
        public string Size { get; private set; }
        public bool Nullable { get; private set; }
        public bool Key { get; private set; }
        public string Description { get; private set; }
    }
}
