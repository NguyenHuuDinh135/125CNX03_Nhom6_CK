using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ChiTietDonHangRepository : IChiTietDonHangRepository
    {
        public List<ChiTietDonHang> GetAll()
        {
            var list = new List<ChiTietDonHang>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ChiTietDonHang", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public ChiTietDonHang GetById(int id)
        {
            ChiTietDonHang ct = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ChiTietDonHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    ct = Map(rd);
                }
            }
            return ct;
        }

        public bool Add(ChiTietDonHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO ChiTietDonHang(MaDonHang, DonGia, SoLuong)
                    VALUES (@DonHang, @DonGia, @SL)", conn);

                cmd.Parameters.AddWithValue("@DonHang", entity.MaDonHang);
                cmd.Parameters.AddWithValue("@DonGia", entity.DonGia);
                cmd.Parameters.AddWithValue("@SL", entity.SoLuong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(ChiTietDonHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE ChiTietDonHang SET MaDonHang=@DonHang,
                        DonGia=@DonGia, SoLuong=@SL
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@DonHang", entity.MaDonHang);
                cmd.Parameters.AddWithValue("@DonGia", entity.DonGia);
                cmd.Parameters.AddWithValue("@SL", entity.SoLuong);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM ChiTietDonHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private ChiTietDonHang Map(SqlDataReader rd)
        {
            return new ChiTietDonHang
            {
                Id = (int)rd["Id"],
                MaDonHang = Convert.ToInt32(rd["MaDonHang"]),
                DonGia = Convert.ToDecimal(rd["DonGia"]),
                SoLuong = Convert.ToInt32(rd["SoLuong"])
            };
        }
    }
}
