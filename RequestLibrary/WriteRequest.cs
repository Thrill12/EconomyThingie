using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class WriteRequest
    {

        public string message;

        public WriteRequest(string message)
        {
            this.message = message;
        }
    }
}
