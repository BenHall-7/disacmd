using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
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
        int UnkCount { get; set; }

        public static Dictionary<uint, CmdDataCollection> CmdData { get; }

        public ACMDScript[] Scripts { get; set; }

        static ACMDFile()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("ACMD.xml");
            foreach (XmlElement xe in xml.DocumentElement.GetElementsByTagName("Command"))
            {
                uint crc = uint.Parse(xe.Attributes["CRC32"].Value.Substring(2), NumberStyles.HexNumber);
                int size = int.Parse(xe.Attributes["Size"].Value);
                CmdData.Add(crc, new CmdDataCollection(xe.ChildNodes, size));
            }
            //enums
            foreach (XmlElement xe in xml.DocumentElement.GetElementsByTagName("Enum"))
            {

            }
        }

        public ACMDFile(string filename)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(filename)))
            {
                for (int i = 0; i < Magic.Length; i++)
                    if (reader.ReadByte() != Magic[i])
                        throw new InvalidDataException("File contains invalid magic");
                Unk4 = reader.ReadInt32();
                ScriptCount = reader.ReadInt32();
                UnkCount = reader.ReadInt32();

                var tableOffset = reader.BaseStream.Position;
                for (int i = 0; i < Scripts.Length; i++)
                {
                    //TODO: open the scripts :)
                }
            }
        }
    }
}
