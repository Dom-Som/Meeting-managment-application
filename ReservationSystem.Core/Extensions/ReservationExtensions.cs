using ReservationSystem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReservationSystem.Core.Extensions
{
    /// <summary>
    /// Extension metodai Reservation klasėms.
    /// </summary>
    public static class ReservationExtensions
    {
        // Tikrina ar rezervacija yra patvirtinta
        public static bool IsConfirmed(this Reservation r) =>
            r.Status.HasFlag(ReservationStatus.Confirmed);

        // Filtruoti pagal statusą
        public static IEnumerable<Reservation> WhereStatus(this IEnumerable<Reservation> items, ReservationStatus status)
        {
            return items.Where(r => (r.Status & status) != 0);
        }
    }
}
