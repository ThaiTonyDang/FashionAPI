using Domain.DTO;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<Tuple<bool, string>> CreateCustomerAsync(CustomerDto customerDto)
        {
            if (customerDto == null)
                return Tuple.Create(false, "Customer To Be Create Cannot Be Found");

            var customer = new Customer()
            {
                Id = customerDto.Id,
                Name = customerDto.Name,
                Email = customerDto.Email,
                Phone = customerDto.Phone,
                City = customerDto.City,
                County = customerDto.County,
                Ward = customerDto.Ward,
                NumberDepartment = customerDto.NumberDepartment
            };

            var result = await _customerRepository.CreateCustomerAsync(customer);
            return result;
        }
    }
}
