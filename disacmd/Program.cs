using ACMD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace disacmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach (var dir in Directory.EnumerateDirectories(@"C:\Users\Breakfast\Documents\GitHub\Sm4shExplorer\Sm4shFileExplorer\bin\Debug\extract\data\fighter"))
            {
                string ft = dir.Substring(dir.LastIndexOf('\\') + 1);
                var scriptdir = Path.Combine(dir, @"script\animcmd\body");
                if (Directory.Exists(scriptdir))
                {
                    foreach (var file in Directory.EnumerateFiles(scriptdir))
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        if (file.EndsWith(".bin"))
                        {
                            ACMDFile acmd = new ACMDFile(file);

                            if (!Directory.Exists($@"output\{ft}"))
                                Directory.CreateDirectory($@"output\{ft}");

                            Decompile(acmd, $@"output\{ft}\{name}.txt");
                        }
                    }
                }
            }
            timer.Stop();
            Console.WriteLine($"finished all in {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadKey();
        }

        static void Decompile(ACMDFile acmd, string outdir)
        {
            using (StreamWriter writer = new StreamWriter(File.Create(outdir)))
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
            writer.WriteLine(script.Name);
            writer.WriteLine("{");
            foreach (var command in script.Commands)
            {
                writer.Write($"    {command.Name}(");
                var args = command.Args;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is uint ivalue)
                        writer.Write(FormatInt(ivalue));
                    else if (args[i] is float fvalue)
                        writer.Write(fvalue.ToString("0.0"));
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
            if (value > 0xffff)
                return $"0x{value.ToString("x")}";
            return value.ToString();
        }
    }
}
