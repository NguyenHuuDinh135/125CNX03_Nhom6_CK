using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    public class ChiTietDonHang
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlIgnore]
        public int MaDonHang { get; set; }

        [XmlElement("ProductId")]
        public int ItemOrdered_MaSanPham { get; set; }

        [XmlElement("ProductName")]
        public string ItemOrdered_TenSanPham { get; set; }

        [XmlElement("ProductImage")]
        public string ItemOrdered_DuongDanAnh { get; set; }

        [XmlElement("Price")]
        public decimal DonGia { get; set; }

        [XmlElement("Quantity")]
        public int SoLuong { get; set; }

        // Thuộc tính tính toán (Thành tiền = Giá * Số lượng)
        [XmlElement("SubTotal")]
        public decimal ThanhTien
        {
            get { return DonGia * SoLuong; }
            set { } // Cần setter rỗng để XML Serializer hoạt động
        }

        public ChiTietDonHang() { }
    }
}