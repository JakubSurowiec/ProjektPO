using Kino;
using System.Globalization;

class Program
{
    static BazaDanych bazaDanych = new BazaDanych();
    private static List<Sala> listaSal = new List<Sala>();

    static void Main(string[] args)
    {
        bool dziala = true;
        while (dziala)
        {
            Console.WriteLine("\nWitaj w systemie rezerwacji biletów do kina!");
            Console.WriteLine("1. Zarzadzaj Filmami");
            Console.WriteLine("2. Zarzadzaj seansami");
            Console.WriteLine("3. Zarzadaj uzytkownikami");
            Console.WriteLine("4. Zarzadaj salami");
            Console.WriteLine("5. Rezerwuj miejsce");
            Console.WriteLine("9. Wyjście");

            var wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    ZarzadzajFilmami();
                    break;
                case "2":
                    ZarzadzajSeansami();
                    break;
                case "3":
                    ZarzadzajUzytkownikami();
                    break;
                case "4":
                   ZarzadzajSalami();
                    break;
                case "5":
                    RezerwujMiejsce();
                    break;
                case "9":
                    dziala = false;
                    break;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    }

    static void RezerwujMiejsce()
    {
        Console.Clear();
        Console.WriteLine("Rezerwacja miejsca.");

        var seanse = bazaDanych.PobierzSeanse();
        var uzytkownicy = bazaDanych.PobierzUzytkownikow();

        if (!seanse.Any())
        {
            Console.WriteLine("Brak dostępnych seansów.");
            return;
        }

       
        
        // Wybór Seansu

        if (!seanse.Any())
        {
            Console.WriteLine("Brak dostępnych seansów.");
            return;
        }

        Console.WriteLine("Dostępne seanse:");
        foreach (var seans in seanse)
        {
            Console.WriteLine($"Film: {seans.Film.Tytul}, Sala: {seans.Sala.NumerSali}, Data i czas: {seans.DataICzas}");
        }

        Console.Write("Wybierz seans (podaj tytuł filmu): ");
        string wybranyTytul = Console.ReadLine();
        var wybranySeans = seanse.FirstOrDefault(s => s.Film.Tytul.ToLower() == wybranyTytul.ToLower());

        if (wybranySeans == null)
        {
            Console.WriteLine("Seans nie został znaleziony.");
            return;
        }




        // Wyświetl dostępne miejsca
        var miejsca = bazaDanych.PobierzMiejsca();
        var dostepneMiejsca = miejsca.Where(m => !m.CzyZajete).ToList();

        if (!dostepneMiejsca.Any())
        {
            Console.WriteLine("Brak dostępnych miejsc na ten seans.");
            return;
        }

        Console.WriteLine("Dostępne miejsca:");
        foreach (var miejsce in dostepneMiejsca)
        {
            Console.WriteLine($"Rząd: {miejsce.Rzad}, Numer: {miejsce.Numer}");
        }




        // Wybór miejsc
        Console.Write("Wybierz miejsce (podaj rząd i numer, np. 3 5): ");
        var wybraneMiejsceInput = Console.ReadLine().Split(' ');
        int rzad, numer;
        if (wybraneMiejsceInput.Length != 2 || !int.TryParse(wybraneMiejsceInput[0], out rzad) || !int.TryParse(wybraneMiejsceInput[1], out numer))
        {
            Console.WriteLine("Nieprawidłowy format wyboru miejsca.");
            return;
        }

        var wybraneMiejsce = miejsca.FirstOrDefault(m => m.Rzad == rzad && m.Numer == numer);

        if (wybraneMiejsce == null || wybraneMiejsce.CzyZajete)
        {
            Console.WriteLine("Miejsce jest już zajęte lub nie istnieje.");
            return;
        }

        Console.Write("Podaj nazwę użytkownika: ");
        string nazwaUzytkownika = Console.ReadLine();
        var uzytkownik = uzytkownicy.FirstOrDefault(u => u.Nazwa.ToLower() == nazwaUzytkownika.ToLower());

        if (uzytkownik == null)
        {
            Console.WriteLine("Nie znaleziono użytkownika.");
            return;
        }




   
        wybraneMiejsce.CzyZajete = true;
        var bilet = new Bilet(wybranySeans, new List<Miejsce> { wybraneMiejsce }, uzytkownik.Znizka);

   
        Console.WriteLine($"Miejsce Rząd: {wybraneMiejsce.Rzad}, Numer: {wybraneMiejsce.Numer} zostało zarezerwowane.");
        Console.WriteLine($"Cena biletu: {bilet.Cena} zł");

   
        bazaDanych.AktualizujMiejsce(rzad, numer, wybraneMiejsce);
    }




    static void ZarzadzajUzytkownikami()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Zarządzanie użytkownikami:");
            Console.WriteLine("1. Dodaj nowego użytkownika");
            Console.WriteLine("2. Wyświetl dostępnych użytkowników");
            Console.WriteLine("3. Aktualizuj dane użytkownika");
            Console.WriteLine("4. Usuń użytkownika");
            Console.WriteLine("5. Powrót do głównego menu");

            var wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajUzytkownika();
                    break;
                case "2":
                    WyswietlDostepnychUzytkownikow();
                    break;
                case "3":
                    AktualizujDaneUzytkownika();
                    break;
                case "4":
                    UsunUzytkownika();
                    break;
                case "5":
                    return; 
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    }



