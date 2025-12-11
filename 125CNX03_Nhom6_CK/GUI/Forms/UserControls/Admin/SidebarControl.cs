using System;
using System.Drawing;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.Admin
{
    public partial class SidebarControl : UserControl
    {
        public event EventHandler<string> MenuItemClicked;

        public SidebarControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(250, 800);
            this.BackColor = Color.FromArgb(33, 33, 33);

            // Logo
            Label logoLabel = new Label();
            logoLabel.Text = "NHÓM 6";
            logoLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            logoLabel.ForeColor = Color.FromArgb(0, 174, 219);
            logoLabel.Location = new Point(20, 20);
            logoLabel.Size = new Size(200, 40);
            this.Controls.Add(logoLabel);

            // Menu items
            AddMenuItem("Dashboard", "Dashboard");
            AddMenuItem("Quản lý sản phẩm", "ProductManagement");
            AddMenuItem("Quản lý danh mục", "CategoryManagement");
            AddMenuItem("Quản lý thương hiệu", "BrandManagement");
            AddMenuItem("Quản lý người dùng", "UserManagement");
            AddMenuItem("Quản lý đơn hàng", "OrderManagement");
            AddMenuItem("Quản lý banner", "BannerManagement");
            AddMenuItem("Bài viết", "Blog");
            AddMenuItem("Liên hệ", "Contact");
            AddMenuItem("Giỏ hàng", "Cart");
        }

        private void AddMenuItem(string text, string tag)
        {
            Button menuItem = new Button();
            menuItem.Text = text;
            menuItem.Tag = tag;
            menuItem.Font = new Font("Segoe UI", 10);
            menuItem.Size = new Size(this.Width - 20, 40);
            menuItem.Location = new Point(10, this.Controls.Count * 45);
            menuItem.BackColor = Color.FromArgb(33, 33, 33);
            menuItem.ForeColor = Color.White;
            menuItem.FlatStyle = FlatStyle.Flat;
            menuItem.FlatAppearance.BorderSize = 0;
            menuItem.Cursor = Cursors.Hand;
            menuItem.Click += MenuItem_Click;
            this.Controls.Add(menuItem);
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                MenuItemClicked?.Invoke(this, button.Tag.ToString());
            }
        }
    }
}