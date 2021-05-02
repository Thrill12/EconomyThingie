using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary.Alerts
{
    public class TestAlert : Alert
    {

        public string MessageFromServer { get; set; }
        
        
        public TestAlert(string v)
        {
            MessageFromServer = v;
        }
    }
}
