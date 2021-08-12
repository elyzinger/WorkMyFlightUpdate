﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkMyFlightWeb.Controllers
{
    public class PageController : Controller
    {
        //GET: Page
        public ActionResult Index()
        {
            //return View();
            return new FilePathResult("~/Views/Page/departures.html", "text/html");
        }
        // GET: GetDeparturesFlights
        public ActionResult GetDeparturesFlights()
        {
            return new FilePathResult("~/Views/Page/departures.html", "text/html");
        }
        // GET: GetLandingFlights
        public ActionResult GetLandingFlights()
        {
            return new FilePathResult("~/Views/Page/landing.html", "text/html");
        }
        // GET: get search page
        public ActionResult SearchFlights()
        {
            return new FilePathResult("~/Views/Page/search.html", "text/html");
        }
        // GET: get angular page
        public ActionResult Angular()
        {
            return new FilePathResult("~/Views/Page/angular.html", "text/html");     
        }
    }
}