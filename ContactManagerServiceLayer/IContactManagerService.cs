using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagerServiceLayer
{

    [DataContract]
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

    [DataContract]
    public class UserData
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
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
            Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "AddUser")]
        string AddUser(UserData uData);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
        Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "DeleteUser?uId={userId}")]
        string DeleteUser(string userId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
        Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetContactInfo?cId={conId}")]
        Task<string> GetContactInfo(string conId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
        Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetSingleContact?search={searchData}")]
        Task<string> GetSingleContact(string searchData);


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

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Bare,
        Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "GetUser")]
        Task<string> GetUser(UserData uData);

    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
}
