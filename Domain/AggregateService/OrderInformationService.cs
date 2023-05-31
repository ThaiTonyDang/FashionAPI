using Domain.AggregateModelDto;
using Domain.Dtos;
using Infrastructure.AggregateRepository;

namespace Domain.AggregateService
{

    public class OrderInformationService : IOrderInformationService
    {
        private readonly IOrderInformationRepository _orderManagementRepository;
        public OrderInformationService(IOrderInformationRepository orderManagementRepository)
        {
            _orderManagementRepository = orderManagementRepository;
        }

        public async Task<List<OrderInforDto>> GetListOrderInformationAsync()
        {
            var listOrder = await _orderManagementRepository.GetListOrderAsync();
            var orders = listOrder.Select(o => new OrderInforDto
            {
                OrderId = o.BaseInformation.OrderId,
                CustomerName = o.BaseInformation.CustomerName,
                Phone = o.BaseInformation.Phone,
                Email = o.BaseInformation.Email,
                ShipAddress = o.BaseInformation.ShipAddress,
                IsPaid = o.IsPaid,
                OrderDate = o.BaseInformation.OrderDate,
                OrderProductsQuantity = o.OrderProductsQuantity,
                TotalPrice = o.BaseInformation.TotalPrice
            }).ToList();

            return orders;
        }

        public async Task<Tuple<SingleOrderDetailDto, string>> GetSingleOrderDetailByIdAsync(Guid orderId)
        {
            var listOrderDetail = await GetListOrderDetailsAsync();
            var products = new List<ProductDto>();
            var baseInformation = new BaseInformationDto();
            var sublistOrderDetail = listOrderDetail.FindAll(l => l.BaseInformationDto.OrderId == orderId);
            if (sublistOrderDetail.Count > 0)
            {
                var flag = true;
                foreach (var orderDetail in sublistOrderDetail)
                {
                    if (flag)
                    {
                        baseInformation = orderDetail.BaseInformationDto;
                    }
                    products.Add(orderDetail.ProductDto);
                    flag = false;
                }

                var single = new SingleOrderDetailDto { BaseInformationDto = baseInformation, ProductDtos = products };
                return Tuple.Create(single, "Get Order Detail Success !");
            }

            return Tuple.Create(default(SingleOrderDetailDto), "Get Order Fail !");
        }

        private async Task<List<OrderInformationDetailDto>> GetListOrderDetailsAsync()
        {
            var orderDetails = await _orderManagementRepository.GetListOrderDetailAsync();
            var orders = orderDetails.Select((o) => new OrderInformationDetailDto
            {
                BaseInformationDto = new BaseInformationDto
                {
                    OrderId = o.BaseInformation.OrderId,
                    CustomerName = o.BaseInformation.CustomerName,
                    Email = o.BaseInformation.Email,
                    OrderDate = o.BaseInformation.OrderDate,
                    Phone = o.BaseInformation.Phone,
                    ShipAddress = o.BaseInformation.ShipAddress,
                    TotalPrice = o.BaseInformation.TotalPrice
                },

                ProductDto = new ProductDto
                {
                    Id = o.Product.Id,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Provider = o.Product.Provider,
                    ImageName = o.Product.ImageName,
                    QuantityInOrder = o.Quantity
                }
            });

            return orders.ToList();
        }
    }
}