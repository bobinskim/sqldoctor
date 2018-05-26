using Autofac;
using Autofac.Core;
using CommandLine;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor
{
    class Program
    {
        private static Logger logger;

        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            try
            {
                SetupLogging();

                var builder = new ContainerBuilder();
                //builder.RegisterType<DocCommandParser>().As<IDocArgsParser>();
                builder.RegisterType<CommandParser>();
                builder.RegisterType<Documenter>();
                Container = builder.Build();

                ParseAndRun(args);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ParseAndRun(string[] args)
        {
            try
            {
                var documenter = Container.Resolve<Documenter>();
                var commParser = Container.Resolve<CommandParser>();
                Options options = commParser.Parse(args);

                documenter.MakeDocs(options);
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Unexpected error occurred");
            }
        }

        private static void SetupLogging()
        {
            var config = new LoggingConfiguration();

            var logfile = new FileTarget() { FileName = "sqldoc.log", Name = "logfile" };
            var logconsole = new ConsoleTarget() { Name = "logconsole" };

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, logconsole));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, logfile));

            LogManager.Configuration = config;

            logger = LogManager.GetCurrentClassLogger();
        }
    }
}
