using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("NguoiDung")]
    public class NguoiDung
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("HoTen")]
        public string HoTen { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        // Đã bỏ XmlIgnore để có thể lưu và đọc mật khẩu từ DB
        [XmlElement("MatKhauHash")]
        public string MatKhauHash { get; set; }

        [XmlElement("SoDienThoai")]
        public string SoDienThoai { get; set; }

        [XmlElement("DiaChi")]
        public string DiaChi { get; set; }

        [XmlElement("VaiTro")]
        public string VaiTro { get; set; } // "Admin" hoặc "Customer"

        [XmlElement("NgayTao")]
        public DateTime NgayTao { get; set; }

        // Đã bỏ XmlIgnore để quản lý trạng thái khóa/mở
        [XmlElement("TrangThai")]
        public bool TrangThai { get; set; }
    }
}