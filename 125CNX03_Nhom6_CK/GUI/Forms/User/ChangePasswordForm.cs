using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ChangePasswordForm : Form
    {
        private readonly INguoiDungService _userService;
        private XElement _currentUser;

        public ChangePasswordForm(XElement user)
        {
            InitializeComponent();
            _userService = new NguoiDungService();
            _currentUser = user;

            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Đổi mật khẩu";
            this.Size = new Size(500, 300);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create form panel
            Panel formPanel = new Panel();
            formPanel.Size = new Size(this.Width - 40, this.Height - 60);
            formPanel.Location = new Point(20, 20);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;

            // Controls
            Label lblCurrentPassword = new Label();
            lblCurrentPassword.Text = "Mật khẩu hiện tại:";
            lblCurrentPassword.Font = new Font("Segoe UI", 9);
            lblCurrentPassword.Location = new Point(20, 20);
            lblCurrentPassword.Size = new Size(120, 20);
            formPanel.Controls.Add(lblCurrentPassword);

            TextBox txtCurrentPassword = new TextBox();
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.Font = new Font("Segoe UI", 10);
            txtCurrentPassword.Size = new Size(200, 20);
            txtCurrentPassword.Location = new Point(150, 20);
            txtCurrentPassword.BorderStyle = BorderStyle.FixedSingle;
            txtCurrentPassword.UseSystemPasswordChar = true;
            formPanel.Controls.Add(txtCurrentPassword);

            Label lblNewPassword = new Label();
            lblNewPassword.Text = "Mật khẩu mới:";
            lblNewPassword.Font = new Font("Segoe UI", 9);
            lblNewPassword.Location = new Point(20, 50);
            lblNewPassword.Size = new Size(120, 20);
            formPanel.Controls.Add(lblNewPassword);

            TextBox txtNewPassword = new TextBox();
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Font = new Font("Segoe UI", 10);
            txtNewPassword.Size = new Size(200, 20);
            txtNewPassword.Location = new Point(150, 50);
            txtNewPassword.BorderStyle = BorderStyle.FixedSingle;
            txtNewPassword.UseSystemPasswordChar = true;
            formPanel.Controls.Add(txtNewPassword);

            Label lblConfirmPassword = new Label();
            lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            lblConfirmPassword.Font = new Font("Segoe UI", 9);
            lblConfirmPassword.Location = new Point(20, 80);
            lblConfirmPassword.Size = new Size(120, 20);
            formPanel.Controls.Add(lblConfirmPassword);

            TextBox txtConfirmPassword = new TextBox();
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Font = new Font("Segoe UI", 10);
            txtConfirmPassword.Size = new Size(200, 20);
            txtConfirmPassword.Location = new Point(150, 80);
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.UseSystemPasswordChar = true;
            formPanel.Controls.Add(txtConfirmPassword);

            // Action buttons
            Button btnChangePassword = new Button();
            btnChangePassword.Text = "Đổi mật khẩu";
            btnChangePassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnChangePassword.Size = new Size(100, 30);
            btnChangePassword.Location = new Point(150, 120);
            btnChangePassword.BackColor = Color.FromArgb(0, 174, 219);
            btnChangePassword.ForeColor = Color.White;
            btnChangePassword.FlatStyle = FlatStyle.Flat;
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Cursor = Cursors.Hand;
            btnChangePassword.Click += BtnChangePassword_Click;
            formPanel.Controls.Add(btnChangePassword);

            Button btnCancel = new Button();
            btnCancel.Text = "Hủy";
            btnCancel.Font = new Font("Segoe UI", 10);
            btnCancel.Size = new Size(100, 30);
            btnCancel.Location = new Point(260, 120);
            btnCancel.BackColor = Color.FromArgb(245, 245, 245);
            btnCancel.ForeColor = Color.FromArgb(50, 50, 50);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += (s, e) => this.Close();
            formPanel.Controls.Add(btnCancel);

            this.Controls.Add(formPanel);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            var currentPasswordControl = this.Controls[0].Controls.Find("txtCurrentPassword", true)[0] as TextBox;
            var newPasswordControl = this.Controls[0].Controls.Find("txtNewPassword", true)[0] as TextBox;
            var confirmPasswordControl = this.Controls[0].Controls.Find("txtConfirmPassword", true)[0] as TextBox;

            var currentPassword = currentPasswordControl?.Text;
            var newPassword = newPasswordControl?.Text;
            var confirmPassword = confirmPasswordControl?.Text;

            if (string.IsNullOrEmpty(currentPassword) ||
                string.IsNullOrEmpty(newPassword) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Authenticate current password
            var authenticatedUser = _userService.AuthenticateUser(_currentUser.Element("Email").Value, currentPassword);
            if (authenticatedUser == null)
            {
                MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update password
            _currentUser.Element("MatKhauHash").Value = newPassword; // Password will be hashed in service
            _userService.UpdateUser(_currentUser);

            MessageBox.Show("Đổi mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}