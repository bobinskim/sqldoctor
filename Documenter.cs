using NLog;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class Documenter
    {
        private readonly IFileLoader loader;
        private readonly ISourceCodeParser parser;
        private readonly IDocGenerator generator;
        private readonly IOutputWriter writer;
        private readonly ILogger logger;

        public Documenter(IFileLoader fl, ISourceCodeParser dp, IDocGenerator gen, IOutputWriter ow, ILogger logger)
        {
            this.loader = fl;
            this.parser = dp;
            this.generator = gen;
            this.writer = ow;
            this.OutputDocs = null;
            this.logger = logger ;
        }

        public string OutputDocs { get; private set; }

        public void MakeDocs(Options options)
        {
            logger.Debug("Generating SQL schema documentation for files in '{0}' filtered by {1}", options.InputDir, options.Filter);
            IEnumerable<string> input_files = loader.LoadFiles(options.InputDir, options.Filter, true);
            
            SchemaInfo schema = this.parser.Parse(input_files);
            this.OutputDocs = this.generator.Generate(schema);
            writer.WriteOutput(this.OutputDocs, options);
        }
    }
}
