using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    public class FileLoader : IFileLoader
    {
        public IEnumerable<string> LoadFiles(string path, string filter, bool recursive)
        {
            return new List<string>();
        }
    }
}
