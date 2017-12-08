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

            // lösung 1

            freunde[1] = string.Empty;

            printArray(freunde);

            freunde[1] = "Uschi";

            printArray(freunde);

            // lösung 2

            //freunde = new string[] { freunde[0], freunde[2] };

            //printArray(freunde);

            //freunde = new string[] { freunde[0], freunde[1], "Uschi" };

            //printArray(freunde);

        }

        private static void printArray(string[] array)
        {
            foreach (string element in array)
            {
                Console.WriteLine(element + " ");
            }
            Console.WriteLine("--------");
        }
    }
}
