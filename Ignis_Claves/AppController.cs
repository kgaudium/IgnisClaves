using System;

namespace IgnisClaves
{
    class AppController
    {
        public static IgnisGame Ignis = new();

        public static void Main()
        {
            Ignis.Run();
        }


    }
}
