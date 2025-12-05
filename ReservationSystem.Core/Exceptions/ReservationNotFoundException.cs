using System;

namespace ReservationSystem.Core.Exceptions
{
    /// <summary>
    /// Metama, kai rezervacija pagal nurodytą ID nerandama sistemoje.
    /// </summary>
    public class ReservationNotFoundException : Exception
    {
        public int ReservationId { get; }

        public ReservationNotFoundException(int reservationId)
            : base($"Rezervacija su ID {reservationId} nerasta.")
        {
            ReservationId = reservationId;
        }

        public ReservationNotFoundException(int reservationId, string message)
            : base(message)
        {
            ReservationId = reservationId;
        }

        public ReservationNotFoundException(int reservationId, string message, Exception inner)
            : base(message, inner)
        {
            ReservationId = reservationId;
        }
    }
}
