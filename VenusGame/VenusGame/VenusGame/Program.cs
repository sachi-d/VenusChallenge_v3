using System;

namespace VenusGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Init initialiseGame = new Init();
        }
    }
#endif
}

