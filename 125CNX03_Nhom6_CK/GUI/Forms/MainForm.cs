using System;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.GUI.Forms;
using _125CNX03_Nhom6_CK.BLL;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class MainForm : Form
    {
        private readonly INguoiDungService _userService;
        private XElement currentUser;

        public MainForm()
        {
            InitializeComponent();
            _userService = new NguoiDungService();
            SetupUI();
        }

        public MainForm(XElement user) : this()
        {
            currentUser = user;
            UpdateUIForUser();
        }

        private void SetupUI()
        {
            this.Text = "Cửa hàng bán laptop";
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;

            // Hide management panels initially
            panelManagement.Visible = false;
        }

        private void UpdateUIForUser()
        {
            if (currentUser != null)
            {
                lblUser.Text = $"Xin chào, {currentUser.Element("HoTen").Value}";

                if (currentUser.Element("VaiTro").Value == "Admin")
                {
                    panelManagement.Visible = true;
                    btnProductManagement.Visible = true;
                    btnCategoryManagement.Visible = true;
                    btnBrandManagement.Visible = true;
                    btnUserManagement.Visible = true;
                    btnOrderManagement.Visible = true;
                    btnBannerManagement.Visible = true;
                }
                else
                {
                    panelManagement.Visible = false;
                }
            }
            else
            {
                lblUser.Text = "Chưa đăng nhập";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            loginForm.UserLoggedIn += OnUserLoggedIn;
            loginForm.ShowDialog();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void OnUserLoggedIn(object sender, UserEventArgs e)
        {
            currentUser = e.User;
            UpdateUIForUser();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            currentUser = null;
            UpdateUIForUser();
        }

        private void btnProductManagement_Click(object sender, EventArgs e)
        {
            var form = new ProductManagementForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnCategoryManagement_Click(object sender, EventArgs e)
        {
            var form = new CategoryManagementForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnBrandManagement_Click(object sender, EventArgs e)
        {
            var form = new BrandManagementForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            var form = new UserManagementForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnOrderManagement_Click(object sender, EventArgs e)
        {
            var form = new OrderManagementForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnBannerManagement_Click(object sender, EventArgs e)
        {
            var form = new BannerForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            var form = new CartForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnBlog_Click(object sender, EventArgs e)
        {
            var form = new BlogForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnContact_Click(object sender, EventArgs e)
        {
            var form = new ContactForm();
            form.MdiParent = this;
            form.Show();
        }

        private void btnProductCatalog_Click(object sender, EventArgs e)
        {
            // You can implement product catalog display here
            MessageBox.Show("Chức năng đang phát triển");
        }
    }

    public class UserEventArgs : EventArgs
    {
        public XElement User { get; }

        public UserEventArgs(XElement user)
        {
            User = user;
        }
    }
}