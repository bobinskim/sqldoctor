using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemWrapper.IO;

namespace SqlDoctor.Parser
{
    public class ParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandParser>();
            builder.RegisterType<FileLoader>().AsImplementedInterfaces();
            builder.RegisterType<SourceCodeParser>().AsImplementedInterfaces();
            builder.RegisterType<DirectoryWrap>().AsImplementedInterfaces();
            builder.RegisterType<FileWrap>().AsImplementedInterfaces();
        }
    }
}
