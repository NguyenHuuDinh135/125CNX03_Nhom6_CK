using System;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.GUI.Forms;
using _125CNX03_Nhom6_CK.Class;
using System.Xml.Linq; // Thêm để dùng XElement

namespace _125CNX03_Nhom6_CK
{
    internal static class Program
    {
        private static FileXml dbHelper = new FileXml();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // ===== 1. LOGIC KHỞI ĐỘNG (Đã điều chỉnh) =====

                // KIỂM TRA: Có file XML nào không?
                if (dbHelper.CoBatKyFileXmlNao())
                {
                    // TRƯỜNG HỢP 1: Đã có XML -> Chạy luôn, KHÔNG ĐỤNG ĐẾN DB.
                    // (DB lúc này có thể cũ hoặc chưa có, kệ nó, ta sẽ cập nhật lúc tắt app)
                }
                else
                {
                    // TRƯỜNG HỢP 2: Mất sạch XML -> Cần khôi phục từ Backup (DB hoặc SQL)

                    if (dbHelper.DatabaseTonTai())
                    {
                        // Ưu tiên 2a: Nếu có Database -> Lấy dữ liệu từ DB đổ ra XML
                        dbHelper.KhoiPhucToanBoXmlTuDB();
                    }
                    else if (dbHelper.CoFileSql())
                    {
                        // Ưu tiên 2b: Nếu không có DB, nhưng có file SQL Backup -> Chạy SQL tạo DB -> Đổ ra XML
                        dbHelper.TaoDatabaseTuFileSql();
                        dbHelper.KhoiPhucToanBoXmlTuDB();
                    }
                    else
                    {
                        // Trường hợp xấu nhất: Mất cả XML, mất cả DB, mất cả file SQL
                        MessageBox.Show("Lỗi nghiêm trọng: Không tìm thấy dữ liệu gốc (XML) và cũng không có bản sao lưu (DB/SQL)!\n" +
                                        "Vui lòng kiểm tra lại thư mục Data.",
                                        "Mất dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Có thể return hoặc để app chạy với dữ liệu trắng tùy bạn
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động: " + ex.Message);
            }

            // ===== 2. CHẠY ỨNG DỤNG =====
            while (true)
            {
                using (var loginForm = new LoginForm())
                {
                    var result = loginForm.ShowDialog();
                    if (result == DialogResult.OK && loginForm.LoggedInUser != null)
                    {
                        var user = loginForm.LoggedInUser;
                        string role = user.Element("VaiTro")?.Value;

                        if (role == "Admin")
                            Application.Run(new GUI.Forms.Admin.MainForm(user));
                        else
                            Application.Run(new GUI.Forms.User.MainForm(user));
                    }
                    else
                    {
                        // 🔥 KHI THOÁT APP: SAO LƯU DỮ LIỆU (QUAN TRỌNG)
                        // Lúc này mới cần đồng bộ từ XML hiện tại vào DB/SQL để làm backup cho lần sau
                        SaoLuuDuLieu();
                        break;
                    }
                }
            }
        }

        // Hàm sao lưu khi tắt ứng dụng
        private static void SaoLuuDuLieu()
        {
            try
            {
                // Chỉ sao lưu nếu đang có XML (dữ liệu đang sống)
                if (dbHelper.CoBatKyFileXmlNao())
                {
                    // 1. Đảm bảo Database tồn tại để chứa backup (Nếu chưa có thì tạo mới)
                    if (!dbHelper.DatabaseTonTai())
                    {
                        dbHelper.TaoDatabaseRong();
                    }

                    // 2. Đẩy toàn bộ dữ liệu từ XML hiện tại vào Database (Ghi đè DB cũ)
                    dbHelper.SaoLuuToanBoSangDB();

                    // 3. Xuất ra file .sql để mang sang máy khác được
                    dbHelper.SaoLuuRaFileSqlTuXml();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sao lưu backup: " + ex.Message);
            }
        }
    }
}