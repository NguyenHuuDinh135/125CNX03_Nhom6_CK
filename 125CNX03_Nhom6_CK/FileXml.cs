using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace _125CNX03_Nhom6_CK.Class
{
    public class FileXml
    {
        private string DbName = "QuanLyCuaHangBanLapTop";

        // Kết nối Server (để tạo DB)
        private string MasterConn = @"Data Source=.; Initial Catalog=master; Integrated Security=true";
        // Kết nối Database chính
        private string Conn = @"Data Source=.; Initial Catalog=QuanLyCuaHangBanLapTop; Integrated Security=true";

        private string _dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        // Danh sách 12 bảng (Thứ tự: Cha -> Con)
        private readonly string[] DanhSachBang =
        {
            "ThuongHieu", "LoaiSanPham", "NguoiDung", "Banner", "LienHe",
            "PhuongThucThanhToan", "BaiViet",
            "SanPham",
            "GioHang", "DonHang",
            "ChiTietGioHang", "ChiTietDonHang"
        };

        public FileXml()
        {
            if (!Directory.Exists(_dataFolder)) Directory.CreateDirectory(_dataFolder);
        }

        public bool CoBatKyFileXmlNao() => Directory.GetFiles(_dataFolder, "*.xml").Length > 0;
        public bool CoFileSql() => File.Exists(Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql"));

        public bool DatabaseTonTai()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MasterConn))
                {
                    con.Open();
                    return (int)new SqlCommand($"SELECT COUNT(*) FROM sys.databases WHERE name='{DbName}'", con).ExecuteScalar() > 0;
                }
            }
            catch { return false; }
        }

        // ================= XỬ LÝ FILE SQL (BACKUP.SQL) =================

        // 1. Chạy file SQL để tạo DB (Khi không có gì)
        public void TaoDatabaseTuFileSql()
        {
            string sqlFile = Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql");
            if (!File.Exists(sqlFile)) return;

            string script = File.ReadAllText(sqlFile);

            // Tách lệnh bằng 'GO' để chạy từng khối
            string[] commands = script.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection con = new SqlConnection(MasterConn))
            {
                con.Open();
                foreach (string cmd in commands)
                {
                    if (string.IsNullOrWhiteSpace(cmd)) continue;
                    try
                    {
                        new SqlCommand(cmd, con).ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi chạy SQL: {ex.Message}");
                    }
                }
            }
        }

        // 2. Tạo file SQL từ XML (Khi thoát App)
        public void SaoLuuRaFileSqlTuXml()
        {
            string path = Path.Combine(_dataFolder, "QuanLyCuaHangBanLapTopBackup.sql");
            using (StreamWriter sw = new StreamWriter(path))
            {
                // Header
                sw.WriteLine("USE [master]; GO");
                sw.WriteLine($"IF DB_ID('{DbName}') IS NULL CREATE DATABASE [{DbName}]; GO");
                sw.WriteLine($"USE [{DbName}]; GO\n");

                foreach (string table in DanhSachBang)
                {
                    try
                    {
                        DataTable dt = HienThi(table + ".xml");
                        if (dt == null) continue;

                        // Xóa bảng cũ
                        sw.WriteLine($"IF OBJECT_ID('{table}', 'U') IS NOT NULL DROP TABLE [{table}]; GO");

                        // Tạo bảng mới (Id là PK)
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

                        // Insert dữ liệu
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                // Bỏ qua dòng lỗi
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

        // ================= CÁC HÀM HỖ TRỢ KHÁC =================

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
                new SqlCommand($"IF DB_ID('{DbName}') IS NULL CREATE DATABASE [{DbName}]", con).ExecuteNonQuery();
            }
        }

        public void KhoiPhucToanBoXmlTuDB()
        {
            using (SqlConnection con = new SqlConnection(Conn))
            {
                con.Open();
                foreach (string bang in DanhSachBang)
                {
                    if (!KiemTraBangTonTai(con, bang)) continue;
                    try
                    {
                        SqlDataAdapter ad = new SqlDataAdapter($"SELECT * FROM [{bang}]", con);
                        DataTable dt = new DataTable(bang);
                        ad.Fill(dt);
                        dt.WriteXml(Path.Combine(_dataFolder, bang + ".xml"), XmlWriteMode.WriteSchema);
                    }
                    catch { }
                }
            }
        }

        public void SaoLuuToanBoSangDB()
        {
            using (SqlConnection con = new SqlConnection(Conn))
            {
                con.Open();
                // Xóa bảng cũ (ngược)
                foreach (string bang in DanhSachBang.Reverse())
                    if (KiemTraBangTonTai(con, bang)) new SqlCommand($"DROP TABLE [{bang}]", con).ExecuteNonQuery();
            }
            // Tạo lại và Insert (xuôi)
            foreach (string bang in DanhSachBang) SaoLuuBangSangDB(bang, bang + ".xml");
        }

        private void SaoLuuBangSangDB(string bang, string file)
        {
            try
            {
                DataTable dt = HienThi(file);
                if (dt == null) return;

                // Lọc bỏ dòng lỗi ID
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

                    // Insert Bulk
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