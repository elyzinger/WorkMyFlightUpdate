using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight.POCO
{
    public class ConfirmedUser : IPoco, IUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string EmailCon { get; set; }
        public string Guid { get; set; }
    }
}
