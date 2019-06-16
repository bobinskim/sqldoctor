using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace SqlDoctor.Generator
{
    public class OutputWriter : IOutputWriter
    {
        private IFileWrap file;
        private ILogger logger;

        public OutputWriter(IFileWrap fw, ILogger logger)
        {
            this.file = fw;
            this.logger = logger;
        }

        public void WriteOutput(string output, Options options)
        {
            this.file.WriteAllText(options.Output, output);
        }
    }
}
