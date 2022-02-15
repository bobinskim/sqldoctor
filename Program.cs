using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using System;
using System.IO.Abstractions;

namespace SqlDoctor
{
    class Program : Options
    {
        public int Main(string[] args)
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
                .AddSingleton<SchemaVisitorBase, SchemaVisitor>()
                .AddLogging(loggerBuilder =>
                {
                    loggerBuilder.AddConsole()
                        .SetMinimumLevel(LogLevel.Information);
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
                    app.Execute(input.Split(' '));
                    input = Console.ReadLine();
                }
            }

            return 0;
        }
    }
}
