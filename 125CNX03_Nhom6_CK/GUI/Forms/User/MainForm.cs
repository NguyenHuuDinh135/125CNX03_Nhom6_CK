using System;
using System.Drawing;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.GUI.UserControls.Admin;
using _125CNX03_Nhom6_CK.GUI.Forms.User;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class MainForm : Form
    {
        private HomeForm _homeForm;
        private ProductCatalogForm _productCatalogForm;
        private CartForm _cartForm;
        private OrderHistoryForm _orderHistoryForm;
        private ProfileForm _profileForm;
        private XElement _currentUser;

        public MainForm(XElement user)
        {
            InitializeComponent();
            _currentUser = user;
            SetupUI();
            ShowHomeForm();
        }

        private void SetupUI()
        {
            this.Text = "Cửa hàng bán laptop - Người dùng";
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.BackColor = Color.White;

            // Create sidebar
            SidebarControl sidebar = new SidebarControl();
            sidebar.Location = new Point(0, 0);
            sidebar.Size = new Size(250, this.ClientSize.Height);
            sidebar.MenuItemClicked += Sidebar_MenuItemClicked;
            this.Controls.Add(sidebar);

            // Create main panel
            Panel mainPanel = new Panel();
            mainPanel.Size = new Size(this.ClientSize.Width - 250, this.ClientSize.Height);
            mainPanel.Location = new Point(250, 0);
            mainPanel.BackColor = Color.White;
            mainPanel.Dock = DockStyle.Right;

            // Create header
            HeaderControl header = new HeaderControl();
            header.Location = new Point(0, 0);
            header.Size = new Size(mainPanel.Width, 80);
            header.Title = "Trang chủ - Người dùng";
            mainPanel.Controls.Add(header);

            this.Controls.Add(mainPanel);
        }

        private void Sidebar_MenuItemClicked(object sender, string e)
        {
            switch (e)
            {
                case "Home":
                    ShowHomeForm();
                    break;
                case "ProductCatalog":
                    ShowProductCatalogForm();
                    break;
                case "Cart":
                    ShowCartForm();
                    break;
                case "OrderHistory":
                    ShowOrderHistoryForm();
                    break;
                case "Profile":
                    ShowProfileForm();
                    break;
            }
        }

        private void ShowHomeForm()
        {
            if (_homeForm == null)
            {
                _homeForm = new HomeForm();
                _homeForm.MdiParent = this;
                _homeForm.FormClosed += ChildForm_FormClosed;
            }
            else
            {
                _homeForm.Activate();
            }
            _homeForm.Show();
        }

        private void ShowProductCatalogForm()
        {
            if (_productCatalogForm == null)
            {
                _productCatalogForm = new ProductCatalogForm();
                _productCatalogForm.MdiParent = this;
                _productCatalogForm.FormClosed += ChildForm_FormClosed;
            }
            else
            {
                _productCatalogForm.Activate();
            }
            _productCatalogForm.Show();
        }

        private void ShowCartForm()
        {
            if (_cartForm == null)
            {
                _cartForm = new CartForm();
                _cartForm.MdiParent = this;
                _cartForm.FormClosed += ChildForm_FormClosed;
            }
            else
            {
                _cartForm.Activate();
            }
            _cartForm.Show();
        }

        private void ShowOrderHistoryForm()
        {
            if (_orderHistoryForm == null)
            {
                _orderHistoryForm = new OrderHistoryForm(_currentUser);
                _orderHistoryForm.MdiParent = this;
                _orderHistoryForm.FormClosed += ChildForm_FormClosed;
            }
            else
            {
                _orderHistoryForm.Activate();
            }
            _orderHistoryForm.Show();
        }

        private void ShowProfileForm()
        {
            if (_profileForm == null)
            {
                _profileForm = new ProfileForm(_currentUser);
                _profileForm.MdiParent = this;
                _profileForm.FormClosed += ChildForm_FormClosed;
            }
            else
            {
                _profileForm.Activate();
            }
            _profileForm.Show();
        }

        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Set the form reference to null when closed
            if (sender == _homeForm)
                _homeForm = null;
            else if (sender == _productCatalogForm)
                _productCatalogForm = null;
            else if (sender == _cartForm)
                _cartForm = null;
            else if (sender == _orderHistoryForm)
                _orderHistoryForm = null;
            else if (sender == _profileForm)
                _profileForm = null;
        }
    }
}