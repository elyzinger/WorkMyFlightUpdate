using FlightRestWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WorkMyFlight;

namespace FlightRestWeb.Controllers
{
    public class LoginController : ApiController
    {
        private FlyingCenterSystem FCS;
        private ILoginToken loginToken = null;

        [Route("api/Login/UserLogin")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;

            FCS = FlyingCenterSystem.GetInstance();
            loginToken = FCS.Login(loginrequest.Username, loginrequest.Password);

            IHttpActionResult response;
            // HttpResponseMessage response=null;
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            //bool isUsernamePasswordValid = false;

            if (loginToken != null)
            //isUsernamePasswordValid = true;
            // if credentials are valid
            // if (isUsernamePasswordValid)
            {
                var token = TokenManager.GenerateToken(loginrequest.Username + ":" + loginrequest.Password);
                //return the token

                //return Request.CreateResponse(HttpStatusCode.Created, token);
                return Ok<string>(token);

            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                //return Request.CreateResponse(HttpStatusCode.Unauthorized);
                response = ResponseMessage(loginResponse.responseMsg);
                return response;
            }
        }

    }
}
