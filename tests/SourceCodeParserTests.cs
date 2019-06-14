using Moq;
using NLog;
using SqlDoctor;
using SqlDoctor.Parser;
using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using Xunit;

namespace SqlDoctor.Tests
{
    public class SourceCodeParserTests
    {
        private readonly SourceCodeParser parser;

        public SourceCodeParserTests()
        {
            Mock<ILogger> logger = new Mock<ILogger>();

            this.parser = new SourceCodeParser(logger.Object);
        }

        [Theory]
        [InlineData("TestDDL0", 1)]
        [InlineData("TestDDL1", 31)]
        public void Parse_CorrectInput_CorrectSchema(string resName, int tables)
        {
            SchemaInfo result = this.parser.Parse(new List<string>() { Properties.Resources.ResourceManager.GetString(resName) });
            Assert.Equal(tables, result.Tables.Count);
        }

        [Fact]
        public void Parse_MultInput_MergeSchema()
        {
            SchemaInfo result = this.parser.Parse(new List<string>() { Properties.Resources.TestDDL0, Properties.Resources.TestDDL1 });
            Assert.Equal(32, result.Tables.Count);
        }

        [Fact]
        public void Parse_EmptyInput_EmptyOutput()
        {
            Assert.Empty(this.parser.Parse(new List<string>()).Tables);
        }
    }
}
