using System;
using System.Xml.Serialization;

namespace _125CNX03_Nhom6_CK.DTO
{
    [Serializable]
    [XmlRoot("ThuongHieu")]
    public class ThuongHieu
    {
        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("TenThuongHieu")]
        public string TenThuongHieu { get; set; }

        [XmlElement("HinhAnh")]
        public string HinhAnh { get; set; }
    }
}