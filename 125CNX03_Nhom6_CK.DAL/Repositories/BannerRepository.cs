using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        public List<Banner> GetAll()
        {
            var list = new List<Banner>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Banner", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public Banner GetById(int id)
        {
            Banner banner = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM Banner WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    banner = Map(rd);
                }
            }
            return banner;
        }

        public bool Add(Banner entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO Banner(TenBanner, HinhAnh, LienKet, ThuTu, HienThi)
                    VALUES (@TenBanner, @HinhAnh, @LienKet, @ThuTu, @HienThi)", conn);

                cmd.Parameters.AddWithValue("@TenBanner", (object)entity.TenBanner ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HinhAnh", entity.HinhAnh);
                cmd.Parameters.AddWithValue("@LienKet", (object)entity.LienKet ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ThuTu", entity.ThuTu);
                cmd.Parameters.AddWithValue("@HienThi", entity.HienThi);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(Banner entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE Banner SET
                        TenBanner=@TenBanner,
                        HinhAnh=@HinhAnh,
                        LienKet=@LienKet,
                        ThuTu=@ThuTu,
                        HienThi=@HienThi
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@TenBanner", (object)entity.TenBanner ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@HinhAnh", entity.HinhAnh);
                cmd.Parameters.AddWithValue("@LienKet", (object)entity.LienKet ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ThuTu", entity.ThuTu);
                cmd.Parameters.AddWithValue("@HienThi", entity.HienThi);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Banner WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private Banner Map(SqlDataReader rd)
        {
            return new Banner
            {
                Id = (int)rd["Id"],
                TenBanner = rd["TenBanner"] as string,
                HinhAnh = rd["HinhAnh"].ToString(),
                LienKet = rd["LienKet"] as string,
                ThuTu = rd["ThuTu"] == DBNull.Value ? 0 : Convert.ToInt32(rd["ThuTu"]),
                HienThi = Convert.ToBoolean(rd["HienThi"])
            };
        }
    }
}
