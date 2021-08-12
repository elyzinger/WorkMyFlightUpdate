using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    
    public class AirlineDAOMSSQL : IAirlineDAO
    {
        private static object key = new object();
        public static SqlCommand cmd = new SqlCommand();
        // checking if exist and inserting a new airline to db
        public long ADD(AirLineCompany t)
        {
            SqlCommand cmd2 = new SqlCommand();
            
            lock (key)
            {
                
                {
                  
                        // connecting to sql
                        using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                        {
                            cmd.Connection.Open();
                            cmd.CommandType = CommandType.Text;
                            // sql query
                            cmd.CommandText = $"SELECT COUNT(*) FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}' OR EXISTS (SELECT USER_NAME FROM Customers WHERE USER_NAME = '{t.UserName}')";
                            //return a string back
                            string res = cmd.ExecuteScalar().ToString();
                            if (res != "0")
                                throw new AlreadyExistException($"AirlineCompany user name {t.UserName} already exists");
                            cmd.Connection.Close();

                        }
                        using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                        {
                            cmd2.Connection.Open();
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandText = $"INSERT INTO AirlineCompanies(AIRLINE_NAME, USER_NAME, PASSWORD, COUNTRY_CODE, EMAIL)" +
                                $"values('{ t.AirLineName}', '{ t.UserName}', '{ t.Password}', { t.CountryCode}, '{t.Email}');" +
                            $"SELECT ID FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}'";
                            // return back as a number 
                            t.ID = (long)cmd2.ExecuteScalar();
                        }
                    
                 
                    
                    return t.ID;   
                }
            }
        }

        // get a airline from db by id
        public AirLineCompany Get(long id)
        {
            AirLineCompany airLineCompany = new AirLineCompany();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM AirlineCompanies WHERE ID = {id}";
                
                //execute reader return data back 
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
       
                        AirLineCompany a = new AirLineCompany
                        {
                            ID = (long)reader["ID"],
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"]
                            
                        };


                        airLineCompany = a;
                    }
                }
                return airLineCompany;
            }
        }
        // get a airline from db by username
        public AirLineCompany GetAirlineByUsername(string username)
        {
            AirLineCompany airLineCompany = null;
           
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM AirlineCompanies WHERE USER_NAME = '{username}'";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        AirLineCompany a = new AirLineCompany
                        {
                            ID = (long)reader["ID"],
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"],
                            Email = (string)reader["EMAIL"]   

                        };


                        airLineCompany = a;
                    }
                    if(airLineCompany == null)
                    {
                        return null;
                    }
                    return airLineCompany;
                }
                
            }
        }
        //  adding a new airline to db before email authentication before adding as a customer
        public bool AddNewAirlineDB(AirLineCompany newAirline)
        {

            bool wasAdded = true;
            string emailConfirmed = "false";


            lock (key)
            {

                {
                    using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                      
                            cmd.Connection.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = $"INSERT INTO AirlineSignUp(AIRLINE_NAME, USER_NAME, PASSWORD, COUNTRY_CODE, EMAIL , REG_DATE) values ('{newAirline.AirLineName}', '{newAirline.UserName}', '{newAirline.Password}','{newAirline.CountryCode}','{newAirline.Email}','{newAirline.RegDate.ToString("yyyy - MM - dd HH: mm:ss.fff")}' )";
                            cmd.ExecuteNonQuery();
                        
                        
                    }

                    return wasAdded;
                }
            }
        }
        // get all airline from db
        public IList<AirLineCompany> GetAll()
        {
    
            IList<AirLineCompany> airLineCompanys = new List<AirLineCompany>();
            using (cmd.Connection  = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM AirlineCompanies";
                
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
             
                {
                    while (reader.Read())
                    {

                        AirLineCompany airLineCompany = new AirLineCompany
                        {
                            ID = (long)reader["ID"],
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"],
                            Email = (string)reader["EMAIL"]

                        };


                        airLineCompanys.Add(airLineCompany);
                    }
                }
                return airLineCompanys;
            }
        }
        public IList<AirLineCompany> GetAllSignedUpAirlineCompaniesFromDB()
        {

            IList<AirLineCompany> airLineCompanys = new List<AirLineCompany>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM AirlineSignUp";

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))

                {
                    while (reader.Read())
                    {

                        AirLineCompany airLineCompany = new AirLineCompany
                        {
                            
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"],
                            Email = (string)reader["EMAIL"],                    
                            RegDate = (DateTime)reader["REG_DATE"]

                        };


                        airLineCompanys.Add(airLineCompany);
                    }
                }
                return airLineCompanys;
            }
        }

        // get all airline from db with the same country id
        public IList<AirLineCompany> GetAllAirlineByCountry(long countryid)
        {
            IList<AirLineCompany> airLineCompanys = new List<AirLineCompany>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM AirlineCompanies WHERE COUNTRY_CODE = {countryid} ";
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {

                        AirLineCompany airLineCompany = new AirLineCompany
                        {
                            ID = (long)reader["ID"],
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"]

                        };


                        airLineCompanys.Add(airLineCompany);
                    }
                }
                return airLineCompanys;
            }
        }
        // delete airline from SIGNUP db 
        public void RemoveSignedUpAirline(AirLineCompany airlinecompany)
        {

            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM AirlineSignUp WHERE (AIRLINE_NAME = '{airlinecompany.AirLineName}')";
               

                cmd.ExecuteNonQuery();
            }
        }
        // delete airline from db 
        public void Remove(AirLineCompany airlinecompany)
        {
            
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM Tickets WHERE Tickets.FLIGHT_ID = (select Flights.ID from Flights WHERE Flights.AIRLINE_COMPANY_ID = { airlinecompany.ID});" +
                $"DELETE FROM Flights WHERE Flights.AIRLINE_COMPANY_ID = {airlinecompany.ID};" +
                $"DELETE FROM AirlineCompanies WHERE (ID = {airlinecompany.ID});";

                cmd.ExecuteNonQuery();
            }
            }
        // update airline from in db
        public void Update(AirLineCompany t)
        {
            //SqlCommand cmd2 = new SqlCommand();
            //using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            //{
            //    cmd.Connection.Open();
            //    cmd.CommandType = CommandType.Text;
            //    // sql query
            //    cmd.CommandText = $"SELECT COUNT(*) FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}' OR EXISTS (SELECT USER_NAME FROM Customers WHERE USER_NAME = '{t.UserName}')";
            //    //return a string back
            //    string res = cmd.ExecuteScalar().ToString();
            //    if (res != "1" && res != "0")
            //        throw new AlreadyExistException($"AirlineCompany user name {t.UserName} already exists");
            //    cmd.Connection.Close();

            //}
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE AirlineCompanies SET AIRLINE_NAME = '{t.AirLineName}', USER_NAME = '{t.UserName}', PASSWORD = '{t.Password}', COUNTRY_CODE = {t.CountryCode}, EMAIL ='{t.Email}'" +  
                $" WHERE ID = {t.ID} ";
                cmd.ExecuteNonQuery();

            }
        }
        public IList<AirLineCompany> GetAirlineSignInList(long countryid)
        {
            IList<AirLineCompany> airLineCompanys = new List<AirLineCompany>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM SignUp WHERE TYPE = {"airline"} ";
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    while (reader.Read())
                    {

                        AirLineCompany airLineCompany = new AirLineCompany
                        {
                            ID = (long)reader["ID"],
                            AirLineName = (string)reader["AIRLINE_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            CountryCode = (long)reader["COUNTRY_CODE"]

                        };


                        airLineCompanys.Add(airLineCompany);
                    }
                }
                return airLineCompanys;
            }
        }
    }
}
