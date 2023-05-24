using Infrastructure.DataContext;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
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
                    throw new ProductException("Product can not be null");
                }

                if (customer.Id == default(Guid))
                    return Tuple.Create(false, "Customer ID Can Not Be Default");

                var customerEntity = _appDbContext.Customers
                                                 .Where(c => c.Name == c.Name && c.Phone == customer.Phone
                                                  && c.Email == customer.Email)
                                                 .FirstOrDefault();
                if (customerEntity != null)
                {
                    return Tuple.Create(true, "Your Information Already Exist");
                }

                await _appDbContext.Customers.AddAsync(customer);
                var result = await _appDbContext.SaveChangesAsync();
                return Tuple.Create(result > 0, "Created Product Success !");

            }
            catch (Exception exception)
            {
                return Tuple.Create(false, $"An Error Occurred : {exception.Message}");
            }
        }
    }
}
