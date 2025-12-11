using System;
using System.Drawing;
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

            // Set form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 500);
            this.BackColor = Color.FromArgb(0, 174, 219); // Teal background

            // Initialize controls
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Create left panel (dark background)
            Panel leftPanel = new Panel();
            leftPanel.Size = new Size(300, 500);
            leftPanel.Location = new Point(0, 0);
            leftPanel.BackColor = Color.FromArgb(33, 33, 33);
            leftPanel.Dock = DockStyle.Left;

            // Add title
            Label titleLabel = new Label();
            titleLabel.Text = "Nhóm 6";
            titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(80, 180);
            titleLabel.Size = new Size(200, 40);
            leftPanel.Controls.Add(titleLabel);

            // Add subtitle
            Label subtitleLabel = new Label();
            subtitleLabel.Text = "Đăng ký";
            subtitleLabel.Font = new Font("Segoe UI", 12);
            subtitleLabel.ForeColor = Color.FromArgb(200, 200, 200);
            subtitleLabel.Location = new Point(100, 230);
            subtitleLabel.Size = new Size(150, 20);
            leftPanel.Controls.Add(subtitleLabel);

            // Add description
            Label descriptionLabel = new Label();
            descriptionLabel.Text = "Tạo tài khoản để bắt đầu sử dụng dịch vụ của chúng tôi.";
            descriptionLabel.Font = new Font("Segoe UI", 10);
            descriptionLabel.ForeColor = Color.FromArgb(200, 200, 200);
            descriptionLabel.Location = new Point(50, 270);
            descriptionLabel.Size = new Size(250, 60);
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(descriptionLabel);

            // Add login button
            Button btnLogin = new Button();
            btnLogin.Text = "Đăng nhập";
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.Size = new Size(120, 30);
            btnLogin.Location = new Point(100, 420);
            btnLogin.BackColor = Color.FromArgb(0, 174, 219);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += LoginButton_Click;
            leftPanel.Controls.Add(btnLogin);

            // Create right panel (white background)
            Panel rightPanel = new Panel();
            rightPanel.Size = new Size(500, 500);
            rightPanel.Location = new Point(300, 0);
            rightPanel.BackColor = Color.White;
            rightPanel.Dock = DockStyle.Right;

            // Add close button
            Button closeButton = new Button();
            closeButton.Text = "×";
            closeButton.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            closeButton.Size = new Size(30, 30);
            closeButton.Location = new Point(460, 10);
            closeButton.BackColor = Color.Transparent;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.ForeColor = Color.FromArgb(100, 100, 100);
            closeButton.Click += CloseButton_Click;
            rightPanel.Controls.Add(closeButton);

            // Add register title
            Label registerTitle = new Label();
            registerTitle.Text = "NHÓM 6";
            registerTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            registerTitle.ForeColor = Color.FromArgb(0, 174, 219);
            registerTitle.Location = new Point(80, 60);
            registerTitle.Size = new Size(300, 30);
            rightPanel.Controls.Add(registerTitle);

            // Add user register label
            Label userRegisterLabel = new Label();
            userRegisterLabel.Text = "Đăng ký";
            userRegisterLabel.Font = new Font("Segoe UI", 12);
            userRegisterLabel.ForeColor = Color.FromArgb(100, 100, 100);
            userRegisterLabel.Location = new Point(80, 95);
            userRegisterLabel.Size = new Size(150, 20);
            rightPanel.Controls.Add(userRegisterLabel);

            // Add full name label
            Label fullNameLabel = new Label();
            fullNameLabel.Text = "Họ và tên";
            fullNameLabel.Font = new Font("Segoe UI", 9);
            fullNameLabel.ForeColor = Color.FromArgb(100, 100, 100);
            fullNameLabel.Location = new Point(80, 130);
            fullNameLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(fullNameLabel);

            // Add full name textbox
            TextBox txtFullName = new TextBox();
            txtFullName.Font = new Font("Segoe UI", 10);
            txtFullName.Size = new Size(300, 30);
            txtFullName.Location = new Point(80, 150);
            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            txtFullName.Padding = new Padding(5);
            rightPanel.Controls.Add(txtFullName);

            // Add email label
            Label emailLabel = new Label();
            emailLabel.Text = "Email";
            emailLabel.Font = new Font("Segoe UI", 9);
            emailLabel.ForeColor = Color.FromArgb(100, 100, 100);
            emailLabel.Location = new Point(80, 190);
            emailLabel.Size = new Size(50, 20);
            rightPanel.Controls.Add(emailLabel);

            // Add email textbox
            TextBox txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Size = new Size(300, 30);
            txtEmail.Location = new Point(80, 210);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Padding = new Padding(5);
            rightPanel.Controls.Add(txtEmail);

            // Add password label
            Label passwordLabel = new Label();
            passwordLabel.Text = "Mật khẩu";
            passwordLabel.Font = new Font("Segoe UI", 9);
            passwordLabel.ForeColor = Color.FromArgb(100, 100, 100);
            passwordLabel.Location = new Point(80, 250);
            passwordLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(passwordLabel);

            // Add password textbox
            TextBox txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(80, 270);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Padding = new Padding(5);
            txtPassword.UseSystemPasswordChar = true;
            rightPanel.Controls.Add(txtPassword);

            // Add confirm password label
            Label confirmPasswordLabel = new Label();
            confirmPasswordLabel.Text = "Xác nhận mật khẩu";
            confirmPasswordLabel.Font = new Font("Segoe UI", 9);
            confirmPasswordLabel.ForeColor = Color.FromArgb(100, 100, 100);
            confirmPasswordLabel.Location = new Point(80, 310);
            confirmPasswordLabel.Size = new Size(150, 20);
            rightPanel.Controls.Add(confirmPasswordLabel);

            // Add confirm password textbox
            TextBox txtConfirmPassword = new TextBox();
            txtConfirmPassword.Font = new Font("Segoe UI", 10);
            txtConfirmPassword.Size = new Size(300, 30);
            txtConfirmPassword.Location = new Point(80, 330);
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.Padding = new Padding(5);
            txtConfirmPassword.UseSystemPasswordChar = true;
            rightPanel.Controls.Add(txtConfirmPassword);

            // Add phone label
            Label phoneLabel = new Label();
            phoneLabel.Text = "Số điện thoại";
            phoneLabel.Font = new Font("Segoe UI", 9);
            phoneLabel.ForeColor = Color.FromArgb(100, 100, 100);
            phoneLabel.Location = new Point(80, 370);
            phoneLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(phoneLabel);

            // Add phone textbox
            TextBox txtPhone = new TextBox();
            txtPhone.Font = new Font("Segoe UI", 10);
            txtPhone.Size = new Size(300, 30);
            txtPhone.Location = new Point(80, 390);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.Padding = new Padding(5);
            rightPanel.Controls.Add(txtPhone);

            // Add register button
            Button btnRegister = new Button();
            btnRegister.Text = "Đăng ký";
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegister.Size = new Size(300, 35);
            btnRegister.Location = new Point(80, 430);
            btnRegister.BackColor = Color.FromArgb(0, 174, 219);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += (s, e) => Register_Click(s, e, txtFullName, txtEmail, txtPassword, txtConfirmPassword, txtPhone);
            rightPanel.Controls.Add(btnRegister);

            // Add controls to form
            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);
        }

        private void Register_Click(object sender, EventArgs e, TextBox fullNameBox, TextBox emailBox, TextBox passwordBox, TextBox confirmPasswordBox, TextBox phoneBox)
        {
            string fullName = fullNameBox.Text.Trim();
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;
            string confirmPassword = confirmPasswordBox.Text;
            string phone = phoneBox.Text.Trim();

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

            try
            {
                // Create user element
                var newUser = new System.Xml.Linq.XElement("NguoiDung",
                    new System.Xml.Linq.XElement("HoTen", fullName),
                    new System.Xml.Linq.XElement("Email", email),
                    new System.Xml.Linq.XElement("MatKhauHash", password), // Password will be hashed in service
                    new System.Xml.Linq.XElement("SoDienThoai", phone),
                    new System.Xml.Linq.XElement("DiaChi", ""), // Address can be added later
                    new System.Xml.Linq.XElement("VaiTro", "Customer"),
                    new System.Xml.Linq.XElement("NgayTao", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new System.Xml.Linq.XElement("TrangThai", "true")
                );

                _userService.AddUser(newUser);
                MessageBox.Show("Đăng ký thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Tạo user login ngay
                var loginUser = _userService.AuthenticateUser(email, password);

                if (loginUser != null)
                {
                    this.Hide();
                    var userForm = new _125CNX03_Nhom6_CK.GUI.Forms.User.MainForm(loginUser);
                    userForm.ShowDialog();
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}