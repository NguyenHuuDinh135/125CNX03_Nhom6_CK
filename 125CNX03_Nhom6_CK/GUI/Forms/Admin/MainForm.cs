using _125CNX03_Nhom6_CK.GUI.Forms.Admin;
using _125CNX03_Nhom6_CK.GUI.UserControls.Admin;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.ActivationContext;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class MainForm : Form
    {
        private DashboardForm _dashboardForm;
        private ProductManagementForm _productManagementForm;
        private CategoryManagementForm _categoryManagementForm;
        private BrandManagementForm _brandManagementForm;
        private UserManagementForm _userManagementForm;
        private OrderManagementForm _orderManagementForm;
        private BannerForm _bannerForm;
        private BlogManagementForm _blogForm;
        private ContactForm _contactForm;
        private XElement _currentUser;
        private Panel _contentPanel;
        private HeaderControl _headerControl;

        public MainForm(XElement user)
        {
            InitializeComponent();
            _currentUser = user;
            SetupUI();
            ShowDashboardForm();
        }

        private void SetupUI()
        {
            this.Text = "Cửa hàng bán laptop - Quản trị hệ thống";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            // Sidebar
            SidebarControl sidebar = new SidebarControl();
            sidebar.Location = new Point(0, 0);
            sidebar.Size = new Size(250, this.ClientSize.Height);
            sidebar.MenuItemClicked += Sidebar_MenuItemClicked;
            sidebar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.Controls.Add(sidebar);

            // Header (store reference so we can update Title)
            _headerControl = new HeaderControl();
            _headerControl.Location = new Point(250, 0);
            _headerControl.Size = new Size(this.ClientSize.Width - 250, 80);
            _headerControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(_headerControl);

            // Content Panel
            _contentPanel = new Panel();
            _contentPanel.Location = new Point(250, 80);
            _contentPanel.Size = new Size(this.ClientSize.Width - 250, this.ClientSize.Height - 80);
            _contentPanel.BackColor = Color.White;
            _contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(_contentPanel);
        }
        private void LoadFormIntoPanel(Form frm)
        {
            _contentPanel.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(frm);
            frm.Show();
        }


        private void Sidebar_MenuItemClicked(object sender, string e)
        {
            switch (e)
            {
                case "Dashboard":
                    ShowDashboardForm();
                    break;
                case "ProductManagement":
                    ShowProductManagementForm();
                    break;
                case "CategoryManagement":
                    ShowCategoryManagementForm();
                    break;
                case "BrandManagement":
                    ShowBrandManagementForm();
                    break;
                case "UserManagement":
                    ShowUserManagementForm();
                    break;
                case "OrderManagement":
                    ShowOrderManagementForm();
                    break;
                case "BannerManagement":
                    ShowBannerForm();
                    break;
                case "Blog":
                    ShowBlogForm();
                    break;
                case "Contact":
                    ShowContactForm();
                    break;
                case "Logout":
                    this.Close(); // đóng MainForm
                    break;
            }
        }

        private void ShowDashboardForm()
        {
            _headerControl.Title = "Bảng điều khiển";

            if (_dashboardForm == null)
                _dashboardForm = new DashboardForm();

            LoadFormIntoPanel(_dashboardForm);
        }


        private void ShowProductManagementForm()
        {
            _headerControl.Title = "Quản lý sản phẩm";

            if (_productManagementForm == null)
                _productManagementForm = new ProductManagementForm();

            LoadFormIntoPanel(_productManagementForm);
        }

        private void ShowCategoryManagementForm()
        {
            _headerControl.Title = "Quản lý danh mục";

            if (_categoryManagementForm == null)
                _categoryManagementForm = new CategoryManagementForm();
            LoadFormIntoPanel(_categoryManagementForm);
        }

        private void ShowBrandManagementForm()
        {
            _headerControl.Title = "Quản lý thương hiệu";

            if (_brandManagementForm == null)
                _brandManagementForm = new BrandManagementForm();
            LoadFormIntoPanel(_brandManagementForm);
        }

        private void ShowUserManagementForm()
        {
            _headerControl.Title = "Quản lý người dùng";

            if (_userManagementForm == null)
                _userManagementForm = new UserManagementForm();
            LoadFormIntoPanel(_userManagementForm);
        }

        private void ShowOrderManagementForm()
        {
            _headerControl.Title = "Quản lý đơn hàng";

            if (_orderManagementForm == null)
                _orderManagementForm = new OrderManagementForm();
            LoadFormIntoPanel(_orderManagementForm);
        }

        private void ShowBannerForm()
        {
            _headerControl.Title = "Quản lý banner";

            if (_bannerForm == null)
                _bannerForm = new BannerForm();
            LoadFormIntoPanel(_bannerForm);
        }

        private void ShowBlogForm()
        {
            _headerControl.Title = "Quản lý bài viết";

            if (_blogForm == null)
                _blogForm = new BlogManagementForm();
            LoadFormIntoPanel(_blogForm);
        }

        private void ShowContactForm()
        {
            _headerControl.Title = "Liên hệ";

            if (_contactForm == null)
                _contactForm = new ContactForm();
            LoadFormIntoPanel(_contactForm);
        }

    }
}