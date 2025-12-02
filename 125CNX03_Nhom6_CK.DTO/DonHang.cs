using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("OrderInvoice")] // Tên thẻ gốc của file XML hóa đơn
    public class DonHang
    {
        [XmlElement("OrderId")]
        public int Id { get; set; }

        [XmlIgnore]
        public int MaNguoiDung { get; set; }

        [XmlElement("CustomerName")]
        public string NguoiNhan_Ten { get; set; }

        [XmlElement("Phone")]
        public string NguoiNhan_SDT { get; set; }

        [XmlElement("Address")]
        public string DiaChi_GiaoHang { get; set; }

        [XmlElement("OrderDate")]
        public DateTime NgayDatHang { get; set; }

        [XmlElement("TotalAmount")]
        public decimal TongTien { get; set; }

        [XmlElement("Status")]
        public int TrangThaiDonHang { get; set; }

        [XmlElement("Note")]
        public string GhiChu { get; set; }

        // --- DANH SÁCH CHI TIẾT ĐƠN HÀNG ---
        // Khi xuất XML sẽ tạo cấu trúc lồng nhau:
        // <Items> <Item>...</Item> </Items>
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<ChiTietDonHang> ChiTiet { get; set; } = new List<ChiTietDonHang>();

        public DonHang() { }
    }
}