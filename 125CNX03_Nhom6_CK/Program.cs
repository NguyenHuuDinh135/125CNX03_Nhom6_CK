using _125CNX03_Nhom6_CK.GUI.Forms;
using _125CNX03_Nhom6_CK.GUI.Forms.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK
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

            while (true)
            {
                using (var loginForm = new LoginForm())
                {
                    var result = loginForm.ShowDialog(); // ShowDialog để chặn luồng

                    if (result == DialogResult.OK && loginForm.LoggedInUser != null)
                    {
                        var user = loginForm.LoggedInUser;
                        if (user.Element("VaiTro").Value == "Admin")
                        {
                            Application.Run(new MainForm(user));
                        }
                        else
                        {
                            Application.Run(new _125CNX03_Nhom6_CK.GUI.Forms.User.MainForm(user));
                        }
                    }
                    else
                    {
                        // Nếu đóng login hoặc login không thành công => thoát
                        break;
                    }
                }
            }
        }
    }
}
