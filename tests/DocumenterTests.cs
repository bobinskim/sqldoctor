using Moq;
using SqlDoctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class DocumenterTests
    {
        private readonly Documenter documenter;

        public DocumenterTests()
        {
            Mock<IFileLoader> loader = new Mock<IFileLoader>();
            loader.Setup(fl => fl.LoadFiles("existing_path", "match", true)).Returns(new string[] { "DDL_1", "DDL_2" });
            loader.Setup(fl => fl.LoadFiles("existing_path", "no_match", true)).Returns(new string[] { });
            loader.Setup(fl => fl.LoadFiles("not_existing_path", It.IsAny<string>(), true)).Throws<InvalidOperationException>();
            loader.Setup(fl => fl.LoadFiles(It.IsAny<string>(), It.IsAny<string>(), false)).Throws<NotImplementedException>();

            SchemaInfo schema = new SchemaInfo();
            Mock<IDDLParser> parser = new Mock<IDDLParser>();
            parser.Setup(p => p.Parse(It.Is<IEnumerable<string>>(input => input.Count() > 0))).Returns(schema);
            parser.Setup(p => p.Parse(It.Is<IEnumerable<string>>(input => input.Count() == 0))).Throws<ArgumentException>();

            Mock<IDocGenerator> gen = new Mock<IDocGenerator>();
            gen.Setup(g => g.Generate(schema)).Returns("=asciidoc output");

            Mock<IOutputWriter> writer = new Mock<IOutputWriter>();
            writer.Setup(w => w.WriteOutput(It.IsAny<string>(), It.Is<Options>(o => o.Output == "correct_output")));
            writer.Setup(w => w.WriteOutput(It.IsAny<string>(), It.Is<Options>(o => o.Output == "incorrect_output"))).Throws<SystemException>();

            this.documenter = new Documenter(loader.Object, parser.Object, gen.Object, writer.Object);
        }

        [Theory]
        [InlineData("existing_path", "match", "correct_output")]
        public void MakeDocs_ValidOptions_GeneratesOutput(string input, string filter, string output)
        {
            Options options = new Options();
            options.InputDir = input;
            options.Filter = filter;
            options.Output = output;
            options.Tables = true;

            this.documenter.MakeDocs(options);

            Assert.Equal("=asciidoc output", this.documenter.OutputDocs);
        }

        [Theory]
        [InlineData("not_existing_path", "match", "correct_output")]
        [InlineData("existing_path", "no_match", "correct_output")]
        [InlineData("existing_path", "match", "incorrect_output")]
        public void MakeDocs_InvalidOptions_Throws(string input, string filter, string output)
        {
            Options options = new Options();
            options.InputDir = input;
            options.Filter = filter;
            options.Output = output;
            options.Tables = true;

            Assert.ThrowsAny<Exception>(() => this.documenter.MakeDocs(options));
        }
    }
}
