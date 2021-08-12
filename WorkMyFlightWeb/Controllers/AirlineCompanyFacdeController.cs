
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WorkMyFlight;

namespace WorkMyFlightWeb.Controllers
{
    [BasicAuthentication]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AirlineCompanyFacdeController : AutenticationDetails
    {
        private FlyingCenterSystem FCS;
        private LoginToken<AirLineCompany> airlineT;
        private LoggedInAirlineFacade  airlineF;

        private void GetLoginToken()
        {
            Request.Properties.TryGetValue("token", out object loginToken);

            airlineT = loginToken as LoginToken<AirLineCompany>;
        }
        [Route("api/AirlineFacde/GetAirline")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult GetAirline()
        {

            string authenticationToken = ActionContext.Request.Headers.Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(
                     Convert.FromBase64String(authenticationToken));
            string userName = decodedAuthenticationToken.Split(':')[0];

            try
            {
                AirLineCompany airlineProfile = ((LoggedInAirlineFacade)LogFacade).GetAirlineProfile((LoginToken<AirLineCompany>)Login, userName);
                return Ok(airlineProfile);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        [Route("api/AnonymousFacade/GetAllFlightsByAirline")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetAllFlightsByAirline()
        {
            GetLoginToken();
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetArrivingNow();
            if (flighs.Count == 0)
                return NotFound();
            return Ok(flighs);
        }
    }
}
