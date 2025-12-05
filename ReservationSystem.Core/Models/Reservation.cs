using System;
using ReservationSystem.Core.Interfaces;

namespace ReservationSystem.Core.Models
{
    /// <summary>
    /// Konkreti rezervacijos klasė.
    /// Paveldi bendrus laukus iš BaseReservation
    /// ir papildomai implementuoja palyginimą, lygybę,
    /// formatavimą bei priminimų generavimą.
    /// </summary>
    public sealed class Reservation : BaseReservation,
        IComparable<Reservation>,
        IEquatable<Reservation>,
        IFormattable,
        INotifiable
    {
        /// <summary>
        /// Unikalus rezervacijos identifikatorius.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Rezervacijos būsena. Gali būti kelių flagų kombinacija.
        /// </summary>
        public ReservationStatus Status { get; set; }

        /// <summary>
        /// Statinis konstruktorius iškviečiamas vieną kartą,
        /// kai tipas pirmą kartą naudojamas.
        /// Šiuo atveju nenaudojamas, bet paliktas demonstracijai.
        /// </summary>
        static Reservation()
        {
        }

        /// <summary>
        /// Pagrindinis rezervacijos konstruktorius.
        /// </summary>
        public Reservation(int id, DateTime date, TimeSpan duration, string title = "Untitled",
            ReservationStatus status = ReservationStatus.None)
            : base(date, duration, title)
        {
            Id = id;
            Status = status;
        }

        /// <summary>
        /// Konkrečios rezervacijos santrauka.
        /// </summary>
        public override string GetSummary() =>
            $"{Title} ({Date:yyyy-MM-dd}) - {Status}";

        // -----------------------------
        // IComparable implementacija
        // -----------------------------

        /// <summary>
        /// Lygina rezervacijas pagal datą.
        /// </summary>
        public int CompareTo(Reservation? other) =>
            other == null ? 1 : Date.CompareTo(other.Date);

        // -----------------------------
        // IEquatable implementacija
        // -----------------------------

        /// <summary>
        /// Rezervacijos laikomos lygiomis,
        /// jei jų ID sutampa.
        /// </summary>
        public bool Equals(Reservation? other) =>
            other != null && Id == other.Id;

        public override bool Equals(object? obj) =>
            obj is Reservation r && Equals(r);

        public override int GetHashCode() => Id.GetHashCode();

        // -----------------------------
        // IFormattable implementacija
        // -----------------------------

        /// <summary>
        /// Leidžia formatuoti rezervacijos tekstą
        /// naudojant formatus: short, long, default.
        /// </summary>
        public string ToString(string? format, IFormatProvider? provider)
        {
            return format?.ToLower() switch
            {
                "short" =>
                    $"{Title} ({Date:yyyy-MM-dd})",

                "long" =>
                    $"{Id}: {Title} ({Date:yyyy-MM-dd HH:mm}) Duration: {Duration} Status: {Status}",

                _ =>
                    $"{Title} ({Date:yyyy-MM-dd}) - {Status}"
            };
        }

        // -----------------------------
        // INotifiable implementacija
        // -----------------------------

        /// <summary>
        /// Generuoja tekstinį priminimą apie rezervaciją.
        /// </summary>
        public string GenerateReminder() =>
            $"Primename apie rezervaciją '{Title}' {Date:yyyy-MM-dd}";

        // -----------------------------
        // Operatoriai
        // -----------------------------

        /// <summary>
        /// Sudeda rezervacijų trukmes
        /// ir sujungia jų statusus (bitų OR operacija).
        /// Demonstracinis operatoriaus pavyzdys.
        /// </summary>
        public static Reservation operator +(Reservation a, Reservation b) =>
            new(a.Id, a.Date, a.Duration + b.Duration, a.Title, a.Status | b.Status);

        public static bool operator ==(Reservation? a, Reservation? b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(Reservation? a, Reservation? b) =>
            !(a == b);
    }
}
