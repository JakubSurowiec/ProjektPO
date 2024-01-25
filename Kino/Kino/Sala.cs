using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Sala
    {
        
        public int NumerSali { get; set; }
        public int LiczbaRzedow { get; set; }
        public int MiejscaWRzedzie { get; set; }

        public Sala(int numerSali, int liczbaRzedow, int miejscaWRzedzie)
        {
            NumerSali = numerSali;
            LiczbaRzedow = liczbaRzedow;
            MiejscaWRzedzie = miejscaWRzedzie;
        }
    }
}
