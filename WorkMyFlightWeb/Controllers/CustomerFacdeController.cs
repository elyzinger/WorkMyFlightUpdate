
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using WorkMyFlight;
using System.Web;
using System.Text;

namespace WorkMyFlightWeb.Controllers
{
    [BasicAuthentication]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class CustomerFacdeController : AutenticationDetails
    {
       

        //private FlyingCenterSystem FCS;
        //private LoginToken<Customer> customerT;
        //private LoggedInCustomerFacade customerF;


        //private void GetLoginToken()
        //{
        //    Request.Properties.TryGetValue("token", out object loginToken);

        //    customerT = loginToken as LoginToken<Customer>;

        //}
        //get customer from db
        [Route("api/CustomerFacde/GetCustomer")]
        [ResponseType(typeof(Customer))]
        [HttpGet]
        public IHttpActionResult GetCustomer()
        {
           
            string authenticationToken = ActionContext.Request.Headers.Authorization.Parameter;
           string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationToken));
            string userName = decodedAuthenticationToken.Split(':')[0];

            try
            {
                Customer customerProfile = ((LoggedInCustomerFacade)LogFacade).GetCustomerProfile((LoginToken<Customer>)Login,userName );
                return Ok(customerProfile);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        //get customer from db
        [Route("api/CustomerFacde/UpdateCustomerProfile")]
        [ResponseType(typeof(Customer))]
        [HttpPut]
        public IHttpActionResult UpdateCustomerProfile(Customer customer)
        {
            

            try
            {
               ((LoggedInCustomerFacade)LogFacade).UpdateCustomerProfile((LoginToken<Customer>)Login, customer);
                return Ok("ok");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        //removing customer from db
        [Route("api/CustomerFacde/RemoveCustomerProfile")]
        [ResponseType(typeof(Customer))]
        [HttpDelete]
        public IHttpActionResult RemoveCustomerProfile(Customer customer)
        {
            try
            {
                ((LoggedInCustomerFacade)LogFacade).RemoveCustomerProfile((LoginToken<Customer>)Login, customer);
                return Ok("deleted");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }

    }
}
