using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using SqlDoctor.Schema;

namespace SqlDoctor.Generator
{
    public class DocGenerator : IDocGenerator
    {
        private readonly ILogger logger;

        public DocGenerator(ILogger logger)
        {
            this.logger = logger;
        }

        public string Generate(SchemaInfo schema)
        {
            this.logger.Debug("Generating asciidoc content ...");

            AsciidocTarget target = new AsciidocTarget();
            target.Schema = schema;
            String res = target.TransformText();
            return res;
        }
    }
}
