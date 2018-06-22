﻿using Autofac;
using Autofac.Core;
using CommandLine;
using NLog;
using NLog.Config;
using NLog.Targets;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace SqlDoctor
{
    static class Program
    {
        private static Logger logger = null;

        public static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            try
            {
                SetupLogging();

                try
                {
                    var builder = new ContainerBuilder();
                    builder.Register<ILogger>(c => logger);
                    builder.RegisterType<CommandParser>();
                    builder.RegisterType<Documenter>();
                    builder.RegisterType<FileLoader>().AsImplementedInterfaces();
                    builder.RegisterType<DDLParser>().AsImplementedInterfaces();
                    builder.RegisterType<DocGenerator>().AsImplementedInterfaces();
                    builder.RegisterType<OutputWriter>().AsImplementedInterfaces();
                    builder.RegisterType<DirectoryWrap>().AsImplementedInterfaces();
                    builder.RegisterType<FileWrap>().AsImplementedInterfaces();
                    Container = builder.Build();
                }
                catch(Exception ex)
                {
                    logger.Error(ex, "Application initialization error");
                    throw;
                }

                if (args.Length > 0)
                {
                    ParseAndRun(args);
                }
                else
                {
                    string input = Console.ReadLine();
                    while (input != "" && input != "exit")
                    {
                        ParseAndRun(input.Split(' '));
                        input = Console.ReadLine();
                    }
                }
            }
            catch(NLogConfigurationException ex)
            {
                Console.WriteLine("[ERROR] Logger initialization failed: {0}", ex.Message);
            }
            catch(Exception ex)
            {
                if (logger != null)
                {
                    logger.Error(ex, "Unexpected error occurred");
                }
                else
                {
                    Console.WriteLine("[ERROR]Unexpected error occurred: {0}", ex.Message);
                }
            }
        }

        private static void ParseAndRun(string[] args)
        {
            try
            {
                var commParser = Container.Resolve<CommandParser>();
                Options options = commParser.Parse(args);

                var documenter = Container.Resolve<Documenter>();
                documenter.MakeDocs(options);
            }
            catch (DependencyResolutionException ex)
            {
                logger.Error(ex, "Application initialization error");
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Documentation generation failure.");
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
