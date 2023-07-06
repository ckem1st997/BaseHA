using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseHA.Domain.Exceptions
{
    public partial class BaseDomainException : Exception
    {
        public BaseDomainException()
        { }

        public BaseDomainException(string message)
            : base(message)
        { }

        public BaseDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}