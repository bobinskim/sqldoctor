using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class Options
    {
        [Option("-d|--directory", "Directory path to search for scripts.", CommandOptionType.SingleValue)]
        public string InputDir { get; set; }

        [Option("t|--no-tables", "Documents tables.", CommandOptionType.SingleOrNoValue)]
        public bool NoTables { get; set; }

        [Option("f|--filter", "Pattern to filter file names.", CommandOptionType.SingleOrNoValue)]
        public string Filter { get; set; }

        [Option("o|--output", "Asciidoc output file.", CommandOptionType.SingleOrNoValue)]
        public string Output { get; set; }

        [Option("s|--schema", "Default DB schema name.", CommandOptionType.SingleOrNoValue)]
        public string Schema { get; set; }
    }
}
