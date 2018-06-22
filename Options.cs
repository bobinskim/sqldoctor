using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class Options
    {
        [Option('d', "directory", Required = true, HelpText = "Directory path to search for scripts.")]
        public string InputDir { get; set; }

        [Option('t', "tables", Default = true, HelpText = "Documents tables.")]
        public bool Tables { get; set; }

        [Option('f', "filter", Default = @"*.sql", HelpText = "Pattern to filter file names.")]
        public string Filter { get; set; }

        [Option('o', "output", Default = "schema.adoc", HelpText = "Asciidoc output file.")]
        public string Output { get; set; }


    }
}
