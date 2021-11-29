using System.Threading.Tasks;
using TestSystem.API.Core.DTO;

namespace TestSystem.API.Core.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponse> RegisterAsync(UserDto newUser);
        Task<AuthResponse> LoginAsync(Credentials credentials);
        Task UpdateUserAsync(UserDto userDto);
        Task<AuthResponse> ChangePasswordAsync(Credentials cred);
    }
}
