using Domain.DTO;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ICustomerService
    {
        public Task<Tuple<bool, string>> CreateCustomerAsync(CustomerDto customerDto);
    }
}
