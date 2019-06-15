using Microsoft.SqlServer.TransactSql.ScriptDom;
using Moq;
using NLog;
using SqlDoctor;
using SqlDoctor.Generator;
using SqlDoctor.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SystemWrapper.IO;
using Xunit;

namespace SqlDoctor.Tests
{
    public class SchemaVisitorTests
    {
        private readonly SchemaVisitor visitor;
        private readonly TSql150Parser parser = new TSql150Parser(false);

        public SchemaVisitorTests()
        {
            var logMock = new Mock<ILogger>();
                
            this.visitor = new SchemaVisitor(logMock.Object);
        }

        [Theory]
        [InlineData("TestDDL0", 1)]
        [InlineData("TestDDL1", 31)]
        public void AcceptVisitor_ValidInput_AllTables(string res, int tabCount)
        {
            string input = Properties.Resources.ResourceManager.GetString(res);
            IList <ParseError> errors;
            var fragment = this.parser.Parse(new StringReader(input), out errors);

            fragment.Accept(this.visitor);

            Assert.Equal(tabCount, this.visitor.Schema.Tables.Count);
        }

        [Fact]
        public void AcceptVisitor_TestTable_CorrectConstraints()
        {
            string input = Properties.Resources.TestDDL0;
            IList<ParseError> errors;
            var fragment = this.parser.Parse(new StringReader(input), out errors);

            fragment.Accept(this.visitor);

            var table = visitor.Schema.Tables.First().Value;

            Assert.Equal(8, table.Columns.Count);
            Assert.True(table.Columns["SalesQuotaKey"].Identity);
            Assert.False(table.Columns["SalesQuotaKey"].Nullable);
            Assert.True(table.Columns["SalesQuotaKey"].PrimaryKey);
            Assert.True(table.Columns["SalesQuotaKey"].Unique);

            Assert.False(table.Columns["EmployeeKey"].Identity);
            Assert.False(table.Columns["EmployeeKey"].Nullable);
            Assert.False(table.Columns["EmployeeKey"].PrimaryKey);
            Assert.False(table.Columns["EmployeeKey"].Unique);

            Assert.False(table.Columns["weight"].Identity);
            Assert.True(table.Columns["weight"].Nullable);
            Assert.False(table.Columns["weight"].PrimaryKey);
            Assert.False(table.Columns["weight"].Unique);
        }
    }
}
