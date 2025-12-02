using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("ChiTietDonHang")]
    public class ChiTietDonHang
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("MaDonHang")]
        public int MaDonHang { get; set; }

        [XmlElement("ItemOrdered_MaSanPham")]
        public int ItemOrdered_MaSanPham { get; set; }

        [XmlElement("ItemOrdered_TenSanPham")]
        public string ItemOrdered_TenSanPham { get; set; }

        [XmlElement("ItemOrdered_DuongDanAnh")]
        public string ItemOrdered_DuongDanAnh { get; set; }

        [XmlElement("DonGia")]
        public decimal DonGia { get; set; }

        [XmlElement("SoLuong")]
        public int SoLuong { get; set; }

        // Tính toán thành tiền (không cần lưu vì có thể tính lại)
        [XmlIgnore]
        public decimal ThanhTien => DonGia * SoLuong;

        public ChiTietDonHang() { }
    }
}