namespace ACMD
{
    public class ACMDCommand
    {
        public uint CRC32 { get; set; }
        public object[] Args { get; set; }//TODO: should I leave this as object?
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
                return $"Unk_{CRC32.ToString("x8")}";
            }
        }

        internal ACMDCommand(ACMDReader reader)
        {
            CRC32 = reader.ReadUInt32();

            int size = Size - 1;
            Args = new object[size];
            for (int i = 0; i < size; i++)
            {
                var argData = ACMDFile.CmdData[CRC32].Args;
                string type = "int";

                //TODO: Move args into a property so command arg types are easier to retrieve
                if (argData[i] != null)
                    type = argData[i].Attributes["Type"].Value;

                switch (type)
                {
                    case "int":
                        Args[i] = reader.ReadUInt32();
                        break;
                    case "float":
                        Args[i] = reader.ReadSingle();
                        break;
                    default:
                        //TODO: unimplemented
                        Args[i] = reader.ReadUInt32();
                        break;
                }
            }
        }
    }
}
