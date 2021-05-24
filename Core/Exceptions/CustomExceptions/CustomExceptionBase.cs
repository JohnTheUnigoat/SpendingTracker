using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions.CustomExceptions
{
    public abstract class CustomExceptionBase : Exception
    {
        public abstract ErrorCode ErrorCode { get; }

        public CustomExceptionBase(string message) :base(message)
        {
        }
    }
}
