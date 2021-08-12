using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class CountryDAOMSSQL : ICountryDAO
    {
        private static object key = new object();
        static SqlCommand cmd = new SqlCommand();
        // checking if exist and inserting a new country to db
        public long ADD(Country t)
        {

            SqlCommand cmd2 = new SqlCommand();

            {
                try
                {
                    using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd.Connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"SELECT  COUNT(*) FROM Countries WHERE COUNTRY_NAME = '{t.CountryName}'";

                        string res = cmd.ExecuteScalar().ToString();
                        if (res != "0")
                            throw new AlreadyExistException($"Country name {t.CountryName} already exists");
                        cmd.Connection.Close();

                    }
                    using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd2.Connection.Open();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = $"INSERT INTO Countries(COUNTRY_NAME) values('{ t.CountryName}');" +
                        $"SELECT ID FROM Countries WHERE COUNTRY_NAME = '{t.CountryName}'";

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
            // get a country from db by id
            public Country Get(long id)
            {
                Country country = new Country();
                using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"SELECT * FROM Countries WHERE(ID = {id})";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Country a = new Country
                            {
                                ID = (long)reader["ID"],
                                CountryName = (string)reader["COUNTRY_NAME"],
                            };


                            country = a;
                        }
                    }
                    return country;
                }
            }
            // get a country from db by name
            public Country Get(string countryName)
            {
                Country country = new Country();
                using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"SELECT * FROM Countries WHERE(COUNTRY_NAME = '{countryName}')";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Country a = new Country
                            {
                                ID = (long)reader["ID"],
                                CountryName = (string)reader["COUNTRY_NAME"],
                            };


                            country = a;
                        }
                    }
                    return country;
                }
            }
            // get all countries from db
            public IList<Country> GetAll()
            {
                IList<Country> countries = new List<Country>();
                using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"SELECT * FROM Countries";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Country a = new Country
                            {
                                ID = (long)reader["ID"],
                                CountryName = (string)reader["COUNTRY_NAME"],
                            };


                            countries.Add(a);
                        }
                    }
                    return countries;
                }
            }
            // delete a country from db
            public void Remove(Country t)
            {

                using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"delete from Tickets WHERE Tickets.FLIGHT_ID in" +
                    $"(select Flights.ID from Flights WHERE Flights.AIRLINE_COMPANY_ID in" +
                    $"(select AirlineCompanies.ID from AirlineCompanies where AirlineCompanies.COUNTRY_CODE = {t.ID}));" +
                    $"delete from Flights where Flights.AIRLINE_COMPANY_ID in" +
                    $"(select AirlineCompanies.ID from AirlineCompanies WHERE AirlineCompanies.COUNTRY_CODE = {t.ID});" +
                    $"delete from AirlineCompanies where  AirlineCompanies.COUNTRY_CODE = {t.ID});" +
                    $"delete from Countries where Countries.ID = {t.ID});";

                    cmd.ExecuteNonQuery();

                }


            }
            // update a country in db
            public void Update(Country t)
            {
                using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                {
                    cmd.Connection.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = $"UPDATE Countries SET COUNTRY_NAME = '{t.CountryName}' WHERE ID = {t.ID}";

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
