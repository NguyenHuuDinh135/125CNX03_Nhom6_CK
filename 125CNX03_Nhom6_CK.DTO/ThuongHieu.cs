using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Brand")]
    public class ThuongHieu
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string TenThuongHieu { get; set; }

        [XmlElement("Logo")]
        public string HinhAnh { get; set; }

        public ThuongHieu() { }
    }
}