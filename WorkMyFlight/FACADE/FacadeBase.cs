using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    // make 
    public abstract class FacadeBase 
    {
        // create a cabel from dao to facade
        protected AirlineDAOMSSQL _airlineDAO { get; set; }
        protected CountryDAOMSSQL _countryDAO { get; set; }
        protected CustomerDAOMSSQL _customerDAO { get; set; }
        protected FlightDAOMSSQL _flightDAO { get; set; }
        protected TicketDAOMSSQL _ticketDAO { get; set; }

        public FacadeBase()
        {
            _airlineDAO = new AirlineDAOMSSQL();
            _countryDAO = new CountryDAOMSSQL();
            _customerDAO = new CustomerDAOMSSQL();
            _flightDAO = new FlightDAOMSSQL();
            _ticketDAO = new TicketDAOMSSQL();
        }
       
    }
}
