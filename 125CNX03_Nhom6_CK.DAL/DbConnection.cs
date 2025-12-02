using System.Data.SqlClient;

namespace _125CNX03_Nhom6_CK.DAL
{
    public class DbConnection
    {
        // CẤU HÌNH SERVER
        // Dấu chấm (.) nghĩa là máy local. Nếu dùng SQLEXPRESS thì sửa thành .\\SQLEXPRESS
        private static string ServerName = ".";

        // Tên Database sẽ được tạo tự động
        private static string DbName = "ECommerceXML_DB";

        /// <summary>
        /// Chuỗi kết nối vào 'master' dùng để kiểm tra và tạo Database mới
        /// </summary>
        public static string GetMasterConnectionString()
        {
            return $"Data Source={ServerName};Initial Catalog=master;Integrated Security=True";
        }

        /// <summary>
        /// Chuỗi kết nối vào Database chính của ứng dụng
        /// </summary>
        public static string GetConnectionString()
        {
            return $"Data Source={ServerName};Initial Catalog={DbName};Integrated Security=True";
        }

        /// <summary>
        /// Trả về đối tượng SqlConnection đang mở (nhớ dùng using khi gọi)
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}