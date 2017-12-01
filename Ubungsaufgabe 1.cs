using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ubungsaufgabe1
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] freunde = new string[] { "Stefan", "Kevin", "Manfred" };

            printArray(freunde);

            // überschreibt wert mit leerem string >> größe bleibt
            freunde[1] = string.Empty;

            printArray(freunde);

            // fügt Uschi in die leere Stelle
            freunde[2] = "Uschi";

            printArray(freunde);

            // erstellt neues array >> lässt einen wert aus >> verkleinert
            // freunde = new string[] {freunde[0], freunde[2]};

            // erstellt neues array >> fügt Uschi hinten an >> vergrößert
            // freunde = new string[] {freunde[0], freunde[1], "Uschi"};

            //printArray(freunde);

        }

        private static void printArray(string[] array)
        {
            foreach (string element in array)
            {
                Console.WriteLine(element + " ");
            }
        }
    }
}
