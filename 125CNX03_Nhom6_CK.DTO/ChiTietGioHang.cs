using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    public class ChiTietGioHang
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlIgnore]
        public int MaGioHang { get; set; }

        [XmlAttribute("ProductId")]
        public int MaSanPham { get; set; }

        // Trường này để hiển thị lên lưới cho tiện (ko lưu trong DB bảng này nhưng cần khi load)
        [XmlIgnore]
        public string TenSanPham { get; set; }

        [XmlElement("Price")]
        public decimal DonGia { get; set; }

        [XmlElement("Quantity")]
        public int SoLuong { get; set; }
    }
}