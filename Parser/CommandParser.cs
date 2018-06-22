using CommandLine;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlDoctor.Parser
{
    public class CommandParser
    {
        private readonly ILogger logger;

        public CommandParser(ILogger logger)
        {
            this.logger = logger;
        }

        public Options Parse(string[] args)
        {
            Options ret = null;

            try
            {
                this.logger.Debug("Execution arguments: {0}", string.Join(" ", args));
                CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(opts => ret = opts)
                    .WithNotParsed((errs) => HandleParseError(errs, args));
            }
            catch(Exception ex)
            {
                this.logger.Error(ex, "Unexpected arguments parsing error");
                throw;
            }

            return ret;
        }

        private void HandleParseError(IEnumerable<Error> errs, string[] args)
        {
            foreach (Error err in errs)
            {
                logger.Error("Error {0} occurred while parsing arguments: {1}", err, string.Join(" ", args));
            }

            throw new ArgumentOutOfRangeException("args", "Command line parsing arguments errors");
        }
    }
}