using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("LienHe")]
    public class LienHe
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("HoTen")]
        public string HoTen { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("SoDienThoai")]
        public string SoDienThoai { get; set; }

        [XmlElement("NoiDung")]
        public string NoiDung { get; set; }

        [XmlElement("NgayGui")]
        public DateTime NgayGui { get; set; }

        [XmlElement("DaXem")]
        public bool DaXem { get; set; }
    }
}