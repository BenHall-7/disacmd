using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ACMD
{
    public class MTable
    {
        public List<uint> Hashes { get; set; }

        public MTable()
        {
            Hashes = new List<uint>();
        }
        public MTable(string filename)
        {
            Hashes = new List<uint>();
            using (BigEndianReader reader = new BigEndianReader(File.OpenRead(filename)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                    Hashes.Add(reader.ReadUInt32());
            }
        }
    }
}