    static void DodajUzytkownika()
    {
        Console.Clear();
        Console.WriteLine("Dodawanie nowego użytkownika.");

        Console.Write("Podaj nazwę użytkownika: ");
        string nazwa = Console.ReadLine();

        Console.Write("Podaj typ użytkownika (np. 'standard', 'pracownik'): ");
        string typUzytkownika = Console.ReadLine();
        double znizka = UstalZnizke(typUzytkownika);

        var uzytkownik = new Uzytkownik(nazwa, typUzytkownika)
        {
            Znizka = znizka
        };
        bazaDanych.DodajUzytkownika(uzytkownik);
        Console.WriteLine("Użytkownik został dodany.");
        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static double UstalZnizke(string typUzytkownika)
    {

        if (typUzytkownika == "pracownik") 
            return 0.3; // 30% zniżki
        if (typUzytkownika == "standard") 
            return 0.0; // Brak zniżki

        return 0.0; 
    }

    static void WyswietlDostepnychUzytkownikow()
    {
        Console.Clear();
        Console.WriteLine("Dostępni użytkownicy:");

        var uzytkownicy = bazaDanych.PobierzUzytkownikow();

        if (uzytkownicy.Any())
        {
            foreach (var uzytkownik in uzytkownicy)
            {
                Console.WriteLine($"Nazwa: {uzytkownik.Nazwa}, Typ: {uzytkownik.TypUzytkownika}, Zniżka: {uzytkownik.Znizka}");
            }
        }
        else
        {
            Console.WriteLine("Brak dostępnych użytkowników.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static void AktualizujDaneUzytkownika()
    {
        Console.Clear();
        Console.WriteLine("Aktualizacja danych użytkownika.");

        Console.Write("Podaj nazwę użytkownika do aktualizacji: ");
        string nazwa = Console.ReadLine();

        var uzytkownikDoAktualizacji = bazaDanych.PobierzUzytkownikow().FirstOrDefault(u => u.Nazwa == nazwa);

        if (uzytkownikDoAktualizacji != null)
        {
            Console.Write("Nowy typ użytkownika (np. 'standard', 'pracownik'): ");
            string nowyTypUzytkownika = Console.ReadLine();
            double znizka = UstalZnizke(nowyTypUzytkownika);

            uzytkownikDoAktualizacji.TypUzytkownika = nowyTypUzytkownika;
            uzytkownikDoAktualizacji.Znizka = znizka;

            bazaDanych.AktualizujUzytkownika(nazwa, uzytkownikDoAktualizacji);
            Console.WriteLine("Dane użytkownika zostały zaktualizowane.");
        }
        else
        {
            Console.WriteLine("Użytkownik o podanej nazwie nie istnieje.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static void UsunUzytkownika()
    {
        Console.Clear();
        Console.WriteLine("Usuwanie użytkownika.");
        Console.Write("Podaj nazwę użytkownika do usunięcia: ");
        string nazwa = Console.ReadLine();

        var uzytkownikDoUsuniecia = bazaDanych.PobierzUzytkownikow().FirstOrDefault(u => u.Nazwa == nazwa);
        if (uzytkownikDoUsuniecia != null)
        {
            bazaDanych.UsunUzytkownika(nazwa);
            Console.WriteLine("Użytkownik został usunięty.");
        }
        else
        {
            Console.WriteLine("Użytkownik o podanej nazwie nie istnieje.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static void ZarzadzajSalami()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Zarządzanie salami:");
            Console.WriteLine("1. Dodaj nową salę");
            Console.WriteLine("2. Wyświetl dostępne sale");
            Console.WriteLine("3. Aktualizuj dane sali");
            Console.WriteLine("4. Usuń salę");
            Console.WriteLine("5. Powrót do głównego menu");
            var wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajSale();
                    break;
                case "2":
                    WyswietlDostepneSale();
                    break;
                case "3":
                    AktualizujSale();
                    break;
                case "4":
                    UsunSale();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    }

    static void DodajSale()
    {
        Console.Clear();
        Console.WriteLine("Dodawanie nowej sali.");
        Console.Write("Podaj numer sali: ");
        int numerSali;
        if (int.TryParse(Console.ReadLine(), out numerSali))
        {
            Console.Write("Podaj liczbę rzędów: ");
            int liczbaRzedow;
            if (int.TryParse(Console.ReadLine(), out liczbaRzedow))
            {
                Console.Write("Podaj liczbę miejsc w rzędzie: ");
                int miejscaWRzedzie;
                if (int.TryParse(Console.ReadLine(), out miejscaWRzedzie))
                {
                    var sala = new Sala(numerSali, liczbaRzedow, miejscaWRzedzie);
                    bazaDanych.DodajSale(sala);
                    Console.WriteLine("Sala została dodana.");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy format liczby miejsc w rzędzie.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy format liczby rzędów.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowy format numeru sali.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }
    static void WyswietlDostepneSale()
    {
        Console.Clear();
        Console.WriteLine("Dostępne sale:");

        var sale = bazaDanych.PobierzSale();

        if (sale.Any())
        {
            foreach (var sala in sale)
            {
                Console.WriteLine($"Numer sali: {sala.NumerSali}, Liczba rzędów: {sala.LiczbaRzedow}, Miejsca w rzędzie: {sala.MiejscaWRzedzie}");
            }
        }
        else
        {
            Console.WriteLine("Brak dostępnych sal.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }
    static void AktualizujSale()
    {
        Console.Clear();
        Console.WriteLine("Aktualizacja danych sali.");

        Console.Write("Podaj numer sali do aktualizacji: ");
        int numerSali;
        if (int.TryParse(Console.ReadLine(), out numerSali))
        {
            var sala = bazaDanych.PobierzSale().FirstOrDefault(s => s.NumerSali == numerSali);

            if (sala != null)
            {
                Console.Write("Nowa liczba rzędów: ");
                int nowaLiczbaRzedow;
                if (int.TryParse(Console.ReadLine(), out nowaLiczbaRzedow))
                {
                    Console.Write("Nowa liczba miejsc w rzędzie: ");
                    int noweMiejscaWRzedzie;
                    if (int.TryParse(Console.ReadLine(), out noweMiejscaWRzedzie))
                    {
                        sala.LiczbaRzedow = nowaLiczbaRzedow;
                        sala.MiejscaWRzedzie = noweMiejscaWRzedzie;
                        bazaDanych.AktualizujSale(numerSali, sala);
                        Console.WriteLine("Dane sali zostały zaktualizowane.");
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format liczby miejsc w rzędzie.");
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy format liczby rzędów.");
                }
            }
            else
            {
                Console.WriteLine("Sala o podanym numerze nie istnieje.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowy format numeru sali.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static void UsunSale()
    {
        Console.Clear();
        Console.WriteLine("Usuwanie sali.");

        Console.Write("Podaj numer sali do usunięcia: ");
        int numerSali;
        if (int.TryParse(Console.ReadLine(), out numerSali))
        {
            var sala = bazaDanych.PobierzSale().FirstOrDefault(s => s.NumerSali == numerSali);

            if (sala != null)
            {
                bazaDanych.UsunSale(numerSali);
                Console.WriteLine("Sala została usunięta.");
            }
            else
            {
                Console.WriteLine("Sala o podanym numerze nie istnieje.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowy format numeru sali.");
        }

        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }
    static void ZarzadzajFilmami()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Zarządzanie filmami:");
            Console.WriteLine("1. Dodaj nowy film");
            Console.WriteLine("2. Wyświetl dostępne filmy");
            Console.WriteLine("3. Aktualizuj dane filmu");
            Console.WriteLine("4. Usuń film");
            Console.WriteLine("5. Powrót do głównego menu");

            var wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajFilm();
                    break;
                case "2":
                    PokazFilmy();
                    break;
                case "3":
                    AktualizujFilm();
                    break;
                case "4":
                    UsunFilm();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    }
    static void DodajFilm()
    {
        Console.Clear();
        Console.WriteLine("Dodawanie nowego filmu.");

        Console.Write("Podaj tytuł filmu: ");
        string tytul = Console.ReadLine();

        Console.Write("Podaj opis filmu: ");
        string opis = Console.ReadLine();

        int dlugosc;
        while (true)
        {
            Console.Write("Podaj długość filmu w minutach: ");
            if (int.TryParse(Console.ReadLine(), out dlugosc))
            {
                break;
            }
            else
            {
                Console.WriteLine("Nieprawidłowy format liczby. Spróbuj ponownie.");
            }
        }

        var film = new Film(tytul, opis, dlugosc);
        bazaDanych.DodajFilm(film);

        Console.WriteLine("Film został dodany.");
    }

    static void PokazFilmy()
    {
        var filmy = bazaDanych.PobierzFilmy();
        if (filmy.Any())
        {
            foreach (var film in filmy)
            {
                Console.WriteLine($"Tytuł: {film.Tytul}, Opis: {film.Opis}, Długość: {film.Dlugosc} min");
            }
        }
        else
        {
            Console.WriteLine("Brak dostępnych filmów.");
        }

        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey(); 
    }
    static void UsunFilm()
    {
        Console.Write("Podaj tytuł filmu do usunięcia: ");
        string tytul = Console.ReadLine();

        bazaDanych.UsunFilm(tytul);

        Console.WriteLine("Film został usunięty.");
    }
    static void AktualizujFilm()
    {
        Console.Clear();
        Console.WriteLine("Aktualizacja danych filmu.");

        var filmy = bazaDanych.PobierzFilmy();
        if (!filmy.Any())
        {
            Console.WriteLine("Brak dostępnych filmów.");
            return;
        }

        foreach (var film in filmy)
        {
            Console.WriteLine($"Tytuł: {film.Tytul}");
        }

        Console.Write("Podaj tytuł filmu do aktualizacji: ");
        string tytulDoAktualizacji = Console.ReadLine();
        var filmDoAktualizacji = filmy.FirstOrDefault(f => f.Tytul.ToLower() == tytulDoAktualizacji.ToLower());

        if (filmDoAktualizacji == null)
        {
            Console.WriteLine("Nie znaleziono filmu o podanym tytule.");
            return;
        }

        Console.Write("Podaj nowy tytuł filmu (lub pozostaw puste, aby nie zmieniać): ");
        string nowyTytul = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nowyTytul))
        {
            filmDoAktualizacji.Tytul = nowyTytul;
        }
        Console.Write("Podaj nowy opis filmu (lub pozostaw puste, aby nie zmieniać): ");
        string nowyOpis = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nowyOpis))
        {
            filmDoAktualizacji.Opis = nowyOpis;
        }
        Console.Write("Podaj nową długość filmu w minutach (lub pozostaw puste, aby nie zmieniać): ");
        string nowaDlugoscString = Console.ReadLine();
        if (int.TryParse(nowaDlugoscString, out int nowaDlugosc))
        {
            filmDoAktualizacji.Dlugosc = nowaDlugosc;
        }

        bazaDanych.AktualizujFilm(tytulDoAktualizacji, filmDoAktualizacji);

        Console.WriteLine("Film został zaktualizowany.");
    }
    static void ZarzadzajSeansami()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Zarządzanie seansami:");
            Console.WriteLine("1. Dodaj nowy seans");
            Console.WriteLine("2. Wyświetl dostępne seanse");
            Console.WriteLine("3. Aktualizuj dane seansu");
            Console.WriteLine("4. Usuń seans");
            Console.WriteLine("5. Powrót do głównego menu");

            var wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    DodajSeans();
                    break;
                case "2":
                    PokazSeans();
                    break;
                case "3":
                    AktualizujSeans();
                    break;
                case "4":
                    UsunSeans();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Nieznana opcja.");
                    break;
            }
        }
    } 
    static void DodajSeans()
    {
        Console.Clear();
        Console.WriteLine("Dodawanie nowego seansu.");

      
        var filmy = bazaDanych.PobierzFilmy();
        if (!filmy.Any())
        {
            Console.WriteLine("Brak dostępnych filmów.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Dostępne filmy:");
        foreach (var film in filmy)
        {
            Console.WriteLine($"{film.Tytul}");
        }
        Console.Write("Wybierz film (wpisz tytuł): ");
        string wybranyTytul = Console.ReadLine();
        var wybranyFilm = filmy.FirstOrDefault(f => f.Tytul.ToLower() == wybranyTytul.ToLower());

        if (wybranyFilm == null)
        {
            Console.WriteLine("Film nie został znaleziony.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować.");
            Console.ReadLine();
            return;
        }

        var sale = bazaDanych.PobierzSale();
        if (!sale.Any())
        {
            Console.WriteLine("Brak dostępnych sal.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Dostępne sale:");
        foreach (var sala in sale)
        {
            Console.WriteLine($"Sala numer: {sala.NumerSali}");
        }

        Console.Write("Wybierz salę (wpisz numer): ");
        int numerSali;
        if (!int.TryParse(Console.ReadLine(), out numerSali) || !sale.Any(s => s.NumerSali == numerSali))
        {
            Console.WriteLine("Nieprawidłowy numer sali.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować.");
            Console.ReadLine();
            return;
        }

        var wybranaSala = sale.First(s => s.NumerSali == numerSali);

        
        Console.Write("Podaj datę i godzinę seansu (format: RRRR-MM-DD HH:MM): ");
        DateTime dataICzas;
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataICzas))
        {
            Console.WriteLine("Nieprawidłowy format daty i godziny.");
            Console.WriteLine("Naciśnij Enter, aby kontynuować.");
            Console.ReadLine();
            return;
        }

      
        var seans = new Seanse(wybranyFilm, wybranaSala, dataICzas);
        bazaDanych.DodajSeans(seans);

        Console.WriteLine("Seans został dodany.");
        Console.WriteLine("Naciśnij Enter, aby kontynuować.");
        Console.ReadLine();
    }

    static void PokazSeans()
    {
        Console.Clear();
        var seanse = bazaDanych.PobierzSeanse();
        if (seanse.Any())
        {
            foreach (var seans in seanse)
            {
                Console.WriteLine($"Film: {seans.Film.Tytul}, Sala: {seans.Sala.NumerSali}, Data i czas: {seans.DataICzas}");
            }
        }
        else
        {
            Console.WriteLine("Brak dostępnych seansów.");
        }
        Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }

    static void AktualizujSeans()
    {
        Console.Clear();
        Console.WriteLine("Aktualizacja seansu.");

        var seanse = bazaDanych.PobierzSeanse();
        if (!seanse.Any())
        {
            Console.WriteLine("Brak dostępnych seansów.");
            return;
        }

        foreach (var seans in seanse)
        {
            Console.WriteLine($"Film: {seans.Film.Tytul}, Sala: {seans.Sala.NumerSali}, Data i czas: {seans.DataICzas}");
        }

        Console.Write("Podaj tytuł filmu seansu do aktualizacji: ");
        string tytul = Console.ReadLine();
        Console.Write("Podaj datę seansu do aktualizacji (format: RRRR-MM-DD HH:MM): ");
        DateTime data;
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
        {
            Console.WriteLine("Nieprawidłowy format daty.");
            return;
        }

        var starySeans = seanse.FirstOrDefault(s => s.Film.Tytul.ToLower() == tytul.ToLower() && s.DataICzas == data);

        if (starySeans == null)
        {
            Console.WriteLine("Nie znaleziono seansu.");
            return;
        }
        Console.Write("Podaj nowy tytuł filmu: ");
        string nowyTytul = Console.ReadLine();
        var nowyFilm = bazaDanych.PobierzFilmy().FirstOrDefault(f => f.Tytul.ToLower() == nowyTytul.ToLower());
        if (nowyFilm == null)
        {
            Console.WriteLine("Nie znaleziono filmu.");
            return;
        }

        Console.Write("Podaj nowy numer sali: ");
        int nowyNumerSali;
        if (!int.TryParse(Console.ReadLine(), out nowyNumerSali))
        {
            Console.WriteLine("Nieprawidłowy numer sali.");
            return;
        }
        var nowaSala = bazaDanych.PobierzSale().FirstOrDefault(s => s.NumerSali == nowyNumerSali);
        if (nowaSala == null)
        {
            Console.WriteLine("Nie znaleziono sali.");
            return;
        }

        Console.Write("Podaj nową datę i godzinę seansu (format: RRRR-MM-DD HH:MM): ");
        DateTime nowaDataICzas;
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out nowaDataICzas))
        {
            Console.WriteLine("Nieprawidłowy format daty i godziny.");
            return;
        }

        var nowySeans = new Seanse(nowyFilm, nowaSala, nowaDataICzas);

        bazaDanych.AktualizujSeans(starySeans, nowySeans);
        Console.WriteLine("Seans został zaktualizowany.");
    }
    static void UsunSeans()
    {
        Console.Clear();
        Console.WriteLine("Usuwanie seansu.");

        var seanse = bazaDanych.PobierzSeanse();
        if (!seanse.Any())
        {
            Console.WriteLine("Brak dostępnych seansów do usunięcia.");
            return;
        }

        foreach (var seans in seanse)
        {
            Console.WriteLine($"Film: {seans.Film.Tytul}, Sala: {seans.Sala.NumerSali}, Data i czas: {seans.DataICzas}");
        }
        Console.Write("Podaj tytuł filmu seansu do usunięcia: ");
        string tytul = Console.ReadLine();
        Console.Write("Podaj datę seansu do usunięcia (format: RRRR-MM-DD HH:MM): ");
        DateTime data;
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
        {
            Console.WriteLine("Nieprawidłowy format daty.");
            return;
        }

        var seansDoUsuniecia = seanse.FirstOrDefault(s => s.Film.Tytul.ToLower() == tytul.ToLower() && s.DataICzas == data);

        if (seansDoUsuniecia == null)
        {
            Console.WriteLine("Nie znaleziono seansu.");
            return;
        }
        bazaDanych.UsunSeans(seansDoUsuniecia.Film); 
        Console.WriteLine("Seans został usunięty.");
    }
}

