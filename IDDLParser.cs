using System.Collections.Generic;

namespace SqlDoctor
{
    public interface IDDLParser
    {
        SchemaInfo Parse(IEnumerable<string> input_files);
    }
}