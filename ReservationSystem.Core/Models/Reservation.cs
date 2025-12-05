using System;
using ReservationSystem.Core.Interfaces;

namespace ReservationSystem.Core.Models
{
    /// <summary>
    /// Konkreti rezervacijos klasė.
    /// Paveldi bendrus laukus iš BaseReservation
    /// ir papildomai implementuoja palyginimą, lygybę,
    /// formatavimą, priminimų generavimą ir klonavimą.
    /// </summary>
    public sealed class Reservation : BaseReservation,
        IComparable<Reservation>,
        IEquatable<Reservation>,
        IFormattable,
        INotifiable,
        ICloneable
    {
        /// <summary>
        /// Unikalus rezervacijos identifikatorius.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// Rezervacijos būsena. Gali būti kelių flagų kombinacija.
        /// </summary>
        public ReservationStatus Status { get; set; }

        static Reservation() { }

        public Reservation(int id, DateTime date, TimeSpan duration, string title = "Untitled",
            ReservationStatus status = ReservationStatus.None)
            : base(date, duration, title)
        {
            Id = id;
            Status = status;
        }

        public override string GetSummary() =>
            $"{Title} ({Date:yyyy-MM-dd}) - {Status}";

        // -----------------------------
        // IComparable
        // -----------------------------
        public int CompareTo(Reservation? other) =>
            other == null ? 1 : Date.CompareTo(other.Date);

        // -----------------------------
        // IEquatable
        // -----------------------------
        public bool Equals(Reservation? other) =>
            other != null && Id == other.Id;

        public override bool Equals(object? obj) =>
            obj is Reservation r && Equals(r);

        public override int GetHashCode() => Id.GetHashCode();

        // -----------------------------
        // IFormattable
        // -----------------------------
        public string ToString(string? format, IFormatProvider? provider)
        {
            return format?.ToLower() switch
            {
                "short" => $"{Title} ({Date:yyyy-MM-dd})",
                "long" => $"{Id}: {Title} ({Date:yyyy-MM-dd HH:mm}) Duration: {Duration} Status: {Status}",
                _ => $"{Title} ({Date:yyyy-MM-dd}) - {Status}"
            };
        }

        // -----------------------------
        // INotifiable
        // -----------------------------
        public string GenerateReminder() =>
            $"Primename apie rezervaciją '{Title}' {Date:yyyy-MM-dd}";

        // -----------------------------
        // ICloneable implementacija
        // -----------------------------
        /// <summary>
        /// Grąžina naują kopiją šios rezervacijos objekto.
        /// </summary>
        public object Clone()
        {
            // Seklus klonas pakanka, nes DateTime, TimeSpan ir enum yra immutable.
            return new Reservation(Id, Date, Duration, Title, Status);
        }

        // -----------------------------
        // Operatoriai
        // -----------------------------
        public static Reservation operator +(Reservation a, Reservation b) =>
            new(a.Id, a.Date, a.Duration + b.Duration, a.Title, a.Status | b.Status);

        public static bool operator ==(Reservation? a, Reservation? b) =>
            a?.Equals(b) ?? b is null;

        public static bool operator !=(Reservation? a, Reservation? b) =>
            !(a == b);
    }
}
