using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProductCatalogForm : UserBaseForm
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IGioHangService _cartService;

        private FlowLayoutPanel _productPanel;
        private ComboBox _cboCategory;
        private TextBox _txtSearch;

        public ProductCatalogForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();
            _categoryService = new LoaiSanPhamService();
            _cartService = new GioHangService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Danh mục sản phẩm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 800);

            // Search + filter panel
            Panel searchBarPanel = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));

            var lblSearch = new Label { Text = "Tìm kiếm:", Location = new Point(10, 18), Size = new Size(60, 24) };
            searchBarPanel.Controls.Add(lblSearch);

            _txtSearch = new TextBox { Location = new Point(80, 16), Size = new Size(360, 28) };
            _txtSearch.TextChanged += (s, e) => ApplyFilters();
            searchBarPanel.Controls.Add(_txtSearch);

            _cboCategory = new ComboBox { Location = new Point(460, 16), Size = new Size(200, 28), DropDownStyle = ComboBoxStyle.DropDownList };
            _cboCategory.SelectedIndexChanged += (s, e) => ApplyFilters();
            searchBarPanel.Controls.Add(_cboCategory);

            var btnClear = CreateButton("Xóa filter", new Point(680, 14), new Size(100, 32), Primary, (s, e) => { _txtSearch.Clear(); _cboCategory.SelectedIndex = 0; });
            searchBarPanel.Controls.Add(btnClear);

            this.Controls.Add(searchBarPanel);

            // Product grid panel
            Panel productGridPanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, this.Height - 140));
            productGridPanel.AutoScroll = true;

            _productPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 10),
                Size = new Size(productGridPanel.Width - 20, productGridPanel.Height - 20),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                BackColor = Surface
            };

            productGridPanel.Controls.Add(_productPanel);
            this.Controls.Add(productGridPanel);

            // Load categories into cbo
            var categories = _categoryService.GetAllCategories();
            _cboCategory.Items.Add("Tất cả");
            foreach (var c in categories)
            {
                _cboCategory.Items.Add(c.Element("TenLoai")?.Value ?? "N/A");
            }
            _cboCategory.SelectedIndex = 0;
        }

        private void LoadData()
        {
            var products = _productService.GetAllProducts();
            DisplayProducts(products);
        }

        private void ApplyFilters()
        {
            var all = _productService.GetAllProducts();
            var search = _txtSearch.Text.Trim();
            var category = _cboCategory.SelectedItem?.ToString();

            var filtered = all.Where(p =>
                (string.IsNullOrEmpty(search) || (p.Element("TenSanPham")?.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) || (p.Element("MoTa")?.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) || (p.Element("ChiTiet")?.Value.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0))
            ).ToList();

            if (!string.IsNullOrEmpty(category) && category != "Tất cả")
            {
                var cat = _categoryService.GetAllCategories().FirstOrDefault(c => (c.Element("TenLoai")?.Value ?? "") == category);
                if (cat != null)
                {
                    var catId = cat.Element("MaLoai")?.Value;
                    filtered = filtered.Where(p => p.Element("MaLoai")?.Value == catId).ToList();
                }
            }

            DisplayProducts(filtered);
        }

        private void DisplayProducts(List<XElement> products)
        {
            _productPanel.Controls.Clear();

            foreach (var product in products)
            {
                var productCard = new ProductCard();
                var id = product.Element("Id")?.Value ?? "0";
                productCard.UpdateData(id,
                    product.Element("TenSanPham")?.Value ?? "Sản phẩm không tên",
                    product.Element("MoTa")?.Value ?? "",
                    product.Element("DuongDanAnh")?.Value ?? "",
                    product.Element("Gia")?.Value ?? "0",
                    product.Element("GiaKhuyenMai")?.Value ?? "0",
                    GetCategoryName(int.Parse(product.Element("MaLoai")?.Value ?? "0")));

                productCard.ItemClicked += ProductCard_ItemClicked;
                productCard.AddToCartClicked += ProductCard_AddToCartClicked;
                _productPanel.Controls.Add(productCard);
            }
        }

        private string GetCategoryName(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            return category?.Element("TenLoai")?.Value ?? "N/A";
        }

        // Đã sửa: thay đổi từ (object sender, string e) thành (object sender, ProductEventArgs e)
        private void ProductCard_ItemClicked(object sender, ProductEventArgs e)
        {
            var productDetailForm = new ProductDetailForm(e.ProductId);
            productDetailForm.MdiParent = this.MdiParent;
            productDetailForm.Show();
        }

        // Đã sửa: thay đổi từ (object sender, string productIdStr) thành (object sender, ProductEventArgs e)
        private void ProductCard_AddToCartClicked(object sender, ProductEventArgs e)
        {
            int productId = int.Parse(e.ProductId);

            // Get current user from main form MdiParent if available
            var main = this.MdiParent as MainForm;
            int userId = 0;
            if (main != null)
            {
                // Attempt to get current user id from MainForm private field via reflection is not ideal.
                // For simplicity, try to find a public property CurrentUser in MainForm (if exists)
                var userField = main.GetType().GetProperty("CurrentUser");
                if (userField != null)
                {
                    var xu = userField.GetValue(main) as XElement;
                    if (xu != null) int.TryParse(xu.Element("Id")?.Value, out userId);
                }
            }

            if (userId <= 0)
            {
                // Fallback: ask user to login
                MessageBox.Show("Vui lòng đăng nhập để thêm vào giỏ hàng.", "Yêu cầu đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _cartService.AddProductToCart(userId, productId, 1);
                MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm vào giỏ hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}