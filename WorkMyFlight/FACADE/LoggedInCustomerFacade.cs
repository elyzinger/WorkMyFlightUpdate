using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    
    public class LoggedInCustomerFacade : AnonymousUserFacade,ILoggedInCustomerFacade, IFacade 
    {
        // delete all tickets for customer in database
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (ValidUserToken(token))
            {
                _ticketDAO.Remove(ticket);
            }
        }
        // get a list of all the customer flights
        public IList<Ticket> GetAllMyFlights(LoginToken<Customer> token )
        {
            IList<Ticket> customerflights = new List<Ticket>();
            if (ValidUserToken(token))
            {
                customerflights =  _flightDAO.GetFlightByCustomer(token.User);
                
            }
            return customerflights;
        }
        // get customer by user name
        public Customer GetCustomerProfile(LoginToken<Customer> token, string userName)
        {
            Customer customer = new Customer();
            if (ValidUserToken(token))
            {
                customer = _customerDAO.GetCustomerByUserName(userName);
                if (customer == null)
                    return null;
                return customer;

            }
            return null;
        }
        // updating the info of a customer the system
        // updating the table in the database
        public void UpdateCustomerProfile(LoginToken<Customer> token, Customer customer)
        {
            if (ValidUserToken(token))
            {
                _customerDAO.Update(customer);
            }
        }
        // delete an existing customer
        // first delete from ticktes after delete the customer
        // remove from data base
        public void RemoveCustomerProfile(LoginToken<Customer> token, Customer customer)
        {
            if (ValidUserToken(token))
            {
                IList<Ticket> tickets = _ticketDAO.GetAll();
                foreach (Ticket t in tickets)
                {
                    if (t.CustomerID == customer.ID)
                    {
                        _ticketDAO.Remove(t);
                    }
                }
                _customerDAO.Remove(customer);
            }
        }
        // create a new ticket on database
        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            Ticket ticket = null;
            if (ValidUserToken(token))
            {
                if (flight.RemaniningTickets > 0)
                {
                    ticket = new Ticket()
                    {
                        FlightID = flight.ID,
                        CustomerID = token.User.ID
                    };
                    ticket.ID = _ticketDAO.ADD(ticket);
                    _flightDAO.UpdateRemainingTickets(flight, 1);
                    return ticket;
                }
               // _flightDAO.Remove(flight);
                throw new NoMoreTicketsException("no more tickets left");
            }
            return ticket;
        }
        // cheack for real customer every func
        public bool ValidUserToken(LoginToken<Customer> token)
        {
            if (token != null && token.User != null)
                return true;
            else
                return false;
        }
    }
}
