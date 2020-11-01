using SecuringWebApiUsingJwtAuthentication.Requests;
using SecuringWebApiUsingJwtAuthentication.Responses;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Interfaces
{
    public interface ICustomerService
    {
        Task<LoginResponse> Login(LoginRequest loginRequest);
    }
}
