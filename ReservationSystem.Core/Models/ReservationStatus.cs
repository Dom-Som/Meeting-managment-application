using System;

namespace ReservationSystem.Core.Models
{
    /// <summary>
    /// Rezervacijos būsenų flagai.
    /// Naudojami su bitų operacijomis (|, &),
    /// todėl pažymėti [Flags] atributu.
    /// Leidžia kombinuoti kelias būsenas vienu metu.
    /// </summary>
    [Flags]
    public enum ReservationStatus
    {
        /// <summary>Jokia būsena nenustatyta.</summary>
        None = 0,

        /// <summary>Rezervacija patvirtinta.</summary>
        Confirmed = 1,

        /// <summary>Rezervacija apmokėta.</summary>
        Paid = 2,

        /// <summary>Paslauga įvykdyta.</summary>
        Completed = 4,

        /// <summary>Rezervacija atšaukta.</summary>
        Cancelled = 8
    }
}
