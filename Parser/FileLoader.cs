using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace SqlDoctor.Parser
{
    public class FileLoader : IFileLoader
    {
        readonly IDirectoryWrap directory;
        readonly IFileWrap file;
        readonly ILogger logger;

        public FileLoader(IDirectoryWrap dirWrap, IFileWrap fileWrap, ILogger log)
        {
            this.directory = dirWrap;
            this.file = fileWrap;
            this.logger = log;
        }

        public IEnumerable<string> LoadFiles(string path, string filter, bool recursive)
        {
          
            string[] filePaths = this.directory.GetFiles(path, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            this.logger.Debug("files loaded: " + string.Join(" ; ", filePaths));

            return filePaths.Select(f => this.file.ReadAllText(f));
        }
    }
}
