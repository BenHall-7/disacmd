﻿using ACMD;
using System;

namespace disacmd
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var ACMDFile = new ACMDFile(@"C:\Users\Breakfast\Documents\GitHub\Sm4shExplorer\Sm4shFileExplorer\bin\Debug\extract\data\fighter\pikachu\script\animcmd\body\game.bin");
            Console.WriteLine("Success!");
            Console.ReadKey();
#endif
        }
    }
}