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
            Args = new XmlElement[CmdSize];
            
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
