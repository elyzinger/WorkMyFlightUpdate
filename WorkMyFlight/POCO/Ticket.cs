using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class Ticket : IPoco
    {
        public long ID { get; set; }
        public long FlightID { get; set; }
        public long CustomerID { get; set; }
        public Ticket()
        {
        }

        public Ticket( long flightID, long customerID)
        {
            FlightID = flightID;
            CustomerID = customerID;
        }

        public static bool operator ==(Ticket a, Ticket b)
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
        public static bool operator !=(Ticket a, Ticket b)
        {
            return !(a.ID == b.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            Ticket country = obj as Ticket;
            return this.ID == country.ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }
    }
}
