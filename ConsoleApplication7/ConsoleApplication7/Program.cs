using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication7
{
    public class Program
    {
        static void Main(string[] args)
        {
            Kinosaal kinosaal = new Kinosaal(10, 20);
            Kassiererin kassiererin= new Kassiererin();
            

            kassiererin.begruesseKunden();
            neueReservierung(kinosaal, kassiererin);

        }

        private static void neueReservierung(Kinosaal kinosaal, Kassiererin kassiererin)
        {
            int reihe = kassiererin.frageNachReihe();
            int sitz = kassiererin.frageNachSitz();

            if (kinosaal.platzIstFrei(reihe, sitz))
            {
                kinosaal.belegePlatz(reihe, sitz);
                kassiererin.bestaetigeReservierung(reihe, sitz);
            }
            else
            {
                kassiererin.lehneReservierungAb(reihe, sitz);
            }

            kassiererin.frageNachFortsetzung();

            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                neueReservierung(kinosaal, kassiererin);
            }
            else if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                kassiererin.verabschiedeKunden();
                Environment.Exit(0);
            }
        }
    }
}
