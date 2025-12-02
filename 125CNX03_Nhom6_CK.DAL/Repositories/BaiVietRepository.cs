using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class BaiVietRepository : IBaiVietRepository
    {
        public List<BaiViet> GetAll()
        {
            var list = new List<BaiViet>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM BaiViet", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public BaiViet GetById(int id)
        {
            BaiViet bv = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM BaiViet WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    bv = Map(rd);
                }
            }
            return bv;
        }

        public bool Add(BaiViet entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO BaiViet(TieuDe, HinhAnh, TomTat, NoiDung, NgayDang, MaNguoiViet, HienThi)
                    VALUES (@TieuDe, @HinhAnh, @TomTat, @NoiDung, @NgayDang, @MaNguoiViet, @HienThi)", conn);

                cmd.Parameters.AddWithValue("@TieuDe", entity.TieuDe);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)entity.HinhAnh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TomTat", (object)entity.TomTat ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NoiDung", (object)entity.NoiDung ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayDang", entity.NgayDang);
                cmd.Parameters.AddWithValue("@MaNguoiViet", entity.MaNguoiViet);
                cmd.Parameters.AddWithValue("@HienThi", entity.HienThi);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(BaiViet entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE BaiViet SET
                        TieuDe=@TieuDe,
                        HinhAnh=@HinhAnh,
                        TomTat=@TomTat,
                        NoiDung=@NoiDung,
                        NgayDang=@NgayDang,
                        MaNguoiViet=@MaNguoiViet,
                        HienThi=@HienThi
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@TieuDe", entity.TieuDe);
                cmd.Parameters.AddWithValue("@HinhAnh", (object)entity.HinhAnh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TomTat", (object)entity.TomTat ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NoiDung", (object)entity.NoiDung ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@NgayDang", entity.NgayDang);
                cmd.Parameters.AddWithValue("@MaNguoiViet", entity.MaNguoiViet);
                cmd.Parameters.AddWithValue("@HienThi", entity.HienThi);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM BaiViet WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private BaiViet Map(SqlDataReader rd)
        {
            return new BaiViet
            {
                Id = (int)rd["Id"],
                TieuDe = rd["TieuDe"].ToString(),
                HinhAnh = rd["HinhAnh"] as string,
                TomTat = rd["TomTat"] as string,
                NoiDung = rd["NoiDung"] as string,
                NgayDang = Convert.ToDateTime(rd["NgayDang"]),
                MaNguoiViet = Convert.ToInt32(rd["MaNguoiViet"]),
                HienThi = Convert.ToBoolean(rd["HienThi"])
            };
        }
    }
}
