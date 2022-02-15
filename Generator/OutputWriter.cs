using Microsoft.Extensions.Logging;
using System.IO.Abstractions;

namespace SqlDoctor.Generator
{
    public class OutputWriter : IOutputWriter
    {
        private IFileSystem fileSystem;
        private ILogger<OutputWriter> logger;

        public OutputWriter(IFileSystem fw, ILogger<OutputWriter> logger)
        {
            this.fileSystem = fw;
            this.logger = logger;
        }

        public void WriteOutput(string output, Options options)
        {
            this.fileSystem.File.WriteAllText(options.Output ?? "schema.adoc", output);
        }
    }
}
