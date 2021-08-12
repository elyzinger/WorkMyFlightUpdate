using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkMyFlight.POCO;

namespace WorkMyFlight
{
    public class CustomerDAOMSSQL : ICustomerDAO
    {
        public static SqlCommand cmd = new SqlCommand();
        private static object key = new object();
        // checking if exist and inserting a new customer to db
        public long ADD(Customer t)
        {
            SqlCommand cmd2 = new SqlCommand();
            
            lock (key)
            {

                {
                    
                        using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                        {
                            cmd.Connection.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = $"SELECT COUNT(*) FROM Customers WHERE USER_NAME = '{t.UserName}' OR EXISTS (SELECT USER_NAME FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}')";
                            string res = cmd.ExecuteScalar().ToString();
                            if (res != "0")
                                throw new AlreadyExistException($"Customers user name {t.UserName} already exists");
                            cmd.Connection.Close();

                        }
                        using (cmd2.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
                        {
                            cmd2.Connection.Open();
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandText = $"INSERT INTO Customers(FIRST_NAME, LAST_NAME, USER_NAME, PASSWORD, ADDRESS, PHONE_NUMBER, CREDIT_CARD_NUMBER, EMAIL )" +
                            $"values('{ t.FirstName}', '{ t.LastName}', '{ t.UserName}', '{ t.Password}','{ t.Address}', '{ t.PhoneNumber}', '{ t.CreditCardNumber}', '{t.Email}');" +
                            $"SELECT ID FROM Customers WHERE USER_NAME = '{t.UserName}'";

                            t.ID = (long)cmd2.ExecuteScalar();
                        }
                    return t.ID;
                    
                }
            }
        }
        // adding a new customer to db before email authentication before adding as a customer 
        public bool AddNewCustomerDB(Customer newCustomer)
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
                        cmd.CommandText = $"INSERT INTO SignUp(USER_NAME, FIRST_NAME, LAST_NAME, PASSWORD, ADDRESS, PHONE_NUMBER, CREDIT_CARD_NUMBER, EMAIL, GUID) values ('{newCustomer.UserName}', '{newCustomer.FirstName}', '{newCustomer.LastName}', '{emailConfirmed}','{newCustomer.Password}','{newCustomer.Address}'),'{newCustomer.PhoneNumber}','{newCustomer.CreditCardNumber}','{newCustomer.Email}','{newCustomer.Password}','{newCustomer.Guide}'";
                        cmd.ExecuteNonQuery();
                    }

                    return wasAdded;
                }
            }
        }
    
        // adding a new user to db after email authentication before adding as a customer 
        public ConfirmedUser ConfirmEmail(string guid)
        {
            ConfirmedUser confirmedUser = new ConfirmedUser();
              //string userName = null;
              //string confirmed = null;
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM SignUp WHERE GUID = '{guid}'";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ConfirmedUser a = new ConfirmedUser
                        {
                            UserName = (string)reader["USER_NAME"],
                            Email = (string)reader["EMAIL"],
                            Type = (string)reader["TYPE"],
                            EmailCon = (string)reader["EMAIL_CON"],
                            Guid = (string)reader["GUID"]
                        };
                        confirmedUser = a;
                    }
                }
                if (confirmedUser.EmailCon == "true")
                {
                    confirmedUser.EmailCon = "was already confirmed";
                    return confirmedUser;
                }
                if (confirmedUser.EmailCon == "false")
                {
                    cmd.Connection.Close();
                    cmd.Connection.Open();
                    cmd.CommandText = $"UPDATE SignUp SET EMAIL_CON = 'true' where USER_NAME = '{confirmedUser.UserName}'";
                    cmd.ExecuteNonQuery();
                }
            }

            return confirmedUser;
        }
        // get customer from db by id
        public Customer Get(long id)
        {
            Customer customer = new Customer();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Customers WHERE(ID = {id})";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {
                            ID = (long)reader["ID"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"]

                        };


                        customer = a;
                    }
                }
                return customer;
            }
        }
        // get all customers from db
        public IList<Customer> GetAll()
        {
            IList<Customer> customers = new List<Customer>();
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Customers";

                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))

                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {
                            ID = (long)reader["ID"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"],
                            Email = (string)reader["EMAIL"]

                        };


                        customers.Add(a);
                    }
                }
                return customers;
            }
        }
        // get customer from db by username
        public Customer GetCustomerByUserName(string username)
        {
            Customer customer = null;
           
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT * FROM Customers WHERE (USER_NAME = '{username}')";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Customer a = new Customer
                        {

                            ID = (long)reader["ID"],
                            UserName = (string)reader["USER_NAME"],
                            Password = (string)reader["PASSWORD"],
                            FirstName = (string)reader["FIRST_NAME"],
                            LastName = (string)reader["LAST_NAME"],
                            Address = (string)reader["ADDRESS"],
                            PhoneNumber = (string)reader["PHONE_NUMBER"],
                            CreditCardNumber = (string)reader["CREDIT_CARD_NUMBER"],
                            Email = (string)reader["EMAIL"]
                        };
                        customer = a;
                    }
                    
                }
            }
            if(customer == null)
            {
                return null;
            }
            return customer;


        }
        
        // delete a customer from db
        public void Remove(Customer t)
        {
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"DELETE FROM Tickets WHERE Tickets.CUSTOMER_ID = {t.ID};" +
                    $"DELETE FROM Customers WHERE (ID = {t.ID} );";

                cmd.ExecuteNonQuery();
            }
        }
        // update a customer inside db
        public void Update(Customer t)
        {
            //SqlCommand cmd2 = new SqlCommand();
            //using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            //{
            //    cmd.Connection.Open();
            //    cmd.CommandType = CommandType.Text;
            //    cmd.CommandText = $"SELECT COUNT(*) FROM Customers WHERE USER_NAME = '{t.UserName}' OR EXISTS (SELECT USER_NAME FROM AirlineCompanies WHERE USER_NAME = '{t.UserName}')";
            //    string res = cmd.ExecuteScalar().ToString();
            //    if (res != "1")
            //        throw new AlreadyExistException($"Customers user name {t.UserName} already exists");
            //    cmd.Connection.Close();

            //}
            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"UPDATE Customers SET FIRST_NAME = '{t.FirstName}',LAST_NAME = '{t.LastName}',USER_NAME = '{t.UserName}',PASSWORD = '{t.Password}',ADDRESS = '{t.Address}',PHONE_NUMBER = '{t.PhoneNumber}',CREDIT_CARD_NUMBER = '{t.CreditCardNumber}',EMAIL= '{t.Email}' WHERE ID = {t.ID} ";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
