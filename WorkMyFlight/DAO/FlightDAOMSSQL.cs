using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
   
    public class FlightDAOMSSQL : IFlightDAO
    {
        private static object key = new object();
        static SqlCommand cmd = new SqlCommand();
        // checking if exist and inserting a new flight to db
        public long ADD(Flight t)
        {
            SqlCommand cmd2 = new SqlCommand();
              
          
                {
                try
                {
                    using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd.Connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"SELECT COUNT(*) FROM Flights WHERE AIRLINE_COMPANY_ID = {t.AirLineCompanyID}" +
                        $" AND ORIGIN_COUNTRY_CODE = {t.OriginCountryCode}" +
                        $" AND DESTINATION_COUNTRY_CODE = {t.DestinationCountryCode}" +
                        $" AND CONVERT(char(16),DEPARTURE_TIME,120) = ('{t.DepartureTime.ToString("yyyy-MM-dd HH:mm")}')" +
                        $" AND CONVERT(char(16),LANDING_TIME,120) = ('{t.LandingTime.ToString("yyyy-MM-dd HH:mm")}')" +
                        $" AND REMANING_TICKETS = {t.RemaniningTickets}";

                        string res = cmd.ExecuteScalar().ToString();
                        if (res != "0")
                            throw new AlreadyExistException($"Flight country code {t.DestinationCountryCode} already exists");
                        cmd.Connection.Close();

                    }
                    using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd2.Connection.Open();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = $"INSERT INTO Flights(AIRLINE_COMPANY_ID, ORIGIN_COUNTRY_CODE, DESTINATION_COUNTRY_CODE, DEPARTURE_TIME, LANDING_TIME, REMANING_TICKETS)" +
                        $"values({t.AirLineCompanyID}, {t.OriginCountryCode}, { t.DestinationCountryCode},'{ t.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss")}','{t.LandingTime.ToString("yyyy-MM-dd HH:mm:ss")}', {t.RemaniningTickets});" +
                        $" SELECT ID FROM Flights WHERE AIRLINE_COMPANY_ID = {t.AirLineCompanyID}" +
                        $" AND ORIGIN_COUNTRY_CODE = {t.OriginCountryCode}" +
                        $" AND DESTINATION_COUNTRY_CODE = {t.DestinationCountryCode}" +
                        $" AND CONVERT(char(16),DEPARTURE_TIME,120) = ('{t.DepartureTime.ToString("yyyy-MM-dd HH:mm")}')" +
                        $" AND CONVERT(char(16),LANDING_TIME,120) = ('{t.LandingTime.ToString("yyyy-MM-dd HH:mm")}')" +
                        $" AND REMANING_TICKETS = {t.RemaniningTickets};";

                        t.ID = (long)cmd2.ExecuteScalar();
                    }
                }
                catch (Exception e)
                {
                    return t.ID;
                }
                return t.ID;
                
                }
            
        }
        // get a flight from db by id
        public Flight GetFlightById(long flightID)
        {
        

            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                Flight flight = new Flight();
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights WHERE ID = {flightID}";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read() == true)
                    {

                        Flight a = new Flight
                        {
                            ID = (long)reader["ID"],
                            AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                            OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                            DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                            DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                            LandingTime = (DateTime)reader["LANDING_TIME"],
                            RemaniningTickets = (int)reader["REMANING_TICKETS"]

                        };


                        flight = a;
                    }
                }
                if(flight == null)
                {
                    return null;
                }
                return flight;
            }
        }

        // get all flights from db
        public IList<Flight> GetAll()
        {
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights";
    
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Flight a = new Flight
                            {
                                ID = (long)reader["ID"],
                                AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                                OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                                DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                                DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                                LandingTime = (DateTime)reader["LANDING_TIME"],
                                RemaniningTickets = (int)reader["REMANING_TICKETS"]

                            };


                            flights.Add(a);
                        }
                    }
                    return flights;
                }

        }
        
        // get all flights to a dictionary 
        public Dictionary<Flight, long> GetAllFlightsVacancy()
        {
          
            Dictionary<Flight, long> flightVacancy = new Dictionary<Flight, long>();
            IList<Flight> Flights = GetAll();
            foreach (Flight Flightitem in Flights)
            {
                flightVacancy.Add(Flightitem, Flightitem.RemaniningTickets);
            }
      
                return flightVacancy;
            
        }
        // get all flights from db for the same customer
        public IList<Ticket> GetFlightByCustomer(Customer customer) // inner join !!!!
        {
            IList<Ticket> tickets = new List<Ticket>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Tickets WHERE CUSTOMER_ID  = {customer.ID}";

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
        
                {
                    while (reader.Read())
                    {

                        Ticket t = new Ticket
                        {
                            ID = (long)reader["ID"],
                            FlightID = (long)reader["FLIGHT_ID"],
                            CustomerID = (long)reader["CUSTOMER_ID"]

                        };


                        tickets.Add(t);
                    }
                }
                return tickets;
            }
        }
        // get all fligths from db with the same departure date
        public IList<Flight> GetFlightByDepartureDate(DateTime departureDate)
        {
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights WHERE DEPARTURE_TIME = '{ departureDate.ToString("yyyy-MM-dd HH:mm:ss")}'";

            
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Flight a = new Flight
                        {
                            ID = (long)reader["ID"],
                            AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                            OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                            DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                            DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                            LandingTime = (DateTime)reader["LANDING_TIME"],
                            RemaniningTickets = (int)reader["REMANING_TICKETS"]

                        };


                        flights.Add(a);
                    }
                }
                return flights;
            }
        }
        // get all fligths from db with the same destination country
        public IList<Flight> GetFlightByDestinationCountry(long destinationcountry)
        {
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights WHERE DESTINATION_COUNTRY_CODE = {destinationcountry}";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Flight a = new Flight
                        {
                            ID = (long)reader["ID"],
                            AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                            OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                            DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                            DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                            LandingTime = (DateTime)reader["LANDING_TIME"],
                            RemaniningTickets = (int)reader["REMANING_TICKETS"]

                        };


                        flights.Add(a);
                    }
                }
                return flights;
            }
        }

        // get all fligths from db with the same lamding date
        public IList<Flight> GetFlightByLandingDate(DateTime landingDate)
        {
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights WHERE LANDING_TIME = '{landingDate.ToString("yyyy-MM-dd HH:mm:ss")}'";
      
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Flight a = new Flight
                        {
                            ID = (long)reader["ID"],
                            AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                            OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                            DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                            DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                            LandingTime = (DateTime)reader["LANDING_TIME"],
                            RemaniningTickets = (int)reader["REMANING_TICKETS"]

                        };


                        flights.Add(a);
                    }
                }
                return flights;
            }
        }
        // get all fligths from db that has more than 5 tickets remaining, and 10 days later from now
        public IList<Flight> GetPromotionFlight()
        {
            DateTime departureTime = DateTime.Now.AddDays(10);
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT AirlineCompanies.AIRLINE_NAME,Flights.ID, OC.COUNTRY_NAME AS 'DEPARTING FROM',DC.COUNTRY_NAME AS 'ARRIVING TO', Flights.DEPARTURE_TIME FROM Flights" +
                    $" INNER JOIN Countries AS OC ON Flights.ORIGIN_COUNTRY_CODE = oc.ID" +
                    $" INNER JOIN Countries AS DC ON Flights.DESTINATION_COUNTRY_CODE = DC.ID" +
                    $" INNER JOIN AirlineCompanies ON Flights.AIRLINE_COMPANY_ID = AirlineCompanies.ID" +
                    $" where REMANING_TICKETS >= 5" +
                    $" and DEPARTURE_TIME > '{departureTime.ToString("yyyy-MM-dd HH:mm:ss")}';";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Flight flight = new Flight();
                             flight.AirlineName = (string)reader["AIRLINE_NAME"];
                             flight.ID = (long)reader["ID"];
                             flight.OriginCountryName = (string)reader["DEPARTING FROM"];
                             flight.DestinationCountryName = (string)reader["ARRIVING TO"];
                             flight.DepartureTime = (DateTime)reader["DEPARTURE_TIME"];
                        flights.Add(flight);
                    };
         
                    
                }
                return flights;
            }
        }
        // get all flights from db with the same origin country
        public IList<Flight> GetFlightByOriginCountry(long origincountry)
        {
            IList<Flight> flights = new List<Flight>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Flights WHERE ORIGIN_COUNTRY_CODE = {origincountry}";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Flight a = new Flight
                        {
                            ID = (long)reader["ID"],
                            AirLineCompanyID = (long)reader["AIRLINE_COMPANY_ID"],
                            OriginCountryCode = (long)reader["ORIGIN_COUNTRY_CODE"],
                            DestinationCountryCode = (long)reader["DESTINATION_COUNTRY_CODE"],
                            DepartureTime = (DateTime)reader["DEPARTURE_TIME"],
                            LandingTime = (DateTime)reader["LANDING_TIME"],
                            RemaniningTickets = (int)reader["REMANING_TICKETS"]

                        };


                        flights.Add(a);
                    }
                }
                return flights;
            }
        }
        // delete a flight from db
        public void Remove(Flight t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM Tickets WHERE Tickets.FLIGHT_ID = {t.ID};" +
                $"DELETE FROM Flights WHERE ID = {t.ID};" ;

                cmd.ExecuteNonQuery();         
            }
        }
        // update a flight inside db
        public void Update(Flight t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE Flights SET AIRLINE_COMPANY_ID = {t.AirLineCompanyID}, ORIGIN_COUNTRY_CODE = {t.OriginCountryCode},"+
                    $"DESTINATION_COUNTRY_CODE = {t.DestinationCountryCode}, DEPARTURE_TIME = '{t.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss")}', LANDING_TIME = '{t.LandingTime.ToString("yyyy-MM-dd HH:mm:ss")}'," +
                    $"REMANING_TICKETS = {t.RemaniningTickets} WHERE ID = {t.ID}";

                cmd.ExecuteNonQuery();
            }
        }
        // update the remaining tickets of a flight inside db
        public void UpdateRemainingTickets(Flight flight, int numberOfTickets)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE Flights SET REMANING_TICKETS = {(flight.RemaniningTickets - numberOfTickets)} WHERE Flights.ID = {flight.ID}";

                cmd.ExecuteNonQuery();

            }
               
        }
        // get the remaining tickets of a flights inside db
        public int GetRemainingTickets(long flightID)
        {
            Flight flight = new Flight();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT REMANING_TICKETS FROM Flights WHERE ID = {flightID}";

              
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flight.RemaniningTickets = (int)reader["REMANING_TICKETS"];
                    }
                }
            }
                return flight.RemaniningTickets;
           
        }
        // get a flight from db by id 
        public Flight Get(long id)
        {
           return GetFlightById(id);
        }
        public List<Flight> GetDepartures()
        {
            List<Flight> flights = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("CHECKDEPARTURES_DATETIME", con))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())
                    {
                        while (reade.Read())
                        {
                            Flight flight = new Flight();
                            flight.AirlineName = (string)reade["AIRLINE_NAME"];
                            flight.ID = (long)reade["ID"];
                            flight.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flight.DestinationCountryName = (string)reade["ARRIVING TO"];    
                            flight.DepartureTime = (DateTime)reade["DEPARTURE_TIME"];

                            flights.Add(flight);
                        }
                    }
                }
                return flights;
            }
        }
        // get all flight from db that arrives in the next 12 hours 
        public List<Flight> GetArrivels()
        {
            List<Flight> flights = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("CHECKLANDINGTIME_DATETIME", con))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())
                    {
                        while (reade.Read())
                        {
                            Flight flight = new Flight();
                            flight.AirlineName = (string)reade["AIRLINE_NAME"];
                            flight.ID = (long)reade["ID"];
                            flight.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flight.DestinationCountryName = (string)reade["ARRIVING TO"];
                            flight.LandingTime = (DateTime)reade["LANDING_TIME"];

                            flights.Add(flight);
                        }
                    }
                }
                return flights;
            }
        }
        // get all flights from db by airline name and flight type(landing or departure)
        public List<Flight> GetAllFlightByAirlineName(string airlineName, string flightType)
        {
            List<Flight> searchInfo = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("SEARCH_FLIGHT", con))
                {
                    cmd.Parameters.Add(new SqlParameter("@search_airlineName", airlineName));
                    cmd.Parameters.Add(new SqlParameter("@search_destinationCountry", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_departureFrom", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_flightNumber", "0"));
                    cmd.Parameters.Add(new SqlParameter("@flight_Type", flightType));
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())

                    {
                        while (reade.Read())
                        {
                            Flight flightSearch = new Flight();
                            flightSearch.AirlineName = (string)reade["AIRLINE_NAME"];
                            flightSearch.ID = (long)reade["ID"];    
                            flightSearch.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flightSearch.DestinationCountryName = (string)reade["ARRIVING TO"];
                            flightSearch.DepartureTime = (DateTime)reade["DEPARTURE_TIME"];
                            flightSearch.LandingTime = (DateTime)reade["LANDING_TIME"];

                            searchInfo.Add(flightSearch);
                        }

                    }
                }
            }

                        return searchInfo;
        }
        // get all flights from db by destination country and flight type(landing or departure)
        public List<Flight> GetAllFlightByDestinationCountry(string countryName, string flightType)
        {
            List<Flight> searchInfo = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("SEARCH_FLIGHT", con))
                {
                    cmd.Parameters.Add(new SqlParameter("@search_airlineName", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_destinationCountry", countryName));
                    cmd.Parameters.Add(new SqlParameter("@search_departureFrom", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_flightNumber", "0"));
                    cmd.Parameters.Add(new SqlParameter("@flight_Type", flightType));
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())

                    {
                        while (reade.Read())
                        {
                            Flight flightSearch = new Flight();
                            flightSearch.AirlineName = (string)reade["AIRLINE_NAME"];
                            flightSearch.ID = (long)reade["ID"];
                            flightSearch.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flightSearch.DestinationCountryName = (string)reade["ARRIVING TO"];
                            flightSearch.DepartureTime = (DateTime)reade["DEPARTURE_TIME"];
                            flightSearch.LandingTime = (DateTime)reade["LANDING_TIME"];

                            searchInfo.Add(flightSearch);
                        }

                    }
                }
            }

            return searchInfo;
        }
        // get all flights from db by origin country and flight type(landing or departure)
        public List<Flight> GetAllFlightByOriginCountry(string countryName, string flightType)
        {
            List<Flight> searchInfo = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("SEARCH_FLIGHT", con))
                {
                    cmd.Parameters.Add(new SqlParameter("@search_airlineName", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_destinationCountry", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_departureFrom", countryName));
                    cmd.Parameters.Add(new SqlParameter("@search_flightNumber", "0"));
                    cmd.Parameters.Add(new SqlParameter("@flight_Type", flightType));
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())

                    {
                        while (reade.Read())
                        {
                            Flight flightSearch = new Flight();
                            flightSearch.AirlineName = (string)reade["AIRLINE_NAME"];
                            flightSearch.ID = (long)reade["ID"];
                            flightSearch.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flightSearch.DestinationCountryName = (string)reade["ARRIVING TO"];
                            flightSearch.DepartureTime = (DateTime)reade["DEPARTURE_TIME"];
                            flightSearch.LandingTime = (DateTime)reade["LANDING_TIME"];

                            searchInfo.Add(flightSearch);
                        }

                    }
                }
            }

            return searchInfo;
        }
        // get all flights from db by fligth id and flight type(landing or departure)
        public List<Flight> GetAllFlightById(string flightNamber, string flightType)
        {
            List<Flight> searchInfo = new List<Flight>();
            using (SqlConnection con = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                using (SqlCommand cmd = new SqlCommand("SEARCH_FLIGHT", con))
                {
                    cmd.Parameters.Add(new SqlParameter("@search_airlineName", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_destinationCountry", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_departureFrom", "null"));
                    cmd.Parameters.Add(new SqlParameter("@search_flightNumber", flightNamber));
                    cmd.Parameters.Add(new SqlParameter("@flight_Type", flightType));
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    using (SqlDataReader reade = cmd.ExecuteReader())

                    {
                        while (reade.Read())
                        {
                            Flight flightSearch = new Flight();
                            flightSearch.AirlineName = (string)reade["AIRLINE_NAME"];
                            flightSearch.ID = (long)reade["ID"];
                            flightSearch.OriginCountryName = (string)reade["DEPARTING FROM"];
                            flightSearch.DestinationCountryName = (string)reade["ARRIVING TO"];
                            flightSearch.DepartureTime = (DateTime)reade["DEPARTURE_TIME"];
                            flightSearch.LandingTime = (DateTime)reade["LANDING_TIME"];

                            searchInfo.Add(flightSearch);
                        }

                    }
                }
            }

            return searchInfo;
        }
    }
}
