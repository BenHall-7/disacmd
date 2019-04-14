using System;
using System.Collections.Generic;
using System.Text;

namespace ACMD
{
    public class ACMDArg
    {
        public string Type { get; set; }
        public object Value { get; set; }

        public void Read(BigEndianReader reader)
        {
            switch (Type)
            {
                case "int":
                    Value = reader.ReadInt32();
                    break;
                case "uint":
                    Value = reader.ReadUInt32();
                    break;
                case "float":
                    Value = reader.ReadSingle();
                    break;
                case "bool":
                    Value = reader.ReadBoolean();
                    break;
                case "script":
                    {
                        Value = ACMDFile.GetScriptNameDefault(reader.ReadUInt32());
                        break;
                    }
                default:
                    {
                        uint argValue = reader.ReadUInt32();

                        var data = ACMDFile.EnumData[Type];
                        if (data.Enums.TryGetValue(argValue, out var label))
                            Value = label.InnerText;
                        else
                            Value = argValue;
                        break;
                    }
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case "uint":
                    var ui = (uint)Value;
                    return ui > 0xffff ? $"0x{ui.ToString("x")}" : ui.ToString();
                case "float":
                    return ((float)Value).ToString("0.0");
            }
            return Value.ToString();
        }
    }
}
