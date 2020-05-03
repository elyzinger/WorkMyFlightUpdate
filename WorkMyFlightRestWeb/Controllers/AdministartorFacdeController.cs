using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WorkMyFlight;

namespace WorkMyFlightRestWeb.Controllers
{
    public class AdministartorFacdeController : ApiController
    {
        private FlyingCenterSystem FCS;
        private LoginToken<Administrator> adminT;
        private LoggedInAdministratorFacade adminF;

        private void GetLoginToken()
        {
            Request.Properties.TryGetValue("token", out object loginToken);

            adminT = loginToken as LoginToken<Administrator>;
        }

        [Route("api/AnonymousFacade/CreateNewAirline")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpPost]
        public IHttpActionResult CreateNewAirline(AirLineCompany airlineCompany)
        {
            FCS = FlyingCenterSystem.GetInstance();
            ILoggedInAdministratorFacade adminiFacade = FCS.GetFacade(adminT) as ILoggedInAdministratorFacade;
            long airlineCompanyId = adminiFacade.CreateNewAirline(adminT, airlineCompany);
            //airlineCompany = adminiFacade.GetAirlineCompanyById(adminT, airlineCompanyId);
            return CreatedAtRoute("createairlinecompany", new { id = airlineCompanyId }, airlineCompany);
        }
        [Route("api/AnonymousFacade/WriteHello")]
        [ResponseType(typeof(AirLineCompany))]
        [HttpGet]
        public IHttpActionResult WriteHello()
        {
            return Ok("hello");
        }
        //[Route("api/AdministartorFacde/GetAllCountries")]
        //[ResponseType(typeof(Country))]
        //    [HttpGet]
        //    public IHttpActionResult GetAllCountries()
        //    {
        //    FCS = FlyingCenterSystem.GetInstance();
        //    adminT = (LoginToken<Administrator>)FCS.Login(FlightCenterConfig.ADMIN_NAME, FlightCenterConfig.ADMIN_PASSWORD);
        //    adminF = (LoggedInAdministratorFacade)FCS.GetFacade(adminT);

        //        if(countries.Count == 0)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(countries);
        //    }
        //[Route("api/AdministartorFacde/GetAllCountries")]
        //[ResponseType(typeof(AirLineCompany))]
        //    [HttpGet]
        //    public IHttpActionResult GetAllAirlineCompanies()
        //    {
        //        IList<AirLineCompany> airLines = _airlineDAO.GetAll();
        //        if (airLines.Count == 0)
        //        {   
        //            return NotFound();
        //        }
        //        return Ok(airLines);
        //    }
        //[Route("api/AdministartorFacde/GetCountryNameByName/{countryName}")]
        //[ResponseType(typeof(AirLineCompany))]
        //    [HttpGet]
        //    public IHttpActionResult GetCountryNameByName(string countryName)
        //    {
        //    Country country = _countryDAO.Get(countryName);

        //    if (country == null)
        //        {
        //            return NotFound();
        //        } 
        //        return Ok(country.CountryName);
        //    }
        //[ResponseType(typeof(AirLineCompany))]
        //[Route("createcompany/{airline}", Name = "CreateNewAirline")]
        //[HttpPost]
        //IHttpActionResult CreateNewAirline([FromBody]AirLineCompany airline)
        //{
        //    if (airline == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(_airlineDAO.ADD(airline));
        //}
        //[ResponseType(typeof(Customer))]
        //[HttpPost]
        //IHttpActionResult CreateNewCustomer([FromBody]Customer customer)
        //{
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(_customerDAO.ADD(customer));
        //}
        //[ResponseType(typeof(AirLineCompany))]
        //[HttpPut]
        //IHttpActionResult UpdateAirlineDetails(AirLineCompany airline)
        //{
        //    if (airline == null)
        //    {
        //        return NotFound();
        //    }
        //    _airlineDAO.Update(airline);
        //    return Ok($"{airline} updated");
        //}
        //[ResponseType(typeof(Customer))]
        //[HttpPut]
        //IHttpActionResult UpdateCustomerDetails(Customer customer)
        //{
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    _customerDAO.Update(customer);
        //    return Ok($"{customer} updated");
        //}
        //[ResponseType(typeof(AirLineCompany))]
        //[HttpDelete]
        //IHttpActionResult RemoveAirline(AirLineCompany airline)
        //{
        //    if (airline == null)
        //    {
        //        return NotFound();
        //    }
        //    _airlineDAO.Remove(airline);
        //    return Ok($"{airline} deleted");
        //}
        //[ResponseType(typeof(Customer))]
        //[HttpDelete]
        //IHttpActionResult RemoveCustomer(Customer customer)
        //{
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }
        //    _customerDAO.Remove(customer);
        //    return Ok($"{customer} deleted");
        //}
        //private void GetLoginToken()
        //{
        //    Request.Properties.TryGetValue("token", out object loginToken);

        //    adminT = loginToken as LoginToken<Administrator>;
        //}

    }

}