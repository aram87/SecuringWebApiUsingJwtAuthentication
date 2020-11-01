using SecuringWebApiUsingJwtAuthentication.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByCustomerId(int id);
    }
}
