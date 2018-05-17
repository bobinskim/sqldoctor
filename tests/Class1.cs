using SqlDoctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace tests
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
            FileLoader loader = new FileLoader();
            Assert.Empty(loader.LoadFiles(null, null, false));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
