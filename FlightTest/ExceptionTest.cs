using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkMyFlight;

namespace FlightTest
{
    // test for exception
    [TestClass]
    public class ExceptionTest
    {
        TestInfo test;
        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void TestUserNotFoundException()
        {
            FlyingCenterSystem fc = FlyingCenterSystem.GetInstance();
            fc.Login("gerger", "fwefwe");
       
        }
        [TestMethod]
        [ExpectedException(typeof(NoMoreTicketsException))]
        public void TestNoMoreTicketsException()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            Country testCountry2 = new Country("russia");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            testCountry2.ID = test.adminF.CreateNewCountry(test.adminT, testCountry2);
            AirLineCompany a = new AirLineCompany("amrican", "amrican", "12345", testCountry.ID);
            a.ID = test.adminF.CreateNewAirline(test.adminT, a);
            Flight flight = new Flight(a.ID, testCountry.ID, testCountry2.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 0);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);          
            test.customerF.PurchaseTicket(test.customerT, flight);
        }
        [TestMethod]
        [ExpectedException(typeof(AlreadyExistException))]
        public void TestAlreadyExistException()
        {
            test = new TestInfo();
            test.FC.Login(test.CreateNewTestCustomer().UserName, "123");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongPasswordException))]
        public void TestWrongPasswordException()
        {
            test = new TestInfo();
            CustomerDAOMSSQL cDAO = new CustomerDAOMSSQL();
            Customer c = new Customer("jon", "dow", "james", "4567", "alkana", "054555440", "321456");
            long id = test.adminF.CreateNewCustomer(test.adminT, c);
           test.FC.Login(c.UserName, "123");
        }

    }
}

