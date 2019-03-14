using System;
using System.Xml;

namespace ACMD
{
    public class CmdDataCollection
    {
        public int CmdSize { get; }
        public string Name { get; }
        public XmlElement[] Args { get; }

        public CmdDataCollection(XmlNodeList childElements, int cmdSize)
        {
            CmdSize = cmdSize;
            if (CmdSize == 0)
                Args = new XmlElement[CmdSize];//don't throw for blank commands
            else
                Args = new XmlElement[CmdSize - 1];

            for (int i = 0; i < childElements.Count; i++)
            {
                XmlElement child = childElements[i] as XmlElement;
                if (child == null) continue;
                if (child.Name == "name")
                {
                    Name = child.InnerText;
                }
                else if (child.Name == "arg")
                {
                    int index = int.Parse(child.Attributes["id"].Value);
                    Args[index] = child;
                }
            }
        }
    }
}
