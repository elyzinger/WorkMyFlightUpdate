using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WorkMyFlight;
using WorkMyFlight.POCO;

namespace FlightRestWeb.Controllers
{
    //[EnableCors(origins: "http://localhost:56894", headers: "*", methods: "*")]
    public class AnonymousFacadeController : ApiController
    {
        private FlyingCenterSystem FCS;


        //[Route("api/AnonymousFacade/GetArrivingNow")]
        //[ResponseType(typeof(Flight))]
        //[HttpGet]
        //public IHttpActionResult GetLandingNow()
        //{
        //    FCS = FlyingCenterSystem.GetInstance();
        //    IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
        //    IList<Flight> flighs = (List<Flight>)af.GetArrivingNow();
        //    if (flighs.Count == 0)
        //        return NotFound();
        //    return Ok(flighs);
        //}
        [Route("api/AnonymousFacade/GetDeparturesNow")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetDeparturesNow()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetDepartingNow();
            if (flighs.Count == 0)
                return NotFound();
            return Ok(flighs);
        }
        // GET all flights
        [Route("api/AnonymousFacade/GetAllFlights")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetAllFlights()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetAllFlights();
            if (flighs.Count == 0)
                return NotFound();
            return Ok(flighs);
        }
        [Route("api/AnonymousFacade/GetAllAirlineCompanies")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<AirLineCompany> airlines = af.GetAllAirlineCompanies();
            if(airlines.Count == 0)       
                return NotFound();
            return Ok(airlines);
        }
        [Route("api/AnonymousFacade/GetAllFlightsVacancy")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetAllFlightsVacancy()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            Dictionary<Flight, long> flightVacancy = af.GetAllFlightsVacancy();
            if (flightVacancy.Count == 0)
                return NotFound();
            return Ok(flightVacancy);
        }
        [Route("api/AnonymousFacade/GetFlightById/{id}")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFlightById(int id)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetAllFlights();
            Flight flightByID = flighs.FirstOrDefault(a => a.ID == id);
            if(flightByID == null)
            {
                return NotFound();
            }
            return Ok(flightByID);
        }
        [Route("api/AnonymousFacade/GetFlightsByOriginCountry/{countryCode}")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFlightsByOriginCountry(int countryCode)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetAllFlights();
            Flight flightByID = flighs.FirstOrDefault(a => a.OriginCountryCode == countryCode);
            if (flightByID == null)
            {
                return NotFound();
            }
            return Ok(flightByID);        
        }
        [Route("api/AnonymousFacade/GetFlightsByDestinationCountry/{countryCode}")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFlightsByDestinationCountry(int countryCode)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetAllFlights();
            Flight flightByID = flighs.FirstOrDefault(a => a.DestinationCountryCode == countryCode);
            if (flightByID == null)
            {
                return NotFound();
            }
            return Ok(flightByID);
        }
        [Route("api/AnonymousFacade/GetFlightsByDepatrureDate/{departureDate}")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFlightsByDepatrureDate(DateTime departureDate)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetAllFlights();
            Flight flightByID = flighs.FirstOrDefault(a => a.DepartureTime == departureDate);
            if (flightByID == null)
            {
                return NotFound();
            }
            return Ok(flightByID);
        }
        [Route("api/AnonymousFacade/GetFlightsByLandingDate/{landingDate}")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFlightsByLandingDate(DateTime landingDate)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flights = (List<Flight>)af.GetAllFlights();
            Flight flightByID = flights.FirstOrDefault(a => a.LandingTime == landingDate);
            if (flightByID == null)
            {
                return NotFound();
            }
            return Ok(flightByID);
        }
        [Route("api/AnonymousFacade/SearchByParams")]
        [ResponseType(typeof(List<SearchParam>))]
        [HttpPost]
        public IHttpActionResult SearchByParams([FromBody] SearchParam searchParams)
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> searchList = af.GetFlightsBySearch(searchParams);
          
                if (searchList.Count <= 0 || searchList == null)
            { 
                    return NotFound();
            }   
            return Ok(searchList);       
        }
    }
}
