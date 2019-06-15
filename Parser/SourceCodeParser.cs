﻿using NLog;
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
        private readonly Func<SchemaVisitorBase> visitorFactory;

        public SourceCodeParser(ILogger logger, Func<SchemaVisitorBase> visitorFac)
        {
            this.logger = logger;
            this.visitorFactory = visitorFac;
        }

        public SchemaInfo Parse(IEnumerable<string> inputFiles)
        {
            SchemaInfo ret = new SchemaInfo();

            foreach(string input in inputFiles)
            {
                this.Append(ret, this.Parse(input));
            }

            return ret;
        }

        public SchemaInfo Parse(string input)
        {
            var fragment = this.parser.Parse(new StringReader(input), out IList<ParseError> errors);

            foreach (var err in errors)
            {
                logger.Warn("[Parse error {0} on line {1}] {2}", err.Number, err.Line, err.Message);
            }

            var visitor = this.visitorFactory();
            fragment.Accept(visitor);

            return visitor.Schema;
        }

        private void Append(SchemaInfo target, SchemaInfo src)
        {
            foreach(var tab in src.Tables)
            {
                if (target.Tables.ContainsKey(tab.Key))
                {
                    throw new ParserException("Duplicated table identifier");
                }

                target.Tables.Add(tab);
            }
        }
    }
}
