using System.Collections.Generic;

namespace SqlDoctor.Parser
{
    public interface IDDLParser
    {
        SchemaInfo Parse(IEnumerable<string> input_files);
    }
}