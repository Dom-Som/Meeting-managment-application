using System;
using System.Linq;
using ReservationSystem.Core.Exceptions;
using ReservationSystem.Core.Models;
using ReservationSystem.Core.Services;

namespace ReservationSystem.App
{
    public static class Menu
    {
        private static readonly ReservationManager _manager = new();

        private const string FilePath = "reservations.txt";

        public static void LoadInitialData()
        {
            if (System.IO.File.Exists(FilePath))
                _manager.LoadFromFile(FilePath);

            // Prisiregistruojame prie įvykių
            _manager.ReservationAdded += (s, r) =>
                Console.WriteLine($"\n[EVENT] Pridėta rezervacija: {r.Title} ({r.Date:yyyy-MM-dd})");
            _manager.ReservationDeleted += (s, r) =>
                Console.WriteLine($"\n[EVENT] Ištrinta rezervacija: {r.Title} ({r.Date:yyyy-MM-dd})");
            _manager.ReservationUpdated += (s, r) =>
                Console.WriteLine($"\n[EVENT] Atnaujinta rezervacija: {r.Title} ({r.Date:yyyy-MM-dd})");
        }

        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Rezervacijų sistema ===");
                Console.WriteLine("1. Peržiūrėti");
                Console.WriteLine("2. Pridėti");
                Console.WriteLine("3. Filtruoti pagal datą");
                Console.WriteLine("4. Filtruoti pagal statusą");
                Console.WriteLine("5. Ištrinti");
                Console.WriteLine("6. Išsaugoti į failą");
                Console.WriteLine("7. Rasti rezervaciją pagal ID");
                Console.WriteLine("0. Išeiti");
                Console.Write("\nPasirinkimas: ");

                switch (Console.ReadLine())
                {
                    case "1": ShowAllReservations(); break;
                    case "2": AddReservation(); break;
                    case "3": FilterReservations(); break;
                    case "4": FilterByStatus(); break;
                    case "5": DeleteReservation(); break;
                    case "6": SaveToFile(); break;
                    case "7": FindReservation(); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Neteisingas pasirinkimas.");
                        Pause();
                        break;
                }
            }
        }

        private static void ShowAllReservations()
        {
            Console.Clear();
            var items = _manager.GetAll().ToList();

            Console.WriteLine("=== Visos rezervacijos ===\n");

            if (!items.Any())
                Console.WriteLine("Nėra rezervacijų.");
            else
                items.ForEach(r => Console.WriteLine(r.ToString("long", null)));

            Pause();
        }

        private static void AddReservation()
        {
            Console.Clear();
            Console.WriteLine("=== Pridėti rezervaciją ===");

            Console.Write("Pavadinimas: ");
            string title = Console.ReadLine() ?? "Untitled";

            Console.Write("Data (YYYY-MM-DD): ");
            DateTime date = DateTime.TryParse(Console.ReadLine(), out var d) ? d : DateTime.Now;

            Console.Write("Trukmė (val.): ");
            TimeSpan duration = double.TryParse(Console.ReadLine(), out var h) ? TimeSpan.FromHours(h) : TimeSpan.FromHours(1);

            Console.WriteLine("\nStatusai: 0=None, 1=Confirmed, 2=Paid, 4=Completed, 8=Cancelled");
            Console.Write("Įveskite reikšmę: ");
            int statusInput = int.TryParse(Console.ReadLine(), out var s) ? s : 0;
            ReservationStatus status = (ReservationStatus)statusInput;

            int newId = _manager.GetAll().Any() ? _manager.GetAll().Max(r => r.Id) + 1 : 1;

            var newRes = new Reservation(newId, date, duration, title, status);
            _manager.Add(newRes);

            Pause();
        }

        private static void FilterReservations()
        {
            Console.Clear();
            try
            {
                Console.Write("Nuo (YYYY-MM-DD): ");
                DateTime start = DateTime.TryParse(Console.ReadLine(), out var s) ? s : DateTime.MinValue;

                Console.Write("Iki (YYYY-MM-DD): ");
                DateTime end = DateTime.TryParse(Console.ReadLine(), out var e) ? e : DateTime.MaxValue;

                var results = _manager.GetByDateRange(start, end).ToList();

                Console.Clear();
                Console.WriteLine("=== Rezultatai ===\n");

                if (!results.Any())
                    Console.WriteLine("Nieko nerasta.");
                else
                    results.ForEach(r => Console.WriteLine(r.ToString("long", null)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klaida filtruojant pagal datas: {ex.Message}");
            }

            Pause();
        }

        private static void FilterByStatus()
        {
            Console.Clear();
            try
            {
                Console.WriteLine("=== Filtruoti pagal statusą ===");
                Console.WriteLine("1 = Confirmed");
                Console.WriteLine("2 = Paid");
                Console.WriteLine("4 = Completed");
                Console.WriteLine("8 = Cancelled");

                Console.Write("\nĮveskite reikšmę: ");
                int input = int.TryParse(Console.ReadLine(), out var s) ? s : 0;
                ReservationStatus selected = (ReservationStatus)input;

                var results = _manager.FilterByStatus(selected).ToList();

                Console.Clear();
                Console.WriteLine("=== Rezultatai ===\n");

                if (!results.Any())
                    Console.WriteLine("Nieko nerasta pagal pasirinktą statusą.");
                else
                    results.ForEach(r =>
                        Console.WriteLine($"{r.Id}. {r.Title} ({r.Date}) - Status: {r.Status}")
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klaida filtruojant pagal statusą: {ex.Message}");
            }

            Pause();
        }

        private static void DeleteReservation()
        {
            Console.Clear();
            try
            {
                Console.Write("Įveskite ID: ");

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    bool ok = _manager.Delete(id);
                    if (!ok) Console.WriteLine("Tokio ID nėra.");
                }
                else
                {
                    Console.WriteLine("Neteisingas ID!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Klaida trinant rezervaciją: {ex.Message}");
            }

            Pause();
        }

        private static void FindReservation()
        {
            Console.Clear();
            Console.Write("Įveskite ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                try
                {
                    var r = _manager.FindOrThrow(id);

                    Console.WriteLine("\n=== Rezervacija rasta: ===");
                    Console.WriteLine(r.ToString("long", null));
                }
                catch (ReservationNotFoundException ex)
                {
                    Console.WriteLine($"Klaida: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Neteisingas ID formatas.");
            }

            Pause();
        }

        private static void SaveToFile()
        {
            _manager.SaveToFile(FilePath);
            Console.WriteLine($"\nIšsaugota į '{FilePath}'");
            Pause();
        }

        private static void Pause()
        {
            Console.WriteLine("\nPaspauskite bet kurį mygtuką...");
            Console.ReadKey();
        }
    }
}
