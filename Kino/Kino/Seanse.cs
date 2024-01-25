using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    public class Seanse
    {
        
        public Film Film { get; set; }
        public Sala Sala { get; set; }
        public DateTime DataICzas { get; set; }

       
        public Seanse(Film film, Sala sala, DateTime dataICzas)
        {
            Film = film;
            Sala = sala;
            DataICzas = dataICzas;
        }
    }
}
