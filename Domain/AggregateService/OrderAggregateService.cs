using Domain.AggregateModelDto;
using Domain.Dtos;
using Infrastructure.AggregateRepository;
using Infrastructure.Models;

namespace Domain.AggregateService
{

    public class OrderAggregateService : IOrderAggregateService
    {
        private readonly IOrderAggregateRepository _orderManagementRepository;
        public OrderAggregateService(IOrderAggregateRepository orderManagementRepository)
        {
            _orderManagementRepository = orderManagementRepository;
        }

        public async Task<List<OrderAggregateDto>> GetOrderListAsync()
        {
            var listOrder = await _orderManagementRepository.GetListOrderAsync();
            var orders = listOrder.Select(o => new OrderAggregateDto
            {
                OrderId = o.BaseInformation.OrderId,
                CustomerName = o.BaseInformation.CustomerName,
                Phone = o.BaseInformation.Phone,
                Email = o.BaseInformation.Email,
                ShipAddress = o.BaseInformation.ShipAddress,
                IsPaid = o.BaseInformation.IsPaid,
                OrderDate = o.BaseInformation.OrderDate,
                OrderProductsQuantity = o.OrderProductsQuantity,
                TotalPrice = o.BaseInformation.TotalPrice
            }).ToList();

            return orders;
        }

        public async Task<List<OrderDetailAggregateDto>> GetOrderDetailByIdAsync(Guid orderId)
        {
            var listOrderDetail = await GetOrderDetailListAsync();
            var sublistOrderDetail = listOrderDetail.FindAll(l => l.OrderId == orderId);

            return sublistOrderDetail;
        }

        public async Task<Tuple<BaseInformationDto, string>> GetOrderedBaseInformationAsync(Guid orderId)
        {
            var sublistOrderDetail = await GetOrderDetailByIdAsync(orderId);

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
            var sublistOrderDetail = await GetOrderDetailByIdAsync(orderId);
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

        public async Task<List<OrderDetailAggregateDto>> GetOrderDetailListAsync()
        {
            var orderDetails = await _orderManagementRepository.GetListOrderDetailAsync();
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
    }
}