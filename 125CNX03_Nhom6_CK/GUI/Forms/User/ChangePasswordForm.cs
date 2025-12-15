using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ChangePasswordForm : UserBaseForm
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
            this.Size = new Size(520, 300);

            var panel = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 60, this.Height - 80));

            panel.Controls.Add(CreateLabel("Mật khẩu hiện tại:", new Point(10, 20), 140));
            panel.Controls.Add(CreateTextBox("txtCurrentPassword", new Point(160, 18), new Size(300, 28), true));

            panel.Controls.Add(CreateLabel("Mật khẩu mới:", new Point(10, 60), 140));
            panel.Controls.Add(CreateTextBox("txtNewPassword", new Point(160, 58), new Size(300, 28), true));

            panel.Controls.Add(CreateLabel("Xác nhận mật khẩu:", new Point(10, 100), 140));
            panel.Controls.Add(CreateTextBox("txtConfirmPassword", new Point(160, 98), new Size(300, 28), true));

            var btnChange = CreateButton("Đổi mật khẩu", new Point(160, 150), new Size(160, 36), Primary, BtnChangePassword_Click);
            panel.Controls.Add(btnChange);

            var btnCancel = CreateButton("Hủy", new Point(330, 150), new Size(100, 36), Color.FromArgb(220, 220, 220), (s, e) => this.Close(), false);
            btnCancel.ForeColor = Color.FromArgb(50, 50, 50);
            panel.Controls.Add(btnCancel);

            this.Controls.Add(panel);
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            var panel = this.Controls[this.Controls.Count - 1] as Panel;
            var currentPasswordControl = panel?.Controls.Find("txtCurrentPassword", true)[0] as TextBox;
            var newPasswordControl = panel?.Controls.Find("txtNewPassword", true)[0] as TextBox;
            var confirmPasswordControl = panel?.Controls.Find("txtConfirmPassword", true)[0] as TextBox;

            var currentPassword = currentPasswordControl?.Text;
            var newPassword = newPasswordControl?.Text;
            var confirmPassword = confirmPasswordControl?.Text;

            if (string.IsNullOrEmpty(currentPassword) ||
                string.IsNullOrEmpty(newPassword) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                ShowWarning("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("Mật khẩu mới không khớp!");
                return;
            }

            var authenticatedUser = _userService.AuthenticateUser(_currentUser.Element("Email")?.Value, currentPassword);
            if (authenticatedUser == null)
            {
                ShowError("Mật khẩu hiện tại không đúng!");
                return;
            }

            _currentUser.Element("MatKhauHash").Value = newPassword; // service will hash
            _userService.UpdateUser(_currentUser);

            ShowInfo("Đổi mật khẩu thành công!");
            this.Close();
        }
    }
}