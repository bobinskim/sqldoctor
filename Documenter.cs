using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class Documenter
    {
        IFileLoader loader;
        IDDLParser parser;
        IDocGenerator generator;
        IOutputWriter writer;

        public Documenter(IFileLoader fl, IDDLParser dp, IDocGenerator gen, IOutputWriter ow)
        {
            this.loader = fl;
            this.parser = dp;
            this.generator = gen;
            this.writer = ow;
            this.OutputDocs = null;
        }

        public string OutputDocs { get; private set; }

        public void MakeDocs(Options options)
        {
            IEnumerable<string> input_files = loader.LoadFiles(options.InputDir, options.Filter, true);
            SchemaInfo schema = this.parser.Parse(input_files);
            this.OutputDocs = this.generator.Generate(schema);
            writer.WriteOutput(this.OutputDocs, options);
        }
    }
}
