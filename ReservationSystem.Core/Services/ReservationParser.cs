using System;
using ReservationSystem.Core.Models;

namespace ReservationSystem.Core.Services
{
    /// <summary>
    /// Atsakingas už rezervacijos atkūrimą iš teksto (CSV tipo eilutės).
    /// Naudojamas nuskaitymui iš failo.
    /// </summary>
    public static class ReservationParser
    {
        /// <summary>
        /// Bando konvertuoti vieną eilutę į Reservation objektą.
        /// Formatas: ID;Date;Duration;Title;StatusFlags
        /// </summary>
        public static bool TryParse(string input, out Reservation? reservation)
        {
            reservation = null;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            var parts = input.Split(';');
            if (parts.Length < 5)
                return false;

            if (!int.TryParse(parts[0], out int id))
                return false;

            if (!DateTime.TryParse(parts[1], out var date))
                return false;

            if (!TimeSpan.TryParse(parts[2], out var duration))
                return false;

            string title = parts[3];

            // Konvertuojame statusų bitų kombinaciją
            ReservationStatus status = ReservationStatus.None;

            foreach (var s in parts[4].Split('|', StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse<ReservationStatus>(s, out var parsed))
                    status |= parsed;
            }

            reservation = new Reservation(id, date, duration, title, status);
            return true;
        }
    }
}
