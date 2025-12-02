using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Category")]
    public class LoaiSanPham
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string TenLoai { get; set; }

        [XmlIgnore]
        public bool HienThi { get; set; }

        public LoaiSanPham() { }
    }
}