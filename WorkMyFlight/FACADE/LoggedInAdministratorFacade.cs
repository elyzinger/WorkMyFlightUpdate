using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade, IFacade
    {
        /// <summary>
        /// Creates a new airline company and adds it to the table in the DB amd take out his id
        /// </summary>
        public long CreateNewAirline(LoginToken<Administrator> token, AirLineCompany airline)
        {     
            if (ValidUserToken(token))
            {
               airline.ID = _airlineDAO.ADD(airline);
                _airlineDAO.RemoveSignedUpAirline(airline);
            }
            return airline.ID;
        }
        public void RemoveAirlineFromSignup(LoginToken<Administrator> token, AirLineCompany airline)
        {
            if (ValidUserToken(token))
            {
                _airlineDAO.RemoveSignedUpAirline(airline);
            }
        }
        // create new customer and add it to the table and take id out
        public long CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (ValidUserToken(token))
            {
              customer.ID = _customerDAO.ADD(customer);
            }
            return customer.ID;
        }
        // create new country and add it to the table and take id out
        public long CreateNewCountry(LoginToken<Administrator> token, Country country)
        {
            if (ValidUserToken(token))
            {
               country.ID = _countryDAO.ADD(country);
            }
            return country.ID;
        }
        // delete an existing airline from the system
        // first delete from ticktes after that from flights and at the end from airline
        // remove from data base
        public void RemoveAirline(LoginToken<Administrator> token, AirLineCompany airline)
        {
            if (ValidUserToken(token))
            {

                _airlineDAO.Remove(airline);
            }
        
        }
        // delete an existing customer
        // first delete from ticktes after delete the customer
        // remove from data base
        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (ValidUserToken(token))
            {
                IList<Ticket> tickets = _ticketDAO.GetAll();
                foreach (Ticket t in tickets)
                {
                    if(t.CustomerID == customer.ID)
                    {
                        _ticketDAO.Remove(t);
                    }
                }
                _customerDAO.Remove(customer);
            }
        }
        //// updating the info of an airline company in the system
        // updating the table in the database
        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirLineCompany airline)
        {
            if (ValidUserToken(token))
            {
                _airlineDAO.Update(airline);
            }
        }
        // updating the info of a customer the system
        // updating the table in the database
        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (ValidUserToken(token))
            {
                _customerDAO.Update(customer);
            }
        }
        // get all countries from db
        public IList<Country> GetAllCountries(LoginToken<Administrator> token)
        {
            IList<Country> countries = new List<Country>();
            if (ValidUserToken(token))
            {
                return countries = _countryDAO.GetAll();
            }
            return countries;
        }
        // get all airline companies from db
        public IList<AirLineCompany> GetAllAirlineCompanies(LoginToken<Administrator> token)
        {
            IList<AirLineCompany> airlines = new List<AirLineCompany>();
            if (ValidUserToken(token))
            {
                return airlines = _airlineDAO.GetAll();
            }
            return airlines;
        }
        // get all airline customers from db
        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            IList<Customer> custumers = new List<Customer>();
            if (ValidUserToken(token))
            {
                return custumers = _customerDAO.GetAll();
            }
            return custumers;
        }
        // get all signeup airlines companies from db
        public IList<AirLineCompany> GetAllSignedUpAirlineCompanies(LoginToken<Administrator> token)
        {
            IList<AirLineCompany> airlines = new List<AirLineCompany>();
            if (ValidUserToken(token))
            {
                return airlines = _airlineDAO.GetAllSignedUpAirlineCompaniesFromDB();
            }
            return airlines;
        }
        // get country name from db by name
        public string GetCountryNameByName(LoginToken<Administrator> token, string countryName)
        {
            Country country = new Country();
            if (ValidUserToken(token))
            {
                 country = _countryDAO.Get(countryName);
                return country.CountryName;
            }
            return country.CountryName;
        }
        // get cousomer user name from db by user name
        public string GetCustomerUserName(LoginToken<Administrator> token, string userName)
        {
            Customer customer = new Customer();
            if (ValidUserToken(token))
            {
                customer = _customerDAO.GetCustomerByUserName(userName);
                if (customer == null)
                    return null;
                return customer.UserName;
                
            }
            if (customer == null)
                return null;
            return customer.UserName;
        }
        // get cystomer by user name
        public Customer GetCustomer(LoginToken<Administrator> token, string userName)
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
        // get airline user name from db by user name
        public string GetAirlineUserName(LoginToken<Administrator> token, string userName)
        {
            AirLineCompany airLine = new AirLineCompany();
            if (ValidUserToken(token))
            {
                airLine = _airlineDAO.GetAirlineByUsername(userName);
                if (airLine == null)
                    return null;
                return airLine.UserName;

            }
            if (airLine == null)
                return null;
            return airLine.UserName;

        }
        public IList<AirLineCompany> GetAirlineSignIn(LoginToken<Administrator> token)
        {
             IList < AirLineCompany > signInList = new List<AirLineCompany>();
            if (ValidUserToken(token))
            {
                return signInList = _airlineDAO.GetAll();
            }
            return signInList;
        }
        // cheack for real admin every func
        public bool ValidUserToken(LoginToken<Administrator> token)
        {
            if (token != null && token.User != null)
                return true;
            else
                return false;
        }
    }
}
