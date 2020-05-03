using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class Flight : IPoco
    {
        public long ID { get; set; }
        public long AirLineCompanyID { get; set; }
        public string AirlineName { get; set; }
        public long OriginCountryCode { get; set; }
        public string OriginCountryName { get; set; }
        public long DestinationCountryCode { get; set; }
        public string DestinationCountryName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime LandingTime { get; set; }
        public int RemaniningTickets { get; set; }


        public Flight()
        {
        }

        public Flight(long airLineCompanyID, long originCountryCode, long destinationCountryCode, DateTime departureTime, DateTime landingTime, int remaniningTickets)
        {
            
            AirLineCompanyID = airLineCompanyID;
            OriginCountryCode = originCountryCode;
            DestinationCountryCode = destinationCountryCode;
            DepartureTime = departureTime;
            LandingTime = landingTime;
            RemaniningTickets = remaniningTickets;
         
        }

        public static bool operator ==(Flight a, Flight b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return true;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            if (a.ID == b.ID)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Flight a, Flight b)
        {
            return !(a.ID == b.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            Flight country = obj as Flight;
            return this.ID == country.ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }

        public override string ToString()
        {
            return $"ID: {ID} AirLineCompanyID: {AirLineCompanyID} OriginCountryCode: {OriginCountryCode} DestinationCountryCode: {DestinationCountryCode} DepartureTime: {DepartureTime} LandingTime: {LandingTime} RemaniningTickets: {RemaniningTickets}";
        }
    }
}
