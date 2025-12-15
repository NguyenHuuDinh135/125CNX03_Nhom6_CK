using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.Forms.Admin;
using _125CNX03_Nhom6_CK.GUI.Forms.User;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;


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
            subtitleLabel.Text = "Đăng nhập";
            subtitleLabel.Font = new Font("Segoe UI", 12);
            subtitleLabel.ForeColor = Color.FromArgb(200, 200, 200);
            subtitleLabel.Location = new Point(100, 230);
            subtitleLabel.Size = new Size(150, 20);
            leftPanel.Controls.Add(subtitleLabel);

            // Add description
            Label descriptionLabel = new Label();
            descriptionLabel.Text = "Chào mừng bạn trở lại! Vui lòng đăng nhập để tiếp tục.";
            descriptionLabel.Font = new Font("Segoe UI", 10);
            descriptionLabel.ForeColor = Color.FromArgb(200, 200, 200);
            descriptionLabel.Location = new Point(50, 270);
            descriptionLabel.Size = new Size(250, 60);
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter;
            leftPanel.Controls.Add(descriptionLabel);

            // Add register button
            Button btnRegister = new Button();
            btnRegister.Text = "Đăng ký";
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegister.Size = new Size(120, 30);
            btnRegister.Location = new Point(100, 420);
            btnRegister.BackColor = Color.FromArgb(0, 174, 219);
            btnRegister.ForeColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += RegisterButton_Click;
            leftPanel.Controls.Add(btnRegister);

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

            // Add login title
            Label loginTitle = new Label();
            loginTitle.Text = "NHÓM 6";
            loginTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            loginTitle.ForeColor = Color.FromArgb(0, 174, 219);
            loginTitle.Location = new Point(80, 60);
            loginTitle.Size = new Size(300, 30);
            rightPanel.Controls.Add(loginTitle);

            // Add user login label
            Label userLoginLabel = new Label();
            userLoginLabel.Text = "Đăng nhập";
            userLoginLabel.Font = new Font("Segoe UI", 12);
            userLoginLabel.ForeColor = Color.FromArgb(100, 100, 100);
            userLoginLabel.Location = new Point(80, 95);
            userLoginLabel.Size = new Size(100, 20);
            rightPanel.Controls.Add(userLoginLabel);

            // Add email label
            Label emailLabel = new Label();
            emailLabel.Text = "Email";
            emailLabel.Font = new Font("Segoe UI", 9);
            emailLabel.ForeColor = Color.FromArgb(100, 100, 100);
            emailLabel.Location = new Point(80, 130);
            emailLabel.Size = new Size(50, 20);
            rightPanel.Controls.Add(emailLabel);

            // Add email textbox
            TextBox txtEmail = new TextBox();
            txtEmail.Text = "admin@shop.com";
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Size = new Size(300, 30);
            txtEmail.Location = new Point(80, 150);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.Padding = new Padding(5);
            rightPanel.Controls.Add(txtEmail);

            // Add password label
            Label passwordLabel = new Label();
            passwordLabel.Text = "Mật khẩu";
            passwordLabel.Font = new Font("Segoe UI", 9);
            passwordLabel.ForeColor = Color.FromArgb(100, 100, 100);
            passwordLabel.Location = new Point(80, 190);
            passwordLabel.Size = new Size(60, 20);
            rightPanel.Controls.Add(passwordLabel);

            // Add password textbox
            TextBox txtPassword = new TextBox();
            txtPassword.Text = "123456";
            txtPassword.Font = new Font("Segoe UI", 10);
            txtPassword.Size = new Size(300, 30);
            txtPassword.Location = new Point(80, 210);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Padding = new Padding(5);
            txtPassword.UseSystemPasswordChar = true;
            rightPanel.Controls.Add(txtPassword);

            // Add remember me checkbox
            CheckBox chkRememberMe = new CheckBox();
            chkRememberMe.Text = "Ghi nhớ tôi";
            chkRememberMe.Font = new Font("Segoe UI", 9);
            chkRememberMe.ForeColor = Color.FromArgb(100, 100, 100);
            chkRememberMe.Location = new Point(80, 250);
            chkRememberMe.Size = new Size(100, 20);
            rightPanel.Controls.Add(chkRememberMe);

            // Add login button
            Button btnLogin = new Button();
            btnLogin.Text = "Đăng nhập";
            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogin.Size = new Size(300, 35);
            btnLogin.Location = new Point(80, 280);
            btnLogin.BackColor = Color.FromArgb(0, 174, 219);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += (s, e) => Login_Click(s, e, txtEmail, txtPassword);
            rightPanel.Controls.Add(btnLogin);

            // Add forgot password link
            LinkLabel forgotPasswordLink = new LinkLabel();
            forgotPasswordLink.Text = "Quên mật khẩu?";
            forgotPasswordLink.Font = new Font("Segoe UI", 9);
            forgotPasswordLink.LinkColor = Color.FromArgb(0, 174, 219);
            forgotPasswordLink.ActiveLinkColor = Color.FromArgb(0, 140, 175);
            forgotPasswordLink.Location = new Point(80, 320);
            forgotPasswordLink.Size = new Size(100, 20);
            forgotPasswordLink.LinkClicked += ForgotPasswordLink_LinkClicked;
            rightPanel.Controls.Add(forgotPasswordLink);

            // Add controls to form
            this.Controls.Add(leftPanel);
            this.Controls.Add(rightPanel);
        }
        public XElement LoggedInUser { get; private set; }

        private void Login_Click(object sender, EventArgs e, TextBox emailBox, TextBox passwordBox)
        {
            string email = emailBox.Text.Trim();
            string password = passwordBox.Text;

            var user = _userService.AuthenticateUser(email, password);
            if (user != null)
            {
                LoggedInUser = user;
                this.DialogResult = DialogResult.OK; // báo hiệu login thành công
                this.Close();
            }
            else
            {
                MessageBox.Show("Email hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
            this.Close();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ForgotPasswordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Chức năng quên mật khẩu đang phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}