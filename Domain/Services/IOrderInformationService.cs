using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IOrderInformationService
    {
        public Task<List<OrderInforDto>> GetListOrderInfor();
    }
}
