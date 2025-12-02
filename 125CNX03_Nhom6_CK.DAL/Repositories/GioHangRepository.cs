using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class GioHangRepository : IGioHangRepository
    {
        public List<GioHang> GetAll()
        {
            var list = new List<GioHang>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM GioHang", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public GioHang GetById(int id)
        {
            GioHang gh = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM GioHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    gh = Map(rd);
                }
            }
            return gh;
        }

        public GioHang GetByNguoiDung(int maNguoiDung)
        {
            GioHang gh = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM GioHang WHERE MaNguoiDung=@MaNguoiDung", conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    gh = Map(rd);
                }
            }
            return gh;
        }

        public bool Add(GioHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO GioHang(MaNguoiDung, NgayCapNhat)
                    VALUES (@MaNguoiDung, @NgayCapNhat)", conn);

                cmd.Parameters.AddWithValue("@MaNguoiDung", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@NgayCapNhat", entity.NgayCapNhat);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(GioHang entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE GioHang SET 
                        MaNguoiDung=@MaNguoiDung,
                        NgayCapNhat=@NgayCapNhat
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@MaNguoiDung", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@NgayCapNhat", entity.NgayCapNhat);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM GioHang WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private GioHang Map(SqlDataReader rd)
        {
            return new GioHang
            {
                Id = (int)rd["Id"],
                MaNguoiDung = Convert.ToInt32(rd["MaNguoiDung"]),
                NgayCapNhat = Convert.ToDateTime(rd["NgayCapNhat"])
            };
        }
    }
}
