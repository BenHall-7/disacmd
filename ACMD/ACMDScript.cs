using System.Collections.Generic;

namespace ACMD
{
    public class ACMDScript
    {
        public uint CRC32 { get; set; }
        public List<ACMDCommand> Commands { get; set; }
        public string Name
        {
            get
            {
                if (ACMDFile.ScriptHashes.TryGetValue(CRC32, out string name))
                    return name;
                return $"func_{CRC32.ToString("x8")}";
            }
        }

        public ACMDScript(uint crc32)
        {
            CRC32 = crc32;
            Commands = new List<ACMDCommand>();
        }

        internal void Read(ACMDReader reader)
        {
            ACMDCommand cmd;
            while (true)
            {
                cmd = new ACMDCommand(reader);
                Commands.Add(cmd);
                //RETURN marks the end of the script
                if (cmd.CRC32 == 0x5766f889)
                    break;
            }
        }
    }
}
