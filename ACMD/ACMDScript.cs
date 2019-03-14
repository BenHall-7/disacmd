﻿using System.Collections.Generic;

namespace ACMD
{
    public class ACMDScript
    {
        public uint CRC32 { get; set; }

        public List<ACMDCommand> Commands { get; set; }

        internal ACMDScript(uint crc32, ACMDReader reader)
        {
            CRC32 = crc32;
            Commands = new List<ACMDCommand>();

            ACMDCommand cmd;
            while (true)
            {
                cmd = new ACMDCommand(reader);
                Commands.Add(cmd);
                //return marks the end of the script
                if (cmd.CRC32 == 0x5766f889)
                    break;
            }
        }
    }
}
