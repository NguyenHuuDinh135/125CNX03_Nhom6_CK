using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Product")]
    public class SanPham
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlElement("Name")]
        public string TenSanPham { get; set; }

        [XmlElement("Description")]
        public string MoTa { get; set; }

        [XmlElement("DetailHtml")]
        public string ChiTiet { get; set; } // Nội dung HTML

        [XmlElement("Price")]
        public decimal Gia { get; set; }

        [XmlElement("SalePrice")]
        public decimal GiaKhuyenMai { get; set; }

        [XmlElement("Image")]
        public string DuongDanAnh { get; set; }

        [XmlElement("Stock")]
        public int SoLuongTon { get; set; }

        [XmlAttribute("CategoryId")]
        public int MaLoai { get; set; }

        [XmlAttribute("BrandId")]
        public int MaThuongHieu { get; set; }

        [XmlIgnore]
        public bool HienThi { get; set; }
    }
}