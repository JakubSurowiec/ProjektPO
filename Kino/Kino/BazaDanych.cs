using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kino
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class BazaDanych
    {
        private string sciezkaDoFilmow = "filmy.txt";
        private string sciezkaDoSal = "sale.txt";
        private string sciezkaDoSeansow = "seanse.txt";
        private string sciezkaDoUzytkownikow = "uzytkownicy.txt";
        private string sciezkaDoMiejsc = "miejsca.txt";

        //OPERACJE CRUD DLA FILMU


        public void DodajFilm(Film film)
        {
            var zapis = $"{film.Tytul};{film.Opis};{film.Dlugosc}\n";
            File.AppendAllText(sciezkaDoFilmow, zapis);
        }

        public List<Film> PobierzFilmy()
        {
            var filmy = new List<Film>();
            var linie = File.ReadAllLines(sciezkaDoFilmow);

            foreach (var linia in linie)
            {
                var dane = linia.Split(';');
                var film = new Film(dane[0], dane[1], int.Parse(dane[2]));
                filmy.Add(film);
            }

            return filmy;
        }

        public void AktualizujFilm(string tytul, Film noweDane)
        {
            var filmy = PobierzFilmy();
            var filmyDoZapisu = new List<string>();

            foreach (var film in filmy)
            {
                if (film.Tytul == tytul)
                {
                    film.Tytul = noweDane.Tytul;
                    film.Opis = noweDane.Opis;
                    film.Dlugosc = noweDane.Dlugosc;
                }
                filmyDoZapisu.Add($"{film.Tytul};{film.Opis};{film.Dlugosc}");
            }

            File.WriteAllLines(sciezkaDoFilmow, filmyDoZapisu);
        }

        public void UsunFilm(string tytul)
        {
            var filmy = PobierzFilmy();
            var filmyDoZachowania = filmy.Where(f => f.Tytul != tytul).ToList();

            var filmyDoZapisu = filmyDoZachowania.Select(f => $"{f.Tytul};{f.Opis};{f.Dlugosc}").ToList();
            File.WriteAllLines(sciezkaDoFilmow, filmyDoZapisu);
        }




        //OPERACJE CRUD DLA SAL
        public void DodajSale(Sala sala)
        {
            var zapis = $"{sala.NumerSali};{sala.LiczbaRzedow};{sala.MiejscaWRzedzie}\n";
            File.AppendAllText(sciezkaDoSal, zapis);

            Random random = new Random();
            for (int rzad = 1; rzad <= sala.LiczbaRzedow; rzad++)
            {
                for (int numer = 1; numer <= sala.MiejscaWRzedzie; numer++)
                {
                    bool czyZajete = random.Next(0, 10) < 2;  //Losowanie kilku zajętych miejsc
                    DodajMiejsce(new Miejsce(rzad, numer));
                }
            }
        }

        public List<Sala> PobierzSale()
        {
            var sale = new List<Sala>();
            var linie = File.ReadAllLines(sciezkaDoSal);

            foreach (var linia in linie)
            {
                var dane = linia.Split(';');
                var sala = new Sala(int.Parse(dane[0]), int.Parse(dane[1]), int.Parse(dane[2]));
                sale.Add(sala);
            }

            return sale;
        }

        public void AktualizujSale(int numerSali, Sala noweDane)
        {
            var sale = PobierzSale();
            var saleDoZapisu = new List<string>();

            foreach (var sala in sale)
            {
                if (sala.NumerSali == numerSali)
                {
                    sala.NumerSali = noweDane.NumerSali;
                    sala.LiczbaRzedow = noweDane.LiczbaRzedow;
                    sala.MiejscaWRzedzie = noweDane.MiejscaWRzedzie;
                }
                saleDoZapisu.Add($"{sala.NumerSali};{sala.LiczbaRzedow};{sala.MiejscaWRzedzie}");
            }

            File.WriteAllLines(sciezkaDoSal, saleDoZapisu);
        }

        public void UsunSale(int numerSali)
        {
            var sale = PobierzSale();
            var saleDoZachowania = sale.Where(s => s.NumerSali != numerSali).ToList();

            var saleDoZapisu = saleDoZachowania.Select(s => $"{s.NumerSali};{s.LiczbaRzedow};{s.MiejscaWRzedzie}").ToList();
            File.WriteAllLines(sciezkaDoSal, saleDoZapisu);
        }




        //OPERACJE CRUD DLA SEANSU
        public void DodajSeans(Seanse seans)
        {
            var zapis = $"{seans.Film.Tytul};{seans.Sala.NumerSali};{seans.DataICzas}\n";
            File.AppendAllText(sciezkaDoSeansow, zapis);
        }

        public List<Seanse> PobierzSeanse()
        {
            var seanse = new List<Seanse>();
            var linie = File.ReadAllLines(sciezkaDoSeansow);

            foreach (var linia in linie)
            {
                var dane = linia.Split(';');
                
                var film = PobierzFilmy().FirstOrDefault(f => f.Tytul == dane[0]);
                var sala = PobierzSale().FirstOrDefault(s => s.NumerSali == int.Parse(dane[1]));
                var dataICzas = DateTime.Parse(dane[2]);

                if (film != null && sala != null)
                {
                    var seans = new Seanse(film, sala, dataICzas);
                    seanse.Add(seans);
                }
            }

            return seanse;
        }

       
        public void AktualizujSeans(Seanse starySeans, Seanse nowySeans)
{
    var seanse = PobierzSeanse();
    var indeks = seanse.FindIndex(s => s.Film.Tytul == starySeans.Film.Tytul && s.DataICzas == starySeans.DataICzas);

    if (indeks != -1)
    {
        seanse[indeks] = nowySeans; 
    }
    else
    {
        Console.WriteLine("Nie znaleziono seansu do aktualizacji.");
        return;
    }

    ZapiszSeanse(seanse); 
}

private void ZapiszSeanse(List<Seanse> seanse)
{
    var linie = seanse.Select(s => $"{s.Film.Tytul};{s.Sala.NumerSali};{s.DataICzas}").ToList();
    File.WriteAllLines(sciezkaDoSeansow, linie);
}


        public void UsunSeans(Film film)
        {
            var seanse = PobierzSeanse();
            seanse.RemoveAll(s => s.Film.Tytul == film.Tytul); 

       
            var seanseDoZapisu = seanse.Select(s => $"{s.Film.Tytul};{s.Sala.NumerSali};{s.DataICzas}").ToList();
            File.WriteAllLines(sciezkaDoSeansow, seanseDoZapisu);
        }



        //OPERACJE CRUD DLA UZYTKOWNIKOW
        public void DodajUzytkownika(Uzytkownik uzytkownik)
        {
            var zapis = $"{uzytkownik.Nazwa};{uzytkownik.TypUzytkownika};{uzytkownik.Znizka}\n";
            File.AppendAllText(sciezkaDoUzytkownikow, zapis);
        }

        public List<Uzytkownik> PobierzUzytkownikow()
        {
            var uzytkownicy = new List<Uzytkownik>();
            var linie = File.ReadAllLines(sciezkaDoUzytkownikow);

            foreach (var linia in linie)
            {
                var dane = linia.Split(';');
                var uzytkownik = new Uzytkownik(dane[0], dane[1])
                {
                    Znizka = double.Parse(dane[2])
                };
                uzytkownicy.Add(uzytkownik);
            }

            return uzytkownicy;
        }

        public void AktualizujUzytkownika(string nazwa, Uzytkownik noweDane)
        {
            var uzytkownicy = PobierzUzytkownikow();
            var uzytkownicyDoZapisu = new List<string>();

            foreach (var uzytkownik in uzytkownicy)
            {
                if (uzytkownik.Nazwa == nazwa)
                {
                    uzytkownik.TypUzytkownika = noweDane.TypUzytkownika;
                    uzytkownik.Znizka = noweDane.Znizka;
                }
                uzytkownicyDoZapisu.Add($"{uzytkownik.Nazwa};{uzytkownik.TypUzytkownika};{uzytkownik.Znizka}");
            }

            File.WriteAllLines(sciezkaDoUzytkownikow, uzytkownicyDoZapisu);
        }

        public void UsunUzytkownika(string nazwa)
        {
            var uzytkownicy = PobierzUzytkownikow();
            var uzytkownicyDoZachowania = uzytkownicy.Where(u => u.Nazwa != nazwa).ToList();

            var uzytkownicyDoZapisu = uzytkownicyDoZachowania.Select(u => $"{u.Nazwa};{u.TypUzytkownika};{u.Znizka}").ToList();
            File.WriteAllLines(sciezkaDoUzytkownikow, uzytkownicyDoZapisu);
        }



        //OPERACJE CRUD DLA MIEJSC

        public void DodajMiejsce(Miejsce miejsce)
        {
            var zapis = $"{miejsce.Rzad};{miejsce.Numer};{miejsce.CzyZajete}\n";
            File.AppendAllText(sciezkaDoMiejsc, zapis);
        }

        public List<Miejsce> PobierzMiejsca()
        {
            var miejsca = new List<Miejsce>();
            var linie = File.ReadAllLines(sciezkaDoMiejsc);

            foreach (var linia in linie)
            {
                var dane = linia.Split(';');
                var rzad = int.Parse(dane[0]);
                var numer = int.Parse(dane[1]);
                var czyZajete = bool.Parse(dane[2]);

                var miejsce = new Miejsce(rzad, numer)
                {
                    CzyZajete = czyZajete
                };
                miejsca.Add(miejsce);
            }

            return miejsca;
        }

        public void AktualizujMiejsce(int rzad, int numer, Miejsce noweDane)
        {
            var miejsca = PobierzMiejsca();
            var miejscaDoZapisu = new List<string>();

            foreach (var miejsce in miejsca)
            {
                if (miejsce.Rzad == rzad && miejsce.Numer == numer)
                {
                    miejsce.CzyZajete = noweDane.CzyZajete;
                }
                miejscaDoZapisu.Add($"{miejsce.Rzad};{miejsce.Numer};{miejsce.CzyZajete}");
            }

            File.WriteAllLines(sciezkaDoMiejsc, miejscaDoZapisu);
        }

        public void UsunMiejsce(int rzad, int numer)
        {
            var miejsca = PobierzMiejsca();
            var miejscaDoZachowania = miejsca.Where(m => m.Rzad != rzad || m.Numer != numer).ToList();

            var miejscaDoZapisu = miejscaDoZachowania.Select(m => $"{m.Rzad};{m.Numer};{m.CzyZajete}").ToList();
            File.WriteAllLines(sciezkaDoMiejsc, miejscaDoZapisu);
        }

      
    }


}
