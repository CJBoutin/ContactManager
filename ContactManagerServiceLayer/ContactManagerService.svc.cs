﻿using ContactManagerServiceLayer.BusinessLogic;
using ContactManagerServiceLayer.DataClasses;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagerServiceLayer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ContactManagerService : IContactManagerService
    {
        public string IsAlive()
        {
            return string.Format("Yes");
        }

        public string NewContact(ContactData cData)
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            var addressInfo = cData.AddressInfo;
            var emailInfo = cData.EmailInfo;
            var businessInfo = cData.BusinessInfo;
            var phoneInfo = cData.PhoneInfo;
            try
            {

                // Insert the new contact into the SQL Database
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());
                connection.Open();
                // Insert the new contact data
                // Contact will be guaranteed a first name. Other data must be set to 'null' if null.
                // PARAMATERS:
                string fullQuery = "";
                string insertContact = string.Format("CALL InsertContact({0}, {1}, {2}); SELECT DISTINCT LAST_INSERT_ID();",
                    DataManipulation.FormatForSql(businessInfo[0]),
                    DataManipulation.FormatForSql(businessInfo[1]),
                    DataManipulation.FormatForSql(businessInfo[2]));

                MySqlCommand newContact = new MySqlCommand(insertContact, connection);
                int contactId = Convert.ToInt32(newContact.ExecuteScalar());

                foreach (var item in addressInfo)
                {
                    string insertAddress = string.Format("CALL InsertAddress({0}, {1}, {2}, {3}, {4}, {5}, {6});",
                    DataManipulation.FormatForSql(contactId.ToString()),
                    DataManipulation.FormatForSql(item[0]),
                    DataManipulation.FormatForSql(item[1]),
                    DataManipulation.FormatForSql(item[2]),
                    DataManipulation.FormatForSql(item[3]),
                    DataManipulation.FormatForSql(item[4]),
                    DataManipulation.FormatForSql(item[5]));

                    fullQuery += insertAddress;
                }
                foreach (var item in emailInfo)
                {
                    string insertEmail = string.Format("CALL InsertEmail({0}, {1}, {2});",
                        DataManipulation.FormatForSql(contactId.ToString()),
                        DataManipulation.FormatForSql(item[0]),
                        DataManipulation.FormatForSql(item[1]));

                    fullQuery += insertEmail;
                }
                foreach (var item in phoneInfo)
                {
                    string ACode = item[1].Substring(0, 3);
                    string sigNumber = item[1].Substring(3, 7);
                    string extension = item[1].Substring(10, item[1].Length - 10);

                    string insertPhoneNumber = string.Format("CALL InsertPhoneNumber({0}, {1}, {2}, {3}, {4});",
                        DataManipulation.FormatForSql(contactId.ToString()),
                        DataManipulation.FormatForSql(item[0]),
                        DataManipulation.FormatForSql(ACode),
                        DataManipulation.FormatForSql(sigNumber),
                        DataManipulation.FormatForSql(extension));

                    fullQuery += insertPhoneNumber;
                }
                MySqlCommand cmd = new MySqlCommand(fullQuery, connection);
                cmd.ExecuteNonQuery();

                resp.Add("InsertStatus", "Success");
                resp.Add("NewContactId", contactId.ToString());
                
                connection.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return JsonConvert.SerializeObject(resp);
        }

        public string GetContacts(string userId)
        {
            string response = "";
            int id = -1;
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());

            bool s = int.TryParse(userId, out id);
            if(s == false)
            {
                return "Failed";
            }
            var cmd = new MySqlCommand(string.Format("CALL GetAllContacts({0});", id), connection);

            connection.Open();
            List<BasicContact> resList = new List<BasicContact>();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {                
                while (reader.Read())
                {
                    BasicContact contact = new BasicContact();
                    contact.Id = int.Parse(reader["Id"].ToString()); 
                    contact.FirstName = reader["FirstName"].ToString();
                    contact.LastName = reader["LastName"].ToString();
                    resList.Add(contact);
                }
            }
            connection.Close();
            response = JsonConvert.SerializeObject(resList);
            return response;
        }

        public string UpdateContact(ContactData cData)
        {
            var addressInfo = cData.AddressInfo;
            var emailInfo = cData.EmailInfo;
            var businessInfo = cData.BusinessInfo;
            var phoneInfo = cData.PhoneInfo;
            try
            {
                // Insert the new contact into the SQL Database
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());
                connection.Open();

                string fullQuery = "";
                int contactId = Convert.ToInt32(businessInfo[3]);

                // must drop all data for this contact on update.
                // Then we must re-add the new data
                string deleteCmd = string.Format("CALL DeleteAddress({0}); CALL DeletePhoneNumber({1}); CALL DeleteEmail({2});", contactId, contactId, contactId);
                MySqlCommand deleteCommand = new MySqlCommand(deleteCmd, connection);
                deleteCommand.ExecuteNonQuery();

                foreach (var item in addressInfo)
                {
                    string insertAddress = string.Format("CALL InsertAddress({0}, {1}, {2}, {3}, {4}, {5}, {6});",
                    DataManipulation.FormatForSql(contactId.ToString()),
                    DataManipulation.FormatForSql(item[0]),
                    DataManipulation.FormatForSql(item[1]),
                    DataManipulation.FormatForSql(item[2]),
                    DataManipulation.FormatForSql(item[3]),
                    DataManipulation.FormatForSql(item[4]),
                    DataManipulation.FormatForSql(item[5]));

                    fullQuery += insertAddress;
                }
                foreach (var item in emailInfo)
                {
                    string insertEmail = string.Format("CALL InsertEmail({0}, {1}, {2});",
                        DataManipulation.FormatForSql(contactId.ToString()),
                        DataManipulation.FormatForSql(item[0]),
                        DataManipulation.FormatForSql(item[1]));

                    fullQuery += insertEmail;
                }
                foreach (var item in phoneInfo)
                {
                    string ACode = item[1].Substring(0, 3);
                    string sigNumber = item[1].Substring(3, 7);
                    string extension = item[1].Substring(10, item[1].Length - 10);

                    string insertPhoneNumber = string.Format("CALL InsertPhoneNumber({0}, {1}, {2}, {3}, {4});",
                        DataManipulation.FormatForSql(contactId.ToString()),
                        DataManipulation.FormatForSql(item[0]),
                        DataManipulation.FormatForSql(ACode),
                        DataManipulation.FormatForSql(sigNumber),
                        DataManipulation.FormatForSql(extension));

                    fullQuery += insertPhoneNumber;
                }
                MySqlCommand cmd = new MySqlCommand(fullQuery, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "Success";

        }

        public string DeleteContact(string conId)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());
            int a = -1;
            string cmd = string.Format("CALL DeleteContact({0});", conId);
            MySqlCommand command = new MySqlCommand(cmd, connection);
            try
            {
                connection.Open();
                a = command.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            if(a > 0)
            {
                return "Success";
            }
            else
            {
                return "Failed";
            }
        }

        public string AddUser(UserData uData)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());

            string cmd = string.Format("INSERT INTO users(UserName, PasswordHash, DateCreated, DateModified) VALUES({0}, {1}, CURRENT_TIMESTAMP(), CURRENT_TIMESTAMP()); SELECT LAST_INSERT_ID();", DataManipulation.FormatForSql(uData.UserName), DataManipulation.FormatForSql(uData.PasswordHash));
            MySqlCommand command = new MySqlCommand(cmd, connection);
            int curUserId = Convert.ToInt32(command.ExecuteScalar());
            Dictionary<string, string> resp = new Dictionary<string, string>();
            resp.Add("UserId", curUserId.ToString());
            return JsonConvert.SerializeObject(resp);
        }

        public string DeleteUser(string userId)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());

            MySqlCommand deleteUser = new MySqlCommand(string.Format("CALL DeleteUser({0})", userId), connection);

            deleteUser.ExecuteNonQuery();
            return "Success";
        }
         public async Task<string> GetContactInfo(string conId)
        {

            Dictionary<string, string> response = new Dictionary<string, string>();
            AdvancedContact contact = new AdvancedContact();
            List<dynamic> contactItem = new List<dynamic>();


            try
            {
                MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["database"].ToString());
                connection.Open();
                string addressCmd = string.Format(@"SELECT * FROM addresses WHERE ContactInfoId={0};", conId);
                string phoneCmd = string.Format(@"SELECT * FROM phonenumbers WHERE ContactInfoId={0};", conId);
                string emailCmd = string.Format(@"SELECT * FROM email WHERE ContactInfoId={0};", conId);


                MySqlCommand command = new MySqlCommand(addressCmd, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    List<Dictionary<string, string>> dict = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        Dictionary<string, string> d = new Dictionary<string, string>();
                        d.Add("EmailType", reader["AddressType"].ToString());
                        d.Add("City", reader["City"].ToString());
                        d.Add("Country", reader["Country"].ToString());
                        d.Add("Province", reader["Province"].ToString());
                        d.Add("StreetNumber", reader["StreetNumber"].ToString());
                        d.Add("StreetName", reader["StreetName"].ToString());
                        dict.Add(d);
                    }
                    contactItem.Add(dict);
                }

                command = new MySqlCommand(phoneCmd, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    List<Dictionary<string, string>> dict = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        Dictionary<string, string> d = new Dictionary<string, string>();
                        d.Add("NumberType", reader["NumberType"].ToString());
                        d.Add("AreaCode", reader["AreaCode"].ToString());
                        d.Add("SignificantNumber", reader["SignificantNumber"].ToString());
                        dict.Add(d);

                    }
                    contactItem.Add(dict);
                }

                command = new MySqlCommand(emailCmd, connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    List<Dictionary<string, string>> dict = new List<Dictionary<string, string>>();
                    while (reader.Read())
                    {
                        Dictionary<string, string> d = new Dictionary<string, string>();
                        d.Add("EmailType", reader["AddressType"].ToString());
                        d.Add("EmailAddress", reader["Address"].ToString());
                        dict.Add(d);
                    }
                    contactItem.Add(dict);
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }
            string jsonObj = JsonConvert.SerializeObject(contactItem);
            return jsonObj;
        }

    }
}
