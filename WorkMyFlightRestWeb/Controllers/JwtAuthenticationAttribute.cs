
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WorkMyFlight;
using WorkMyFlightRestWeb.Models;

namespace WorkMyFlightRestWeb.Controllers
{
    public class JwtAuthenticationAttribute : AuthorizeAttribute
    {
        private FlyingCenterSystem FCS;
        private ILoginToken loginToken = null;

        //[ThreadStatic]
        //public static Airline CurrentAirline = null;


        public override void OnAuthorization(HttpActionContext actionContext)
        {

            // got user name + password here in server
            // How to get username and password?
            // does the request have username +psw?
            if (actionContext.Request.Headers.Authorization == null)
            {
                //stops the request -will not arrive to web api controller
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "you must send name +pwd in basic authentication");
                return;
            }

            string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            //string tokenUserNameAndPasswordArray = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            string tokenUserNameAndPasswordArray = TokenManager.ValidateToken(authenticationToken);
            string[] usernamePasswordArray = tokenUserNameAndPasswordArray.Split(':');
            string username = usernamePasswordArray[0];
            string password = usernamePasswordArray[1];
            FCS = FlyingCenterSystem.GetInstance();
            loginToken = FCS.Login(username, password);

            if (loginToken != null /* username == "a" && password == "123"*/)
            {
                actionContext.Request.Properties["token"] = loginToken;
                return;
            }

            //stops the request -will not arrive to web api controller
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "You are not allowed!");
        }
    }
}