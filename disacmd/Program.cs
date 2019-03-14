using ACMD;
using System;
using System.IO;

namespace disacmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var ACMD = new ACMDFile(@"C:\Users\Breakfast\Documents\GitHub\Sm4shExplorer\Sm4shFileExplorer\bin\Debug\extract\data\fighter\cloud\script\animcmd\body\game.bin");
            Console.WriteLine("load success!");
            Decompile(ACMD);
            Console.WriteLine("save success!");
            Console.ReadKey();
        }

        static void Decompile(ACMDFile acmd)
        {
            using (StreamWriter writer = new StreamWriter(File.Create("game.txt")))
            {
                foreach (var script in acmd.Scripts)
                {
                    DecompileScript(writer, script);
                    writer.WriteLine();
                }
            }
        }

        static void DecompileScript(StreamWriter writer, ACMDScript script)
        {
            writer.WriteLine($"0x{script.CRC32.ToString("x8")}");
            writer.WriteLine("{");
            foreach (var command in script.Commands)
            {
                writer.Write($"    {command.Name}(");
                var args = command.Args;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is uint value)
                        writer.Write(FormatInt(value));
                    else
                        writer.Write(args[i]);
                    if (i < args.Length - 1)
                        writer.Write(", ");
                }
                writer.WriteLine(")");
            }
            writer.WriteLine("}");
        }

        //make a generic method to "encapsulate" data into brackets, easiest way to pretty-print things

        static string FormatInt(uint value)
        {
            if (value > 0x1000)
                return $"0x{value.ToString("x")}";
            return value.ToString();
        }
    }
}
