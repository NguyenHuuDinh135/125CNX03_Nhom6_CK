using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("PhuongThucThanhToan")]
    public class PhuongThucThanhToan
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("MaNguoiDung")]
        public int MaNguoiDung { get; set; }

        [XmlElement("TenGoiNho")]
        public string TenGoiNho { get; set; }

        [XmlElement("MaThe")]
        public string MaThe { get; set; }

        [XmlElement("BonSoCuoi")]
        public string BonSoCuoi { get; set; }
    }
}