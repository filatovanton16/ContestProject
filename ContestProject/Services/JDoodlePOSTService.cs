using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace ContestProject
{
    public class JDoodlePOSTObject
    {
        public string clientId = "2dc5dafe92194a5629efb80069d583a1";   
        public string clientSecret = "b5804f1b2a42df4a99afe3774e32d11fe59be944596b168ed9947e186514fb1";
        public string stdin;
        public string language = "java";
        public string versionIndex = "3";
        public string script;
        public JDoodlePOSTObject(string _script, int _stdin)
        {
            stdin = _stdin.ToString();
            script = _script;
        }
    }

    public class JDoodlePOSTService : IJDoodleService
    {
        const string outerLeftTemplate = @"import java.util.Scanner;
                                            public class MyClass { 
                                            public static void main(String args[]) {
                                            Scanner in = new Scanner(System.in);
                                            int input = in.nextInt();
                                            in.close();
		                                    System.out.println(MyMethod(input));
	                                        }";
        const string outerRightTemplate = @" }";
        const string mediaType = "application/json";
        const string uri = "https://api.jdoodle.com/v1/execute";

        private readonly HttpClient _httpClient;

        public JDoodlePOSTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> TryCompilateAsync(string code, int input)
        {
            JDoodlePOSTObject jdPOSTObject = new JDoodlePOSTObject(outerLeftTemplate + code + outerRightTemplate, input);

            string json = JsonConvert.SerializeObject(jdPOSTObject);
            HttpContent content = new StringContent(json, Encoding.ASCII, mediaType);

            HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
            string strResponse = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject(strResponse);
        }

    }
}
