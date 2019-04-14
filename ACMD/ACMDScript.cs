using System.Collections.Generic;

namespace ACMD
{
    public class ACMDScript
    {
        public List<ACMDCommand> Commands { get; set; }

        public ACMDScript(BigEndianReader reader)
        {
            Commands = new List<ACMDCommand>();

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
