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
            // setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging()
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
                        loggerBuilder.AddConsole()
                            .SetMinimumLevel(LogLevel.Information);
                    }
                    else
                    {
                        loggerBuilder.AddConsole()
                            .SetMinimumLevel(LogLevel.Debug);
                    }
                })
                .BuildServiceProvider();

            var app = new CommandLineApplication<Documenter>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);

            if (args.Length > 0)
            {
                return app.Execute(args);
            }
            else
            {
                string input = Console.ReadLine();
                while (input != "" && input != "exit")
                {
                    app.Execute(input.SplitArgs().ToArray());
                    input = Console.ReadLine();
                }
            }

            return 0;
        }
    }
}
