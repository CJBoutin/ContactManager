using ContactManagerServiceLayer.BusinessLogic;
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
            string s = "";

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
                s = "Success";
                connection.Close();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return s;
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

    }
}
