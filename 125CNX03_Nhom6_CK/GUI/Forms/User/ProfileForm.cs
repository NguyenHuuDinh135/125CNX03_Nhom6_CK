using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProfileForm : UserBaseForm
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
            this.AutoScroll = true;

            // Header
            var headerPanel = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            var headerTitle = new Label { Text = "Thông tin cá nhân", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 30) };
            headerPanel.Controls.Add(headerTitle);
            this.Controls.Add(headerPanel);

            // Profile panel
            var profilePanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, 600));

            // Controls
            profilePanel.Controls.Add(CreateLabel("Họ và tên:", new Point(20, 20)));
            profilePanel.Controls.Add(CreateTextBox("txtFullName", new Point(130, 20), new Size(300, 28)));

            profilePanel.Controls.Add(CreateLabel("Email:", new Point(20, 60)));
            var txtEmail = CreateTextBox("txtEmail", new Point(130, 60), new Size(300, 28));
            txtEmail.ReadOnly = true;
            profilePanel.Controls.Add(txtEmail);

            profilePanel.Controls.Add(CreateLabel("Số điện thoại:", new Point(20, 100)));
            profilePanel.Controls.Add(CreateTextBox("txtPhone", new Point(130, 100), new Size(300, 28)));

            profilePanel.Controls.Add(CreateLabel("Địa chỉ:", new Point(20, 140)));
            profilePanel.Controls.Add(CreateTextBox("txtAddress", new Point(130, 140), new Size(300, 28)));

            // Buttons
            var btnSave = CreateButton("Lưu thay đổi", new Point(20, 190), new Size(140, 36), Primary, BtnSave_Click);
            profilePanel.Controls.Add(btnSave);

            var btnChangePassword = CreateButton("Đổi mật khẩu", new Point(180, 190), new Size(140, 36), Color.FromArgb(219, 0, 0), BtnChangePassword_Click);
            profilePanel.Controls.Add(btnChangePassword);

            this.Controls.Add(profilePanel);
        }

        private void LoadUserData()
        {
            var profilePanel = this.Controls[this.Controls.Count - 1] as Panel;
            var fullNameControl = profilePanel?.Controls.Find("txtFullName", true)[0] as TextBox;
            var emailControl = profilePanel?.Controls.Find("txtEmail", true)[0] as TextBox;
            var phoneControl = profilePanel?.Controls.Find("txtPhone", true)[0] as TextBox;
            var addressControl = profilePanel?.Controls.Find("txtAddress", true)[0] as TextBox;

            if (fullNameControl != null) fullNameControl.Text = _currentUser.Element("HoTen")?.Value ?? "";
            if (emailControl != null) emailControl.Text = _currentUser.Element("Email")?.Value ?? "";
            if (phoneControl != null) phoneControl.Text = _currentUser.Element("SoDienThoai")?.Value ?? "";
            if (addressControl != null) addressControl.Text = _currentUser.Element("DiaChi")?.Value ?? "";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var profilePanel = this.Controls[this.Controls.Count - 1] as Panel;
            var fullNameControl = profilePanel?.Controls.Find("txtFullName", true)[0] as TextBox;
            var phoneControl = profilePanel?.Controls.Find("txtPhone", true)[0] as TextBox;
            var addressControl = profilePanel?.Controls.Find("txtAddress", true)[0] as TextBox;

            if (string.IsNullOrEmpty(fullNameControl?.Text))
            {
                ShowWarning("Vui lòng nhập họ tên!", "Lỗi");
                return;
            }

            _currentUser.Element("HoTen").Value = fullNameControl.Text;
            _currentUser.Element("SoDienThoai").Value = phoneControl?.Text ?? "";
            _currentUser.Element("DiaChi").Value = addressControl?.Text ?? "";

            _userService.UpdateUser(_currentUser);

            ShowInfo("Cập nhật thông tin thành công!", "Thành công");
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            var changePasswordForm = new ChangePasswordForm(_currentUser);
            changePasswordForm.ShowDialog();
        }
    }
}