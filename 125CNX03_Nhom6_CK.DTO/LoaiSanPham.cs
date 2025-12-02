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

        [XmlIgnore] // Không cần xuất ra XML
        public bool HienThi { get; set; }
    }
}