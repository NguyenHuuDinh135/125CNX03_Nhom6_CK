using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL
{
    public class DbInitializer
    {
        private const string SchemaFilePath = "Data/db_schema.xml";

        public static void Initialize()
        {
            if (!File.Exists(SchemaFilePath))
                throw new Exception($"Không tìm thấy file: {Path.GetFullPath(SchemaFilePath)}");

            XDocument doc = XDocument.Load(SchemaFilePath);
            string dbName = doc.Element("Schema").Element("DatabaseName").Value;

            // 1. Tạo Database
            CreateDatabaseIfNotExists(dbName);

            // 2. Tạo Bảng (Logic MỚI: Dịch từ thẻ XML -> SQL)
            CreateTablesDynamic(doc);
        }

        private static void CreateDatabaseIfNotExists(string dbName)
        {
            using (var conn = new SqlConnection(DbConnection.GetMasterConnectionString()))
            {
                conn.Open();
                string checkSql = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{dbName}'";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    if ((int)cmd.ExecuteScalar() == 0)
                    {
                        new SqlCommand($"CREATE DATABASE {dbName}", conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // --- CORE: BIẾN XML THUẦN SANG SQL CREATE TABLE ---
        private static void CreateTablesDynamic(XDocument doc)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                // Lấy tất cả thẻ con trực tiếp trong <Tables> (ThuongHieu, SanPham...)
                var tableNodes = doc.Element("Schema").Element("Tables").Elements();

                // Giai đoạn 1: Tạo bảng sơ khai (chưa có FK)
                foreach (var tableNode in tableNodes)
                {
                    string tableName = tableNode.Name.LocalName; // <ThuongHieu> -> "ThuongHieu"

                    // Kiểm tra bảng tồn tại chưa
                    if (CheckTableExists(conn, tableName)) continue;

                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine($"CREATE TABLE [{tableName}] (");

                    List<string> columnsSql = new List<string>();

                    // Duyệt các thẻ con bên trong (Chính là các cột: <Id>, <Ten>...)
                    foreach (var colNode in tableNode.Elements())
                    {
                        string colName = colNode.Name.LocalName; // <Id> -> "Id"

                        // Lấy thuộc tính Type, nếu không có thì mặc định NVARCHAR(MAX)
                        string colType = colNode.Attribute("Type")?.Value ?? "NVARCHAR(MAX)";

                        string line = $"[{colName}] {colType}";

                        // Đọc các thuộc tính cấu hình cột từ XML
                        if (colNode.Attribute("PK")?.Value == "true") line += " PRIMARY KEY";
                        if (colNode.Attribute("Identity")?.Value == "true") line += " IDENTITY(1,1)";
                        if (colNode.Attribute("NotNull")?.Value == "true") line += " NOT NULL";
                        if (colNode.Attribute("Unique")?.Value == "true") line += " UNIQUE";

                        var defVal = colNode.Attribute("Default")?.Value;
                        if (defVal != null) line += $" DEFAULT {defVal}";

                        columnsSql.Add(line);
                    }

                    sql.AppendLine(string.Join(",\n", columnsSql));
                    sql.AppendLine(");");

                    // Chạy lệnh tạo bảng
                    try
                    {
                        new SqlCommand(sql.ToString(), conn).ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi tạo bảng {tableName}: {ex.Message}");
                    }
                }

                // Giai đoạn 2: Bổ sung Khóa Ngoại (Foreign Keys)
                foreach (var tableNode in tableNodes)
                {
                    string tableName = tableNode.Name.LocalName;

                    foreach (var colNode in tableNode.Elements())
                    {
                        // Tìm thuộc tính FK="Bang.Cot" (Ví dụ: FK="LoaiSanPham.Id")
                        var fkAttr = colNode.Attribute("FK");
                        if (fkAttr != null)
                        {
                            string[] parts = fkAttr.Value.Split('.');
                            if (parts.Length == 2)
                            {
                                string refTable = parts[0];
                                string refCol = parts[1];
                                string colName = colNode.Name.LocalName;
                                string fkName = $"FK_{tableName}_{colName}";

                                string sqlFK = $@"
                                    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = '{fkName}')
                                    ALTER TABLE [{tableName}] 
                                    ADD CONSTRAINT [{fkName}] 
                                    FOREIGN KEY ([{colName}]) REFERENCES [{refTable}]([{refCol}])";

                                try { new SqlCommand(sqlFK, conn).ExecuteNonQuery(); } catch { }
                            }
                        }
                    }
                }
            }
        }

        private static bool CheckTableExists(SqlConnection conn, string tableName)
        {
            string sql = $"SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[{tableName}]') AND type in (N'U')";
            return (int)new SqlCommand(sql, conn).ExecuteScalar() > 0;
        }
    }
}