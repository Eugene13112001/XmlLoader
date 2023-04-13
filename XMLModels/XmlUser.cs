using System.Xml.Serialization;

namespace XmlLoader.XMLModels
{
    public class XmlUser
    {
        [XmlElement("fio")]
        public string Name { get; set; } = null!;

        [XmlElement("email")]
        public string Email { get; set; } = null!;

    }
}
