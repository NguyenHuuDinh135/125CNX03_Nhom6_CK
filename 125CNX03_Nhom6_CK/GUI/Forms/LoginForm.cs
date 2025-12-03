using System;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class LoginForm : Form
    {
        private readonly INguoiDungService _userService;
        public event EventHandler<UserEventArgs> UserLoggedIn;

        public LoginForm()
        {
            InitializeComponent();
            _userService = new NguoiDungService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập email và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _userService.AuthenticateUser(email, password);
            if (user != null)
            {
                UserLoggedIn?.Invoke(this, new UserEventArgs(user));
                MessageBox.Show($"Đăng nhập thành công! Xin chào {user.Element("HoTen")?.Value}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}