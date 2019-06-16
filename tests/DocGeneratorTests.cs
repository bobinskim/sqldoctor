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

            testSchema.Tables.Add("tab2", tab2);

            string expected = @".tab1
[options=""header"", cols=""1,4,2,2,8,2,2,2""]
|====
|| ColumnName | DataType | Size | Description | Identity | Nullable | Unique

| icon:key[]
| col1
| int
| 
| Column 1
| true
| false
| true

| 
| col2
| numeric
| 10, 2
| Column 2
| false
| true
| false
|====

.tab2
[options=""header"", cols=""1,4,2,2,8,2,2,2""]
|====
|| ColumnName | DataType | Size | Description | Identity | Nullable | Unique

| 
| col1
| varchar
| 10
| Column 1a
| false
| false
| true
|====

";

            string result = this.generator.Generate(testSchema);
            Assert.Equal(expected, result);
        }
    }
}
