using System;

namespace _125CNX03_Nhom6_CK.DTO
{
    public class NguoiDung
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string MatKhauHash { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; }
        public string VaiTro { get; set; } // "Admin" hoặc "Customer"
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; }

        public NguoiDung() { }
    }
}