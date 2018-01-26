using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DebuggingConsole
{
    class Requests
    {
        public async Task<string> PostContent(string data, string endpt)
        {
            
            string jsonResponse = "";
            using (var httpClient = new HttpClient())
            {
                string api = ConfigurationManager.AppSettings["apiConnection"].ToString();
                httpClient.BaseAddress = new Uri(api);

                //set header mediaType to json
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));


                string endpoint = @"/ContactManagerService.svc/" + endpt;

                try
                {
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonResponse = await response.Content.ReadAsStringAsync();
                        //do something with json response here
                    }
                    else
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {
                    jsonResponse = "Error";
                }
            }

            return jsonResponse;
        }

    }
}
