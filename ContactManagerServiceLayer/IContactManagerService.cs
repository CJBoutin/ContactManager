using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ContactManagerServiceLayer
{

    [DataContract]
    /*public class ContactData
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string PhoneType { get; set; }

        [DataMember]
        public string EmailType { get; set; }

        [DataMember]
        public string EmailAddress { get; set; }

        [DataMember]
        public string AddressType { get; set; }

        [DataMember]
        public string StreetNo { get; set; }

        [DataMember]
        public string StreetName { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string UserName { get; set; }

    }
    */

    public class ContactData
    {
        [DataMember]
        public List<string> BusinessInfo { get; set; }

        [DataMember]
        public List<List<string>> AddressInfo = new List<List<string>>();

        [DataMember]
        public List<List<string>> EmailInfo = new List<List<string>>();

        [DataMember]
        public List<List<string>> PhoneInfo = new List<List<string>>();
    }
        // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
        [ServiceContract]
    public interface IContactManagerService
    {

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "IsAlive")]
            string IsAlive();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "NewContact")]
        string NewContact(ContactData cData);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "UpdateContact")]
        string UpdateContact(ContactData cData);


        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetAllContacts?uId={userId}")]
        string GetContacts(string userId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
            Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "DeleteContact?cId={conId}")]
        string DeleteContact(string conId);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
}
