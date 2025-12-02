using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL
{
    public static class DataSeeder
    {
        private static string xmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DataSeeder.xml");

        public static void Seed()
        {
            if (!File.Exists(xmlFilePath))
            {
                Console.WriteLine("File XML seed dữ liệu không tồn tại: " + xmlFilePath);
                return;
            }

            XDocument doc = XDocument.Load(xmlFilePath);

            SeedThuongHieu(doc);
            SeedLoaiSanPham(doc);
            SeedNguoiDung(doc);
            SeedSanPham(doc);
            SeedBaiViet(doc);
            SeedBanner(doc);
            SeedLienHe(doc);
            SeedDonHang(doc);
            SeedChiTietDonHang(doc);

            Console.WriteLine("Seed dữ liệu hoàn tất!");
        }

        private static void SeedThuongHieu(XDocument doc)
        {
            var items = doc.Descendants("ThuongHieu");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM ThuongHieu WHERE Id = @Id";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@Id", int.Parse(item.Attribute("Id").Value));
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = "INSERT INTO ThuongHieu (Id, TenThuongHieu) VALUES (@Id, @Ten)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@Id", int.Parse(item.Attribute("Id").Value));
                cmd.Parameters.AddWithValue("@Ten", item.Attribute("TenThuongHieu").Value);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedLoaiSanPham(XDocument doc)
        {
            var items = doc.Descendants("LoaiSanPham");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM LoaiSanPham WHERE Id = @Id";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@Id", int.Parse(item.Attribute("Id").Value));
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = "INSERT INTO LoaiSanPham (Id, TenLoai) VALUES (@Id, @Ten)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@Id", int.Parse(item.Attribute("Id").Value));
                cmd.Parameters.AddWithValue("@Ten", item.Attribute("TenLoai").Value);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedNguoiDung(XDocument doc)
        {
            var items = doc.Descendants("NguoiDung");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM NguoiDung WHERE Email = @Email";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@Email", item.Attribute("Email").Value);
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO NguoiDung (HoTen, Email, MatKhauHash, VaiTro, SoDienThoai, DiaChi)
                    VALUES (@HoTen, @Email, @MatKhauHash, @VaiTro, @SDT, @DiaChi)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@HoTen", item.Attribute("HoTen").Value);
                cmd.Parameters.AddWithValue("@Email", item.Attribute("Email").Value);
                cmd.Parameters.AddWithValue("@MatKhauHash", item.Attribute("MatKhauHash").Value);
                cmd.Parameters.AddWithValue("@VaiTro", item.Attribute("VaiTro").Value);
                cmd.Parameters.AddWithValue("@SDT", item.Attribute("SoDienThoai").Value);
                cmd.Parameters.AddWithValue("@DiaChi", item.Attribute("DiaChi").Value);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedSanPham(XDocument doc)
        {
            var items = doc.Descendants("SanPham");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM SanPham WHERE TenSanPham = @TenSanPham";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@TenSanPham", item.Attribute("TenSanPham").Value);
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO SanPham 
                    (TenSanPham, MoTa, ChiTiet, Gia, GiaKhuyenMai, DuongDanAnh,
                     SoLuongTon, MaLoai, MaThuongHieu, HienThi)
                    VALUES
                    (@TenSanPham, @MoTa, @ChiTiet, @Gia, @GiaKhuyenMai, @DuongDanAnh,
                     @SoLuongTon, @MaLoai, @MaThuongHieu, @HienThi)";

                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@TenSanPham", item.Attribute("TenSanPham").Value);
                cmd.Parameters.AddWithValue("@MoTa", item.Attribute("MoTa").Value);
                cmd.Parameters.AddWithValue("@ChiTiet", item.Attribute("ChiTiet").Value);
                cmd.Parameters.AddWithValue("@Gia", decimal.Parse(item.Attribute("Gia").Value));
                cmd.Parameters.AddWithValue("@GiaKhuyenMai", decimal.Parse(item.Attribute("GiaKhuyenMai").Value));
                cmd.Parameters.AddWithValue("@DuongDanAnh", item.Attribute("DuongDanAnh").Value);
                cmd.Parameters.AddWithValue("@SoLuongTon", int.Parse(item.Attribute("SoLuongTon").Value));
                cmd.Parameters.AddWithValue("@MaLoai", int.Parse(item.Attribute("MaLoai").Value));
                cmd.Parameters.AddWithValue("@MaThuongHieu", int.Parse(item.Attribute("MaThuongHieu").Value));
                cmd.Parameters.AddWithValue("@HienThi", int.Parse(item.Attribute("HienThi").Value));
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedBaiViet(XDocument doc)
        {
            var items = doc.Descendants("BaiViet");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM BaiViet WHERE TieuDe = @TieuDe";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@TieuDe", item.Attribute("TieuDe").Value);
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO BaiViet (TieuDe, HinhAnh, TomTat, NoiDung, MaNguoiViet)
                    VALUES (@TieuDe, @HinhAnh, @TomTat, @NoiDung, @MaNguoiViet)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@TieuDe", item.Attribute("TieuDe").Value);
                cmd.Parameters.AddWithValue("@HinhAnh", item.Attribute("HinhAnh").Value);
                cmd.Parameters.AddWithValue("@TomTat", item.Attribute("TomTat").Value);
                cmd.Parameters.AddWithValue("@NoiDung", item.Attribute("NoiDung").Value);
                cmd.Parameters.AddWithValue("@MaNguoiViet", int.Parse(item.Attribute("MaNguoiViet").Value));
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedBanner(XDocument doc)
        {
            var items = doc.Descendants("Banner");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM Banner WHERE TenBanner = @TenBanner";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@TenBanner", item.Attribute("TenBanner").Value);
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO Banner (TenBanner, HinhAnh, LienKet, ThuTu)
                    VALUES (@TenBanner, @HinhAnh, @LienKet, @ThuTu)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@TenBanner", item.Attribute("TenBanner").Value);
                cmd.Parameters.AddWithValue("@HinhAnh", item.Attribute("HinhAnh").Value);
                cmd.Parameters.AddWithValue("@LienKet", item.Attribute("LienKet").Value);
                cmd.Parameters.AddWithValue("@ThuTu", int.Parse(item.Attribute("ThuTu").Value));
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedLienHe(XDocument doc)
        {
            var items = doc.Descendants("LienHe");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = "SELECT COUNT(*) FROM LienHe WHERE SoDienThoai = @SDT";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@SDT", item.Attribute("SoDienThoai").Value);
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO LienHe (HoTen, Email, SoDienThoai, NoiDung)
                    VALUES (@HoTen, @Email, @SDT, @NoiDung)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@HoTen", item.Attribute("HoTen").Value);
                cmd.Parameters.AddWithValue("@Email", item.Attribute("Email").Value);
                cmd.Parameters.AddWithValue("@SDT", item.Attribute("SoDienThoai").Value);
                cmd.Parameters.AddWithValue("@NoiDung", item.Attribute("NoiDung").Value);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedDonHang(XDocument doc)
        {
            var items = doc.Descendants("DonHang");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = @"
                    SELECT COUNT(*) FROM DonHang 
                    WHERE NguoiNhan_Ten = @Ten AND MaNguoiDung = @MaNguoiDung";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@Ten", item.Attribute("NguoiNhan_Ten").Value);
                checkCmd.Parameters.AddWithValue("@MaNguoiDung", int.Parse(item.Attribute("MaNguoiDung").Value));
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO DonHang (MaNguoiDung, NgayDatHang, TrangThaiDonHang,
                                         NguoiNhan_Ten, NguoiNhan_SDT, DiaChi_Duong, DiaChi_ThanhPho, DiaChi_Tinh, TongTien)
                    VALUES (@MaNguoiDung, @NgayDatHang, @TrangThai,
                            @Ten, @SDT, @Duong, @ThanhPho, @Tinh, @TongTien)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@MaNguoiDung", int.Parse(item.Attribute("MaNguoiDung").Value));
                cmd.Parameters.AddWithValue("@NgayDatHang", DateTime.Parse(item.Attribute("NgayDatHang").Value));
                cmd.Parameters.AddWithValue("@TrangThai", int.Parse(item.Attribute("TrangThaiDonHang").Value));
                cmd.Parameters.AddWithValue("@Ten", item.Attribute("NguoiNhan_Ten").Value);
                cmd.Parameters.AddWithValue("@SDT", item.Attribute("NguoiNhan_SDT").Value);
                cmd.Parameters.AddWithValue("@Duong", item.Attribute("DiaChi_Duong").Value);
                cmd.Parameters.AddWithValue("@ThanhPho", item.Attribute("DiaChi_ThanhPho").Value);
                cmd.Parameters.AddWithValue("@Tinh", item.Attribute("DiaChi_Tinh").Value);
                cmd.Parameters.AddWithValue("@TongTien", decimal.Parse(item.Attribute("TongTien").Value));
                cmd.ExecuteNonQuery();
            }
        }

        private static void SeedChiTietDonHang(XDocument doc)
        {
            var items = doc.Descendants("ChiTietDonHang");
            using var con = DbConnection.GetConnection();
            con.Open();

            foreach (var item in items)
            {
                string checkSql = @"
                    SELECT COUNT(*) FROM ChiTietDonHang 
                    WHERE MaDonHang = @MaDonHang AND ItemOrdered_MaSanPham = @MaSP";
                using var checkCmd = new SqlCommand(checkSql, con);
                checkCmd.Parameters.AddWithValue("@MaDonHang", int.Parse(item.Attribute("MaDonHang").Value));
                checkCmd.Parameters.AddWithValue("@MaSP", int.Parse(item.Attribute("ItemOrdered_MaSanPham").Value));
                if ((int)checkCmd.ExecuteScalar() > 0) continue;

                string insertSql = @"
                    INSERT INTO ChiTietDonHang (MaDonHang, ItemOrdered_MaSanPham, ItemOrdered_TenSanPham, DonGia, SoLuong)
                    VALUES (@MaDonHang, @MaSP, @TenSP, @DonGia, @SoLuong)";
                using var cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.AddWithValue("@MaDonHang", int.Parse(item.Attribute("MaDonHang").Value));
                cmd.Parameters.AddWithValue("@MaSP", int.Parse(item.Attribute("ItemOrdered_MaSanPham").Value));
                cmd.Parameters.AddWithValue("@TenSP", item.Attribute("ItemOrdered_TenSanPham").Value);
                cmd.Parameters.AddWithValue("@DonGia", decimal.Parse(item.Attribute("DonGia").Value));
                cmd.Parameters.AddWithValue("@SoLuong", int.Parse(item.Attribute("SoLuong").Value));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
