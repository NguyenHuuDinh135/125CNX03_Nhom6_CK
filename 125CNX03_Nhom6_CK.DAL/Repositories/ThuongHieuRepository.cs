using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        public List<ThuongHieu> GetAll()
        {
            var list = new List<ThuongHieu>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ThuongHieu", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public ThuongHieu GetById(int id)
        {
            ThuongHieu th = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM ThuongHieu WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    th = Map(rd);
                }
            }
            return th;
        }

        public bool Add(ThuongHieu entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO ThuongHieu (TenThuongHieu, HinhAnh) VALUES (@Ten, @Anh)", conn);
                cmd.Parameters.AddWithValue("@Ten", entity.TenThuongHieu);
                cmd.Parameters.AddWithValue("@Anh", entity.HinhAnh);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(ThuongHieu entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE ThuongHieu SET TenThuongHieu=@Ten, HinhAnh=@Anh WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@Ten", entity.TenThuongHieu);
                cmd.Parameters.AddWithValue("@Anh", entity.HinhAnh);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM ThuongHieu WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private ThuongHieu Map(SqlDataReader rd)
        {
            return new ThuongHieu
            {
                Id = (int)rd["Id"],
                TenThuongHieu = rd["TenThuongHieu"].ToString(),
                HinhAnh = rd["HinhAnh"].ToString()
            };
        }
    }
}
