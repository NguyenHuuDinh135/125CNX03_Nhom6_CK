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
        private List<XElement> _allProducts;
        private List<XElement> _allCategories;

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

            var lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Location = new Point(10, 18),
                Size = new Size(80, 24),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            searchBarPanel.Controls.Add(lblSearch);

            _txtSearch = new TextBox
            {
                Location = new Point(100, 16),
                Size = new Size(300, 28),
                Font = new Font("Segoe UI", 10),
            };
            _txtSearch.TextChanged += (s, e) => ApplyFilters();
            searchBarPanel.Controls.Add(_txtSearch);

            var lblCategory = new Label
            {
                Text = "Danh mục:",
                Location = new Point(420, 18),
                Size = new Size(90, 24),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            searchBarPanel.Controls.Add(lblCategory);

            _cboCategory = new ComboBox
            {
                Location = new Point(520, 16),
                Size = new Size(200, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            _cboCategory.SelectedIndexChanged += (s, e) => ApplyFilters();
            searchBarPanel.Controls.Add(_cboCategory);

            var btnClear = CreateButton(
                "🔄 Xóa bộ lọc",
                new Point(740, 14),
                new Size(120, 32),
                Primary,
                (s, e) => {
                    _txtSearch.Clear();
                    _cboCategory.SelectedIndex = 0;
                }
            );
            searchBarPanel.Controls.Add(btnClear);

            this.Controls.Add(searchBarPanel);

            // Product grid panel
            Panel productGridPanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, this.Height - 140));
            productGridPanel.AutoScroll = true;

            _productPanel = new FlowLayoutPanel
            {
                Location = new Point(10, 10),
                Size = new Size(productGridPanel.Width - 30, productGridPanel.Height - 20),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                BackColor = Surface
            };

            productGridPanel.Controls.Add(_productPanel);
            this.Controls.Add(productGridPanel);
        }

        private void LoadData()
        {
            try
            {
                // Load products
                _allProducts = _productService.GetAllProducts();

                // Load categories
                _allCategories = _categoryService.GetAllCategories();

                // Populate category combobox
                _cboCategory.Items.Clear();
                _cboCategory.Items.Add(new ComboBoxItem { Text = "Tất cả danh mục", Value = -1 });

                foreach (var c in _allCategories)
                {
                    var catId = int.Parse(c.Element("MaLoai")?.Value ?? "0");
                    var catName = c.Element("TenLoai")?.Value ?? "N/A";
                    _cboCategory.Items.Add(new ComboBoxItem { Text = catName, Value = catId });
                }

                _cboCategory.DisplayMember = "Text";
                _cboCategory.ValueMember = "Value";
                _cboCategory.SelectedIndex = 0;

                DisplayProducts(_allProducts);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            try
            {
                var filtered = _allProducts.AsEnumerable();

                // Filter by search text
                var searchText = _txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    filtered = filtered.Where(p =>
                        (p.Element("TenSanPham")?.Value?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (p.Element("MoTa")?.Value?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (p.Element("ChiTiet")?.Value?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    );
                }

                // Filter by category
                if (_cboCategory.SelectedItem != null && _cboCategory.SelectedItem is ComboBoxItem selectedItem)
                {
                    int categoryId = selectedItem.Value;

                    // Nếu không phải "Tất cả" thì lọc theo category
                    if (categoryId != -1)
                    {
                        filtered = filtered.Where(p =>
                        {
                            var productCatId = int.Parse(p.Element("MaLoai")?.Value ?? "0");
                            return productCatId == categoryId;
                        });
                    }
                }

                DisplayProducts(filtered.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayProducts(List<XElement> products)
        {
            _productPanel.Controls.Clear();

            if (products == null || !products.Any())
            {
                Label emptyLabel = new Label
                {
                    Text = "Không tìm thấy sản phẩm nào",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true
                };
                _productPanel.Controls.Add(emptyLabel);
                return;
            }

            foreach (var product in products)
            {
                var productCard = new ProductCard();
                var id = product.Element("Id")?.Value ?? "0";
                productCard.UpdateData(
                    id,
                    product.Element("TenSanPham")?.Value ?? "Sản phẩm không tên",
                    product.Element("MoTa")?.Value ?? "",
                    product.Element("DuongDanAnh")?.Value ?? "",
                    product.Element("Gia")?.Value ?? "0",
                    product.Element("GiaKhuyenMai")?.Value ?? "0",
                    GetCategoryName(int.Parse(product.Element("MaLoai")?.Value ?? "0"))
                );

                productCard.ItemClicked += ProductCard_ItemClicked;
                productCard.AddToCartClicked += ProductCard_AddToCartClicked;
                _productPanel.Controls.Add(productCard);
            }
        }

        private string GetCategoryName(int categoryId)
        {
            var category = _allCategories?.FirstOrDefault(c =>
                int.Parse(c.Element("MaLoai")?.Value ?? "0") == categoryId
            );
            return category?.Element("TenLoai")?.Value ?? "N/A";
        }

        private void ProductCard_ItemClicked(object sender, ProductEventArgs e)
        {
            var productDetailForm = new ProductDetailForm(e.ProductId);
            productDetailForm.MdiParent = this.MdiParent;
            productDetailForm.Show();
        }

        private void ProductCard_AddToCartClicked(object sender, ProductEventArgs e)
        {
            int productId = int.Parse(e.ProductId);

            // Get current user from MainForm
            var main = this.MdiParent as MainForm;
            XElement currentUser = null;

            if (main != null)
            {
                var userProperty = main.GetType().GetProperty("CurrentUser");
                if (userProperty != null)
                {
                    currentUser = userProperty.GetValue(main) as XElement;
                }
            }

            if (currentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để thêm vào giỏ hàng.", "Yêu cầu đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int userId = int.Parse(currentUser.Element("Id")?.Value ?? "0");
                _cartService.AddProductToCart(userId, productId, 1);

                // Refresh cart count
                if (main != null)
                {
                    var refreshMethod = main.GetType().GetMethod("RefreshCartCount");
                    refreshMethod?.Invoke(main, null);
                }

                MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm vào giỏ hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper class for ComboBox items
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}