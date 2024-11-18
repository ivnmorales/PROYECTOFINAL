using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Pomodoro.WEB.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider

    {

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()

        {

           // Simulación de un retraso de 3 segundos
            await Task.Delay(3000);

            // Crear una identidad anónima
            var anonymous = new ClaimsIdentity();

            // Devolver el estado de autenticación
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));

        }

    }
}
