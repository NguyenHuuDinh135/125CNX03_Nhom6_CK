using System;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq; // Thư viện LINQ to XML quan trọng

namespace _125CNX03_Nhom6_CK.DAL
{
    public class DbInitializer
    {
        // Đường dẫn tương đối đến file XML cấu trúc (đã copy ra bin/Debug)
        private const string SchemaFilePath = "Data/db_schema.xml";

        public static void Initialize()
        {
            // 1. Kiểm tra file XML tồn tại không
            if (!File.Exists(SchemaFilePath))
            {
                throw new Exception($"LỖI: Không tìm thấy file cấu trúc tại '{Path.GetFullPath(SchemaFilePath)}'. Hãy chắc chắn bạn đã chọn 'Copy to Output Directory = Copy always' cho file xml.");
            }

            // 2. Đọc nội dung XML
            XDocument doc = XDocument.Load(SchemaFilePath);

            // Lấy tên Database từ XML (để đồng bộ)
            string dbName = doc.Element("Schema").Element("DatabaseName").Value;

            // 3. Kết nối Master để tạo DB nếu chưa có
            CreateDatabaseIfNotExists(dbName);

            // 4. Kết nối vào DB vừa tạo để chạy lệnh tạo bảng
            CreateTables(doc);
        }

        private static void CreateDatabaseIfNotExists(string dbName)
        {
            using (var conn = new SqlConnection(DbConnection.GetMasterConnectionString()))
            {
                conn.Open();
                // Kiểm tra DB tồn tại chưa
                string checkSql = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{dbName}'";
                using (var cmd = new SqlCommand(checkSql, conn))
                {
                    int exists = (int)cmd.ExecuteScalar();
                    if (exists == 0)
                    {
                        // Tạo mới nếu chưa có
                        string createSql = $"CREATE DATABASE {dbName}";
                        using (var createCmd = new SqlCommand(createSql, conn))
                        {
                            createCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void CreateTables(XDocument doc)
        {
            // Lấy danh sách thẻ <Table> trong XML
            var tables = doc.Descendants("Table");

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                foreach (var table in tables)
                {
                    string tableName = table.Attribute("Name")?.Value;
                    string script = table.Element("Script")?.Value;

                    if (!string.IsNullOrEmpty(script))
                    {
                        try
                        {
                            using (var cmd = new SqlCommand(script, conn))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Lỗi khi tạo bảng '{tableName}': {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}