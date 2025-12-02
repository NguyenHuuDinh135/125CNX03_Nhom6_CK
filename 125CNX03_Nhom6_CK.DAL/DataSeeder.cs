using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL
{
    public static class DataSeeder
    {
        // Đường dẫn file chứa dữ liệu mẫu (dùng chung file db_schema.xml)
        private const string SchemaFilePath = "Data/db_schema.xml";

        public static void Seed()
        {
            if (!File.Exists(SchemaFilePath)) return;

            try
            {
                XDocument doc = XDocument.Load(SchemaFilePath);
                var dataSeeder = doc.Element("Schema")?.Element("DataSeeder");

                if (dataSeeder == null) return;

                using (var conn = DbConnection.GetConnection())
                {
                    conn.Open();

                    // Duyệt qua từng bảng (Ví dụ: <ThuongHieu>, <SanPham>...)
                    foreach (var tableElement in dataSeeder.Elements())
                    {
                        string tableName = tableElement.Name.LocalName;

                        // Duyệt qua từng dòng dữ liệu (<Row ... />)
                        foreach (var row in tableElement.Elements("Row"))
                        {
                            InsertDataSmart(conn, tableName, row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi Seed dữ liệu: {ex.Message}");
            }
        }

        private static void InsertDataSmart(SqlConnection conn, string tableName, XElement row)
        {
            var attributes = row.Attributes().ToList();
            if (attributes.Count == 0) return;

            // 1. Xác định cột dùng để kiểm tra trùng lặp (Ưu tiên cột 'Id' hoặc 'Email' hoặc cột đầu tiên)
            var idAttr = attributes.FirstOrDefault(a => a.Name.LocalName.Equals("Id", StringComparison.OrdinalIgnoreCase));
            var checkAttr = idAttr ?? attributes.First(); // Nếu có Id thì check theo Id, không thì check theo cột đầu tiên

            string checkCol = checkAttr.Name.LocalName;
            string checkVal = checkAttr.Value;

            // 2. Kiểm tra xem dữ liệu đã tồn tại chưa
            string checkSql = $"SELECT COUNT(1) FROM [{tableName}] WHERE [{checkCol}] = @checkVal";
            using (var checkCmd = new SqlCommand(checkSql, conn))
            {
                checkCmd.Parameters.AddWithValue("@checkVal", checkVal);
                // Nếu bảng chưa được tạo hoặc dữ liệu đã có thì bỏ qua
                try
                {
                    if ((int)checkCmd.ExecuteScalar() > 0) return;
                }
                catch
                {
                    return; // Bảng có thể chưa tồn tại
                }
            }

            // 3. Xây dựng câu lệnh INSERT
            var colNames = attributes.Select(a => $"[{a.Name.LocalName}]");
            var paramNames = attributes.Select(a => "@" + a.Name.LocalName);

            string insertSql = $"INSERT INTO [{tableName}] ({string.Join(", ", colNames)}) VALUES ({string.Join(", ", paramNames)})";

            // 4. Xử lý IDENTITY_INSERT (Nếu trong XML có cột Id, ta cần bật chế độ này để ép nhập ID)
            bool hasIdentity = attributes.Any(a => a.Name.LocalName.Equals("Id", StringComparison.OrdinalIgnoreCase));

            if (hasIdentity)
            {
                insertSql = $"SET IDENTITY_INSERT [{tableName}] ON; {insertSql}; SET IDENTITY_INSERT [{tableName}] OFF;";
            }

            // 5. Thực thi
            using (var cmd = new SqlCommand(insertSql, conn))
            {
                foreach (var attr in attributes)
                {
                    // Tự động Add Parameter (Ví dụ: @TenThuongHieu = "Apple")
                    cmd.Parameters.AddWithValue("@" + attr.Name.LocalName, attr.Value);
                }
                cmd.ExecuteNonQuery();
            }
        }
    }
}