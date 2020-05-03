using System;
using System.Collections.Generic;
using WorkMyFlight;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightTest
{
    // test 5 options from AirlineFacade
    [TestClass]
    public class AirlineFacadeTest
    {
        TestInfo test;
        [TestMethod]
        public void TestCreateFlight()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            Assert.AreEqual(flight.AirLineCompanyID, test.airlineF.GetFlightById(flight.ID).AirLineCompanyID);
        }
        [TestMethod]
        public void TestCancelFlight()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            test.airlineF.CancelFlight(test.airlineT, flight);
            Assert.AreNotEqual(test.airlineF.GetFlightById(flight.ID), flight);
        }
        [TestMethod]
        public void TestChangePassowrd()
        {
            test = new TestInfo();
            test.airlineF.ChangeMyPassword(test.airlineT, test.airlineT.User.Password, "123456");
            Assert.AreEqual(test.airlineT.User.Password, "123456");
        }
        [TestMethod]
        public void TestGetAllflightsAirlineFacade()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            IList<Flight> flights =  test.airlineF.GetAllFlights();
            Assert.AreEqual(flights.Count, 1);
        }
        [TestMethod]
        public void TestUpdadteFlight()
        {
            test = new TestInfo();
            Country testCountry1 = new Country("usa");
            Country testCountry2 = new Country("italy");
            testCountry1.ID = test.adminF.CreateNewCountry(test.adminT, testCountry1);
            testCountry2.ID = test.adminF.CreateNewCountry(test.adminT, testCountry2);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry1.ID, testCountry2.ID, DateTime.ParseExact("2019-07-08 07:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 17:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            flight.ID = test.airlineF.CreateFlight(test.airlineT, flight);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.RemaniningTickets = 4;
            Assert.AreNotEqual(Fdao.Get(flight.ID).RemaniningTickets, flight.RemaniningTickets);
            test.airlineF.UpdateFlight(test.airlineT, flight);
            Assert.AreEqual(Fdao.Get(flight.ID).RemaniningTickets, flight.RemaniningTickets);

        }
    }
}
