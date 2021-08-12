 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WorkMyFlight;

namespace WorkMyFlightWeb.Controllers
{

    [BasicAuthentication]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AdministartorFacdeController : AutenticationDetails
    {

        //private FlyingCenterSystem FCS;
        //private LoginToken<Administrator> adminT;
        //private LoggedInAdministratorFacade adminF;
        //updating airline in db
        [Route("api/AdministartorFacde/UpdateAirline")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpPut]
        public IHttpActionResult UpdateAirline(AirLineCompany airlineCompany)
        {
            try
            {
                ((LoggedInAdministratorFacade)LogFacade).UpdateAirlineDetails((LoginToken<Administrator>)Login, airlineCompany);
                    return Ok("updated");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
           
        }
        //updating customer in db
        [Route("api/AdministartorFacde/UpdateCustomer")]
        [ResponseType(typeof(Customer))]
        [HttpPut]
        public IHttpActionResult UpdateCustomer(Customer customer)
        {
            try
            {
                ((LoggedInAdministratorFacade)LogFacade).UpdateCustomerDetails((LoginToken<Administrator>)Login, customer);
                return Ok("updated");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        //removing airline from db
        [Route("api/AdministartorFacde/RemoveAirline")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpDelete]
        public IHttpActionResult RemoveAirline(AirLineCompany airlineCompany)
        {
            try
            {
                ((LoggedInAdministratorFacade)LogFacade).RemoveAirline((LoginToken<Administrator>)Login, airlineCompany);
                return Ok("delted");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        //removing customer from db
        [Route("api/AdministartorFacde/RemoveCustomer")]
        [ResponseType(typeof(Customer))]
        [HttpDelete]
        public IHttpActionResult RemoveCustomer(Customer customer)
        {
            try
            {
                ((LoggedInAdministratorFacade)LogFacade).RemoveCustomer((LoginToken<Administrator>)Login, customer);
                return Ok("delted");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }

        }
        //moving a signdup airline to the airline table and removing it from signup
        [Route("api/AdministartorFacde/AddNewAirlineToDB")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpPost]
        public IHttpActionResult AddNewAirlineToDB(AirLineCompany airlineCompany)
        {
           
                AirLineCompany newAirline = new AirLineCompany
            {
                AirLineName = airlineCompany.AirLineName,
                UserName = airlineCompany.UserName,
                Password = airlineCompany.Password,
                CountryCode = airlineCompany.CountryCode,
                Email = airlineCompany.Email
            };
            try
            {
                long airlineCompanyId = ((LoggedInAdministratorFacade)LogFacade).CreateNewAirline((LoginToken<Administrator>)Login, newAirline);
             if(airlineCompanyId > 0)
                return Ok(airlineCompanyId);
            }
            catch(Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
            return StatusCode(HttpStatusCode.NotFound);
        }
        [Route("api/AdministartorFacde/RemoveAirlineFromSignUP")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpPost]
        public IHttpActionResult RemoveAirlineFromSignUP(AirLineCompany airlineCompany)
        {
            try
            {
                ((LoggedInAdministratorFacade)LogFacade).RemoveAirlineFromSignup((LoginToken<Administrator>)Login, airlineCompany);
                
                    return Ok($"{airlineCompany.AirLineName} was deleted");
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
            
        }
        //get all airlines from db
        [Route("api/AdministartorFacde/GetAllAirlines")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult GetAllAirlines()
        {
            try
            {
                IList<AirLineCompany> AirlineList = ((ILoggedInAdministratorFacade)LogFacade).GetAllAirlineCompanies((LoginToken<Administrator>)Login);
                if (AirlineList.Count == 0)
                {
                    return Ok(AirlineList.Count);
                }
                return Ok(AirlineList);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
        }
        //get all airlines from db
        [Route("api/AdministartorFacde/GetAllCustomers")]
        [ResponseType(typeof(Customer))]
        [HttpGet]
        public IHttpActionResult GetAllCustomers()
        {
            try
            {
                IList<Customer> customersList = ((ILoggedInAdministratorFacade)LogFacade).GetAllCustomers((LoginToken<Administrator>)Login);
                if (customersList.Count == 0)
                {
                    return Ok(customersList.Count);
                }
                return Ok(customersList);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
        }
        [Route("api/AdministartorFacde/GetAirlineSignInList")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult GetAirlineSignInList()
        {
            try
            {
                IList<AirLineCompany> AirlineSignInList = ((ILoggedInAdministratorFacade)LogFacade).GetAllSignedUpAirlineCompanies((LoginToken<Administrator>)Login);
                if (AirlineSignInList.Count == 0)
                {
                    return Ok(AirlineSignInList.Count);
                }
                return Ok(AirlineSignInList);
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
        }
        [Route("api/AdministartorFacde/GetAllAirlineSignIn")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult GetAllAirlineCompanies()
        {
            try
            {
                IList<AirLineCompany> airlines = ((LoggedInAdministratorFacade)LogFacade).GetAllAirlineCompanies((LoginToken<Administrator>)Login);
                if (airlines.Count == 0)
                {
                    return NotFound();
                }
                return Ok(airlines);
            }
            catch(Exception e)
            {
                  return Content(HttpStatusCode.NotAcceptable, $"{e.Message}");
            }
        }


    }
}
