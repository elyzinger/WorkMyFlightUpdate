using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public static class FlightCenterConfig
    {

        public const string ADMIN_NAME = "ADMIN";
        public const string ADMIN_PASSWORD = "9999";
        public const string DAO_CON = @"Data Source=.;Initial Catalog=WorkMyFlights;Integrated Security=True";
        // (@"Server = tcp:workmyflight.database.windows.net,1433;Initial Catalog = WorkMyFlight; Persist Security Info=False;User ID = workmyflight ; Password=Password1!; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30");
        public const int historyTimer = 1000 * 60 * 60 * 24;
        //email key for sendgrid the key is inside  computer system settings inside system variables
        //public const string EmailKey = ""; 
    }
}
