
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WorkMyFlight;

namespace WorkMyFlightWeb.Controllers
{
    [BasicAuthentication]
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
