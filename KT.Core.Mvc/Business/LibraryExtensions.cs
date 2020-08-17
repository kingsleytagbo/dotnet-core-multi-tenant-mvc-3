using System;
using System.Threading.Tasks;

namespace KT.Core.Mvc.Business
{
    public static class LibraryExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this System.Net.Http.HttpContent content) =>
            await System.Text.Json.JsonSerializer.DeserializeAsync<T>(await content.ReadAsStreamAsync());

        public static async Task<System.Net.Http.HttpResponseMessage> GetAsJsonAsync<T>(this System.Net.Http.HttpClient client, string requestUrl, T value)
        {
            var response = await client.GetAsync(requestUrl);
            return response;
        }

        public static async Task<System.Net.Http.HttpResponseMessage> PostAsJsonAsync<T>(this System.Net.Http.HttpClient client, string requestUrl, T value)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(value);
            var content = new System.Net.Http.StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUrl, content);
            return response;
            //return await System.Text.Json.JsonSerializer.SerializeAsync<T>(jsonString, options);
        }

        public static async Task<System.Net.Http.HttpResponseMessage> PutAsJsonAsync<T>(this System.Net.Http.HttpClient client, string requestUrl, T value)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(value);
            var content = new System.Net.Http.StringContent(data, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PutAsync(requestUrl, content);
            return response;
            //return await System.Text.Json.JsonSerializer.SerializeAsync<T>(jsonString, options);
        }
    }
}
