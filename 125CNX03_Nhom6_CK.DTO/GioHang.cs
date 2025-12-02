using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Cart")]
    public class GioHang
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlAttribute("UserId")]
        public int MaNguoiDung { get; set; }

        [XmlElement("LastUpdated")]
        public DateTime NgayCapNhat { get; set; }

        // Một giỏ hàng chứa nhiều chi tiết
        [XmlArray("CartItems")]
        [XmlArrayItem("Item")]
        public List<ChiTietGioHang> ChiTiet { get; set; } = new List<ChiTietGioHang>();
    }
}