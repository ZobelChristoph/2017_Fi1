using MySql.Data.MySqlClient;
using NormalizeDB.Datamodels;
using NormalizeDB.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NormalizeDB
{
  class Program
  {
    static void Main(string[] args)
    {
      ConsoleLogger logger = new ConsoleLogger();
      try
      {
        ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["MySqlStolpersteine"];
        logger.Log($"ConnectionString: {setting.ConnectionString}", LogType.Special);

        logger.Log("Press Enter to continue...");
        Console.ReadLine();

        using (MySqlConnection connection = new MySqlConnection(setting.ConnectionString))
        {
          connection.Open();

          /* * * * * * * * * * * * * * * * * * *
           * Step 1: Read table [tbl_st_opfer] *
           * * * * * * * * * * * * * * * * * * */
          List<Opfer> victims = CollectVictims(logger, connection);

          /* * * * * * * * * * * * * * * * * * * * * * *
           * Step 2: Read table [tbl_st_maps_address]  *
           * * * * * * * * * * * * * * * * * * * * * * */
          List<Stolperstein> stones = CollectMemorialStones(logger, connection);

          /* * * * * * * * * * * * * * *
           * Step 3: Create matchings  *
           * * * * * * * * * * * * * * */
          foreach (Stolperstein stone in stones)
          {
            var matches = FindMatch(stone, victims);

            if (matches.Count == 0)
            {
              logger.Log("No matching victims for this stone.", LogType.Warning);
            }

            if (matches.Count > 1)
            {
              logger.Log("Multiple matching victims for this stone.", LogType.Warning);
            }
          }

          connection.Close();
        }
        logger.Log("Finished.", LogType.Special);
      }
      catch (Exception exception)
      {
        logger.Log(exception.Message, LogType.Error);
      }
      finally
      {
        Console.ReadLine();
        Environment.Exit(0);
      }
    }

    #region Outsourced Code

    private static List<Opfer> FindMatch(Stolperstein stone, List<Opfer> victims)
    {
      List<Opfer> matches = new List<Opfer>();

      foreach (Opfer victim in victims)
      {
        if (victim.Geburtsort == stone.Ort 
          && victim.Strasse == stone.Strasse
          && victim.Hausnummer == stone.Hausnummer)
        {
          matches.Add(victim);
        }
      }

      return matches;
    }

    private static List<Opfer> CollectVictims(ConsoleLogger logger, MySqlConnection connection)
    {
      List<Opfer> victims = new List<Opfer>();

      MySqlCommand command = new MySqlCommand("SELECT opfer_id, strasse, hausnummer, geburtsort FROM tbl_st_opfer;", connection);

      using (MySqlDataReader reader = command.ExecuteReader())
      {
        int col1 = reader.GetOrdinal("opfer_id");
        int col2 = reader.GetOrdinal("strasse");
        int col3 = reader.GetOrdinal("hausnummer");
        int col4 = reader.GetOrdinal("geburtsort");

        int counter = 0;

        while (reader.Read())
        {
          object obj1 = reader.GetValue(col1);
          object obj2 = reader.GetValue(col2);
          object obj3 = reader.GetValue(col3);
          object obj4 = reader.GetValue(col4);

          int val1 = Convert.ToInt32(obj1);
          string val2 = obj2 == DBNull.Value ? null : (string)obj2;
          string val3 = obj3 == DBNull.Value ? null : (string)obj3;
          string val4 = obj4 == DBNull.Value ? null : (string)obj4;

          Opfer opfer = new Opfer()
          {
            ID = val1,
            Strasse = val2,
            Hausnummer = val3,
            Geburtsort = val4
          };
          logger.Log($"[{counter}] {opfer.ToString()}");

          victims.Add(opfer);
          counter += 1;
        }
      }
      return victims;
    }

    private static List<Stolperstein> CollectMemorialStones(ConsoleLogger logger, MySqlConnection connection)
    {
      List<Stolperstein> memorialStones = new List<Stolperstein>();

      MySqlCommand command = new MySqlCommand("SELECT id, strasse, hausnummer, ort FROM tbl_st_maps_address;", connection);

      using (MySqlDataReader reader = command.ExecuteReader())
      {
        int col1 = reader.GetOrdinal("id");
        int col2 = reader.GetOrdinal("strasse");
        int col3 = reader.GetOrdinal("hausnummer");
        int col4 = reader.GetOrdinal("ort");

        int counter = 0;

        while (reader.Read())
        {
          object obj1 = reader.GetValue(col1);
          object obj2 = reader.GetValue(col2);
          object obj3 = reader.GetValue(col3);
          object obj4 = reader.GetValue(col4);

          int val1 = Convert.ToInt32(obj1);
          string val2 = obj2 == DBNull.Value ? null : (string)obj2;
          string val3 = obj3 == DBNull.Value ? null : (string)obj3;
          string val4 = obj4 == DBNull.Value ? null : (string)obj4;

          Stolperstein stolperstein = new Stolperstein()
          {
            ID = val1,
            Strasse = val2,
            Hausnummer = val3,
            Ort = val4
          };
          logger.Log($"[{counter}] {stolperstein.ToString()}");

          memorialStones.Add(stolperstein);
          counter += 1;
        }
      }
      return memorialStones;
    }
  }

  #endregion
}