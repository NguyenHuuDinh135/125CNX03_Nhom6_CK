using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.DAL; // Cần dòng này để gọi DAL

namespace _125CNX03_Nhom6_CK.GUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // --- BẮT ĐẦU ĐOẠN CODE TỰ ĐỘNG TẠO DB ---
            try
            {
                // Gọi hàm khởi tạo từ lớp DAL
                // Hàm này sẽ đọc db_schema.xml và db_config.xml để tạo CSDL
                DbInitializer.Initialize();
            }
            catch (Exception ex)
            {
                // Nếu lỗi (ví dụ chưa cài SQL Server), báo lỗi và dừng chương trình
                MessageBox.Show("Lỗi khởi tạo Cơ sở dữ liệu: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // --- KẾT THÚC ĐOẠN CODE TỰ ĐỘNG ---
            Application.Run(new Form1());
        }
    }
}
