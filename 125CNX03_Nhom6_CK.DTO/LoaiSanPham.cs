using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("LoaiSanPham")]
    public class LoaiSanPham
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TenLoai")]
        public string TenLoai { get; set; }

        [XmlElement("HienThi")]
        public bool HienThi { get; set; }
    }
}