using System;
using System.Collections.Generic;
using WorkMyFlight;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightTest
{
    // test 3 options from CustomerFacade
    [TestClass]
    public class CustomerFacadeTest
    {
        TestInfo test;
        [TestMethod]
        public void TestPurchaseTicket()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            Country testCountry2 = new Country("russia");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            testCountry2.ID = test.adminF.CreateNewCountry(test.adminT, testCountry2);
            AirLineCompany a = new AirLineCompany("amrican", "amrican", "12345", testCountry.ID);
            a.ID = test.adminF.CreateNewAirline(test.adminT, a);
            Flight flight = new Flight(a.ID, testCountry.ID, testCountry2.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            test.customerF.PurchaseTicket(test.customerT, flight);            
            Assert.AreEqual(Fdao.GetRemainingTickets(flight.ID), 4);
        }
        [TestMethod]
        public void TestGetAllMyFlights()
        {
            
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 08:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 11:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            test.airlineF.CreateFlight(test.airlineT, flight);
            Assert.AreEqual(test.customerF.GetAllMyFlights(test.customerT).Count, 0);
            test.customerF.PurchaseTicket(test.customerT, flight);
            Assert.AreEqual(test.customerF.GetAllMyFlights(test.customerT).Count, 1);
        }
        [TestMethod]
        public void TestCancelTicket()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            Ticket t = test.customerF.PurchaseTicket(test.customerT, flight);
            TicketDAOMSSQL tDAO = new TicketDAOMSSQL();
            test.customerF.CancelTicket(test.customerT, t);
            Assert.AreEqual(0, tDAO.Get(t.ID).ID);
        }
      
    }
}
