namespace ReservationSystem.Core.Interfaces
{
    /// <summary>
    /// Abstrakcija klasėms, kurios gali sugeneruoti priminimą.
    /// Implementuojama rezervacijos objekte.
    /// </summary>
    public interface INotifiable
    {
        /// <summary>
        /// Sugeneruoja tekstinį priminimą vartotojui.
        /// </summary>
        string GenerateReminder();
    }
}
