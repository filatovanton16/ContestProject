using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ContestProject
{
    public static class Requests
    {
        public async static Task<dynamic> POST(JDoodlePOSTObject obj, string uri)
        {
            string json = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpClient client = new HttpClient(); // Use HttpClientFabric in case DI
            HttpResponseMessage response = await client.PostAsync(uri, content);
            string strResponse = await response.Content.ReadAsStringAsync();
            dynamic output = JsonConvert.DeserializeObject(strResponse);
            return output;
        }
    }
}
