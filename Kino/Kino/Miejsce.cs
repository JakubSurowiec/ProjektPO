using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Miejsce
    {
        public int Rzad { get; private set; }
        public int Numer { get; private set; }
        public bool CzyZajete { get; set; }

        public Miejsce(int rzad, int numer)
        {
            Rzad = rzad;
            Numer = numer;
            CzyZajete = false;
        }
    }
}
