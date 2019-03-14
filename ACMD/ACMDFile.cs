using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace ACMD
{
    public class ACMDFile
    {
        //0x10 byte header
        const string Magic = "ACMD";
        int Unk4 { get; set; }
        int ScriptCount
        {
            get { return Scripts.Length; }
            set { Scripts = new ACMDScript[value]; }
        }
        int CmdCount { get; set; }
        
        private static bool IsStaticDataInit { get; set; }
        public static Dictionary<uint, CmdDataCollection> CmdData { get; }
        public static Dictionary<string, EnumDataCollection> EnumData { get; }

        public ACMDScript[] Scripts { get; set; }

        static ACMDFile()
        {
            IsStaticDataInit = false;
            CmdData = new Dictionary<uint, CmdDataCollection>();
            EnumData = new Dictionary<string, EnumDataCollection>();
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

                var tableOffset = reader.BaseStream.Position;
                for (int i = 0; i < Scripts.Length; i++)
                {
                    reader.BaseStream.Position = tableOffset + 8 * i;
                    uint crc32 = reader.ReadUInt32();
                    uint offset = reader.ReadUInt32();
                    reader.BaseStream.Position = offset;
                    Scripts[i] = new ACMDScript(crc32, reader);
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
            IsStaticDataInit = true;
        }
    }
}
