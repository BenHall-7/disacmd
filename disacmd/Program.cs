using ACMD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace disacmd
{
    class Program
    {
        static Stopwatch timer { get; set; }

        static void Main(string[] args)
        {
            timer = new Stopwatch();
            timer.Start();

            foreach (var dir in Directory.EnumerateDirectories(@"C:\Users\Breakfast\Documents\GitHub\Sm4shExplorer\Sm4shFileExplorer\bin\Debug\extract\data\fighter"))
            {
                string ft = dir.Substring(dir.LastIndexOf('\\') + 1);
                var scriptdir = Path.Combine(dir, @"script\animcmd\body");
                if (ft == "common")
                    scriptdir = Path.Combine(dir, @"script\animcmd");
                if (Directory.Exists(scriptdir))
                {
                    if (!Directory.Exists($@"output\{ft}"))
                        Directory.CreateDirectory($@"output\{ft}");

                    MTable mtable = null;
                    var files = Directory.EnumerateFiles(scriptdir);
                    List<string> acmdFiles = new List<string>();
                    
                    //check for motion.mtable; if exists, sort scripts by entry ID
                    foreach (var file in files)
                    {
                        if (Path.GetFileName(file) == "motion.mtable")
                            mtable = new MTable(file);
                        else
                            acmdFiles.Add(file);
                    }

                    //read acmd scripts
                    foreach (var file in acmdFiles)
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        ACMDFile acmd = new ACMDFile(file);

                        Decompile(acmd, mtable, $@"output\{ft}\{name}.txt");
                    }
                }
            }
            timer.Stop();
            Console.WriteLine($"finished all in {timer.Elapsed.TotalSeconds} seconds");
            Console.ReadKey();
        }

        static void Decompile(ACMDFile acmd, MTable mtable, string outdir)
        {
            using (StreamWriter writer = new StreamWriter(File.Create(outdir)))
            {
                if (mtable != null)
                {
                    //converting -1 hash index to uint makes it so
                    //unlisted scripts move to the end of the file instead of beginning
                    foreach (var script in acmd.Scripts.OrderBy(x => (uint)mtable.Hashes.IndexOf(x.Key)))
                    {
                        int index = mtable.Hashes.IndexOf(script.Key);
                        writer.WriteLine($"# motion_kind: {(index >= 0 ? "0x" + index.ToString("x") : "n/a")}");
                        writer.WriteLine(ACMDFile.GetScriptNameDefault(script.Key));
                        writer.WriteLine("{");
                        DecompileScript(writer, script.Value);
                        writer.WriteLine("}");
                        writer.WriteLine();
                    }
                }
                else
                {
                    foreach (var script in acmd.Scripts.OrderBy(x => ACMDFile.GetScriptNameDefault(x.Key)))
                    {
                        writer.WriteLine(ACMDFile.GetScriptNameDefault(script.Key));
                        writer.WriteLine("{");
                        DecompileScript(writer, script.Value);
                        writer.WriteLine("}");
                        writer.WriteLine();
                    }
                }
            }
        }

        static void DecompileScript(StreamWriter writer, ACMDScript script)
        {
            foreach (var command in script.Commands)
            {
                writer.Write($"    {command.Name}(");
                var args = command.Args;
                for (int i = 0; i < args.Length; i++)
                {
                    writer.Write(FormatObject(args[i]));
                    if (i < args.Length - 1)
                        writer.Write(", ");
                }
                writer.WriteLine(")");
            }
        }

        //make a generic method to "encapsulate" data into brackets, easiest way to pretty-print things

        static string FormatObject(object obj)
        {
            if (obj is uint ivalue)
            {
                if (ivalue > 0xffff)
                    return $"0x{ivalue.ToString("x")}";
                return ivalue.ToString();
            }
            if (obj is float fvalue)
                return fvalue.ToString("0.0");
            return obj.ToString();
        }

        static void SortScripts(ACMDFile file, MTable mTable)
        {
            if (mTable != null)
            {

            }
            else
            {

            }
        }
    }
}
