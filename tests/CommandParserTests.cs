using Moq;
using NLog;
using SqlDoctor;
using System;
using Xunit;

namespace tests
{
    public class CommandParserTests
    {
        private readonly CommandParser parser;

        public CommandParserTests()
        {
            Mock<ILogger> logger = new Mock<ILogger>();

            this.parser = new CommandParser(logger.Object);
        }

        [Theory]
        [InlineData("--directory=inputDir")]
        [InlineData("--directory=inputDir", "--tables=false", "--filter='_regex_'", "--output=out.adoc")]
        [InlineData("-d", "inputDir", "-t", "-f", "'_regex_'", "-o", "out.adoc")]
        [InlineData("--directory", "inputDir", "--filter='_regex_'", "--output=out.adoc", "--tables", "false")]
        public void Parse_ValidArguments_ReturnOptions(params string[] input)
        {
            Options opts = this.parser.Parse(input);
            Assert.NotNull(opts);
        }

        [Theory]
        [InlineData("invalid_command")]
        [InlineData("--tables=false", "--filter='_regex_'", "--output=out.adoc")]
        [InlineData("-t", "-f", "'_regex_'", "-o", "out.adoc")]
        [InlineData("-d", "inputDir", "-t", "-f", "-o", "out.adoc")]
        public void Parse_InvalidArguments_Throws(params string[] input)
        {
            Assert.ThrowsAny<Exception>(() => this.parser.Parse(input));
        }
    }
}
