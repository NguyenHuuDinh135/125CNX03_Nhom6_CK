using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Product")] // Tên thẻ gốc khi làm việc với XML
    public class SanPham
    {
        // --- Mapping với SQL Server ---
        [XmlIgnore] // Không đọc/ghi trường này vào XML nhập hàng
        public int Id { get; set; }

        // --- Mapping với XML & SQL ---

        [XmlElement("Name")] // <Name>Tên Sản Phẩm</Name>
        public string TenSanPham { get; set; }

        [XmlElement("Description")]
        public string MoTa { get; set; }

        [XmlElement("Detail")]
        public string ChiTiet { get; set; }

        [XmlElement("Price")]
        public decimal Gia { get; set; }

        [XmlElement("SalePrice")]
        public decimal GiaKhuyenMai { get; set; }

        [XmlElement("Image")]
        public string DuongDanAnh { get; set; }

        [XmlElement("Stock")]
        public int SoLuongTon { get; set; }

        [XmlAttribute("CatID")] // <Product CatID="1">
        public int MaLoai { get; set; }

        [XmlAttribute("BrandID")]
        public int MaThuongHieu { get; set; }

        [XmlIgnore]
        public bool HienThi { get; set; } = true;

        public SanPham() { }
    }
}