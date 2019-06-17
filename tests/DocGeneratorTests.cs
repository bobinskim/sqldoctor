using Moq;
using NLog;
using SqlDoctor;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using SqlDoctor.Schema;
using System;
using System.Collections.Generic;
using Xunit;

namespace SqlDoctor.Tests
{
    public class DocGeneratorTests
    {
        private readonly DocGenerator generator;

        public DocGeneratorTests()
        {
            Mock<ILogger> logger = new Mock<ILogger>();

            this.generator = new DocGenerator(logger.Object);
        }

        [Fact]
        public void Generate_TestSchema_AsciidocResult()
        {
            SchemaInfo testSchema = new SchemaInfo();
            var tab1 = new TableInfo("tab1");
            tab1.Columns["col1"] = new ColumnInfo();
            tab1.Columns["col1"].Name = "col1";
            tab1.Columns["col1"].DataType = "int";
            tab1.Columns["col1"].Description = "Column 1";
            tab1.Columns["col1"].Identity = true;
            tab1.Columns["col1"].Nullable = false;
            tab1.Columns["col1"].PrimaryKey = true;
            tab1.Columns["col1"].Unique = true;

            tab1.Columns["col2"] = new ColumnInfo();
            tab1.Columns["col2"].Name = "col2";
            tab1.Columns["col2"].DataType = "numeric";
            tab1.Columns["col2"].Size = "10, 2";
            tab1.Columns["col2"].Description = "Column 2";
            tab1.Columns["col2"].Identity = false;
            tab1.Columns["col2"].Nullable = true;
            tab1.Columns["col2"].PrimaryKey = false;
            tab1.Columns["col2"].Unique = false;

            testSchema.Tables.Add("tab1", tab1);

            var tab2 = new TableInfo("tab2");
            tab2.Columns["col1"] = new ColumnInfo();
            tab2.Columns["col1"].Name = "col1";
            tab2.Columns["col1"].DataType = "varchar";
            tab2.Columns["col1"].Size = "10";
            tab2.Columns["col1"].Description = "Column 1a";
            tab2.Columns["col1"].Identity = false;
            tab2.Columns["col1"].Nullable = false;
            tab2.Columns["col1"].PrimaryKey = false;
            tab2.Columns["col1"].Unique = true;

            testSchema.Tables.Add(@"tab2", tab2);

            string result = this.generator.Generate(testSchema);
            Assert.Equal("tab1 ::", result.Substring(0, 8).Trim());
            Assert.Equal(42, result.Split('\n').Length);
        }
    }
}
