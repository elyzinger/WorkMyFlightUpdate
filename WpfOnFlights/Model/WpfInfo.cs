using WorkMyFlight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOnFlights.Model
{
    class WpfInfo
    {
        static SqlCommand cmd = new SqlCommand();
        public LoginToken<Administrator> adminT;
        public LoggedInAdministratorFacade adminF;
        public FlyingCenterSystem FC;

        public WpfInfo()
        {
            FC = FlyingCenterSystem.GetInstance();
            adminT = (LoginToken<Administrator>)FC.Login(FlightCenterConfig.ADMIN_NAME, FlightCenterConfig.ADMIN_PASSWORD);
            adminF = (LoggedInAdministratorFacade)FC.GetFacade(adminT);
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
        // Generate a random string
        public string RandomString(bool lowerCase)    
        {
            Random random = new Random();
            int size = random.Next(1, 7);
            StringBuilder builder = new StringBuilder();       
            char ch;    
            for (int i = 0; i < size; i++)    
                  {    
                       ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));    
                       builder.Append(ch);    
                  }    
            if (lowerCase)    
                       return builder.ToString().ToLower();    
                          return builder.ToString();    
        }
        // Generate a random password    
        public string RandomPassword()
        {
            Random randomNumber = new Random();
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(true));
            builder.Append(randomNumber.Next(1000, 9999));
            builder.Append(RandomString(false));
            return builder.ToString();
        }
        // Create a random amount of countries
        public void CreateRandomCountries(int randomNum)
        {
            List<Country> countries = new List<Country>();
            Random random = new Random();
            for (int i = 0; i < randomNum; i++)
            {
                Country c = new Country
                {
                    CountryName = RandomString(false),

                };
                foreach (Country country in countries)
                {
                    if (country.CountryName == c.CountryName || adminF.GetCountryNameByName(adminT, country.CountryName) == c.CountryName)
                    {
                        i--;
                    }
                    continue;
                }
                adminF.CreateNewCountry(adminT, c);
                    countries.Add(c);
               
            }
          
        }
        // Create a random amount of customers
        public void CreateRandomCustomers(int randomNum)
        {
            Random random = new Random();
            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < randomNum; i++)
            {
                Customer c = new Customer
                {
                    FirstName = RandomString(true),
                    LastName = RandomString(true),
                    UserName = RandomString(true),
                    Password = RandomPassword(),
                    Address = RandomString(true),
                    PhoneNumber = random.Next(10000, 100000).ToString(),
                    CreditCardNumber = random.Next(100000, 999999).ToString(),
                };
                foreach (Customer customer in customers)
                {
                    if (customer.UserName == c.UserName || customer.Password == c.Password || adminF.GetCustomerUserName(adminT, c.UserName) == c.UserName)
                    {
                        i--;
                    }
                    continue;
                }
                    adminF.CreateNewCustomer(adminT, c);
                    customers.Add(c); 
            }
             
        }
        // Create a random amount of airlines
        public void CreateRandomAirlineCompanies(int numberOfAirlines)
        {
            Country randomCountry = new Country();
            IList<Country> dbCountries = adminF.GetAllCountries(adminT);
            List<AirLineCompany> airlines = new List<AirLineCompany>();
            Random random = new Random();
           
            for (int i = 0; i < numberOfAirlines; i++)
            {
                randomCountry = dbCountries[random.Next(0, dbCountries.Count)];
                AirLineCompany c = new AirLineCompany
                {
                    AirLineName = RandomString(true),
                    UserName = RandomString(true),
                    Password = RandomPassword(),
                    CountryCode = randomCountry.ID,
                };
                foreach (AirLineCompany airLine in airlines)
                {
                    if (airLine.UserName == c.UserName || airLine.Password == c.Password || adminF.GetAirlineUserName(adminT, c.UserName) == c.UserName)
                    {
                        i--;
                    }

                }
                airlines.Add(c);
                adminF.CreateNewAirline(adminT, c);                 
            }
     
        }
        // Create a random amount of flights
        public void CreateRandomFlights(int randomNum)
        {
            Random random = new Random();
            Country randomCountry = new Country();
            Country randomCountry2 = new Country();
            AirLineCompany randomAirline = new AirLineCompany();
            List<Flight> flights = new List<Flight>();
            IList<Country> dbCountries = adminF.GetAllCountries(adminT);
            IList<AirLineCompany> dbAirlines = adminF.GetAllAirlineCompanies();
            
            for (int i = 0; i < randomNum; i++)
            {
                randomCountry = dbCountries[random.Next(0, dbCountries.Count)];
                randomAirline = dbAirlines[random.Next(0, dbAirlines.Count)];
                dbCountries.Remove(randomCountry);
                randomCountry2 = dbCountries[random.Next(0, dbCountries.Count)];
                Flight flight = new Flight
                 {
                  AirLineCompanyID = randomAirline.ID,
                  OriginCountryCode = randomCountry.ID,
                  DestinationCountryCode = randomCountry2.ID,
                  DepartureTime = DateTime.Now,
                  LandingTime = DateTime.Now,
                  RemaniningTickets = random.Next(0,200),
                 };
                foreach (Flight f in flights)
                {
                    if (f.OriginCountryCode == flight.OriginCountryCode && f.DestinationCountryCode == flight.DestinationCountryCode)
                    {
                        i--;
                    }
                    else
                    {
                        flights.Add(flight);

                    }
                }
            }
          
        }
        // Create a random amount of tickets
        //public List<Ticket> CreateTickets(int randomNum)
        //{
        //    List<Ticket> tickets;
        //    return tickets;
        //}
    }
}
