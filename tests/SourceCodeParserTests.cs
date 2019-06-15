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
            SchemaInfo mockSchema = new SchemaInfo();
            var tab1 = new TableInfo("tab1");
            tab1.Columns["col1"] = new ColumnInfo();
            mockSchema.Tables.Add("tab1", tab1);

            var tab2 = new TableInfo("tab2");
            tab1.Columns["col2"] = new ColumnInfo();
            mockSchema.Tables.Add("tab2", tab2);

            Mock<ILogger> logger = new Mock<ILogger>();
            Mock<SchemaVisitorBase> visitor = new Mock<SchemaVisitorBase>();
            visitor.SetupAllProperties();
            visitor.Object.Schema = mockSchema;

            this.parser = new SourceCodeParser(logger.Object, () => visitor.Object);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public void Parse_AnyInput_GetSchema(int fileNum)
        {
            var input = new List<string>();
            for(int i = 0; i<fileNum; i++)
            {
                input.Add("qwerty asdfg zxcvb");
            }

            SchemaInfo result = this.parser.Parse(input);
            Assert.Equal(fileNum * 2, result.Tables.Count);
        }

        [Fact]
        public void Parse_EmptyInput_EmptyOutput()
        {
            Assert.Empty(this.parser.Parse(new List<string>()).Tables);
        }
    }
}
