using System.Xml.Serialization;

namespace XmlLoader.XMLModels
{
    public  class XmlProduct
    {
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("quantity")]
        public int Count { get; set; }

        [XmlIgnore]
        public double Price { get; set; }

        [XmlElement("price")]
        public string InputProce
        {
            get { return Price.ToString(); }
            set { Price = double.Parse(value.Replace(",", ".")); }
        }
    }
}
