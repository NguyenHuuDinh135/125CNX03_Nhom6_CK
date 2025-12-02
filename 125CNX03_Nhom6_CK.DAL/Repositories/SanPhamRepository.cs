using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class SanPhamRepository : ISanPhamRepository
    {
        public List<SanPham> GetAll()
        {
            var list = new List<SanPham>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM SanPham";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public SanPham GetById(int id)
        {
            SanPham sp = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SanPham WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    sp = Map(rd);
                }
            }
            return sp;
        }

        public bool Add(SanPham sp)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO SanPham (TenSanPham, Gia, MaLoai, MaThuongHieu)
                    VALUES (@Ten, @Gia, @Loai, @ThuongHieu)
                ", conn);

                cmd.Parameters.AddWithValue("@Ten", sp.TenSanPham);
                cmd.Parameters.AddWithValue("@Gia", sp.Gia);
                cmd.Parameters.AddWithValue("@Loai", sp.MaLoai);
                cmd.Parameters.AddWithValue("@ThuongHieu", sp.MaThuongHieu);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(SanPham sp)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE SanPham SET TenSanPham=@Ten, Gia=@Gia,
                    MaLoai=@Loai, MaThuongHieu=@ThuongHieu
                    WHERE Id=@Id
                ", conn);

                cmd.Parameters.AddWithValue("@Id", sp.Id);
                cmd.Parameters.AddWithValue("@Ten", sp.TenSanPham);
                cmd.Parameters.AddWithValue("@Gia", sp.Gia);
                cmd.Parameters.AddWithValue("@Loai", sp.MaLoai);
                cmd.Parameters.AddWithValue("@ThuongHieu", sp.MaThuongHieu);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM SanPham WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private SanPham Map(SqlDataReader rd)
        {
            return new SanPham
            {
                Id = Convert.ToInt32(rd["Id"]),
                TenSanPham = rd["TenSanPham"].ToString(),
                Gia = Convert.ToDecimal(rd["Gia"]),
                MaLoai = Convert.ToInt32(rd["MaLoai"]),
                MaThuongHieu = Convert.ToInt32(rd["MaThuongHieu"])
            };
        }
    }
}
