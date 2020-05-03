using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public interface IFlightDAO : IBasicDB<Flight>
    {
        Dictionary<Flight, long> GetAllFlightsVacancy();
        Flight GetFlightById(long id);
        IList<Ticket> GetFlightByCustomer(Customer customer);
        IList<Flight> GetFlightByDepartureDate(DateTime Date);
        IList<Flight> GetFlightByDestinationCountry(long destinationcountry);
        IList<Flight> GetFlightByLandingDate(DateTime date);
        IList<Flight> GetFlightByOriginCountry(long origincountry);

    }
}
