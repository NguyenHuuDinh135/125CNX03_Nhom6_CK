using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProductCatalogForm : Form
    {
        private readonly ISanPhamService _productService;

        public ProductCatalogForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Danh mục sản phẩm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 800);
            this.BackColor = Color.White;

            // Create search bar
            Panel searchBarPanel = new Panel();
            searchBarPanel.Size = new Size(this.Width - 40, 60);
            searchBarPanel.Location = new Point(20, 20);
            searchBarPanel.BackColor = Color.White;
            searchBarPanel.BorderStyle = BorderStyle.FixedSingle;

            Label searchLabel = new Label();
            searchLabel.Text = "Tìm kiếm:";
            searchLabel.Font = new Font("Segoe UI", 10);
            searchLabel.Location = new Point(20, 15);
            searchLabel.Size = new Size(60, 30);
            searchBarPanel.Controls.Add(searchLabel);

            TextBox txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Size = new Size(300, 30);
            txtSearch.Location = new Point(80, 15);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Padding = new Padding(5);
            txtSearch.TextChanged += (s, e) => SearchProducts(txtSearch.Text);
            searchBarPanel.Controls.Add(txtSearch);

            // Add filter buttons
            Button btnAll = new Button();
            btnAll.Text = "Tất cả";
            btnAll.Font = new Font("Segoe UI", 9);
            btnAll.Size = new Size(80, 30);
            btnAll.Location = new Point(400, 15);
            btnAll.BackColor = Color.FromArgb(245, 245, 245);
            btnAll.ForeColor = Color.FromArgb(50, 50, 50);
            btnAll.FlatStyle = FlatStyle.Flat;
            btnAll.FlatAppearance.BorderSize = 0;
            btnAll.Cursor = Cursors.Hand;
            btnAll.Click += (s, e) => FilterProducts("");
            searchBarPanel.Controls.Add(btnAll);

            Button btnLaptops = new Button();
            btnLaptops.Text = "Laptop";
            btnLaptops.Font = new Font("Segoe UI", 9);
            btnLaptops.Size = new Size(80, 30);
            btnLaptops.Location = new Point(490, 15);
            btnLaptops.BackColor = Color.FromArgb(245, 245, 245);
            btnLaptops.ForeColor = Color.FromArgb(50, 50, 50);
            btnLaptops.FlatStyle = FlatStyle.Flat;
            btnLaptops.FlatAppearance.BorderSize = 0;
            btnLaptops.Cursor = Cursors.Hand;
            btnLaptops.Click += (s, e) => FilterProducts("Laptop");
            searchBarPanel.Controls.Add(btnLaptops);

            Button btnAccessories = new Button();
            btnAccessories.Text = "Phụ kiện";
            btnAccessories.Font = new Font("Segoe UI", 9);
            btnAccessories.Size = new Size(80, 30);
            btnAccessories.Location = new Point(580, 15);
            btnAccessories.BackColor = Color.FromArgb(245, 245, 245);
            btnAccessories.ForeColor = Color.FromArgb(50, 50, 50);
            btnAccessories.FlatStyle = FlatStyle.Flat;
            btnAccessories.FlatAppearance.BorderSize = 0;
            btnAccessories.Cursor = Cursors.Hand;
            btnAccessories.Click += (s, e) => FilterProducts("Phụ kiện");
            searchBarPanel.Controls.Add(btnAccessories);

            this.Controls.Add(searchBarPanel);

            // Create product grid
            Panel productGridPanel = new Panel();
            productGridPanel.Size = new Size(this.Width - 40, this.Height - 120);
            productGridPanel.Location = new Point(20, 100);
            productGridPanel.BackColor = Color.White;
            productGridPanel.AutoScroll = true;

            // Create flow layout for products
            FlowLayoutPanel productPanel = new FlowLayoutPanel();
            productPanel.Size = new Size(productGridPanel.Width, productGridPanel.Height);
            productPanel.Location = new Point(0, 0);
            productPanel.FlowDirection = FlowDirection.TopDown;
            productPanel.WrapContents = true;
            productPanel.AutoScroll = true;
            productPanel.BackColor = Color.White;

            productGridPanel.Controls.Add(productPanel);

            this.Controls.Add(productGridPanel);
        }

        private void LoadData()
        {
            var products = _productService.GetAllProducts();
            DisplayProducts(products);
        }

        private void DisplayProducts(List<XElement> products)
        {
            // Get the product panel
            var productGridPanel = this.Controls[1] as Panel;
            if (productGridPanel == null) return;

            // Get the product panel
            var productPanel = productGridPanel.Controls[0] as FlowLayoutPanel;
            if (productPanel == null) return;

            // Clear existing items
            productPanel.Controls.Clear();

            // Add product items
            foreach (var product in products)
            {
                var productCard = new ProductCard();
                productCard.Id = product.Element("Id")?.Value ?? "0";
                productCard.Title = product.Element("TenSanPham")?.Value ?? "Sản phẩm không tên";
                productCard.Description = product.Element("MoTa")?.Value ?? "Không có mô tả";
                productCard.ImageUrl = product.Element("DuongDanAnh")?.Value ?? "";
                productCard.Price = product.Element("Gia")?.Value ?? "0";
                productCard.DiscountPrice = product.Element("GiaKhuyenMai")?.Value ?? "";
                productCard.Category = GetCategoryName(int.Parse(product.Element("MaLoai").Value));

                productCard.ItemClicked += ProductCard_ItemClicked;
                productPanel.Controls.Add(productCard);
            }
        }

        private void SearchProducts(string searchText)
        {
            var products = _productService.GetAllProducts();
            var filteredProducts = products.Where(p =>
                p.Element("TenSanPham")?.Value.Contains(searchText) == true ||
                p.Element("MoTa")?.Value.Contains(searchText) == true ||
                p.Element("ChiTiet")?.Value.Contains(searchText) == true).ToList();
            DisplayProducts(filteredProducts);
        }

        private void FilterProducts(string category)
        {
            var products = _productService.GetAllProducts();
            if (!string.IsNullOrEmpty(category))
            {
                var categoryService = new LoaiSanPhamService();
                var categoryId = products.FirstOrDefault(p =>
                    categoryService.GetCategoryById(int.Parse(p.Element("MaLoai").Value)).Element("TenLoai")?.Value == category)?.Element("MaLoai")?.Value;

                if (!string.IsNullOrEmpty(categoryId))
                {
                    products = products.Where(p => p.Element("MaLoai")?.Value == categoryId).ToList();
                }
            }
            DisplayProducts(products);
        }

        private string GetCategoryName(int categoryId)
        {
            var categoryService = new LoaiSanPhamService();
            var category = categoryService.GetCategoryById(categoryId);
            return category?.Element("TenLoai")?.Value ?? "N/A";
        }

        private void ProductCard_ItemClicked(object sender, string e)
        {
            // Show product detail form
            var productDetailForm = new ProductDetailForm(e);
            productDetailForm.MdiParent = this.MdiParent;
            productDetailForm.Show();
        }
    }
}