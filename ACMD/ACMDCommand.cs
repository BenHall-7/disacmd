using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACMD
{
    public class ACMDCommand
    {
        public uint CRC32 { get; set; }
        public int Size { get; set; }
        
        public string Name
        {
            get
            {
                string name = ACMDFile.CmdData[CRC32].Name;
                if (!string.IsNullOrEmpty(name))
                    return name;                    
                return $"Unk_{CRC32.ToString("x8")}";
            }
        }

        internal ACMDCommand()
        {

        }
    }
}
