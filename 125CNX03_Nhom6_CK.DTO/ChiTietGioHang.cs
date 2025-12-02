using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("ChiTietGioHang")]
    public class ChiTietGioHang
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("MaGioHang")]
        public int MaGioHang { get; set; }

        [XmlElement("MaSanPham")]
        public int MaSanPham { get; set; }

        // Trường này để hiển thị UI, không lưu trong bảng ChiTietGioHang
        [XmlIgnore]
        public string TenSanPham { get; set; }

        [XmlElement("DonGia")]
        public decimal DonGia { get; set; }

        [XmlElement("SoLuong")]
        public int SoLuong { get; set; }
    }
}