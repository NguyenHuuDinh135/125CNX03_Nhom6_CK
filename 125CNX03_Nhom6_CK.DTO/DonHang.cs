using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("DonHang")]
    public class DonHang
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("MaNguoiDung")]
        public int MaNguoiDung { get; set; }

        [XmlElement("NguoiNhan_Ten")]
        public string NguoiNhan_Ten { get; set; }

        [XmlElement("NguoiNhan_SDT")]
        public string NguoiNhan_SDT { get; set; }

        // --- Sửa đổi để khớp với Schema XML (Tách 3 trường) ---
        [XmlElement("DiaChi_Duong")]
        public string DiaChi_Duong { get; set; }

        [XmlElement("DiaChi_ThanhPho")]
        public string DiaChi_ThanhPho { get; set; }

        [XmlElement("DiaChi_Tinh")]
        public string DiaChi_Tinh { get; set; }

        // Thuộc tính phụ trợ để hiển thị (không lưu vào XML)
        [XmlIgnore]
        public string DiaChi_DayDu => $"{DiaChi_Duong}, {DiaChi_ThanhPho}, {DiaChi_Tinh}";

        [XmlElement("NgayDatHang")]
        public DateTime NgayDatHang { get; set; }

        [XmlElement("TongTien")]
        public decimal TongTien { get; set; }

        [XmlElement("TrangThaiDonHang")]
        public int TrangThaiDonHang { get; set; }

        [XmlElement("GhiChu")]
        public string GhiChu { get; set; }

        // Danh sách chi tiết (Dùng khi load object đầy đủ, nhưng trong bảng DB XML thì nó nằm bảng riêng)
        [XmlIgnore]
        public List<ChiTietDonHang> ChiTiet { get; set; } = new List<ChiTietDonHang>();

        public DonHang() { }
    }
}