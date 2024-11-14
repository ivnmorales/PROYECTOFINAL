using System.Net;

namespace Pomodoro.WEB.Repositories
{
    public class HttpResponseWrapper<T>
    {
        // Constructor que inicializa las propiedades del wrapper
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
            Success = !error; // Éxito es verdadero si no hay error
        }

        public bool Error { get; set; } // Indica si hubo un error
        public T? Response { get; set; } // Respuesta del tipo genérico
        public HttpResponseMessage HttpResponseMessage { get; set; } // Mensaje de respuesta HTTP
        public bool Success { get; } // Propiedad que indica si la respuesta fue exitosa

        // Método que obtiene un mensaje de error basado en el código de estado HTTP
        public async Task<string?> GetErrorMessage()
        {
            if (!Error)
            {
                return null; // No hay error, retornamos null
            }

            var statusCode = HttpResponseMessage.StatusCode;
            if (statusCode == HttpStatusCode.NotFound)
            {
                return "Recurso no encontrado"; // Mensaje para recurso no encontrado
            }
            else if (statusCode == HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync(); // Retorna el contenido de error
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return "Debes iniciar sesión para realizar esta acción"; // Mensaje para autorización fallida
            }
            else if (statusCode == HttpStatusCode.Forbidden)
            {
                return "No tienes permisos para ejecutar esta acción"; // Mensaje para permisos denegados
            }

            return "Ha ocurrido un error inesperado"; // Mensaje para errores no manejados
        }
    }
}
