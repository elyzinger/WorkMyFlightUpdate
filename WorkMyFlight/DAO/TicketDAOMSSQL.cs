using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class TicketDAOMSSQL : ITicketDAO
    {
        private static object key = new object();
        public static SqlCommand cmd = new SqlCommand();
        // checking if exist and inserting a new ticket to db
        public long ADD(Ticket t)
        {
            SqlCommand cmd2 = new SqlCommand();
      
            lock (key)
            {

                {
                    using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd.Connection.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"SELECT COUNT(*) FROM Tickets WHERE FLIGHT_ID = {t.FlightID} AND CUSTOMER_ID = {t.CustomerID}";
                        string res = cmd.ExecuteScalar().ToString();
                        if (res != "0")
                            throw new AlreadyExistException($"Ticket for customer id {t.CustomerID} already exists");
                        cmd.Connection.Close();

                    }
                    using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                    {
                        cmd2.Connection.Open();
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = $"INSERT INTO Tickets(FLIGHT_ID, CUSTOMER_ID) values({ t.FlightID}, { t.CustomerID});"+
                        $"SELECT ID FROM Tickets WHERE FLIGHT_ID = {t.FlightID} AND CUSTOMER_ID = {t.CustomerID}";

                        t.ID = (long)cmd2.ExecuteScalar();
                    }
                    return t.ID;
                }
            }
        }
        //get ticket from db by id 
        public Ticket Get(long id)
        {
            Ticket ticket = new Ticket();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Tickets WHERE ID = {id}";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Ticket a = new Ticket
                        {
                            ID = (long)reader["ID"],
                            FlightID = (long)reader["FLIGHT_ID"],
                            CustomerID = (long)reader["CUSTOMER_ID"]
                        };


                        ticket = a;
                    }
                }
                return ticket;
            }
        }
        // get all tickets from db
        public IList<Ticket> GetAll()
        {
            IList<Ticket> alltickets = new List<Ticket>();
            Ticket ticket = new Ticket();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Tickets";
            
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Ticket a = new Ticket
                        {
                            ID = (long)reader["ID"],   
                            FlightID = (long)reader["FLIGHT_ID"],
                            CustomerID = (long)reader["CUSTOMER_ID"]
                        };


                        alltickets.Add(a);
                    }
                }
                return alltickets;
            }
        }
        //get all tickets from db with the same airline id
        public IList<Ticket> GetTicketByAirlineID(long airLineCompanyID)
        {
            IList<Ticket> airlineTickets = new List<Ticket>();
            Ticket ticket = new Ticket();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Tickets WHERE FLIGHT_ID IN (SELECT ID FROM Flights WHERE Flights.AIRLINE_COMPANY_ID = {airLineCompanyID})";
             
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Ticket a = new Ticket
                        {
                            ID = (long)reader["ID"],
                            FlightID = (long)reader["FLIGHT_ID"],
                            CustomerID = (long)reader["CUSTOMER_ID"]
                        };


                        airlineTickets.Add(a);
                    }
                }
                return airlineTickets;
            }
        }

        // delete a ticket from db
        public void Remove(Ticket t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM Tickets WHERE ID = {t.ID}";

                cmd.ExecuteNonQuery();

            }
        }
        // update a ticket in db
        public void Update(Ticket t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE Tickets SET FLIGHT_ID = {t.FlightID}, CUSTOMER_ID = {t.CustomerID}";

                cmd.ExecuteNonQuery();

            }
        }
    }
}
