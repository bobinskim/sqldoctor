using Microsoft.Extensions.Logging;
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
    public class Documenter : Options
    {
        private readonly IFileLoader loader;
        private readonly ISourceCodeParser parser;
        private readonly IDocGenerator generator;
        private readonly IOutputWriter writer;
        private readonly ILogger<Documenter> logger;

        public Documenter(IFileLoader fl, ISourceCodeParser dp, IDocGenerator gen, IOutputWriter ow, ILogger<Documenter> logger)
        {
            this.loader = fl;
            this.parser = dp;
            this.generator = gen;
            this.writer = ow;
            this.OutputDocs = null;
            this.logger = logger ;
        }

        public string OutputDocs { get; private set; }
        public int OnExecute()
        {
            try
            {
                logger.LogDebug("Generating SQL schema documentation for files in '{0}' filtered by {1}", this.InputDir, this.Filter);
                IEnumerable<string> input_files = loader.LoadFiles(this.InputDir, this.Filter, true);

                SchemaInfo schema = this.parser.Parse(input_files, this);
                this.OutputDocs = this.generator.Generate(schema, this);
                writer.WriteOutput(this.OutputDocs, this);
                return 0;
            }
            catch(Exception ex)
            {
                if (this.Verbose)
                {
                    this.logger.LogError(ex, "Docs generation error");
                }
                else
                {
                    this.logger.LogError($"Docs generation error: {ex.Message}");
                }

                return 1;
            }
        }
    }
}
