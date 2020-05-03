using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class FlyingCenterSystem
    {
        static SqlCommand cmd = new SqlCommand();
        static private FlyingCenterSystem _instance;
        private static object key = new object();
         
        public FlyingCenterSystem()
        {
            new Task(() =>
            {
                Move3HoursOldFlightsAndTickets();
                Thread.Sleep(FlightCenterConfig.historyTimer);
            });
        }

        static public FlyingCenterSystem GetInstance()
        {
            if (_instance == null)
            {
                lock (key)
                {
                    if (_instance == null)
                    {
                        _instance = new FlyingCenterSystem();
                    }
                }
            }
            return _instance;
        }

        public ILoginToken Login(string userName, string Password)
        {
            LoginService loginService = new LoginService();

            if (loginService.TryAdminLogin(userName, Password, out LoginToken<Administrator> AdminToken))
            {
                return AdminToken;
            }
            else if (loginService.TryAirLineLogin(userName, Password, out LoginToken<AirLineCompany> AirlineCompanyToken))
            {
                return AirlineCompanyToken;
            }
            else if (loginService.TryCustomerLogin(userName, Password, out LoginToken<Customer> CustomerToken))
            {
                return CustomerToken;
            }
             throw new UserNotFoundException("user not found"); ;
        }

        public IFacade GetFacade(ILoginToken loginToken)
        {
            // IloginToken is null - > user is Anonymous
            if (loginToken == null)
            {
                return new AnonymousUserFacade();
            }

            if (loginToken.GetType() == typeof(LoginToken<Administrator>))
            {
                return new LoggedInAdministratorFacade();
            }
             if (loginToken.GetType() == typeof(LoginToken<AirLineCompany>))
            {
                return new LoggedInAirlineFacade();
            }
             if (loginToken.GetType() == typeof(LoginToken<Customer>))
            {
                return new LoggedInCustomerFacade();
            }
            // if no other option user is Anonymous
            return new AnonymousUserFacade();

        }


        public void Move3HoursOldFlightsAndTickets()
        {
            //find the time 3 hours ago and move to history all tickets and flights           
            DateTime time3HoursAgo = DateTime.Now.Subtract(DateTime.Now.AddHours(3) - DateTime.Now);
            string threeHoursAgo = time3HoursAgo.ToString("yyyy-MM-dd HH:mm:ss");

            using (cmd.Connection = new SqlConnection(FlightCenterConfig.DAO_CON))
            {
                cmd.Connection.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"INSERT INTO TicketHistory SELECT * FROM Tickets " +
                $"WHERE FLIGHT ID IN (SELECT ID FROM Flights WHERE LANDING_TIME < '{threeHoursAgo}')" +
                $"ND Tickets.ID NOT IN(SELECT ID FROM TicketHistory); "+
                $"INSERT INTO FlightHistory SELECT * FROM Flights "+
                $"WHERE LANDING_TIME < '{threeHoursAgo}')" +
                $"AND ID NOT IN(SELECT ID FROM FlightHistory);" +
                $"DELETE FROM Tickets "+
                $"WHERE FLIGHT ID IN(SELECT ID FROM Flights WHERE LANDING_TIME < '{threeHoursAgo}')" +
                $"AND Tickets.ID NOT IN(SELECT ID FROM TicketHistory); " +
                $"DELETE FROM Flights "+
                $"WHERE LANDING_TIME < '{threeHoursAgo}')" +
                $"AND ID NOT IN(SELECT ID FROM FlightHistory);";
                cmd.ExecuteNonQuery();
            }
        }

    }
}
