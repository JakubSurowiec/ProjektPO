using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Rezerwacja
    {
        public int NumerMiejsca { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }

        public Rezerwacja(int numerMiejsca, string imie, string nazwisko)
        {
            NumerMiejsca = numerMiejsca;
            Imie = imie;
            Nazwisko = nazwisko;
        }
    }

    public class Rezerwacje
    {
        private List<Rezerwacja> listaRezerwacji = new List<Rezerwacja>();
    }
}
