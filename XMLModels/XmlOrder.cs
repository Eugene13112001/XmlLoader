using System.Xml.Serialization;

namespace XmlLoader.XMLModels
{
    public class XmlOrder
    {
        [XmlElement("no")]
        public int No { get; set; }

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlElement("reg_date")]
        public string InputDate
        {
            get { return Date.ToString("yyyy.MM.dd"); }
            set { Date = DateTime.Parse(value); }
        }

        [XmlIgnore]
        public double Sum { get; set; }

        [XmlElement("sum")]
        public string InputSum
        {
            get { return Sum.ToString(); }
            set { Sum = double.Parse(value.Replace(",", ".")); }
        }

        [XmlElement("user")]
        public XmlUser User { get; set; } = null!;

        [XmlElement("product")]
        public List<XmlProduct> Products { get; set; } = null!;
    }
}
