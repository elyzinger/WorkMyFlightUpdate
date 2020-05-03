using SendGrid;
using SendGrid.Helpers.Mail;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using WorkMyFlight;
using WorkMyFlight.POCO;
using WorkMyFlightWeb.Models;

namespace WorkMyFlightWeb.Controllers
{
    //[EnableCors(origins: "http://localhost:56894", headers: "*", methods: "*")]
    public class AnonymousFacadeController : ApiController
    {
        private FlyingCenterSystem FCS;
        public static string guideForAuth = null;

        // adding a customer user to reddis and adding him to db before creating him as a customer. and sending an email to him for confirmation
        [Route("api/AnonymousFacade/AddNewCustomerToRedis")]
        [ResponseType(typeof(Customer))]
        [HttpPost]        
        public IHttpActionResult AddNewCustomerToRedis([FromBody] Customer customer)
        {
            bool redisBool = false;
            redisBool = Redis.RedisSaveNewCustomer("localhost", customer.UserName, customer);
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade; 

            if (redisBool == false)
            {
                return NotFound();
            }
            guideForAuth = Guid.NewGuid().ToString();
            NewUser customerUser = new NewUser { UserNAME = customer.UserName, Email = customer.Email, Guid = guideForAuth, Type = "customer"};
            af.AddNewUser(customerUser);
            SendEmail(customer.Email, customer.FirstName, guideForAuth);
            return Ok(redisBool);
            
        }
        // adding a airline user to reddis and adding him to db before creating him as a airline. and sending an email to him for confirmation
        [Route("api/AnonymousFacade/AddNewAirlineToRedis")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpPost]        
        public IHttpActionResult AddNewAirlineToRedis([FromBody] AirLineCompany airline)
        {
            bool redisBool = false;
            //var jsonStringAccount = Newtonsoft.Json.JsonConvert.SerializeObject(airline);
            redisBool = Redis.RedisSaveNewAirline("localhost", airline.UserName, airline);
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;       

            if (redisBool == false)
            {
                return NotFound();
            }
            guideForAuth = Guid.NewGuid().ToString();
            NewUser airlinerUser = new NewUser { UserNAME = airline.UserName, Email = airline.Email, Guid = guideForAuth, Type = "airline" };
            af.AddNewUser(airlinerUser);
            SendEmail(airline.Email, airline.AirLineName, guideForAuth);
            return Ok(redisBool);
        }
        // get all flights that lands now
        [Route("api/AnonymousFacade/GetLandingNow")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetLandingNow()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetArrivingNow();
            if (flighs.Count == 0)
                return NotFound();
            return Ok(flighs);
        }
        // get future flights for promotion
        [Route("api/AnonymousFacade/GetFutureFlights")]
        [ResponseType(typeof(Flight))]
        [HttpGet]
        public IHttpActionResult GetFutureFlights()
        {
            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            IList<Flight> flighs = (List<Flight>)af.GetPromotionFlights();
            if (flighs.Count == 0)
                return NotFound();
            return Ok(flighs);
        }
        //get all flights that depart now
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
        // get all airline companies
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
        // get all flights by id 
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
        // get all flights by origin country code
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
        // get all flights with the same destination country 
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
        // get all flights with the same departure date
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
        // get all flights with the same landing date 
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
        // get all flights with the same search parameter they get from search
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
        // email confirmation after the user get an email from us 
        // if the email was already confirmed he gets a message 
        // if is a customer he will move into db as a customer if airline the admin will need to aprove him
        [Route("api/AnonymousFacade/ConfirmEmail")]
        [HttpPost]
        public IHttpActionResult ConfirmEmail([FromUri] string guid)
        {
            ConfirmedUser user = null;

            FCS = FlyingCenterSystem.GetInstance();
            IAnonymousUserFacade af = FCS.GetFacade(null) as AnonymousUserFacade;
            user = af.ConfirmMyEmail(guid);

            if (user.UserName == null)
            {
                return NotFound();
            }
            if (user.EmailCon == "was already confirmed")
            {
                return Ok(user.EmailCon);
            }
            if(user.Type == "customer")
            {
                Customer newCustomer = Redis.RedisGetCustomer("localhost", user.UserName);
                newCustomer.ID = af.AddCustomerToData(newCustomer);
               
                return Ok($"{user.UserName} was confirmed");
            }

            AirLineCompany airline = Redis.RedisGetAirline("localhost", user.UserName);
                return Ok($"{user.UserName} was confirmed one of our admins will get back to you. thank you!");       

        }

        //sendin email confirmation
        private static void SendEmail(string email, string name, string guid)
        {

            var apiKey = Environment.GetEnvironmentVariable("EmailKey", EnvironmentVariableTarget.Machine);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("gnlguys@gmail.com", "WorkMyFlight");
            var subject = "Email Verification";    
            var to = new EmailAddress(email, name);
            var plainTextContent = "Please confirm your email" + name ;
            var htmlContent = "Hello " + name + "<br>Click here to confirm your email: http://localhost:61909/api/AnonymousFacade/ConfirmEmail?guid=" + guideForAuth; 
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg).Result;
        }
      
    }
}
