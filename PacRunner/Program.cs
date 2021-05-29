namespace PacLang
{
    /// <summary>
    ///  X - Add string concatenation
    ///  X - Add highlighting for strings
    ///  X - Add support for calling built-in functions
    ///  Add support for looking up functions
    ///  Add support for random
    ///  Add conversions
    /// </summary>
    internal static class Program
    {
        private static void Main()
        {
            var repl = new PaclangRepl();
            repl.Run();
        }
    }
}
