using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace SqlDoctor.Parser
{
    public class FileLoader : IFileLoader
    {
        readonly IFileSystem filesystem;
        readonly ILogger<FileLoader> logger;

        public FileLoader(IFileSystem filesystem, ILogger<FileLoader> log)
        {
            this.filesystem = filesystem;
            this.logger = log;
        }

        public IEnumerable<string> LoadFiles(string path, string filter, bool recursive)
        {
          
            string[] filePaths = this.filesystem.Directory.GetFiles(path ?? "./", filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            this.logger.LogDebug("files loaded: " + string.Join(" ; ", filePaths));

            return filePaths.Select(f => this.filesystem.File.ReadAllText(f));
        }
    }
}
