using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SqlDoctor.Schema;

namespace SqlDoctor.Generator
{
    public class DocGenerator : IDocGenerator
    {
        private readonly ILogger<DocGenerator> logger;

        public DocGenerator(ILogger<DocGenerator> logger)
        {
            this.logger = logger;
        }

        public string Generate(SchemaInfo schema)
        {
            this.logger.LogDebug("Generating asciidoc content ...");

            AsciidocTarget target = new AsciidocTarget();
            target.Schema = schema;
            String res = target.TransformText();
            return res;
        }
    }
}
