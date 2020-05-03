using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight.POCO
{
    public class SearchParam : IPoco
    {
        public string ID { get; set; }
        public string AirlineName  { get; set; }
        public string DesCountry { get; set; }
        public string OriCountry { get; set; }
        public string FlightType { get; set; }
        public string Selected { get; set; }

    }
}
