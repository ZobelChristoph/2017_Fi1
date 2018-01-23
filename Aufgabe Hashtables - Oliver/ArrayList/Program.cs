using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ArrayList
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable htawp = new Hashtable();
            Hashtable htbwp = new Hashtable();

            htawp.Add("Robin", 6);
            htawp.Add("Dennis", 2);
            htawp.Add("Stefan", 3);

            htbwp.Add("Robin", 3);
            htbwp.Add("Dennis", 2);
            htbwp.Add("Stefan", 1);

            durchschnittsnotenberechnung(htawp,htbwp);
            Console.ReadLine();
        }

        static void durchschnittsnotenberechnung(Hashtable htawp, Hashtable htbwp)
        {
            loop("Fach AWP Schüler: {0}, Note: {1}", htawp);
            loop("Fach BWP Schüler: {0}, Note: {1}", htbwp);
            Console.WriteLine("---");
            Hashtable ds = durchschnitt(htawp, htbwp);
            loop("Schüler: {0}, Durchschnitt: {1}",ds);
        }

        static void loop(string msg, Hashtable tb)
        {
            foreach (DictionaryEntry entry in tb)
            {
                Console.WriteLine(msg, entry.Key, entry.Value);
            }
        }

        static Hashtable durchschnitt(Hashtable htawp, Hashtable htbwp)
        {
            Hashtable res = new Hashtable();
            foreach (DictionaryEntry entry in htawp)
            {
                double awp = Convert.ToInt32(entry.Value);
                double bwp = Convert.ToInt32(htbwp[entry.Key]);
                double sum = (awp + bwp) / 2;
                res.Add(entry.Key, sum);
            }
            return res;
        }
    }
}
