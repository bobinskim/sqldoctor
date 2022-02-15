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

        [Option("-n|--no", "Object types to skip (tables, views, sprocs).", CommandOptionType.MultipleValue)]
        public string[] Skip { get; set; } = new string[] { };

        [Option("-f|--filter", "Pattern to filter file names.", CommandOptionType.SingleValue)]
        public string Filter { get; set; } = "*.sql";

        [Option("-o|--output", "Asciidoc output file.", CommandOptionType.SingleValue)]
        public string Output { get; set; } = "schema.adoc";

        [Option("-s|--schema", "Default DB schema name.", CommandOptionType.SingleValue)]
        public string Schema { get; set; } = "dbo";

        [Option("-t|--title", "Title of the generated adoc section", CommandOptionType.SingleValue)]
        public string Title { get; set; } = string.Empty;

        [Option("-i|--icons", "Use 'icon:check[]' instead of '✓' character", CommandOptionType.NoValue)]
        public bool Icons { get; set; }
    }
}
