using Infrastructure.DataContext;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CustomerException : Exception
    {
        public CustomerException(string message) : base(message) { }
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;
        public CustomerRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Tuple<bool, string>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                if (customer == null)
                {
                    throw new CustomerException("Customer can not be null");
                }

                await _appDbContext.Customers.AddAsync(customer);
                var result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Created Customer Success !");

            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}");
            }
        }
    }
}
