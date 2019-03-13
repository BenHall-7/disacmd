using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

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
                if (child.Name == "Name")
                {
                    Name = child.InnerText;
                    if (Name == "Float_Compare")
                    {

                    }
                }
                else if (child.Name == "Arg")
                {
                    int index = int.Parse(child.Attributes["ID"].Value);
                    Args[index] = child;
                }
                Console.WriteLine(Name);
            }
        }
    }
}
