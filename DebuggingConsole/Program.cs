using ContactManagerServiceLayer.BusinessLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
            DataManipulation.FormatForSql("stri-ng");
            Requests request = new Requests();
            Dictionary<string, dynamic> jsonInfo = new Dictionary<string, dynamic>();
            List<string> dict = new List<string>();
            
            dict.Add("1");
            dict.Add("John");
            dict.Add("Smith");
            //dict.Add("4");

            List<List<string>> pList = new List<List<string>>();
            List<string> phoneData = new List<string>();
            phoneData.Add("NewBlue");
            phoneData.Add("6669954647");
            pList.Add(phoneData);

            List<List<string>> eList = new List<List<string>>();
            List<string> emailData = new List<string>();
            emailData.Add("NewMail");
            emailData.Add("NewAddress");

            eList.Add(emailData);

            List<List<string>> aList = new List<List<string>>();
            List<string> addressData = new List<string>();
            addressData.Add("NewAData");
            addressData.Add("New");
            addressData.Add("New");
            addressData.Add("New");
            addressData.Add("New");
            addressData.Add("New");
            aList.Add(addressData);
            addressData.Clear();

            addressData.Add("new Address");
            addressData.Add("80");
            addressData.Add("New Street");
            addressData.Add("New Orlando");
            addressData.Add("New USA");
            addressData.Add("New Fla");
            aList.Add(addressData);
            

            jsonInfo.Add("BusinessInfo", dict);
            jsonInfo.Add("AddressInfo", aList);
            jsonInfo.Add("EmailInfo", eList);
            jsonInfo.Add("PhoneInfo", pList);
            
            
             Dictionary<string, string> req = new Dictionary<string, string>();
            req.Add("UserName", "John");
            req.Add("PasswordHash", "Smith");

            string json = JsonConvert.SerializeObject(jsonInfo);
            var response = request.PostContent(json, "NewContact").Result;
            /*
            string api = ConfigurationManager.AppSettings["apiConnection"].ToString();
            string endpoint = @"GetContactInfo?&Id=14";



            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(api + endpoint);
            object resp = null;
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                var stream = response.GetResponseStream();
                StreamReader strReader = new StreamReader(stream);
                var str = strReader.ReadToEnd();
                resp = JsonConvert.DeserializeObject(str);
            }
            
            */


        }

    }
}
