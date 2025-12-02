using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ChiTietGioHangRepository : IChiTietGioHangRepository
    {
        public List<ChiTietGioHang> GetAll()
        {
            var list = new List<ChiTietGioHang>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ChiTietGioHang", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public ChiTietGioHang GetById(int id)
        {
            ChiTietGioHang ct = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ChiTietGioHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    ct = Map(rd);
                }
            }
            return ct;
        }

        public List<ChiTietGioHang> GetByGioHang(int maGioHang)
        {
            var list = new List<ChiTietGioHang>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ChiTietGioHang WHERE MaGioHang=@MaGioHang", conn);
                cmd.Parameters.AddWithValue("@MaGioHang", maGioHang);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public bool Add(ChiTietGioHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO ChiTietGioHang(MaGioHang, MaSanPham, DonGia, SoLuong)
                    VALUES (@MaGioHang, @MaSanPham, @DonGia, @SoLuong)", conn);

                cmd.Parameters.AddWithValue("@MaGioHang", entity.MaGioHang);
                cmd.Parameters.AddWithValue("@MaSanPham", entity.MaSanPham);
                cmd.Parameters.AddWithValue("@DonGia", entity.DonGia);
                cmd.Parameters.AddWithValue("@SoLuong", entity.SoLuong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(ChiTietGioHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE ChiTietGioHang SET 
                        MaGioHang=@MaGioHang,
                        MaSanPham=@MaSanPham,
                        DonGia=@DonGia,
                        SoLuong=@SoLuong
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@MaGioHang", entity.MaGioHang);
                cmd.Parameters.AddWithValue("@MaSanPham", entity.MaSanPham);
                cmd.Parameters.AddWithValue("@DonGia", entity.DonGia);
                cmd.Parameters.AddWithValue("@SoLuong", entity.SoLuong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM ChiTietGioHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteByGioHang(int maGioHang)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM ChiTietGioHang WHERE MaGioHang=@MaGioHang", conn);
                cmd.Parameters.AddWithValue("@MaGioHang", maGioHang);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private ChiTietGioHang Map(SqlDataReader rd)
        {
            return new ChiTietGioHang
            {
                Id = (int)rd["Id"],
                MaGioHang = Convert.ToInt32(rd["MaGioHang"]),
                MaSanPham = Convert.ToInt32(rd["MaSanPham"]),
                DonGia = Convert.ToDecimal(rd["DonGia"]),
                SoLuong = Convert.ToInt32(rd["SoLuong"])
            };
        }
    }
}
