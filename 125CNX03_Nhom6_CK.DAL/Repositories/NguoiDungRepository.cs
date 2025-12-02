using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _125CNX03_Nhom6_CK.DTO;
using _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        public List<NguoiDung> GetAll()
        {
            var list = new List<NguoiDung>();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM NguoiDung", conn);
                var rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    list.Add(Map(rd));
                }
            }
            return list;
        }

        public NguoiDung GetById(int id)
        {
            NguoiDung nd = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM NguoiDung WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    nd = Map(rd);
                }
            }
            return nd;
        }

        public NguoiDung GetByEmail(string email)
        {
            NguoiDung nd = null;
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM NguoiDung WHERE Email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                var rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    nd = Map(rd);
                }
            }
            return nd;
        }

        public bool Add(NguoiDung entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    INSERT INTO NguoiDung(HoTen, Email, MatKhauHash, SoDienThoai, DiaChi, VaiTro, NgayTao, TrangThai)
                    VALUES (@HoTen, @Email, @MatKhauHash, @SDT, @DiaChi, @VaiTro, @NgayTao, @TrangThai)", conn);

                cmd.Parameters.AddWithValue("@HoTen", entity.HoTen);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@MatKhauHash", entity.MatKhauHash);
                cmd.Parameters.AddWithValue("@SDT", (object)entity.SoDienThoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", (object)entity.DiaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VaiTro", entity.VaiTro);
                cmd.Parameters.AddWithValue("@NgayTao", entity.NgayTao);
                cmd.Parameters.AddWithValue("@TrangThai", entity.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(NguoiDung entity)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand(@"
                    UPDATE NguoiDung SET 
                        HoTen=@HoTen,
                        Email=@Email,
                        MatKhauHash=@MatKhauHash,
                        SoDienThoai=@SDT,
                        DiaChi=@DiaChi,
                        VaiTro=@VaiTro,
                        TrangThai=@TrangThai
                    WHERE Id=@Id", conn);

                cmd.Parameters.AddWithValue("@Id", entity.Id);
                cmd.Parameters.AddWithValue("@HoTen", entity.HoTen);
                cmd.Parameters.AddWithValue("@Email", entity.Email);
                cmd.Parameters.AddWithValue("@MatKhauHash", entity.MatKhauHash);
                cmd.Parameters.AddWithValue("@SDT", (object)entity.SoDienThoai ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DiaChi", (object)entity.DiaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@VaiTro", entity.VaiTro);
                cmd.Parameters.AddWithValue("@TrangThai", entity.TrangThai);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM NguoiDung WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        private NguoiDung Map(SqlDataReader rd)
        {
            return new NguoiDung
            {
                Id = (int)rd["Id"],
                HoTen = rd["HoTen"].ToString(),
                Email = rd["Email"].ToString(),
                MatKhauHash = rd["MatKhauHash"].ToString(),
                SoDienThoai = rd["SoDienThoai"] as string,
                DiaChi = rd["DiaChi"] as string,
                VaiTro = rd["VaiTro"].ToString(),
                NgayTao = Convert.ToDateTime(rd["NgayTao"]),
                TrangThai = Convert.ToBoolean(rd["TrangThai"])
            };
        }
    }
}
