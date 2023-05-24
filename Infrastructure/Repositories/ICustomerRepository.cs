using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        public Task<Tuple<bool, string>> CreateCustomerAsync(Customer customer);
    }
}
