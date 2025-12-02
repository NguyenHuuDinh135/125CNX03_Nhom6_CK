using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class PhuongThucThanhToanRepository : IPhuongThucThanhToanRepository
    {
        public List<PhuongThucThanhToan> GetAll()
        {
            var list = new List<PhuongThucThanhToan>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM PhuongThucThanhToan", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public PhuongThucThanhToan GetById(int id)
        {
            PhuongThucThanhToan pt = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM PhuongThucThanhToan WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    pt = Map(rd);
                }
            }
            return pt;
        }

        public List<PhuongThucThanhToan> GetByNguoiDung(int maNguoiDung)
        {
            var list = new List<PhuongThucThanhToan>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM PhuongThucThanhToan WHERE MaNguoiDung=@MaNguoiDung", conn);
                cmd.Parameters.AddWithValue("@MaNguoiDung", maNguoiDung);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public bool Add(PhuongThucThanhToan entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO PhuongThucThanhToan(MaNguoiDung, TenGoiNho, MaThe, BonSoCuoi)
                    VALUES (@MaNguoiDung, @TenGoiNho, @MaThe, @BonSoCuoi)", conn);

                cmd.Parameters.AddWithValue("@MaNguoiDung", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@TenGoiNho", (object)entity.TenGoiNho ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaThe", (object)entity.MaThe ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BonSoCuoi", (object)entity.BonSoCuoi ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(PhuongThucThanhToan entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE PhuongThucThanhToan SET 
                        MaNguoiDung=@MaNguoiDung,
                        TenGoiNho=@TenGoiNho,
                        MaThe=@MaThe,
                        BonSoCuoi=@BonSoCuoi
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@MaNguoiDung", entity.MaNguoiDung);
                cmd.Parameters.AddWithValue("@TenGoiNho", (object)entity.TenGoiNho ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@MaThe", (object)entity.MaThe ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BonSoCuoi", (object)entity.BonSoCuoi ?? DBNull.Value);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM PhuongThucThanhToan WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private PhuongThucThanhToan Map(SqlDataReader rd)
        {
            return new PhuongThucThanhToan
            {
                Id = (int)rd["Id"],
                MaNguoiDung = Convert.ToInt32(rd["MaNguoiDung"]),
                TenGoiNho = rd["TenGoiNho"] as string,
                MaThe = rd["MaThe"] as string,
                BonSoCuoi = rd["BonSoCuoi"] as string
            };
        }
    }
}
