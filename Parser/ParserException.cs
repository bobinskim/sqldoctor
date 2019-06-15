using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SqlDoctor.Parser
{
    public class ParserException : ApplicationException
    {
        public ParserException()
        {
        }

        public ParserException(string msg) : base(msg)
        {

        }

        public ParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
