using MySql.Data.MySqlClient;
using System.Collections;
using System.Data.Common;
using System.Net;
using static System.Net.WebRequestMethods;

namespace TeczkaCore.Entities
{
  public class TableReader
  {
      protected MySqlConnection connection;
      protected DbDataReader reader;

  //protected string InMotionConnectionString = "server=sarata.pl;database=clusor6_teczka;user=clusor6_teczka;password={*UTJ8L88z$~";

  public TableReader(WebApplication app)
  {
    string InMotionConnectionString = app.Configuration.GetConnectionString("InMotionConnectionString");
    connection = new MySqlConnection(InMotionConnectionString);
  }
  //public TableReader()
  //    {
  //        connection = new MySqlConnection(InMotionConnectionString);
  //    }

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
