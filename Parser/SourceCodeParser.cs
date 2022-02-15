using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SqlDoctor.Parser
{
    public class SourceCodeParser : ISourceCodeParser
    {
        private readonly ILogger<SourceCodeParser> logger;
        private readonly TSql150Parser parser = new TSql150Parser(false);
        private readonly Func<SchemaVisitorBase> visitorFactory;
        private readonly Documenter options;

        public SourceCodeParser(ILogger<SourceCodeParser> logger, Func<SchemaVisitorBase> visitorFac, Documenter options)
        {
            this.logger = logger;
            this.visitorFactory = visitorFac;
            this.options = options;
        }

        public SchemaInfo Parse(IEnumerable<string> inputFiles, Options options)
        {
            SchemaInfo ret = new SchemaInfo();

            foreach(string input in inputFiles)
            {
                this.Append(ret, this.Parse(input, options));
            }

            return ret;
        }

        public SchemaInfo Parse(string input, Options options)
        {
            var fragment = this.parser.Parse(new StringReader(input), out IList<ParseError> errors);

            foreach (var err in errors)
            {
                logger.LogWarning("[Parse error {0} on line {1}] {2}", err.Number, err.Line, err.Message);
            }

            var visitor = this.visitorFactory();
            visitor.Options = options;
            fragment.Accept(visitor);

            return visitor.Schema;
        }

        private void Append(SchemaInfo target, SchemaInfo src)
        {
            foreach(var tab in src.Tables)
            {
                if (target.Tables.ContainsKey(tab.Key))
                {
                    this.logger.LogWarning("Duplicated table identifier {0}", tab.Key);
                }
                else
                {
                    target.Tables.Add(tab);
                }
            }
        }
    }
}
