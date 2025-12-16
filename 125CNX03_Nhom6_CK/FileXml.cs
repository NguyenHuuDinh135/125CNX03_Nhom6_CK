using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text; // Thêm thư viện này nếu thiếu

namespace _125CNX03_Nhom6_CK.Class
{
    public class FileXml
    {
        private string DbName = "QuanLyCuaHangBanLapTop";

        // Lưu ý: "Data Source=." có thể cần đổi thành "Data Source=TenMay\\SQLEXPRESS" tùy máy
        private string MasterConn = @"Data Source=.; Initial Catalog=master; Integrated Security=true";
        private string Conn = @"Data Source=.; Initial Catalog=QuanLyCuaHangBanLapTop; Integrated Security=true";

        private string _dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        // Thứ tự bảng quan trọng để tránh lỗi khóa ngoại khi Insert/Delete
        private readonly string[] DanhSachBang =
        {
            "ThuongHieu", "LoaiSanPham", "NguoiDung", "Banner", "LienHe",
            "PhuongThucThanhToan", "BaiViet",
            "SanPham", // Phụ thuộc ThuongHieu, LoaiSanPham
            "GioHang", // Phụ thuộc NguoiDung
            "DonHang", // Phụ thuộc NguoiDung, PhuongThucThanhToan
            "ChiTietGioHang", // Phụ thuộc GioHang, SanPham
            "ChiTietDonHang"  // Phụ thuộc DonHang, SanPham
        };

        public FileXml()
        {
            if (!Directory.Exists(_dataFolder)) Directory.CreateDirectory(_dataFolder);
        }

        public bool CoBatKyFileXmlNao()
        {
            // Kiểm tra xem có file XML nào trong thư mục Data không
            var files = Directory.GetFiles(_dataFolder, "*.xml");
            return files.Length > 0;
        }

