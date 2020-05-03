using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    
    public class AirLineCompany: IPoco, IUser
    {
        public long ID { get; set; }
        public string AirLineName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Email { get; set; }

        public AirLineCompany()
        {
        }

        public AirLineCompany(string airLineName, string userName, string password, long countryCode)
        {  
            AirLineName = airLineName;
            UserName = userName;
            Password = password;
            CountryCode = countryCode;
        }

        public static bool operator ==(AirLineCompany a, AirLineCompany b)
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
        public static bool operator !=(AirLineCompany a, AirLineCompany b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return false;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return true;
            return !(a.ID == b.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            AirLineCompany country = obj as AirLineCompany;
            return this.ID == country.ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID ;
        }
    }
}
