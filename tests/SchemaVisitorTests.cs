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

        //[Fact]
        //public void LoadFiles_InvalidPath_Throws()
        //{
        //    
        //}
    }
}
