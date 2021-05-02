using RequestLibrary.Accounts;
using RequestLibrary.ObjectClasses.Artificial.ShipThings.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestLibrary
{
    public class AddModuleRequest : AuthenticatedRequest
    {
        public User owningUser;
        public BaseModule modToAdd;

        public AddModuleRequest(BaseModule modToAdd, User owningUser)
        {
            this.modToAdd = modToAdd;
            this.owningUser = owningUser;
        }
    }
}
