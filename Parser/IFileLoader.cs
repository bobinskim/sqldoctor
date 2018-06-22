using System.Collections.Generic;

namespace SqlDoctor.Parser
{
    public interface IFileLoader
    {
        IEnumerable<string> LoadFiles(string path, string filter, bool recursive);
    }
}