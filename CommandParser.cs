using CommandLine;
using NLog;
using System;
using System.Collections.Generic;

namespace SqlDoctor
{
    internal class CommandParser
    {
        private static Logger logger;

        public Options Parse(string[] args)
        {
            Options ret = null;

            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(opts => ret = opts)
                .WithNotParsed((errs) => HandleParseError(errs));
            return ret;
        }

        private void HandleParseError(IEnumerable<Error> errs)
        {
            foreach (Error err in errs)
            {
                if (err.StopsProcessing)
                {
                    logger.Error("Critical parsing error occurred", err);
                }
                else
                {
                    logger.Info("Parsing error occurred", err);
                }
            }
        }
    }
}