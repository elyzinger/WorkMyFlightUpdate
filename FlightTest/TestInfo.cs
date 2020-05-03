using WorkMyFlight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightTest
{
    public class TestInfo
    {
        static SqlCommand cmd = new SqlCommand();
        public LoginToken<Administrator> adminT;
        public LoggedInAdministratorFacade adminF;
        public LoginToken<AirLineCompany> airlineT;
        public LoggedInAirlineFacade airlineF;
        public LoginToken<Customer> customerT;
        public LoggedInCustomerFacade customerF;
        public AnonymousUserFacade anonymousF;
        public FlyingCenterSystem FC;

        public TestInfo()
        {
            ClearDB();
            FC = FlyingCenterSystem.GetInstance();
            adminT = (LoginToken<Administrator>)FC.Login(FlightCenterConfig.ADMIN_NAME, FlightCenterConfig.ADMIN_PASSWORD);
            adminF = (LoggedInAdministratorFacade)FC.GetFacade(adminT);
            Customer customer = CreateNewTestCustomer();
            customerT = (LoginToken<Customer>)FC.Login(customer.UserName, customer.Password);
            customerF = (LoggedInCustomerFacade)FC.GetFacade(customerT);
            AirLineCompany airlineCompany = CreateNewTestAirlineCompany();
            airlineT = (LoginToken<AirLineCompany>)FC.Login(airlineCompany.UserName, airlineCompany.Password);
            airlineF = (LoggedInAirlineFacade)FC.GetFacade(airlineT);
            anonymousF = (AnonymousUserFacade)FC.GetFacade(null);

        }

        // clear all data base
        public void ClearDB()
        {
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("CLEAR_ALL_TABLES", con))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

            }
        }

 

        public Customer CreateNewTestCustomer()
        {
            Customer testCustomer = new Customer("jon", "dow", "jon", "1234", "alkana", "054555440", "321456");
            adminF.CreateNewCustomer(adminT, testCustomer);
            return testCustomer;
        }
        public AirLineCompany CreateNewTestAirlineCompany()
        {
            Country testCountry = new Country("isreal");
            testCountry.ID = adminF.CreateNewCountry(adminT, testCountry);
            AirLineCompany testAirline = new AirLineCompany("elal", "elal", "4321", testCountry.ID);
            adminF.CreateNewAirline(adminT, testAirline);
            return testAirline;
        }

    }
}
