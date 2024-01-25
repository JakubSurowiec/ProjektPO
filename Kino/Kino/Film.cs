using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Film
    {
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public int Dlugosc { get; set; } // Długość w minutach

        public Film(string tytul, string opis, int dlugosc)
        {
            Tytul = tytul;
            Opis = opis;
            Dlugosc = dlugosc;
        }
    }
}
