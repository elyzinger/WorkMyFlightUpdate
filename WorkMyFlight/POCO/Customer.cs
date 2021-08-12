using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public class Customer : IPoco, IUser
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public string Email { get; set; }
        public string Guide { get; set; }

        public Customer()
        {
        }

        public Customer( string firstName, string lastName, string userName, string password, string address, string phoneNumber, string creditCardNumber)
        {
            
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Password = password;
            Address = address;
            PhoneNumber = phoneNumber;
            CreditCardNumber = creditCardNumber;
        }

        public static bool operator ==(Customer a, Customer b)
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
        public static bool operator !=(Customer a, Customer b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            {
                return false;
            }
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return true;
            }
            return !(a.ID == b.ID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            Customer country = obj as Customer;
            return this.ID == country.ID;
        }

        public override int GetHashCode()
        {
            return (int)this.ID;
        }

    }
}
