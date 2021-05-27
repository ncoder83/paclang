namespace PacLang
{
    /// <summary>
    ///  Add string concatenation
    ///  Add highlighting for strings
    ///  Add support for calling built-in functions
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
