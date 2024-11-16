using Pomodoro.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Pomodoro.Shared.Dtos;

namespace Pomodoro.API.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);



        Task<IdentityResult> AddUserAsync(User user, string password);



        Task CheckRoleAsync(string roleName);



        Task AddUserToRoleAsync(User user, string roleName);



        Task<bool> IsUserInRoleAsync(User user, string roleName);

        // Nuevos métodos de autenticación
        Task<SignInResult> LoginAsync(LoginDTO model); // Método de inicio de sesión
        Task LogoutAsync(); // Método de cierre de sesión
    }
}
