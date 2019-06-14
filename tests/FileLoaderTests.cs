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
    public class FileLoaderTests
    {
        private readonly FileLoader loader;

        public FileLoaderTests()
        {
            Mock<IDirectoryWrap> dirMock = new Mock<IDirectoryWrap>();
            Mock<IFileWrap> fileMock = new Mock<IFileWrap>();
            Mock<ILogger> logMock = new Mock<ILogger>();

            dirMock.Setup(d => d.GetFiles("invalid_path", "*.sql", It.IsAny<SearchOption>())).Throws<ArgumentException>();
            dirMock.Setup(d => d.GetFiles("existing_path", "[A-Z]+", It.IsAny<SearchOption>())).Throws<ArgumentException>();
            dirMock.Setup(d => d.GetFiles("existing_path", "*.sql", SearchOption.TopDirectoryOnly)).Returns(new string[] { "A", "B" });
            dirMock.Setup(d => d.GetFiles("existing_path", "*.sql", SearchOption.AllDirectories)).Returns(new string[] { "A", "B", "C", "D" });

            fileMock.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns<string>(f => string.Format("t{0}", f));

            this.loader = new FileLoader(dirMock.Object, fileMock.Object, logMock.Object);
        }

        [Theory]
        [InlineData("existing_path", "*.sql", true, new string[] {"tA", "tB", "tC", "tD"} )]
        [InlineData("existing_path", "*.sql", false, new string[] { "tA", "tB"})]
        public void LoadFiles_ValidOptions_GeneratesOutput(string path, string filter, bool recursive, string[] results)
        {
            IEnumerable<string> content = this.loader.LoadFiles(path, filter, recursive);
            Assert.Equal(results, content);
        }

        [Fact]
        public void LoadFiles_InvalidPath_Throws()
        {
            Assert.ThrowsAny<ArgumentException>(() => this.loader.LoadFiles("invalid_path", "*.sql", false));
        }

        [Fact]
        public void LoadFiles_InvalidFilter_Throws()
        { 
            Assert.ThrowsAny<ArgumentException>(() => this.loader.LoadFiles("existing_path", "[A-Z]+", false));
        }
    }
}
