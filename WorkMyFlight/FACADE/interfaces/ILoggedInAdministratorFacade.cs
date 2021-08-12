using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public interface ILoggedInAdministratorFacade
    {
        long CreateNewAirline(LoginToken<Administrator> token, AirLineCompany airline);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirLineCompany customer);
        void RemoveAirline(LoginToken<Administrator> token, AirLineCompany airline);
        long CreateNewCustomer(LoginToken<Administrator> token, Customer customer);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer);
        void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        IList<AirLineCompany> GetAllSignedUpAirlineCompanies(LoginToken<Administrator> token);
        IList<AirLineCompany> GetAllAirlineCompanies(LoginToken<Administrator> token);
        void RemoveAirlineFromSignup(LoginToken<Administrator> token, AirLineCompany airline);
        IList<Customer> GetAllCustomers(LoginToken<Administrator> token);
    }
}
