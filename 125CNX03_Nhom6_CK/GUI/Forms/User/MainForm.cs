using System;
using System.Drawing;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.GUI.UserControls.User;
using _125CNX03_Nhom6_CK.GUI.Forms.User;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class MainForm : UserBaseForm
    {
        private HomeForm _homeForm;
        private ProductCatalogForm _productCatalogForm;
        private CartForm _cartForm;
        private OrderHistoryForm _orderHistoryForm;
        private ProfileForm _profileForm;
        private BlogForm _blogForm;
        private ContactForm _contactForm;
        private HeaderControl _header;
        private SidebarControl _sidebar;
        private XElement _currentUser;

        private readonly IGioHangService _cartService = new GioHangService();

        public XElement CurrentUser => _currentUser;

        public MainForm(XElement user)
        {
            InitializeComponent();
            _currentUser = user;

            if (_currentUser == null)
            {
                var login = new _125CNX03_Nhom6_CK.GUI.Forms.LoginForm();
                login.ShowDialog();
                this.Close();
                return;
            }

            SetupUI();
            ShowHomeForm();
            RefreshCartCount();
        }

        private void SetupUI()
        {
            this.Text = "Cửa hàng bán laptop - Người dùng";
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            this.BackColor = ColorTranslator.FromHtml("#f5f5f5");

            // Create header first (it will be at the top)
            _header = new HeaderControl();
            _header.Dock = DockStyle.Top;
            _header.NavClicked += Header_NavClicked;
            _header.CartClicked += (s, e) => ShowCartForm();
            _header.LogoutClicked += (s, e) => DoLogout();
            _header.SearchTextChanged += (s, q) => OnSearch(q);
            this.Controls.Add(_header);

            // Create sidebar
            _sidebar = new SidebarControl();
            _sidebar.Dock = DockStyle.Left;
            _sidebar.MenuItemClicked += Sidebar_MenuItemClicked;
            this.Controls.Add(_sidebar);

            // Configure MDI Client
            MdiClient mdiClient = null;
            foreach (Control c in this.Controls)
            {
                if (c is MdiClient)
                {
                    mdiClient = (MdiClient)c;
                    break;
                }
            }

            if (mdiClient != null)
            {
                mdiClient.BackColor = ColorTranslator.FromHtml("#f5f5f5");
                mdiClient.Dock = DockStyle.Fill;
            }

            // Ensure header and sidebar are on top
            _header.BringToFront();
            _sidebar.BringToFront();
        }

        public void RefreshCartCount()
        {
            try
            {
                if (_currentUser == null) return;
                int userId = int.Parse(_currentUser.Element("Id").Value);
                var cart = _cartService.GetCartByUserId(userId);
                if (cart == null)
                {
                    _cartService.CreateCartForUser(userId);
                    cart = _cartService.GetCartByUserId(userId);
                }
                int cartId = int.Parse(cart.Element("Id").Value);
                int count = _cartService.GetCartItemCount(cartId);
                _header?.UpdateCartCount(count);
            }
            catch { }
        }

        private void OnSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return;
            ShowProductCatalogForm();

            if (_productCatalogForm != null && !_productCatalogForm.IsDisposed)
            {
                var field = _productCatalogForm.GetType().GetField("_txtSearch",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    var tb = field.GetValue(_productCatalogForm) as TextBox;
                    if (tb != null) tb.Text = query;
                }
            }
        }

        private void Header_NavClicked(object sender, string e)
        {
            switch (e)
            {
                case "Home":
                    ShowHomeForm();
                    break;
                case "Blog":
                    ShowBlogForm();
                    break;
                case "Contact":
                    ShowContactForm();
                    break;
                case "Profile":
                    ShowProfileForm();
                    break;
                case "OrderHistory":
                    ShowOrderHistoryForm();
                    break;
            }
        }

        private void DoLogout()
        {
            this.Hide();
            var login = new _125CNX03_Nhom6_CK.GUI.Forms.LoginForm();
            login.ShowDialog();
            this.Close();
        }

        private void Sidebar_MenuItemClicked(object sender, string e)
        {
            switch (e)
            {
                case "Home":
                    ShowHomeForm();
                    break;
                case "Danh mục":
                case "Sản phẩm":
                case "ProductCatalog":
                    ShowProductCatalogForm();
                    break;
                case "Blog":
                    ShowBlogForm();
                    break;
                case "Liên hệ":
                case "Contact":
                    ShowContactForm();
                    break;
                case "Giỏ hàng":
                case "Cart":
                    ShowCartForm();
                    break;
                case "Logout":
                    DoLogout();
                    break;
                default:
                    ShowProductCatalogForm();
                    break;
            }
        }

        private void ShowHomeForm()
        {
            CloseAllChildForms();

            if (_homeForm == null || _homeForm.IsDisposed)
            {
                _homeForm = new HomeForm();
                _homeForm.MdiParent = this;
                _homeForm.FormClosed += ChildForm_FormClosed;
                _homeForm.Show();
            }
            else
            {
                _homeForm.Activate();
            }
        }

        private void ShowProductCatalogForm()
        {
            CloseAllChildForms();

            if (_productCatalogForm == null || _productCatalogForm.IsDisposed)
            {
                _productCatalogForm = new ProductCatalogForm();
                _productCatalogForm.MdiParent = this;
                _productCatalogForm.FormClosed += ChildForm_FormClosed;
                _productCatalogForm.Show();
            }
            else
            {
                _productCatalogForm.Activate();
            }
        }

        private void ShowCartForm()
        {
            CloseAllChildForms();

            if (_cartForm == null || _cartForm.IsDisposed)
            {
                _cartForm = new CartForm(_currentUser);
                _cartForm.MdiParent = this;
                _cartForm.FormClosed += ChildForm_FormClosed;
                _cartForm.Show();
            }
            else
            {
                _cartForm.Activate();
            }
        }

        private void ShowOrderHistoryForm()
        {
            CloseAllChildForms();

            if (_orderHistoryForm == null || _orderHistoryForm.IsDisposed)
            {
                _orderHistoryForm = new OrderHistoryForm(_currentUser);
                _orderHistoryForm.MdiParent = this;
                _orderHistoryForm.FormClosed += ChildForm_FormClosed;
                _orderHistoryForm.Show();
            }
            else
            {
                _orderHistoryForm.Activate();
            }
        }

        private void ShowProfileForm()
        {
            CloseAllChildForms();

            if (_profileForm == null || _profileForm.IsDisposed)
            {
                _profileForm = new ProfileForm(_currentUser);
                _profileForm.MdiParent = this;
                _profileForm.FormClosed += ChildForm_FormClosed;
                _profileForm.Show();
            }
            else
            {
                _profileForm.Activate();
            }
        }

        private void ShowBlogForm()
        {
            CloseAllChildForms();

            if (_blogForm == null || _blogForm.IsDisposed)
            {
                _blogForm = new BlogForm();
                _blogForm.MdiParent = this;
                _blogForm.FormClosed += ChildForm_FormClosed;
                _blogForm.Show();
            }
            else
            {
                _blogForm.Activate();
            }
        }

        private void ShowContactForm()
        {
            CloseAllChildForms();

            if (_contactForm == null || _contactForm.IsDisposed)
            {
                _contactForm = new ContactForm();
                _contactForm.MdiParent = this;
                _contactForm.FormClosed += ChildForm_FormClosed;
                _contactForm.Show();
            }
            else
            {
                _contactForm.Activate();
            }
        }

        private void CloseAllChildForms()
        {
            // Close all MDI children except the one we're about to show
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm != _homeForm && childForm != _productCatalogForm &&
                    childForm != _cartForm && childForm != _orderHistoryForm &&
                    childForm != _profileForm && childForm != _blogForm &&
                    childForm != _contactForm)
                {
                    childForm.Close();
                }
            }
        }

        private void ChildForm_FormClosed(object sender, FormClosedEventArgs e)
        {
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
            else if (sender == _blogForm)
                _blogForm = null;
            else if (sender == _contactForm)
                _contactForm = null;
        }
    }
}