using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("BannerItem")]
    public class Banner
    {
        public int Id { get; set; }

        [XmlElement("Name")]
        public string TenBanner { get; set; }

        [XmlElement("ImageUrl")]
        public string HinhAnh { get; set; }

        [XmlElement("Link")]
        public string LienKet { get; set; }

        [XmlAttribute("Order")]
        public int ThuTu { get; set; }

        public bool HienThi { get; set; }
    }
}