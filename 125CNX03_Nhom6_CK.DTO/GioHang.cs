using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("GioHang")]
    public class GioHang
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("MaNguoiDung")]
        public int MaNguoiDung { get; set; }

        [XmlElement("NgayCapNhat")]
        public DateTime NgayCapNhat { get; set; }

        [XmlIgnore] // Load riêng từ bảng ChiTietGioHang
        public List<ChiTietGioHang> ChiTiet { get; set; } = new List<ChiTietGioHang>();
    }
}