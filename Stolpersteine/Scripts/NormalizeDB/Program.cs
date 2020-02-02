using MySql.Data.MySqlClient;
using NormalizeDB.Datamodels;
using NormalizeDB.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace NormalizeDB
{
  class Program
  {
    static void Main(string[] args)
    {
      ConsoleLogger consoleLogger = new ConsoleLogger();
      try
      {
        ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["MySqlStolpersteine"];
        consoleLogger.Log($"ConnectionString: {setting.ConnectionString}", LogType.Special);

        consoleLogger.Log("Press Enter to continue...");
        Console.ReadLine();

        using (MySqlConnection mySqlConnection = new MySqlConnection(setting.ConnectionString))
        {
          mySqlConnection.Open();

          /* * * * * * * * * * * * * * * * * * *
           * Step 1: Read table [tbl_st_opfer] *
           * * * * * * * * * * * * * * * * * * */
          List<Opfer> victims = CollectVictims(consoleLogger, mySqlConnection);

          /* * * * * * * * * * * * * * * * * * * * * * *
           * Step 2: Read table [tbl_st_maps_address]  *
           * * * * * * * * * * * * * * * * * * * * * * */
          List<Stolperstein> stones = CollectMemorialStones(consoleLogger, mySqlConnection);

          /* * * * * * * * * * * * * * *
           * Step 3: Create matchings  *
           * * * * * * * * * * * * * * */
          var exactMatching = new Dictionary<Stolperstein, Opfer>();
          var notMatching = new List<Stolperstein>();
          var multipleMatching = new Dictionary<Stolperstein, List<Opfer>>();

          foreach (Stolperstein stone in stones)
          {
            List<Opfer> matches = FindMatches(stone, victims);

            if (matches.Count == 0)
            {
              notMatching.Add(stone);
              consoleLogger.Log("No matching victims for this stone.", LogType.Warning);
            }
            else if (matches.Count > 1)
            {
              multipleMatching.Add(stone, matches);
              consoleLogger.Log("Multiple matching victims for this stone.", LogType.Warning);
            }
            else
            {
              exactMatching.Add(stone, matches[0]);
            }
          }

          /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
           * Step 4: Update table [tbl_st_opfer] with geo-data from matchings) *
           * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
          using (MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction())
          {
            int updatedRows = 0;
            foreach (KeyValuePair<Stolperstein, Opfer> matching in exactMatching)
            {
              MySqlCommand updateCommand = new MySqlCommand
              (
                cmdText: "UPDATE tbl_st_opfer SET stolperstein_id = @fkValue WHERE opfer_id = @idValue;",
                connection: mySqlConnection,
                transaction: mySqlTransaction
              );

              updateCommand.Parameters.AddRange(new MySqlParameter[]
              {
                new MySqlParameter("@fkValue", matching.Key.ID),
                new MySqlParameter("@idValue", matching.Value.ID)
              });

              updatedRows += updateCommand.ExecuteNonQuery();
              consoleLogger.Log($"Updated {matching.Value.GetType().Name} ({nameof(matching.Value.ID)}) {matching.Value.ID.ToString()}:\t inserted foreign key of {matching.Key.GetType().Name} ({nameof(matching.Key.ID)}) {matching.Key.ID.ToString()}.", LogType.Success);
            }

            mySqlTransaction.Commit();
          }

          mySqlConnection.Close();
        }
        consoleLogger.Log("Finished.", LogType.Special);
      }
      catch (Exception exception)
      {
        consoleLogger.Log(exception.Message, LogType.Error);
      }
      finally
      {
        Console.ReadLine();
        Environment.Exit(0);
      }
    }

    #region Outsourced Code

    private static List<Opfer> FindMatches(Stolperstein stone, List<Opfer> victims)
    {
      List<Opfer> matches = new List<Opfer>();

      foreach (Opfer victim in victims)
      {
        if (victim.Geburtsort == stone.Ort
          && victim.Strasse == stone.Strasse
          && victim.Hausnummer == stone.Hausnummer)
        {
          victim.MatchingCount += 1;
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