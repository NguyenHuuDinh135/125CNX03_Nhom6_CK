using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class LienHeRepository : ILienHeRepository
    {
        public List<LienHe> GetAll()
        {
            var list = new List<LienHe>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM LienHe", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public LienHe GetById(int id)
        {
            LienHe lh = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM LienHe WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    lh = Map(rd);
                }
            }
            return lh;
        }

        public bool Add(LienHe entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO LienHe(HoTen, Email, SoDienThoai, NoiDung, NgayGui, DaXem)
                    VALUES (@HoTen, @Email, @SoDienThoai, @NoiDung, @NgayGui, @DaXem)", conn);

                cmd.Parameters.AddWithValue("@HoTen", (object)entity.HoTen ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)entity.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SoDienThoai", (object)entity.SoDienThoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NoiDung", (object)entity.NoiDung ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayGui", entity.NgayGui);
                cmd.Parameters.AddWithValue("@DaXem", entity.DaXem);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(LienHe entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE LienHe SET
                        HoTen=@HoTen,
                        Email=@Email,
                        SoDienThoai=@SoDienThoai,
                        NoiDung=@NoiDung,
                        NgayGui=@NgayGui,
                        DaXem=@DaXem
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@HoTen", (object)entity.HoTen ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)entity.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@SoDienThoai", (object)entity.SoDienThoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NoiDung", (object)entity.NoiDung ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayGui", entity.NgayGui);
                cmd.Parameters.AddWithValue("@DaXem", entity.DaXem);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM LienHe WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private LienHe Map(SqlDataReader rd)
        {
            return new LienHe
            {
                Id = (int)rd["Id"],
                HoTen = rd["HoTen"] as string,
                Email = rd["Email"] as string,
                SoDienThoai = rd["SoDienThoai"] as string,
                NoiDung = rd["NoiDung"] as string,
                NgayGui = Convert.ToDateTime(rd["NgayGui"]),
                DaXem = Convert.ToBoolean(rd["DaXem"])
            };
        }
    }
}