        public bool CoFileSql() => File.Exists(Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql"));

        public bool DatabaseTonTai()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MasterConn))
                {
                    con.Open();
                    object result = new SqlCommand($"SELECT DB_ID('{DbName}')", con).ExecuteScalar();
                    return result != DBNull.Value && result != null;
                }
            }
            catch { return false; }
        }

        // ... [GIỮ NGUYÊN CÁC HÀM XỬ LÝ SQL CỦA BẠN: TaoDatabaseTuFileSql, SaoLuuRaFileSqlTuXml] ...
        // Mình rút gọn hiển thị ở đây vì code cũ của bạn đoạn này ok rồi.

        public void TaoDatabaseTuFileSql()
        {
            string sqlFile = Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql");
            if (!File.Exists(sqlFile)) return;
            string script = File.ReadAllText(sqlFile);
            string[] commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection con = new SqlConnection(MasterConn))
            {
                con.Open();
                foreach (string cmd in commands)
                {
                    if (string.IsNullOrWhiteSpace(cmd)) continue;
                    try { new SqlCommand(cmd, con).ExecuteNonQuery(); } catch { }
                }
            }
        }

        public void SaoLuuRaFileSqlTuXml()
        {
            string path = Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql");
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("USE [master]; GO");
                sw.WriteLine($"IF DB_ID('{DbName}') IS NULL CREATE DATABASE [{DbName}]; GO");
                sw.WriteLine($"USE [{DbName}]; GO\n");

                foreach (string table in DanhSachBang)
                {
                    try
                    {
                        DataTable dt = HienThi(table + ".xml");
                        if (dt == null) continue;

                        sw.WriteLine($"IF OBJECT_ID('{table}', 'U') IS NOT NULL DROP TABLE [{table}]; GO");

                        List<string> cols = new List<string>();
                        foreach (DataColumn c in dt.Columns)
                        {
                            string type = GetSqlTypeFromNetType(c.DataType);
                            if (c.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                                cols.Add($"[{c.ColumnName}] INT NOT NULL PRIMARY KEY");
                            else
                                cols.Add($"[{c.ColumnName}] {type}");
                        }
                        sw.WriteLine($"CREATE TABLE [{table}] ({string.Join(",", cols)}); GO");

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                if (dt.Columns.Contains("Id") && (row["Id"] == DBNull.Value || string.IsNullOrEmpty(row["Id"].ToString()))) continue;

                                List<string> vals = new List<string>();
                                foreach (DataColumn col in dt.Columns)
                                {
                                    object val = row[col];
                                    if (val == DBNull.Value || string.IsNullOrEmpty(val.ToString()))
                                        vals.Add("NULL");
                                    else if (col.DataType == typeof(bool))
                                        vals.Add((val.ToString().ToLower() == "true") ? "1" : "0");
                                    else if (col.DataType == typeof(DateTime))
                                        vals.Add($"'{Convert.ToDateTime(val):yyyy-MM-dd HH:mm:ss}'");
                                    else if (col.DataType == typeof(decimal) || col.DataType == typeof(int))
                                        vals.Add(val.ToString().Replace(",", "."));
                                    else
                                        vals.Add($"N'{val.ToString().Replace("'", "''")}'");
                                }
                                sw.WriteLine($"INSERT INTO [{table}] VALUES ({string.Join(",", vals)});");
                            }
                            sw.WriteLine("GO");
                        }
                    }
                    catch { }
                }
            }
        }

        // ===============================================

        public DataTable HienThi(string tenFileXML)
        {
            string path = Path.Combine(_dataFolder, tenFileXML);
            if (!File.Exists(path)) return null;
            try
            {
                DataSet ds = new DataSet(); ds.ReadXml(path, XmlReadMode.ReadSchema);
                return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            }
            catch
            {
                try { DataSet ds = new DataSet(); ds.ReadXml(path, XmlReadMode.InferSchema); return ds.Tables.Count > 0 ? ds.Tables[0] : null; } catch { return null; }
            }
        }

        public void TaoDatabaseRong()
        {
            using (SqlConnection con = new SqlConnection(MasterConn))
            {
                con.Open();
                // Chỉ tạo nếu chưa tồn tại
                new SqlCommand($"IF DB_ID('{DbName}') IS NULL CREATE DATABASE [{DbName}]", con).ExecuteNonQuery();
            }
        }

        public void KhoiPhucToanBoXmlTuDB()
        {
            // Hàm này chỉ được gọi khi mất XML
            using (SqlConnection con = new SqlConnection(Conn))
            {
                con.Open();
                foreach (string bang in DanhSachBang)
                {
                    // Nếu bảng không tồn tại trong DB thì bỏ qua
                    if (!KiemTraBangTonTai(con, bang)) continue;

                    try
                    {
                        SqlDataAdapter ad = new SqlDataAdapter($"SELECT * FROM [{bang}]", con);
                        DataTable dt = new DataTable(bang);
                        ad.Fill(dt);
                        // Ghi đè lại XML từ dữ liệu DB
                        dt.WriteXml(Path.Combine(_dataFolder, bang + ".xml"), XmlWriteMode.WriteSchema);
                    }
                    catch { }
                }
            }
        }

        public void SaoLuuToanBoSangDB()
        {
            // Hàm này được gọi khi tắt App để Backup
            using (SqlConnection con = new SqlConnection(Conn))
            {
                con.Open();
                // Xóa bảng cũ (ngược từ con lên cha để tránh lỗi khóa ngoại)
                foreach (string bang in DanhSachBang.Reverse())
                    if (KiemTraBangTonTai(con, bang)) new SqlCommand($"DROP TABLE [{bang}]", con).ExecuteNonQuery();
            }

            // Tạo lại và Insert (xuôi từ cha xuống con)
            foreach (string bang in DanhSachBang) SaoLuuBangSangDB(bang, bang + ".xml");
        }

        private void SaoLuuBangSangDB(string bang, string file)
        {
            try
            {
                DataTable dt = HienThi(file);
                if (dt == null) return;

                // Lọc bỏ dòng lỗi ID rỗng
                if (dt.Columns.Contains("Id"))
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        if (dt.Rows[i]["Id"] == DBNull.Value || string.IsNullOrEmpty(dt.Rows[i]["Id"].ToString())) dt.Rows[i].Delete();
                    dt.AcceptChanges();
                }

                using (SqlConnection con = new SqlConnection(Conn))
                {
                    con.Open();
                    // Tạo bảng
                    List<string> cols = new List<string>();
                    foreach (DataColumn c in dt.Columns)
                    {
                        string type = GetSqlTypeFromNetType(c.DataType);
                        if (c.ColumnName.Equals("Id", StringComparison.OrdinalIgnoreCase)) cols.Add($"[{c.ColumnName}] INT NOT NULL PRIMARY KEY");
                        else cols.Add($"[{c.ColumnName}] {type}");
                    }
                    new SqlCommand($"CREATE TABLE [{bang}] ({string.Join(",", cols)})", con).ExecuteNonQuery();

                    // Insert Bulk Copy (Nhanh hơn insert từng dòng)
                    if (dt.Rows.Count > 0)
                    {
                        using (SqlBulkCopy bulk = new SqlBulkCopy(con))
                        {
                            bulk.DestinationTableName = bang;
                            foreach (DataColumn c in dt.Columns) bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                            bulk.WriteToServer(dt);
                        }
                    }
                }
            }
            catch { }
        }

        private bool KiemTraBangTonTai(SqlConnection con, string table)
        {
            return (int)new SqlCommand($"SELECT COUNT(*) FROM sysobjects WHERE name='{table}' AND xtype='U'", con).ExecuteScalar() > 0;
        }

        private string GetSqlTypeFromNetType(Type t)
        {
            if (t == typeof(int)) return "INT";
            if (t == typeof(decimal)) return "DECIMAL(18,2)";
            if (t == typeof(DateTime)) return "DATETIME";
            if (t == typeof(bool)) return "BIT";
            return "NVARCHAR(MAX)";
        }
    }
}