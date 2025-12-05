using System;

namespace ReservationSystem.Core.Models
{
    /// <summary>
    /// Abstrakti rezervacijos bazinė klasė.
    /// Apibrėžia bendrus laukus ir elgseną
    /// visiems rezervacijos tipo objektams.
    /// </summary>
    public abstract class BaseReservation
    {
        /// <summary>
        /// Rezervacijos data ir laikas.
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Rezervacijos trukmė.
        /// </summary>
        public TimeSpan Duration { get; init; }

        /// <summary>
        /// Rezervacijos pavadinimas.
        /// </summary>
        public string Title { get; init; }

        /// <summary>
        /// Bazinis konstruktorius bendrai rezervacijos informacijai sukurti.
        /// </summary>
        protected BaseReservation(DateTime date, TimeSpan duration, string title = "Untitled")
        {
            Date = date;
            Duration = duration;
            Title = title;
        }

        /// <summary>
        /// Turi grąžinti rezervacijos santraukos tekstą.
        /// Implementuojama paveldinčiose klasėse.
        /// </summary>
        public abstract string GetSummary();

        /// <summary>
        /// Leidžia išskaidyti rezervaciją į tuple (Date, Duration)
        /// naudojant 'deconstruct' sintaksę.
        /// </summary>
        public void Deconstruct(out DateTime date, out TimeSpan duration)
        {
            date = Date;
            duration = Duration;
        }
    }
}
