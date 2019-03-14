using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ACMD
{
    //one might argue the use of a class here wouldn't be necessary
    //they are probably right but this is in part due to organization
    public class EnumDataCollection
    {
        public Dictionary<uint, XmlElement> Enums { get; }

        public EnumDataCollection(XmlNodeList nodeList)
        {
            Enums = new Dictionary<uint, XmlElement>();

            foreach (XmlNode node in nodeList)
            {
                if (node is XmlElement elem)
                {
                    uint index = ParseUIntGeneric(elem.Attributes["id"].Value);
                    Enums.Add(index, elem);
                }
            }
        }

        private uint ParseUIntGeneric(string value)
        {
            if (value.StartsWith("0x"))
                return uint.Parse(value.Substring(2), NumberStyles.HexNumber);
            return uint.Parse(value);
        }
    }
}
