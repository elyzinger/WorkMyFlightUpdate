

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WorkMyFlight;

namespace WorkMyFlightWeb.Controllers
{
  
    public class BasicAuthenticationAttribute : /*AuthorizeAttribute*/AuthorizationFilterAttribute
    {
        int userIsBlocked = 0;
        // when you send name and password in the header.authorization request the information goes throw here 
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    "You must enter user name + password");
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;

                string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationToken));

                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                // check the type of token we get
                FlyingCenterSystem fcs = FlyingCenterSystem.GetInstance();
                try
                {
                    ILoginToken loginToken = fcs.Login(username, password);
                    IFacade facade = fcs.GetFacade(loginToken);

                    if (loginToken.GetType() == typeof(LoginToken<Administrator>))
                    {
                        LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
                        LoggedInAdministratorFacade LogFacade = (LoggedInAdministratorFacade)facade;
                        actionContext.Request.Properties["AdminUser"] = token;
                        actionContext.Request.Properties["AdminFacade"] = LogFacade;
                    }
                    else if (loginToken.GetType() == typeof(LoginToken<Customer>))
                    {
                        LoginToken<Customer> token = (LoginToken<Customer>)loginToken;
                        LoggedInCustomerFacade LogFacade = (LoggedInCustomerFacade)facade;
                        actionContext.Request.Properties["CustomerUser"] = token;
                        actionContext.Request.Properties["CustomerFacade"] = LogFacade;

                    }
                    else if (loginToken.GetType() == typeof(LoginToken<AirLineCompany>))
                    {
                        LoginToken<AirLineCompany> token = (LoginToken<AirLineCompany>)loginToken;
                        LoggedInAirlineFacade LogFacade = (LoggedInAirlineFacade)facade;
                        actionContext.Request.Properties["AirlineUser"] = token;
                        actionContext.Request.Properties["AirlineFacade"] = LogFacade;

                    }
                    userIsBlocked = 0;
                }
                catch (Exception e)
                {
                    userIsBlocked++;
                    if (userIsBlocked == 3)
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, $"you blocked!");
                    else
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, $"{e.Message}");
                }


            }
        }
    }
}
