using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProfileForm : Form
    {
        private readonly INguoiDungService _userService;
        private XElement _currentUser;

        public ProfileForm(XElement user)
        {
            InitializeComponent();
            _userService = new NguoiDungService();
            _currentUser = user;

            InitializeUI();
            LoadUserData();
        }

        private void InitializeUI()
        {
            this.Text = "Thông tin cá nhân";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create header
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.Width - 40, 60);
            headerPanel.Location = new Point(20, 20);
            headerPanel.BackColor = Color.White;
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            Label headerTitle = new Label();
            headerTitle.Text = "Thông tin cá nhân";
            headerTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            headerTitle.Location = new Point(20, 15);
            headerTitle.Size = new Size(200, 30);
            headerPanel.Controls.Add(headerTitle);

            this.Controls.Add(headerPanel);

            // Create profile form panel
            Panel profilePanel = new Panel();
            profilePanel.Size = new Size(this.Width - 40, 600);
            profilePanel.Location = new Point(20, 100);
            profilePanel.BackColor = Color.White;
            profilePanel.BorderStyle = BorderStyle.FixedSingle;

            // Form controls
            Label lblFullName = new Label();
            lblFullName.Text = "Họ và tên:";
            lblFullName.Font = new Font("Segoe UI", 9);
            lblFullName.Location = new Point(20, 20);
            lblFullName.Size = new Size(100, 20);
            profilePanel.Controls.Add(lblFullName);

            TextBox txtFullName = new TextBox();
            txtFullName.Name = "txtFullName";
            txtFullName.Font = new Font("Segoe UI", 10);
            txtFullName.Size = new Size(200, 20);
            txtFullName.Location = new Point(130, 20);
            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            profilePanel.Controls.Add(txtFullName);

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 9);
            lblEmail.Location = new Point(20, 50);
            lblEmail.Size = new Size(100, 20);
            profilePanel.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Size = new Size(200, 20);
            txtEmail.Location = new Point(130, 50);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ReadOnly = true; // Email shouldn't be editable
            profilePanel.Controls.Add(txtEmail);

            Label lblPhone = new Label();
            lblPhone.Text = "Số điện thoại:";
            lblPhone.Font = new Font("Segoe UI", 9);
            lblPhone.Location = new Point(20, 80);
            lblPhone.Size = new Size(100, 20);
            profilePanel.Controls.Add(lblPhone);

            TextBox txtPhone = new TextBox();
            txtPhone.Name = "txtPhone";
            txtPhone.Font = new Font("Segoe UI", 10);
            txtPhone.Size = new Size(200, 20);
            txtPhone.Location = new Point(130, 80);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            profilePanel.Controls.Add(txtPhone);

            Label lblAddress = new Label();
            lblAddress.Text = "Địa chỉ:";
            lblAddress.Font = new Font("Segoe UI", 9);
            lblAddress.Location = new Point(20, 110);
            lblAddress.Size = new Size(100, 20);
            profilePanel.Controls.Add(lblAddress);

            TextBox txtAddress = new TextBox();
            txtAddress.Name = "txtAddress";
            txtAddress.Font = new Font("Segoe UI", 10);
            txtAddress.Size = new Size(200, 20);
            txtAddress.Location = new Point(130, 110);
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            profilePanel.Controls.Add(txtAddress);

            // Action buttons
            Button btnSave = new Button();
            btnSave.Text = "Lưu thay đổi";
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSave.Size = new Size(100, 30);
            btnSave.Location = new Point(20, 150);
            btnSave.BackColor = Color.FromArgb(0, 174, 219);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Cursor = Cursors.Hand;
            btnSave.Click += BtnSave_Click;
            profilePanel.Controls.Add(btnSave);

            Button btnChangePassword = new Button();
            btnChangePassword.Text = "Đổi mật khẩu";
            btnChangePassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnChangePassword.Size = new Size(120, 30);
            btnChangePassword.Location = new Point(130, 150);
            btnChangePassword.BackColor = Color.FromArgb(219, 0, 0);
            btnChangePassword.ForeColor = Color.White;
            btnChangePassword.FlatStyle = FlatStyle.Flat;
            btnChangePassword.FlatAppearance.BorderSize = 0;
            btnChangePassword.Cursor = Cursors.Hand;
            btnChangePassword.Click += BtnChangePassword_Click;
            profilePanel.Controls.Add(btnChangePassword);

            this.Controls.Add(profilePanel);
        }

        private void LoadUserData()
        {
            var fullNameControl = this.Controls[1].Controls.Find("txtFullName", true)[0] as TextBox;
            var emailControl = this.Controls[1].Controls.Find("txtEmail", true)[0] as TextBox;
            var phoneControl = this.Controls[1].Controls.Find("txtPhone", true)[0] as TextBox;
            var addressControl = this.Controls[1].Controls.Find("txtAddress", true)[0] as TextBox;

            if (fullNameControl != null) fullNameControl.Text = _currentUser.Element("HoTen").Value;
            if (emailControl != null) emailControl.Text = _currentUser.Element("Email").Value;
            if (phoneControl != null) phoneControl.Text = _currentUser.Element("SoDienThoai").Value;
            if (addressControl != null) addressControl.Text = _currentUser.Element("DiaChi").Value;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var fullNameControl = this.Controls[1].Controls.Find("txtFullName", true)[0] as TextBox;
            var phoneControl = this.Controls[1].Controls.Find("txtPhone", true)[0] as TextBox;
            var addressControl = this.Controls[1].Controls.Find("txtAddress", true)[0] as TextBox;

            if (string.IsNullOrEmpty(fullNameControl?.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update user data
            _currentUser.Element("HoTen").Value = fullNameControl.Text;
            _currentUser.Element("SoDienThoai").Value = phoneControl?.Text ?? "";
            _currentUser.Element("DiaChi").Value = addressControl?.Text ?? "";

            // Save to database
            _userService.UpdateUser(_currentUser);

            MessageBox.Show("Cập nhật thông tin thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            var changePasswordForm = new ChangePasswordForm(_currentUser);
            changePasswordForm.ShowDialog();
        }
    }
}