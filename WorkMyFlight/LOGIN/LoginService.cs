using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    // login service return a type of token to flyingcentersystem calss for the login func
     public class LoginService : ILoginService
    {
        private AirlineDAOMSSQL _airlineDAO;
        private CustomerDAOMSSQL _customerDAO;
        public LoginService()
        {
            _airlineDAO = new AirlineDAOMSSQL();
            _customerDAO = new CustomerDAOMSSQL();
        }

        public bool TryAdminLogin(string username, string password, out LoginToken<Administrator> token)
        {
            if (username.ToUpper() == FlightCenterConfig.ADMIN_NAME && password.ToUpper() == FlightCenterConfig.ADMIN_PASSWORD)
            {
                token = new LoginToken<Administrator>();
                token.User = new Administrator();
                return true;
            }
            token = null;
            return false;
            
        }

        public bool TryAirLineLogin(string username, string password, out LoginToken<AirLineCompany> token)
        {
                AirLineCompany company = _airlineDAO.GetAirlineByUsername(username);
            if(company != null)
            {
                if(company.Password == password)
                {
                    token = new LoginToken<AirLineCompany>() { User = company};
                    return true;
                }
                throw new WrongPasswordException("airline company wrong password");
            }
            token = null;
            return false;
                     
        }

        public bool TryCustomerLogin(string username, string password, out LoginToken<Customer> token)
        {
            Customer customer = _customerDAO.GetCustomerByUserName(username);
            if(customer != null)
            {
                if(customer.Password == password)
                {
                    token = new LoginToken<Customer>() {User = customer };
                    return true;
                }
                throw new WrongPasswordException("customer wrong password");
            }
            token = null;
            return false;
            //throw new UserNotFoundException("user not found");
         
        }
        
    }
    
}
