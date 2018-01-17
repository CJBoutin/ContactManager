using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DebuggingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Requests request = new Requests();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("FirstName", "Connor");
            dict.Add("LastName", "Boutin");
            string json = JsonConvert.SerializeObject(dict);
            var response = request.PostContent(json).Result;
        }

    }
}
