using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel.Parser
{
    public class ParseErrorException : Exception
    {
        string errTok;
        public ParseErrorException() : base() { }
        public ParseErrorException(string str) : base(str) { }
        public ParseErrorException(string msg, string info)
            : base(msg)
        {
            errTok = info;
        }
        public ParseErrorException(string str, Exception inner) { }
        protected ParseErrorException(
            System.Runtime.Serialization.SerializationInfo si,
            System.Runtime.Serialization.StreamingContext sc) :
            base(si, sc) { }

        public string ErrTok
        {
            get { return errTok; }
        }

        public override string ToString()
        {
            return Message;
        }

    }

}
