using System;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.GUI.Forms;
using _125CNX03_Nhom6_CK.Class;

namespace _125CNX03_Nhom6_CK
{
    internal static class Program
    {
        // Khai báo static để dùng chung
        private static FileXml dbHelper = new FileXml();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // ===== 1. KIỂM TRA DỮ LIỆU KHI KHỞI ĐỘNG =====

                // Trường hợp A: Máy mới tinh (Không XML, Không Database)
                if (!dbHelper.CoBatKyFileXmlNao() && !dbHelper.DatabaseTonTai())
                {
                    if (dbHelper.CoFileSql())
                    {
                        // Tự động chạy file SQL để tạo Database và dữ liệu
                        dbHelper.TaoDatabaseTuFileSql();

                        // Sau đó lấy dữ liệu từ DB vừa tạo đổ ra XML để App dùng
                        dbHelper.KhoiPhucToanBoXmlTuDB();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không tìm thấy dữ liệu khởi tạo!\nVui lòng đảm bảo file 'QuanLyCuaHangBanLapTopBackup.sql' có trong thư mục Data.",
                                        "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Dừng app nếu không có gì để chạy
                    }
                }
                // Trường hợp B: Đã có Database (ưu tiên lấy dữ liệu mới nhất từ DB)
                else if (dbHelper.DatabaseTonTai())
                {
                    dbHelper.KhoiPhucToanBoXmlTuDB();
                }
                // Trường hợp C: Có XML nhưng mất DB (Tự tạo lại DB từ XML để đồng bộ)
                else if (dbHelper.CoBatKyFileXmlNao())
                {
                    dbHelper.TaoDatabaseRong();
                    dbHelper.SaoLuuToanBoSangDB();
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
                        // 🔥 KHI THOÁT APP: SAO LƯU DỮ LIỆU
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
                if (dbHelper.CoBatKyFileXmlNao())
                {
                    // 1. Đảm bảo Database tồn tại
                    if (!dbHelper.DatabaseTonTai()) dbHelper.TaoDatabaseRong();

                    // 2. Lưu từ XML -> Database
                    dbHelper.SaoLuuToanBoSangDB();

                    // 3. Tạo file SQL dự phòng (backup.sql)
                    dbHelper.SaoLuuRaFileSqlTuXml();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sao lưu: " + ex.Message);
            }
        }
    }
}