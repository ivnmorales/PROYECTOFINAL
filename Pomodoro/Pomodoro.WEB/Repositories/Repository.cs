using System.Text.Json;
using System.Text;

namespace Pomodoro.WEB.Repositories
{
    // Clase que implementa IRepository para gestionar las solicitudes HTTP
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;  // Cliente HTTP para realizar solicitudes

        // Opciones predeterminadas de serialización JSON con sensibilidad a mayúsculas y minúsculas de las propiedades
        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, // No diferencia entre mayúsculas y minúsculas en los nombres de las propiedades
        };
        // Constructor que recibe un HttpClient inyectado para realizar solicitudes HTTP
        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient; // Asigna el HttpClient al campo privado
        }
        // Método para realizar una solicitud GET genérica que devuelve un tipo T
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            // Realiza la solicitud GET
            var responseHttp = await _httpClient.GetAsync(url);
            // Si la respuesta es exitosa
            if (responseHttp.IsSuccessStatusCode)
            {
                // Deserializa la respuesta HTTP en un objeto de tipo T
                var response = await UnserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<T>(response, false, responseHttp);  // Retorna la respuesta con éxito
            }
            // Si la respuesta no fue exitosa, devuelve un error
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }
        // Método POST que no devuelve ningún valor de respuesta
        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        // Método POST genérico que devuelve una respuesta de tipo TResponse
        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        // Método PUT que no devuelve ningún valor de respuesta
        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        // Método privado para deserializar la respuesta HTTP en el tipo genérico T
        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHttp = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSerializerOptions)
        {
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            // Deserializa el string a un objeto de tipo T usando las opciones de serialización configuradas
            return JsonSerializer.Deserialize<T>(responseString, jsonSerializerOptions)!;
        }
    }
}
