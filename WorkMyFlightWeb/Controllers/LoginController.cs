using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;


namespace WorkMyFlightWeb.Controllers
{
    [BasicAuthentication]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class LoginController : AutenticationDetails
    {
        // checks the token when you log in
        [Route("api/LoginController/GetLoginToken")]
        [HttpGet]
        public IHttpActionResult GetLoginToken()
        {
            if (Request.Properties.TryGetValue("AdminUser", out object tokenAd))
            {
                return Ok("Admin");

            }
            if (Request.Properties.TryGetValue("AirlineUser", out object tokenAi))
            {
                return Ok("airline");

            }
            if (Request.Properties.TryGetValue("CustomerUser", out object tokenC))
            {
                return Ok("customer");

            }
            return null;
        }
    }
}
