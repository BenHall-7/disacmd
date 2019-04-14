using System;
using System.IO;

namespace ACMD
{
    public class BigEndianReader : BinaryReader
    {
        public BigEndianReader(Stream input) : base(input) { }

        public override bool ReadBoolean()
        {
            return base.ReadInt32() == 0 ? false : true;
        }

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
