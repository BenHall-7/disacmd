namespace ACMD
{
    public class ACMDCommand
    {
        public uint CRC32 { get; set; }
        public ACMDArg[] Args { get; set; }//TODO: should I leave this as object?
        public int Size
        {
            get { return ACMDFile.CmdData[CRC32].CmdSize; }
        }
        
        public string Name
        {
            get
            {
                string name = ACMDFile.CmdData[CRC32].Name;
                if (!string.IsNullOrEmpty(name))
                    return name;                    
                return $"cmd_{CRC32.ToString("x8")}";
            }
        }

        internal ACMDCommand(BigEndianReader reader)
        {
            CRC32 = reader.ReadUInt32();

            int size = Size - 1;
            Args = new ACMDArg[size];
            for (int i = 0; i < size; i++)
            {
                var argData = ACMDFile.CmdData[CRC32].Args;

                var arg = Args[i] = new ACMDArg();
                arg.Type = argData[i] == null ? "uint" : argData[i].Attributes["type"].Value;
                arg.Read(reader);
            }
        }
    }
}
