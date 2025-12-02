using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("BaiViet")]
    public class BaiViet
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TieuDe")]
        public string TieuDe { get; set; }

        [XmlElement("HinhAnh")]
        public string HinhAnh { get; set; }

        [XmlElement("TomTat")]
        public string TomTat { get; set; }

        [XmlElement("NoiDung")]
        public string NoiDung { get; set; }

        [XmlElement("NgayDang")]
        public DateTime NgayDang { get; set; }

        [XmlElement("MaNguoiViet")]
        public int MaNguoiViet { get; set; }

        [XmlElement("HienThi")]
        public bool HienThi { get; set; }
    }
}