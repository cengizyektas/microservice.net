using orderApi.Interface;
using System.Net;
using System.Text;

namespace orderApi.Helpers
{
    public class ServiceCallHelpers : IServiceCallHelper
    {
        public ServiceCallHelpers()
        {
                
        }
        public async Task<string> Post(string uri, HttpMethod httpmethod, string content)
        {
            using (var client= new HttpClient())
            {
                var request = new HttpRequestMessage(httpmethod, uri);
                request.Content = new StringContent(content,Encoding.UTF8,"application/json");
                request.Content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
        public async Task<object> Get(string uri)
        {
            using (WebClient webClient= new WebClient())
            {
                var response = webClient.DownloadString(uri);
                return response;
            }
        }     
    }
}
