using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Generator
{
    partial class AsciidocTarget
    {
        public SchemaInfo Schema { get; set; }
        public Options Options { get; set; }

        private string Tick { get => this.Options.Icons ? "icon:key[]" : "✓"; }

        private string HeadLevel(int n)
        {
            return new string('=', n);
        }
    }
}
