using WorkMyFlight.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
     public interface IAnonymousUserFacade 
    {
        IList<Flight> GetAllFlights();
        IList<AirLineCompany> GetAllAirlineCompanies();
        Dictionary<Flight, long> GetAllFlightsVacancy();
        Flight GetFlightById(long id);
        IList<Flight> GetFlightsByOriginCountry(long countryCode);
        IList<Flight> GetFlightsByDestinationCountry(long countryCode);
        IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        IList<Flight> GetDepartingNow();
        IList<Flight> GetArrivingNow();
        IList<Flight> GetPromotionFlights();
        IList<Flight> GetFlightsBySearch(SearchParam search);
        bool AddNewUser(NewUser newUser);
        ConfirmedUser ConfirmMyEmail(string guid);
        long AddCustomerToData(Customer newCustomer);
    }
}
