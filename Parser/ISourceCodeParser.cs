﻿using SqlDoctor.Schema;
using System.Collections.Generic;

namespace SqlDoctor.Parser
{
    public interface ISourceCodeParser
    {
        SchemaInfo Parse(IEnumerable<string> input_files);
    }
}