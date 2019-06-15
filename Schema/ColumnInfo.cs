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
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string Size { get; set; }
        public bool Nullable { get; set; }
        public bool Key { get; set; }
        public bool Identity { get; set; }
        public string Description { get; set; }
    }
}
