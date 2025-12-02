using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("Article")]
    public class BaiViet
    {
        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("Title")]
        public string TieuDe { get; set; }

        [XmlElement("Image")]
        public string HinhAnh { get; set; }

        [XmlElement("Summary")]
        public string TomTat { get; set; }

        [XmlElement("Content")]
        public string NoiDung { get; set; } // HTML

        [XmlElement("PublishDate")]
        public DateTime NgayDang { get; set; }

        [XmlAttribute("AuthorId")]
        public int MaNguoiViet { get; set; }

        [XmlIgnore]
        public bool HienThi { get; set; }
    }
}