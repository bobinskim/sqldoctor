using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqldoc
{
    class Options
    {
        [Option('s', "script", HelpText = "Input file to be processed.")]
        public string InputFile { get; set; }

        [Option('d', "directory", HelpText = "Directory to search for scripts.")]
        public string InputDir { get; set; }

        [Option('t', "tables", Default = true, HelpText = "Documents tables.")]
        public bool Tables { get; set; }

        [Option('f', "filter", HelpText = "Filters for object names to be documented.")]
        public IEnumerable<string> Filters { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('o', "output", HelpText = "Asciidoc output file.")]
        public string Output { get; set; }


    }
}
