using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    public class PhuongThucThanhToan
    {
        public int Id { get; set; }
        public int MaNguoiDung { get; set; }

        [XmlElement("Alias")]
        public string TenGoiNho { get; set; } // Ví dụ: Thẻ VISA chính

        [XmlElement("CardToken")]
        public string MaThe { get; set; }

        [XmlElement("Last4")]
        public string BonSoCuoi { get; set; }
    }
}