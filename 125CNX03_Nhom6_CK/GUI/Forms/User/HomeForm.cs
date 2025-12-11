using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class HomeForm : Form
    {
        private readonly ISanPhamService _productService;

        public HomeForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Trang chủ";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create banner section
            Panel bannerPanel = new Panel();
            bannerPanel.Size = new Size(this.Width - 40, 300);
            bannerPanel.Location = new Point(20, 20);
            bannerPanel.BackColor = Color.FromArgb(245, 245, 245);
            bannerPanel.BorderStyle = BorderStyle.FixedSingle;

            // Add banner image
            PictureBox bannerImage = new PictureBox();
            bannerImage.Size = new Size(bannerPanel.Width - 40, 250);
            bannerImage.Location = new Point(20, 20);
            bannerImage.SizeMode = PictureBoxSizeMode.StretchImage;
            bannerImage.BackColor = Color.FromArgb(245, 245, 245);
            bannerImage.Image = Properties.Resources.DefaultBanner; // You should add a default banner image
            bannerPanel.Controls.Add(bannerImage);

            this.Controls.Add(bannerPanel);

            // Create featured products section
            Panel featuredPanel = new Panel();
            featuredPanel.Size = new Size(this.Width - 40, 400);
            featuredPanel.Location = new Point(20, 340);
            featuredPanel.BackColor = Color.White;
            featuredPanel.BorderStyle = BorderStyle.FixedSingle;

            Label featuredTitle = new Label();
            featuredTitle.Text = "Sản phẩm nổi bật";
            featuredTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            featuredTitle.Location = new Point(20, 20);
            featuredTitle.Size = new Size(200, 30);
            featuredPanel.Controls.Add(featuredTitle);

            // Create flow layout for featured products
            FlowLayoutPanel productPanel = new FlowLayoutPanel();
            productPanel.Size = new Size(featuredPanel.Width - 40, featuredPanel.Height - 60);
            productPanel.Location = new Point(20, 60);
            productPanel.FlowDirection = FlowDirection.LeftToRight;
            productPanel.WrapContents = false;
            productPanel.AutoScroll = true;
            productPanel.BackColor = Color.White;

            // Add sample products
            var products = _productService.GetAllProducts().Take(6).ToList();
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

            featuredPanel.Controls.Add(productPanel);

            this.Controls.Add(featuredPanel);
        }

        private void LoadData()
        {
            // Data is loaded in InitializeUI
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