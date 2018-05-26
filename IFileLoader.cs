using System.Collections.Generic;

namespace SqlDoctor
{
    public interface IFileLoader
    {
        IEnumerable<string> LoadFiles(string path, string filter, bool recursive);
    }
}