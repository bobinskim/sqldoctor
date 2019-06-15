using Autofac;
using Moq;
using NLog;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SqlDoctor.Tests.IntegrationTests
{
    public class GenerateDocsTests
    {
        private Documenter documenter;

        public GenerateDocsTests()
        {
            var builder = new ContainerBuilder();
            var logger = new Mock<ILogger>();

            // main facade object
            builder.RegisterType<Documenter>();

            // common logger
            builder.Register(c => logger.Object);

            // parser module
            builder.RegisterModule<ParserModule>();

            // generator
            builder.RegisterType<DocGenerator>().As<IDocGenerator>();
            builder.RegisterType<OutputWriter>().As<IOutputWriter>();
            var container = builder.Build();

            this.documenter = container.Resolve<Documenter>();
        }

    //    [Fact]
    //    public void MakeDocs_EmptyInput_EmptyOutput()
    //    {
    //        Options opts = new Options();
    //        opts.InputDir = ".";
    //        opts.Filter = "*.empty";
    //        this.documenter.MakeDocs(opts);
    //        // TODO
    //    }
    }
}
