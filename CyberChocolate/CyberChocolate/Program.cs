using System;

namespace CyberChocolate
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                //to jest komentarz Brugier
                game.Run();
        }
    }
#endif
}
