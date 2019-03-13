using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace ACMD
{
    public class CmdDataCollection : Dictionary<DataCollectionKey, object>
    {
        public int CmdSize { get; set; }//required

        public CmdDataCollection(XmlElement cmdNode) : base()
        {
            CmdSize = int.Parse(cmdNode.Attributes["Size"].Value);
        }
    }
}
