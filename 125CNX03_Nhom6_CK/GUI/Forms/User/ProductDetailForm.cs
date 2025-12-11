using System;
using System.Drawing;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.BLL;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProductDetailForm : Form
    {
        private readonly ISanPhamService _productService;
        private XElement _currentProduct;

        public ProductDetailForm(string productId)
        {
            InitializeComponent();
            _productService = new SanPhamService();
            LoadProductDetails(productId);
        }

        private void LoadProductDetails(string productId)
        {
            _currentProduct = _productService.GetProductById(int.Parse(productId));
            if (_currentProduct != null)
            {
                // Set form properties
                this.Text = $"Chi tiết sản phẩm: {_currentProduct.Element("TenSanPham").Value}";
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Size = new Size(800, 600);
                this.BackColor = Color.White;

                InitializeUI();
            }
        }

        private void InitializeUI()
        {
            // Create close button
            Button btnClose = new Button();
            btnClose.Text = "×";
            btnClose.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnClose.Size = new Size(30, 30);
            btnClose.Location = new Point(760, 10);
            btnClose.BackColor = Color.Transparent;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.ForeColor = Color.FromArgb(100, 100, 100);
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Create main panel
            Panel mainPanel = new Panel();
            mainPanel.Size = new Size(780, 580);
            mainPanel.Location = new Point(10, 50);
            mainPanel.BackColor = Color.White;
            mainPanel.BorderStyle = BorderStyle.FixedSingle;

            // Product image
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(200, 200);
            pictureBox.Location = new Point(20, 20);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.BackColor = Color.FromArgb(245, 245, 245);
            if (!string.IsNullOrEmpty(_currentProduct.Element("DuongDanAnh")?.Value))
            {
                try
                {
                    pictureBox.LoadAsync(_currentProduct.Element("DuongDanAnh").Value);
                }
                catch
                {
                    pictureBox.Image = Properties.Resources.DefaultProductImage;
                }
            }
            else
            {
                pictureBox.Image = Properties.Resources.DefaultProductImage;
            }
            mainPanel.Controls.Add(pictureBox);

            // Product details
            Label titleLabel = new Label();
            titleLabel.Text = _currentProduct.Element("TenSanPham").Value;
            titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titleLabel.Location = new Point(240, 20);
            titleLabel.Size = new Size(500, 30);
            mainPanel.Controls.Add(titleLabel);

            Label descriptionLabel = new Label();
            descriptionLabel.Text = _currentProduct.Element("MoTa").Value;
            descriptionLabel.Font = new Font("Segoe UI", 10);
            descriptionLabel.Location = new Point(240, 60);
            descriptionLabel.Size = new Size(500, 80);
            descriptionLabel.MaximumSize = new Size(500, 80);
            descriptionLabel.AutoEllipsis = true;
            mainPanel.Controls.Add(descriptionLabel);

            Label priceLabel = new Label();
            priceLabel.Text = $"Giá: {_currentProduct.Element("Gia").Value}đ";
            priceLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            priceLabel.Location = new Point(240, 150);
            priceLabel.Size = new Size(500, 20);
            mainPanel.Controls.Add(priceLabel);

            if (!string.IsNullOrEmpty(_currentProduct.Element("GiaKhuyenMai")?.Value) &&
                decimal.Parse(_currentProduct.Element("GiaKhuyenMai").Value) > 0)
            {
                Label discountPriceLabel = new Label();
                discountPriceLabel.Text = $"Giá khuyến mãi: {_currentProduct.Element("GiaKhuyenMai").Value}đ";
                discountPriceLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                discountPriceLabel.ForeColor = Color.Red;
                discountPriceLabel.Location = new Point(240, 180);
                discountPriceLabel.Size = new Size(500, 20);
                mainPanel.Controls.Add(discountPriceLabel);
            }

            Label stockLabel = new Label();
            stockLabel.Text = $"Số lượng tồn: {_currentProduct.Element("SoLuongTon").Value}";
            stockLabel.Font = new Font("Segoe UI", 10);
            stockLabel.Location = new Point(240, 210);
            stockLabel.Size = new Size(500, 20);
            mainPanel.Controls.Add(stockLabel);

            // Category and brand
            Label categoryLabel = new Label();
            categoryLabel.Text = $"Danh mục: {GetCategoryName(int.Parse(_currentProduct.Element("MaLoai").Value))}";
            categoryLabel.Font = new Font("Segoe UI", 10);
            categoryLabel.Location = new Point(240, 240);
            categoryLabel.Size = new Size(500, 20);
            mainPanel.Controls.Add(categoryLabel);

            Label brandLabel = new Label();
            brandLabel.Text = $"Thương hiệu: {GetBrandName(int.Parse(_currentProduct.Element("MaThuongHieu").Value))}";
            brandLabel.Font = new Font("Segoe UI", 10);
            brandLabel.Location = new Point(240, 270);
            brandLabel.Size = new Size(500, 20);
            mainPanel.Controls.Add(brandLabel);

            // Add to cart button
            Button btnAddToCart = new Button();
            btnAddToCart.Text = "Thêm vào giỏ hàng";
            btnAddToCart.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAddToCart.Size = new Size(200, 30);
            btnAddToCart.Location = new Point(240, 310);
            btnAddToCart.BackColor = Color.FromArgb(0, 174, 219);
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.FlatStyle = FlatStyle.Flat;
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Cursor = Cursors.Hand;
            btnAddToCart.Click += BtnAddToCart_Click;
            mainPanel.Controls.Add(btnAddToCart);

            this.Controls.Add(mainPanel);
        }

        private string GetCategoryName(int categoryId)
        {
            var categoryService = new LoaiSanPhamService();
            var category = categoryService.GetCategoryById(categoryId);
            return category?.Element("TenLoai")?.Value ?? "N/A";
        }

        private string GetBrandName(int brandId)
        {
            var brandService = new ThuongHieuService();
            var brand = brandService.GetBrandById(brandId);
            return brand?.Element("TenThuongHieu")?.Value ?? "N/A";
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            // Add to cart logic
            MessageBox.Show($"Đã thêm {_currentProduct.Element("TenSanPham").Value} vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}