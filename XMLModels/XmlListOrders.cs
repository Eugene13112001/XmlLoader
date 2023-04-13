using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlLoader.XMLModels
{
    [XmlRoot("orders")]
    public class XmlListOrders
    {
        [XmlElement("order")]
        public List<XmlOrder>? orders;
    }
}
