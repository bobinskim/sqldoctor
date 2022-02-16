using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class Options
    {
        [Option("-d|--directory <PATH>", "Directory path to search for scripts.", CommandOptionType.SingleValue)]
        public string InputDir { get; set; } = ".";

        [Option("-n|--no <OBJ_TYPE>", "Object types to skip (tables, views, sprocs).", CommandOptionType.MultipleValue)]
        public string[] Skip { get; set; } = new string[] { };

        [Option("-f|--filter <PATTERN>", "Pattern to filter file names.", CommandOptionType.SingleValue)]
        public string Filter { get; set; } = "*.sql";

        [Option("-o|--output <FILENAME>", "Asciidoc output file.", CommandOptionType.SingleValue)]
        public string Output { get; set; } = "schema.adoc";

        [Option("-s|--schema <SCHEMA>", "Default DB schema name.", CommandOptionType.SingleValue)]
        public string Schema { get; set; } = "dbo";

        [Option("-t|--title <TITLE>", "Title of the generated adoc section", CommandOptionType.SingleValue)]
        public string Title { get; set; } = string.Empty;

        [Option("-l|--level <LEVEL>", "Topmost section heading level", CommandOptionType.SingleValue)]
        [Range(0, 6)]
        public int Level { get; set; } = 1;

        [Option("-i|--icons", "Use 'icon:check[]' instead of '✓' character", CommandOptionType.NoValue)]
        public bool Icons { get; set; }

        [Option("-v|--verbose", "Log debug information to the console", CommandOptionType.NoValue)]
        public bool Verbose { get; set; }

        [Option("-c|--console", "Run in interactive console mode", CommandOptionType.NoValue)]
        public bool Console { get; set; }
    }
}
