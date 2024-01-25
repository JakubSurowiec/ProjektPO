using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Uzytkownik
    {
        public string Nazwa { get; set; }
        public string TypUzytkownika { get; set; } 
        public double Znizka { get; set; }

        public Uzytkownik(string nazwa, string typUzytkownika)
        {
            Nazwa = nazwa;
            TypUzytkownika = typUzytkownika;
            Znizka = UstalZnizke(typUzytkownika);
        }

        private double UstalZnizke(string typUzytkownika)
        {
            
            if (typUzytkownika == "pracownik") return 0.3; // 30% zniżki
            if (typUzytkownika == "standard") return 0.0; // Brak zniżki
                                                         
            return 0.0;
        }
    }
}
