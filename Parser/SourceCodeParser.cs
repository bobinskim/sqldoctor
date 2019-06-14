using NLog;
using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;

namespace SqlDoctor.Parser
{
    public class SourceCodeParser : ISourceCodeParser
    {
        private readonly ILogger logger;
        private readonly TSql150Parser parser = new TSql150Parser(false);

        public SourceCodeParser(ILogger logger)
        {
            this.logger = logger;
        }

        public SchemaInfo Parse(IEnumerable<string> input_files)
        {
            SchemaInfo ret = new SchemaInfo();

            foreach(string input in input_files)
            {
                IList<ParseError> errors;
                var fragment = this.parser.Parse(new StringReader(input), out errors);

                foreach(var err in errors)
                {
                    logger.Warn("[Parse error {0} on line {1}] {2}", err.Number, err.Line, err.Message);
                }

                //TODO: visitor
            }

            return ret;
        }
    }
}
