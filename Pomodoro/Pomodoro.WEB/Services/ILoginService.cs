using Pomodoro.Shared.Dtos;

namespace Pomodoro.WEB.Services
{
    public interface ILoginService

    {

        Task LoginAsync(string token);



        Task LogoutAsync();

  

    }
}
