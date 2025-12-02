using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class LoaiSanPhamRepository : ILoaiSanPhamRepository
    {
        public List<LoaiSanPham> GetAll()
        {
            var list = new List<LoaiSanPham>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM LoaiSanPham", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public LoaiSanPham GetById(int id)
        {
            LoaiSanPham loai = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM LoaiSanPham WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    loai = Map(rd);
                }
            }
            return loai;
        }

        public bool Add(LoaiSanPham entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO LoaiSanPham (TenLoai) VALUES (@Ten)", conn);
                cmd.Parameters.AddWithValue("@Ten", entity.TenLoai);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(LoaiSanPham entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE LoaiSanPham SET TenLoai=@Ten WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Ten", entity.TenLoai);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM LoaiSanPham WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private LoaiSanPham Map(SqlDataReader rd)
        {
            return new LoaiSanPham
            {
                Id = (int)rd["Id"],
                TenLoai = rd["TenLoai"].ToString()
            };
        }
    }
}
