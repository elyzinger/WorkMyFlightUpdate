using System;
using WorkMyFlight;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightTest
{
    // test 4 options from AdministratorFacade
    [TestClass]
    public class AdministratorFacadeTest
    {
        TestInfo test;
        [TestMethod]
        public void TestCreateAirline()
        {
            test = new TestInfo();
            Assert.AreEqual("elal", test.airlineT.User.UserName);
        }
        [TestMethod]
        public void TestCreateCustomer()
        {
            test = new TestInfo();
            Assert.AreEqual("jon", test.customerT.User.UserName);
        }
        [TestMethod]
        public void TestRemoveCustomer()
        {
            test = new TestInfo();
            Customer testCustomer = new Customer("dan", "horesh", "dan", "2345", "alkana", "054555440", "321456");
          testCustomer.ID =  test.adminF.CreateNewCustomer(test.adminT, testCustomer);
            CustomerDAOMSSQL cusDAO = new CustomerDAOMSSQL();
            Assert.AreEqual(testCustomer.ID, cusDAO.Get(testCustomer.ID).ID );
        }
        [TestMethod]
        public void TestUpdateCustomer()
        {
            test = new TestInfo();
            Customer customer = new Customer("rami", "lavy", "rami", "1234", "alkana", "054555440", "321456");
            customer.ID = test.adminF.CreateNewCustomer(test.adminT, customer);
            CustomerDAOMSSQL customerDAO = new CustomerDAOMSSQL();
            customer.FirstName = "levi";
            Assert.AreNotEqual(customer.FirstName , customerDAO.Get(customer.ID).FirstName);
            test.adminF.UpdateCustomerDetails(test.adminT, customer);
            Assert.AreEqual(customer.FirstName, customerDAO.Get(customer.ID).FirstName);


        }
    }
}
