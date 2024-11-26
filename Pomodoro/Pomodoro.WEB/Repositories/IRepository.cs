namespace Pomodoro.WEB.Repositories
{
    public interface IRepository
    {
        // Obtiene datos de la URL especificada
        Task<HttpResponseWrapper<T>> Get<T>(string url);

        // Envía un modelo a la URL y devuelve un HttpResponseWrapper
        Task<HttpResponseWrapper<object>> Post<T>(string url, T model);

        // Envía un modelo a la URL y devuelve un tipo de respuesta específico
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model);

        // Actualiza un modelo en la URL especificada
        Task<HttpResponseWrapper<object>> Put<T>(string url, T model);

        // Elimina un recurso en la URL especificada
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<object>> Get(string url);
        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T model);
    }
}
