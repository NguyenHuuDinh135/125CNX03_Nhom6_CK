using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("SanPham")]
    public class SanPham
    {
        // Phải có Id để định danh sản phẩm
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TenSanPham")]
        public string TenSanPham { get; set; }

        [XmlElement("MoTa")]
        public string MoTa { get; set; }

        [XmlElement("ChiTiet")]
        public string ChiTiet { get; set; } // Nội dung HTML hoặc text dài

        [XmlElement("Gia")]
        public decimal Gia { get; set; }

        [XmlElement("GiaKhuyenMai")]
        public decimal GiaKhuyenMai { get; set; }

        [XmlElement("DuongDanAnh")]
        public string DuongDanAnh { get; set; }

        [XmlElement("SoLuongTon")]
        public int SoLuongTon { get; set; }

        [XmlElement("MaLoai")]
        public int MaLoai { get; set; }

        [XmlElement("MaThuongHieu")]
        public int MaThuongHieu { get; set; }

        // Cần thiết để ẩn/hiện sản phẩm trên web
        [XmlElement("HienThi")]
        public bool HienThi { get; set; }
    }
}