using MySql.Data.MySqlClient;
using System.Collections;
using System.Data.Common;
using System.Net;
using static System.Net.WebRequestMethods;

namespace TeczkaCore.Models.Seeds
{
    public class MySQLReader
    {
        protected MySqlConnection connection;
        protected DbDataReader reader;

        public MySQLReader(WebApplication app)
        {
            string mySqlConnectionString = app.Configuration.GetConnectionString("MyDevilConnectionString");
            connection = new MySqlConnection(mySqlConnectionString);
        }

        public List<Hashtable> ReadTable(string table)
        {
            List<Hashtable> list = new List<Hashtable>();
            try
            {
                connection.Open();
                string SQLcommand = "select * from " + table + ";";
                var command = new MySqlCommand(SQLcommand, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var name = reader.GetName(i);
                        var type = reader.GetDataTypeName(i);
                        var value = reader.GetValue(i);
                        ht.Add(name, value);
                    }
                    list.Add(ht);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
