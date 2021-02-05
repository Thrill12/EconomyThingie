using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class ReadRequest
    {

        public string message;

        public ReadRequest(string message)
        {
            this.message = message;
        }
    }
}
