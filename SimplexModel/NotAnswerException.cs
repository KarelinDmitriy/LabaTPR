using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplexModel
{
    class NoAnswerException : Exception
    {
        string errTok;
        public NoAnswerException() : base() { }
        public NoAnswerException(string str) : base(str) { }
        public NoAnswerException(string msg, string info)
            : base(msg)
        {
            errTok = info;
        }
        public NoAnswerException(string str, Exception inner) { }
        protected NoAnswerException(
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
