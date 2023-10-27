namespace orderApi.Interface
{
    public interface IServiceCallHelper
    {
        Task<string> Post(string uri, HttpMethod httpmethod, string content);
        Task<object> Get(string uri);
    }
}
