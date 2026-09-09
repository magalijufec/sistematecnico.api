namespace SistemaTecnico.Helpers
{
    public static class FechaHelper
    {
        public static DateTime AhoraArgentina()
        {
            return DateTime.UtcNow.AddHours(-3);
        }
        
        public static DateTime? AhoraArgentina(DateTime? fechaUtc)
        {
            return fechaUtc?.AddHours(-3);
        }

        public static DateTime AhoraArgentina(DateTime fechaUtc)
        {
            return fechaUtc.AddHours(-3);
        }
    }
}
