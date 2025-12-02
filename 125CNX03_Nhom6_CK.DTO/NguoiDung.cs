using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("User")]
    public class NguoiDung
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("FullName")]
        public string HoTen { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlIgnore] // Mật khẩu tuyệt đối không xuất ra XML
        public string MatKhauHash { get; set; }

        [XmlElement("Phone")]
        public string SoDienThoai { get; set; }

        [XmlElement("Address")]
        public string DiaChi { get; set; }

        [XmlAttribute("Role")]
        public string VaiTro { get; set; } // Admin, Customer

        [XmlElement("CreatedDate")]
        public DateTime NgayTao { get; set; }

        [XmlIgnore]
        public bool TrangThai { get; set; }
    }
}