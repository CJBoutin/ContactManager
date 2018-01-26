using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactManagerServiceLayer.BusinessLogic
{
    public class Id
    {
        public static int GetId(MySqlConnection connect, string table, Dictionary<string, string> pairs)
        {
            int id = -2;
            string command = string.Format("SELECT Id FROM {0} WHERE ", table);
            foreach (var pair in pairs)
            {
                if (id == -2)
                {
                    command += string.Format("{0}={1}", pair.Key, pair.Value);
                    id++;
                }
                else
                    command += string.Format(" AND {0}={1}", pair.Key, pair.Value);
            }
            command += ";";
            MySqlCommand cmd = new MySqlCommand(command, connect);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                id = Convert.ToInt32(reader["Id"]);
            }
            reader.Close();

            return id;

        }

    }
}