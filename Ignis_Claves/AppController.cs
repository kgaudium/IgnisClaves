using System;

namespace IgnisClaves
{
    class AppController
    {
        public static void Main()
        {
            using var game = new IgnisGame();
            game.Run();
        }
    }
}
