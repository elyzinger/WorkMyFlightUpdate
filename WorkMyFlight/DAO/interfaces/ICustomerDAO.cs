using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkMyFlight
{
    public interface ICustomerDAO: IBasicDB<Customer>
    {
        Customer GetCustomerByUserName(string username);
    }
}
