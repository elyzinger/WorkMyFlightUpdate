using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlightRestWeb.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        //public ActionResult Index()
        //{
        //    //return View();
        //    return new FilePathResult("~/Views/Page/departures.html", "text/html");
        //}
        // GET: GetDeparturesFlights
        public ActionResult GetDeparturesFlights()
        {
            return new FilePathResult("~/Views/Page/departures.html", "text/html");
        }
        // GET: GetLandingFlights
        //public ActionResult GetLandingFlights()
        //{
        //    return new FilePathResult("~/Views/Page/landing.html", "text/html");
        //}
        //// GET: GetLandingFlights
        public ActionResult SearchFlights()
        {
            return new FilePathResult("~/Views/Page/search.html", "text/html");
        }
    }
}