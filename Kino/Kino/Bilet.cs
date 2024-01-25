using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Bilet
    {
        public Seanse Seanse { get; set; }
        public List<Miejsce> WybraneMiejsca { get; set; }
        public double Cena { get; set; }
        public double Znizka { get; set; }

        public Bilet(Seanse seanse, List<Miejsce> wybraneMiejsca, double znizka)
        {
            Seanse = seanse;
            WybraneMiejsca = wybraneMiejsca;
            Znizka = znizka;
            Cena = ObliczCene();
        }

        private double ObliczCene()
        {
         
            double cenaBazowa = 20.0;
            return cenaBazowa * WybraneMiejsca.Count * (1 - Znizka);
        }
    }
}
