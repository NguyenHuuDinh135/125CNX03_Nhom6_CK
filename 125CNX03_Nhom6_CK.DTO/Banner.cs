using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Banner")]
    public class Banner
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TenBanner")]
        public string TenBanner { get; set; }

        [XmlElement("HinhAnh")]
        public string HinhAnh { get; set; }

        [XmlElement("LienKet")]
        public string LienKet { get; set; }

        [XmlElement("ThuTu")]
        public int ThuTu { get; set; }

        [XmlElement("HienThi")]
        public bool HienThi { get; set; }
    }
}