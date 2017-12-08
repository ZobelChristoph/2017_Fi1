using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication7
{
    public class Kassiererin
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public Kassiererin() {

        }

        public void begruesseKunden()
        {
            Console.WriteLine("Willkommen im Kino!\n\n");
        }

        public void verabschiedeKunden()
        {
            Console.WriteLine("\n\nAuf Wiedersehen!");
        }

        public void bestaetigeReservierung(int reihe, int sitz)
        {
            Console.WriteLine("Der Platz R" + reihe.ToString() + "-S" + sitz.ToString() + " ist jetzt für Sie reserviert.");
        }

        public void lehneReservierungAb(int reihe, int sitz)
        {
            Console.WriteLine("Der Platz R" + reihe.ToString() + "-S" + sitz.ToString() + " ist leider schon belegt.");
        }

        public void frageNachFortsetzung()
        {
            Console.WriteLine("Möchten Sie fortfahren? Ja: ENTER | Nein: SPACE");
        }

        public int frageNachReihe()
        {
            Console.WriteLine("In welcher Reihe möchten Sie sitzen? (1-10)");
            return int.Parse(Console.ReadLine());
        }

        public int frageNachSitz()
        {
            Console.WriteLine("In welchem Platz möchten Sie sitzen? (1-20)");
            return int.Parse(Console.ReadLine());
        }
    }
}
