using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using System;
using System.IO.Abstractions;
using System.Linq;

namespace SqlDoctor
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDocGenerator, DocGenerator>()
                .AddSingleton<IOutputWriter, OutputWriter>()
                .AddSingleton<IDocGenerator, DocGenerator>()
                .AddSingleton<IOutputWriter, OutputWriter>()
                .AddSingleton<IFileLoader, FileLoader>()
                .AddSingleton<ISourceCodeParser, SourceCodeParser>()
                .AddSingleton<IFileSystem, FileSystem>()
                .AddTransient<SchemaVisitorBase, SchemaVisitor>()
                .AddSingleton<Func<SchemaVisitorBase>>( (c) => () => c.GetService<SchemaVisitorBase>())
                .AddLogging(loggerBuilder =>
                {
                    if (args.Length > 0)
                    {
                        if (args.Contains("--verbose") || args.Contains("-v") || args.Contains("--console") || args.Contains("-c"))
                        {
                            loggerBuilder.AddConsole().SetMinimumLevel(LogLevel.Debug);
                        }
                        else
                        {
                            loggerBuilder.AddConsole().SetMinimumLevel(LogLevel.Information);
                        }
                    }
                    else
                    {
                        loggerBuilder.AddConsole().SetMinimumLevel(LogLevel.Information);
                    }
                })
                .BuildServiceProvider();

            var app = new CommandLineApplication<Documenter>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger("main");

            if (args.Contains("--console") || args.Contains("-c"))
            {
                string input = Console.ReadLine();
                while (input != string.Empty && input != "exit")
                {
                    var cargs = input.SplitArgs().ToArray();
                    Run(app, logger, cargs);
                    input = Console.ReadLine();
                }
            }
            else
            {
                return Run(app, logger, args);
            }

            return 0;
        }

        private static int Run(CommandLineApplication<Documenter> app, ILogger logger, string[] cargs)
        {
            try
            {
                return app.Execute(cargs);
            }
            catch (Exception ex)
            {
                if (cargs.Contains("--verbose") || cargs.Contains("-v"))
                {
                    logger.LogError(ex, "Command line error");
                }
                else
                {
                    logger.LogError($"Command line error: {ex.Message}");
                }

                return 1;
            }
        }
    }
}
