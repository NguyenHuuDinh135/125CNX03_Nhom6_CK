using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            this.Size = new Size(800, 600);
            this.BackColor = Color.FromArgb(0, 174, 219);

            // Initialize controls
            InitializeControls();
        }

        private void InitializeControls()
        {
            // Create left panel (dark background)
            Panel leftPanel = new Panel();
            leftPanel.Size = new Size(300, 600);
            leftPanel.Location = new Point(0, 0);
            leftPanel.BackColor = Color.FromArgb(33, 33, 33);
            leftPanel.Dock = DockStyle.Left;

            // Add title
            Label titleLabel = new Label();
            titleLabel.Text = "Nhóm 6";
            titleLabel.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.Location = new Point(80, 220);
            titleLabel.Size = new Size(200, 40);
            leftPanel.Controls.Add(titleLabel);

            // Add subtitle
            Label subtitleLabel = new Label();
            subtitleLabel.Text = "Đăng ký";
            subtitleLabel.Font = new Font("Segoe UI", 12);
            subtitleLabel.ForeColor = Color.FromArgb(200, 200, 200);
            subtitleLabel.Location = new Point(100, 270);
            subtitleLabel.Size = new Size(150, 20);
            leftPanel.Controls.Add(subtitleLabel);

            // Add description
            Label descriptionLabel = new Label();
            descriptionLabel.Text = "Tạo tài khoản để bắt đầu sử dụng dịch vụ của chúng tôi.";
            descriptionLabel.Font = new Font("Segoe UI", 10);
            descriptionLabel.ForeColor = Color.FromArgb(200, 200, 200);
            descriptionLabel.Location = new Point(50, 310);
            descriptionLabel.Size = new Size(250, 60);
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(descriptionLabel);

            // Add login button
            Button btnLogin = new Button();
            btnLogin.Text = "Đăng nhập";
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.Size = new Size(120, 30);
            btnLogin.Location = new Point(100, 520);
            btnLogin.BackColor = Color.FromArgb(0, 174, 219);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += LoginButton_Click;
            leftPanel.Controls.Add(btnLogin);

            // Create right panel (white background) with scroll
            Panel rightPanel = new Panel();
            rightPanel.Size = new Size(500, 600);
            rightPanel.Location = new Point(300, 0);
            rightPanel.BackColor = Color.White;
            rightPanel.Dock = DockStyle.Right;
            rightPanel.AutoScroll = true;

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
            registerTitle.Location = new Point(80, 40);
            registerTitle.Size = new Size(300, 30);
            rightPanel.Controls.Add(registerTitle);

            // Add user register label
            Label userRegisterLabel = new Label();
            userRegisterLabel.Text = "Đăng ký tài khoản mới";
            userRegisterLabel.Font = new Font("Segoe UI", 12);
            userRegisterLabel.ForeColor = Color.FromArgb(100, 100, 100);
            userRegisterLabel.Location = new Point(80, 75);
            userRegisterLabel.Size = new Size(200, 20);
            rightPanel.Controls.Add(userRegisterLabel);

            // Add full name label
            Label fullNameLabel = new Label();
            fullNameLabel.Text = "Họ và tên *";
            fullNameLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            fullNameLabel.ForeColor = Color.FromArgb(100, 100, 100);
            fullNameLabel.Location = new Point(80, 110);
            fullNameLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(fullNameLabel);

            // Add full name textbox
            TextBox txtFullName = new TextBox();
            txtFullName.Font = new Font("Segoe UI", 10);
            txtFullName.Size = new Size(300, 30);
            txtFullName.Location = new Point(80, 130);
            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(txtFullName);

            // Add email label
            Label emailLabel = new Label();
            emailLabel.Text = "Email *";
            emailLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            emailLabel.ForeColor = Color.FromArgb(100, 100, 100);
            emailLabel.Location = new Point(80, 170);
            emailLabel.Size = new Size(50, 20);
            rightPanel.Controls.Add(emailLabel);

            // Add email textbox
            TextBox txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Size = new Size(300, 30);
            txtEmail.Location = new Point(80, 190);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(txtEmail);

            // Add password label
            Label passwordLabel = new Label();
            passwordLabel.Text = "Mật khẩu *";
            passwordLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            passwordLabel.ForeColor = Color.FromArgb(100, 100, 100);
            passwordLabel.Location = new Point(80, 230);
            passwordLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(passwordLabel);

            // Add password textbox
            TextBox txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(80, 250);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.UseSystemPasswordChar = true;
            rightPanel.Controls.Add(txtPassword);

            // Add confirm password label
            Label confirmPasswordLabel = new Label();
            confirmPasswordLabel.Text = "Xác nhận mật khẩu *";
            confirmPasswordLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            confirmPasswordLabel.ForeColor = Color.FromArgb(100, 100, 100);
            confirmPasswordLabel.Location = new Point(80, 290);
            confirmPasswordLabel.Size = new Size(150, 20);
            rightPanel.Controls.Add(confirmPasswordLabel);

            // Add confirm password textbox
            TextBox txtConfirmPassword = new TextBox();
            txtConfirmPassword.Font = new Font("Segoe UI", 10);
            txtConfirmPassword.Size = new Size(300, 30);
            txtConfirmPassword.Location = new Point(80, 310);
            txtConfirmPassword.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmPassword.UseSystemPasswordChar = true;
            rightPanel.Controls.Add(txtConfirmPassword);

            // Add phone label
            Label phoneLabel = new Label();
            phoneLabel.Text = "Số điện thoại *";
            phoneLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            phoneLabel.ForeColor = Color.FromArgb(100, 100, 100);
            phoneLabel.Location = new Point(80, 350);
            phoneLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(phoneLabel);

            // Add phone textbox
            TextBox txtPhone = new TextBox();
            txtPhone.Font = new Font("Segoe UI", 10);
            txtPhone.Size = new Size(300, 30);
            txtPhone.Location = new Point(80, 370);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(txtPhone);

            // Add address label
            Label addressLabel = new Label();
            addressLabel.Text = "Địa chỉ (tùy chọn)";
            addressLabel.Font = new Font("Segoe UI", 9);
            addressLabel.ForeColor = Color.FromArgb(100, 100, 100);
            addressLabel.Location = new Point(80, 410);
            addressLabel.Size = new Size(150, 20);
            rightPanel.Controls.Add(addressLabel);

            // Add address textbox
            TextBox txtAddress = new TextBox();
            txtAddress.Font = new Font("Segoe UI", 10);
            txtAddress.Size = new Size(300, 30);
            txtAddress.Location = new Point(80, 430);
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            rightPanel.Controls.Add(txtAddress);

            // Add register button
            Button btnRegister = new Button();
            btnRegister.Text = "Đăng ký";
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegister.Size = new Size(300, 40);
            btnRegister.Location = new Point(80, 480);
            btnRegister.BackColor = Color.FromArgb(0, 174, 219);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += (s, e) => Register_Click(s, e, txtFullName, txtEmail, txtPassword, txtConfirmPassword, txtPhone, txtAddress);
            rightPanel.Controls.Add(btnRegister);

            // Add note label
            Label noteLabel = new Label();
            noteLabel.Text = "* Trường bắt buộc";
            noteLabel.Font = new Font("Segoe UI", 8, FontStyle.Italic);
            noteLabel.ForeColor = Color.FromArgb(150, 150, 150);
            noteLabel.Location = new Point(80, 530);
            noteLabel.Size = new Size(200, 20);
            rightPanel.Controls.Add(noteLabel);

            // Add controls to form
            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);
        }

        private void Register_Click(object sender, EventArgs e, TextBox fullNameBox, TextBox emailBox,
            TextBox passwordBox, TextBox confirmPasswordBox, TextBox phoneBox, TextBox addressBox)
        {
            string fullName = fullNameBox.Text.Trim();
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;
            string confirmPassword = confirmPasswordBox.Text;
            string phone = phoneBox.Text.Trim();
            string address = addressBox.Text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                fullNameBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                emailBox.Focus();
                return;
            }

            // Validate email format
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                emailBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordBox.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                passwordBox.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                confirmPasswordBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                phoneBox.Focus();
                return;
            }

            // Validate phone format (10-11 digits)
            if (phone.Length < 10 || phone.Length > 11 || !phone.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! (10-11 chữ số)", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                phoneBox.Focus();
                return;
            }

            // Check if email exists
            if (!_userService.IsEmailUnique(email))
            {
                MessageBox.Show("Email đã được sử dụng! Vui lòng chọn email khác.", "Email đã tồn tại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                emailBox.Focus();
                return;
            }

            try
            {
                // Generate new ID
                var allUsers = _userService.GetAllUsers();
                int newId = allUsers.Any()
                    ? allUsers.Max(u => (int?)u.Element("Id") ?? 0) + 1
                    : 1;

                // Create user element with Id
                var newUser = new System.Xml.Linq.XElement("NguoiDung",
                    new System.Xml.Linq.XElement("Id", newId),
                    new System.Xml.Linq.XElement("HoTen", fullName),
                    new System.Xml.Linq.XElement("Email", email),
                    new System.Xml.Linq.XElement("MatKhauHash", password), // Will be hashed in service
                    new System.Xml.Linq.XElement("SoDienThoai", phone),
                    new System.Xml.Linq.XElement("DiaChi", address),
                    new System.Xml.Linq.XElement("VaiTro", "Customer"),
                    new System.Xml.Linq.XElement("NgayTao", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new System.Xml.Linq.XElement("TrangThai", "true")
                );

                _userService.AddUser(newUser);
                // Tạo giỏ hàng ngay cho user mới
                var cartService = new GioHangService();
                cartService.CreateCartForUser(newId);

                MessageBox.Show("Đăng ký thành công! Bạn sẽ được tự động đăng nhập.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Auto login
                var loginUser = _userService.AuthenticateUser(email, password);

                if (loginUser != null)
                {
                    this.Hide();
                    var userForm = new _125CNX03_Nhom6_CK.GUI.Forms.User.MainForm(loginUser);
                    userForm.FormClosed += (s, args) => this.Close();
                    userForm.Show();
                }
                else
                {
                    MessageBox.Show("Đăng ký thành công nhưng không thể tự động đăng nhập. Vui lòng đăng nhập thủ công.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoginButton_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng ký: {ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            var loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => this.Close();
            loginForm.Show();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}