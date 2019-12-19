using System;

namespace NormalizeDB.Logging
{
  public class ConsoleLogger
  {
    public void Log(string message, LogType logType = LogType.Info)
    {
      Console.ForegroundColor = this.GetConsoleColor(logType);
      Console.WriteLine(message);
    }

    private ConsoleColor GetConsoleColor(LogType logType)
    {
      switch (logType)
      {
        case LogType.Info:
          return ConsoleColor.White;

        case LogType.Success:
          return ConsoleColor.Green;

        case LogType.Warning:
          return ConsoleColor.Yellow;

        case LogType.Error:
          return ConsoleColor.Red;

        case LogType.Special:
          return ConsoleColor.Cyan;

        default:
          throw new NotSupportedException($"{nameof(LogType)} {nameof(logType)} was not supported.");
      }
    }
  }
}