namespace PacLang
{
    internal static class Program
    {
        private static void Main()
        {
            var repl = new PaclangRepl();
            repl.Run();
        }
    }
}
