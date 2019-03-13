﻿using System;
using System.IO;

namespace ACMD
{
    public class ACMDReader : BinaryReader
    {
        public ACMDReader(Stream input) : base(input) { }
        
        public override int ReadInt32()
        {
            byte[] bytes = base.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public override uint ReadUInt32()
        {
            byte[] bytes = base.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        public override float ReadSingle()
        {
            byte[] bytes = base.ReadBytes(4);
            Array.Reverse(bytes);
            return BitConverter.ToSingle(bytes, 0);
        }
    }
}
