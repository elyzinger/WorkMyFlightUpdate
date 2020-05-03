using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight.POCO
{
    // a user that exist when registration happend 
    public class NewUser
    {
        public string UserNAME { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Guid { get; set; }
    }
}
