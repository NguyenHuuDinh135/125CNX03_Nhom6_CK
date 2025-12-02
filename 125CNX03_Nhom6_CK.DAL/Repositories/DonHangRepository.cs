using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class DonHangRepository : IDonHangRepository
    {
        public List<DonHang> GetAll()
        {
            var list = new List<DonHang>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM DonHang", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public DonHang GetById(int id)
        {
            DonHang dh = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM DonHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    dh = Map(rd);
                }
            }
            return dh;
        }

        public bool Add(DonHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO DonHang(MaNguoiDung, NgayDatHang, TrangThaiDonHang)
                    VALUES (@User, @Ngay, @TrangThai)", conn);

                cmd.Parameters.AddWithValue("@User", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@Ngay", entity.NgayDatHang);
                cmd.Parameters.AddWithValue("@TrangThai", entity.TrangThaiDonHang);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(DonHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE DonHang SET MaNguoiDung=@User,
                        NgayDatHang=@Ngay, TrangThaiDonHang=@TrangThai
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@User", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@Ngay", entity.NgayDatHang);
                cmd.Parameters.AddWithValue("@TrangThai", entity.TrangThaiDonHang);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM DonHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private DonHang Map(SqlDataReader rd)
        {
            return new DonHang
            {
                Id = (int)rd["Id"],
                MaNguoiDung = Convert.ToInt32(rd["MaNguoiDung"]),
                NgayDatHang = Convert.ToDateTime(rd["NgayDatHang"]),
                TrangThaiDonHang = Convert.ToInt32(rd["TrangThaiDonHang"])
            };
        }
    }
}
