
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkMyFlight;

namespace WorkMyFlightWeb.Controllers
{
    [BasicAuthentication]
    public class CustomerFacdeController : AutenticationDetails
    {
        private FlyingCenterSystem FCS;
        private LoginToken<Customer> customerT;
        private LoggedInCustomerFacade customerF;

        private void GetLoginToken()
        {
            Request.Properties.TryGetValue("token", out object loginToken);

            customerT = loginToken as LoginToken<Customer>;
        }
    }
}
