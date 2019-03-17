using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace ACMD
{
    public class ACMDFile
    {
        //0x10 byte header
        const string Magic = "ACMD";
        int Unk4 { get; set; }
        int ScriptCount { get; set; }
        int CmdCount { get; set; }
        
        private static bool IsStaticDataInit { get; set; }
        public static Dictionary<uint, CmdDataCollection> CmdData { get; }
        public static Dictionary<string, EnumDataCollection> EnumData { get; }
        public static Dictionary<uint, string> ScriptHashes { get; }

        public Dictionary<uint, ACMDScript> Scripts { get; set; }

        static ACMDFile()
        {
            IsStaticDataInit = false;
            CmdData = new Dictionary<uint, CmdDataCollection>();
            EnumData = new Dictionary<string, EnumDataCollection>();
            ScriptHashes = new Dictionary<uint, string>();
        }

        public ACMDFile(string filename)
        {
            if (!IsStaticDataInit)
                InitStatic();
            using (ACMDReader reader = new ACMDReader(File.OpenRead(filename)))
            {
                for (int i = 0; i < Magic.Length; i++)
                    if (reader.ReadByte() != Magic[i])
                        throw new InvalidDataException("File contains invalid magic");
                Unk4 = reader.ReadInt32();
                ScriptCount = reader.ReadInt32();
                CmdCount = reader.ReadInt32();

                Scripts = new Dictionary<uint, ACMDScript>(ScriptCount);
                
                var tableOffset = reader.BaseStream.Position;
                for (int i = 0; i < ScriptCount; i++)
                {
                    reader.BaseStream.Position = tableOffset + 8 * i;
                    uint crc = reader.ReadUInt32();
                    uint offset = reader.ReadUInt32();

                    reader.BaseStream.Position = offset;
                    Scripts.Add(crc, new ACMDScript(reader));
                }
            }
        }

        public static void InitStatic()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("ACMD.xml");
            foreach (XmlElement xe in xml.DocumentElement.GetElementsByTagName("command"))
            {
                uint crc = uint.Parse(xe.Attributes["crc32"].Value.Substring(2), NumberStyles.HexNumber);
                int size = int.Parse(xe.Attributes["size"].Value);
                CmdData.Add(crc, new CmdDataCollection(xe.ChildNodes, size));
            }
            //TODO: handle enums better?
            foreach (XmlElement xe in xml.DocumentElement.GetElementsByTagName("enum"))
            {
                string name = xe.Attributes["name"].Value;
                EnumData.Add(name, new EnumDataCollection(xe.ChildNodes));
            }
            foreach (var line in File.ReadAllLines("ScriptNames.txt"))
            {
                ScriptHashes.Add(CRC.CRC32(line), line);
            }
            IsStaticDataInit = true;
        }

        public static string GetScriptNameDefault(uint hash)
        {
            if (ScriptHashes.TryGetValue(hash, out string name))
                return name;
            return $"func_{hash.ToString("x8")}"; 
        }
    }
}
