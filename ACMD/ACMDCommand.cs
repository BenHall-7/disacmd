using System;
using System.Collections.Generic;
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

                return $"Unk_{CRC32.ToString("x8")}";
            }
        }
    }
}
