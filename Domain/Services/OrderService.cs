using Domain.AggregateModelDto;
using Domain.DTO;
using Domain.Dtos;
using Infrastructure.Models;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderAggregateDto>> GetAggregatedOrderListAsync()
        {
            var listOrder = await _orderRepository.GettAggregatedOrderListAsync();
            var orders = listOrder.Select(o => new OrderAggregateDto
            {
                OrderId = o.BaseInformation.OrderId,
                CustomerName = o.BaseInformation.CustomerName,
                IsPaid = o.BaseInformation.IsPaid,
                OrderDate = o.BaseInformation.OrderDate,
                OrderProductsQuantity = o.OrderProductsQuantity,
                TotalPrice = o.BaseInformation.TotalPrice,
            }).ToList();

            return orders;
        }

        public async Task<Tuple<bool, string>> CreateOrderAsync(OrderDto orderDto)
        {

            if (orderDto == null)
                return Tuple.Create(false, "The Order To Be Created Doesn't Exist");

            var orderDetails = orderDto.OrderDetails.Select(o => new OrderDetail
            {
                OrderId = o.OrderId,
                ProductId = o.ProductId,
                Discount = o.Discount,
                Price = o.Price,
                Quantity = o.Quantity
            }).ToList();
            var order = new Order()
            {
                Id = orderDto.Id,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShipAddress = orderDto.ShipAddress,
                IsPaid = orderDto.IsPaid,
                TotalPrice = orderDto.TotalPrice,
                UserId = orderDto.UserId,
                OrderDetails = orderDetails,

            };

            var result = await _orderRepository.CreateOrderAsync(order);
            return result;
        }

        public async Task<List<OrderDetailAggregateDto>> GetOrderDetailListByIdAsync(Guid orderId)
        {
            var listOrderDetail = await GetAggregatedOrderDetailListAsync();
            var sublistOrderDetail = listOrderDetail.FindAll(l => l.OrderId == orderId);

            return sublistOrderDetail;
        }

        public async Task<List<OrderDetailAggregateDto>> GetAggregatedOrderDetailListAsync()
        {
            var orderDetails = await _orderRepository.GetAggregatedOrderDetailAsync();
            var orders = orderDetails.Select((orderDetail) => new OrderDetailAggregateDto
            {

                OrderId = orderDetail.BaseInformation.OrderId,
                CustomerName = orderDetail.BaseInformation.CustomerName,
                Email = orderDetail.BaseInformation.Email,
                OrderDate = orderDetail.BaseInformation.OrderDate,
                Phone = orderDetail.BaseInformation.Phone,
                ShipAddress = orderDetail.BaseInformation.ShipAddress,
                TotalPrice = orderDetail.BaseInformation.TotalPrice,
                IsPaid = orderDetail.BaseInformation.IsPaid,

                ProductDto = new ProductDto
                {
                    Id = orderDetail.Product.Id,
                    CategoryId = orderDetail.Product.CategoryId,
                    CreateDate = orderDetail.Product.CreatedDate,
                    ModifiedDate = orderDetail.Product.ModifiedDate,
                    Name = orderDetail.Product.Name,
                    Price = orderDetail.Product.Price,
                    Provider = orderDetail.Product.Provider,
                    MainImageName = orderDetail.Product.MainImageName,
                    Description = orderDetail.Product.Description,
                    QuantityInOrder = orderDetail.QuantityInOrder,
                },
            });

            return orders.ToList();
        }

        public async Task<Tuple<BaseInformationDto, string>> GetOrderedBaseInformationAsync(Guid orderId)
        {
            var sublistOrderDetail = await GetOrderDetailListByIdAsync(orderId);

            if (sublistOrderDetail == null || sublistOrderDetail.Count == 0)
                return Tuple.Create(default(BaseInformationDto), "Can't find any orders");
            var baseInformation = new BaseInformationDto
            {
                OrderId = sublistOrderDetail[0].OrderId,
                CustomerName = sublistOrderDetail[0].CustomerName,
                Phone = sublistOrderDetail[0].Phone,
                Email = sublistOrderDetail[0].Email,
                ShipAddress = sublistOrderDetail[0].ShipAddress,
                IsPaid = sublistOrderDetail[0].IsPaid,
                OrderDate = sublistOrderDetail[0].OrderDate,
                TotalPrice = sublistOrderDetail[0].TotalPrice
            };

            return Tuple.Create(baseInformation, "Finding success information");
        }

        public async Task<Tuple<List<ProductDto>, string>> GetOrderedProductListAsync(Guid orderId)
        {
            var sublistOrderDetail = await GetOrderDetailListByIdAsync(orderId);
            var products = new List<ProductDto>();
            if (sublistOrderDetail == null || sublistOrderDetail.Count == 0)
                return Tuple.Create(default(List<ProductDto>), "Can't product list found");
            foreach (var orderDetail in sublistOrderDetail)
            {
                var product = new ProductDto
                {
                    Id = orderDetail.ProductDto.Id,
                    Name = orderDetail.ProductDto.Name,
                    Price = orderDetail.ProductDto.Price,
                    Provider = orderDetail.ProductDto.Provider,
                    MainImageName = orderDetail.ProductDto.MainImageName,
                    CategoryId = orderDetail.ProductDto.CategoryId,
                    Description = orderDetail.ProductDto.Description,
                    QuantityInOrder = orderDetail.ProductDto.QuantityInOrder,
                    CreateDate = orderDetail.ProductDto.CreateDate,
                    ModifiedDate = orderDetail.ProductDto.ModifiedDate,
                };

                products.Add(product);
            }

            return Tuple.Create(products, "Successfully found product list");
        }

        public Task<int> GetTotal()
        {
            return _orderRepository.GetTotal();
        }
    }
}
