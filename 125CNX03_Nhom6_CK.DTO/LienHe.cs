using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Contact")]
    public class LienHe
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("SenderName")]
        public string HoTen { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlElement("Phone")]
        public string SoDienThoai { get; set; }

        [XmlElement("Message")]
        public string NoiDung { get; set; }

        [XmlElement("SentDate")]
        public DateTime NgayGui { get; set; }

        [XmlAttribute("IsRead")]
        public bool DaXem { get; set; }
    }
}