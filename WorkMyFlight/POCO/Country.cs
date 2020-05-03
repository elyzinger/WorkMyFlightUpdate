using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class Country : IPoco
    {
        public long ID { get; set; }
        public string CountryName { get; set; }

        public Country()
        {
        }

        public Country(string countryName)
        {

            CountryName = countryName;
        }

        public static bool operator == (Country a, Country b)
        {
            if(ReferenceEquals(a,null) && ReferenceEquals(b, null))
            {
                return true;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }
            if(a.ID == b.ID)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Country a, Country b)
        {
            return !(a.ID == b.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            Country country = obj as Country;
            return this.ID == country.ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }
    }
  

  
}
