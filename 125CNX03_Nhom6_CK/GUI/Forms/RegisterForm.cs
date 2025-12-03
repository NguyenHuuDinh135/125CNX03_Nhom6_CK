using System;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly INguoiDungService _userService;

        public RegisterForm()
        {
            InitializeComponent();
            _userService = new NguoiDungService();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_userService.IsEmailUnique(email))
            {
                MessageBox.Show("Email đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create user element
            var newUser = new System.Xml.Linq.XElement("NguoiDung",
                new System.Xml.Linq.XElement("HoTen", fullName),
                new System.Xml.Linq.XElement("Email", email),
                new System.Xml.Linq.XElement("MatKhauHash", password), // Password will be hashed in service
                new System.Xml.Linq.XElement("SoDienThoai", phone),
                new System.Xml.Linq.XElement("DiaChi", address),
                new System.Xml.Linq.XElement("VaiTro", "Customer"),
                new System.Xml.Linq.XElement("NgayTao", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                new System.Xml.Linq.XElement("TrangThai", "true")
            );

            try
            {
                _userService.AddUser(newUser);
                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}