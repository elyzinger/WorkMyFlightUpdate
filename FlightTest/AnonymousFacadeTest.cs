using System;
using System.Collections.Generic;
using WorkMyFlight;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightTest
{
    // test 4 options from AnonymousFacade
    [TestClass]
    public class AnonymousFacadeTest
    {
        TestInfo test;
        [TestMethod]
        public void TestGetFlightByID()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);          
            TicketDAOMSSQL tDAO = new TicketDAOMSSQL();
            Assert.AreEqual(flight.RemaniningTickets, test.airlineF.GetFlightById(flight.ID).RemaniningTickets);

        }
        [TestMethod]
        public void TestGetAllFlightsAnonymousFacade()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);      
            IList <Flight> flights =  test.anonymousF.GetAllFlights();
            Assert.AreEqual(flights.Count, 1);

        }
        [TestMethod]
        public void TestGetAllAirlineCompanies()
        {
            test = new TestInfo();
           IList<AirLineCompany> companies =  test.anonymousF.GetAllAirlineCompanies();
            Assert.AreEqual(companies.Count, 1);
        }
        [TestMethod]
        public void TestGetFlightByDepartureDate()
        {
            test = new TestInfo();
            Country testCountry = new Country("usa");
            testCountry.ID = test.adminF.CreateNewCountry(test.adminT, testCountry);
            Flight flight = new Flight(test.airlineT.User.ID, testCountry.ID, testCountry.ID, DateTime.ParseExact("2019-07-08 12:00:00", "yyyy-MM-dd HH:mm:ss", null), DateTime.ParseExact("2019-07-18 12:00:00", "yyyy-MM-dd HH:mm:ss", null), 5);
            FlightDAOMSSQL Fdao = new FlightDAOMSSQL();
            flight.ID = Fdao.ADD(flight);
            IList<Flight>flights = test.anonymousF.GetFlightsByDepatrureDate(flight.DepartureTime);
            Assert.AreEqual(flights.Count, 1);
        }
        
    }
}
