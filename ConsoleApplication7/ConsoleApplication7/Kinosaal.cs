using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication7
{
    public class Kinosaal
    {
        /// <summary>
        /// Kinosaal mit Reihen und Sitzen
        /// </summary>
        /// <param name="reihen"></param>
        /// <param name="sitze"></param>
        public Kinosaal(int reihen, int sitze)
        {
            // Properties festlegen
            Reihen = reihen;
            Sitze = sitze;
            AlleSitze = new bool[Reihen, Sitze];

            // Saal in den Anfangszustand versetzen
            setzeAnfangsZustand();
        }

        /// <summary>
        /// Anzahl der Reihen im Saal
        /// </summary>
        public int Reihen { get; set; }

        /// <summary>
        /// Anzahl der Sitze einer Reihe
        /// </summary>
        public int Sitze { get; set; }

        /// <summary>
        /// Kinosaal mit Reihen und Sitzen
        /// </summary>
        public bool[,] AlleSitze;

        /// <summary>
        /// Sitze aller Reihen auf true setzen
        /// </summary>
        private void setzeAnfangsZustand()
        {
            for (int reihe = 0; reihe < Reihen; reihe++) {

                for (int sitz = 0; sitz < Sitze; sitz++) {

                    AlleSitze[reihe, sitz] = true;
                }
            }
        }

        /// <summary>
        /// Gibt die Verfügbarkeit zurück
        /// </summary>
        public bool platzIstFrei(int reihe, int sitz)
        {
            return AlleSitze[reihe, sitz];
        }

        /// <summary>
        /// Belegt den Sitz
        /// </summary>
        public void belegePlatz(int reihe, int sitz)
        {
            AlleSitze[reihe, sitz] = false;
        }
    }
}
