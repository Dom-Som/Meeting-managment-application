using ReservationSystem.App;

internal class Program
{
    private static void Main()
    {
        // Automatinis duomenų įkėlimas iš failo paleidžiant programą
        Menu.LoadInitialData();

        // Paleidžiamas pagrindinis meniu ciklas
        Menu.Run();
    }
}
