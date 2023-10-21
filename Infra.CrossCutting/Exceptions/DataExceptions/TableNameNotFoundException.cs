using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.CrossCutting.Exceptions.DataExceptions
{
    public class TableNameNotFoundException : Exception
    {
        public TableNameNotFoundException() { }

        public TableNameNotFoundException(string message) : base(message) { }
    }
}
