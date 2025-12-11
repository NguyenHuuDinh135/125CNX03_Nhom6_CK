using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class ProductCard : UserControl
    {
        public event EventHandler<string> ItemClicked;
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Price { get; set; }
        public string DiscountPrice { get; set; }
        public string Category { get; set; }

        public ProductCard()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(200, 300);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Image placeholder
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(200, 150);
            pictureBox.Location = new Point(0, 0);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.BackColor = Color.FromArgb(245, 245, 245);
            pictureBox.Click += (s, e) => ItemClicked?.Invoke(this, Id);
            this.Controls.Add(pictureBox);

            // Content panel
            Panel contentPanel = new Panel();
            contentPanel.Size = new Size(200, 150);
            contentPanel.Location = new Point(0, 150);
            contentPanel.BackColor = Color.White;

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = Title ?? "Tiêu đề sản phẩm";
            titleLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            titleLabel.Location = new Point(10, 5);
            titleLabel.Size = new Size(180, 30);
            titleLabel.MaximumSize = new Size(180, 30);
            titleLabel.AutoEllipsis = true;
            titleLabel.Click += (s, e) => ItemClicked?.Invoke(this, Id);
            contentPanel.Controls.Add(titleLabel);

            // Price
            Label priceLabel = new Label();
            priceLabel.Text = $"Giá: {Price}đ";
            priceLabel.Font = new Font("Segoe UI", 9);
            priceLabel.Location = new Point(10, 35);
            priceLabel.Size = new Size(180, 20);
            priceLabel.Click += (s, e) => ItemClicked?.Invoke(this, Id);
            contentPanel.Controls.Add(priceLabel);

            // Discount price
            if (!string.IsNullOrEmpty(DiscountPrice) && decimal.Parse(DiscountPrice) > 0)
            {
                Label discountPriceLabel = new Label();
                discountPriceLabel.Text = $"Giá khuyến mãi: {DiscountPrice}đ";
                discountPriceLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                discountPriceLabel.ForeColor = Color.Red;
                discountPriceLabel.Location = new Point(10, 55);
                discountPriceLabel.Size = new Size(180, 20);
                discountPriceLabel.Click += (s, e) => ItemClicked?.Invoke(this, Id);
                contentPanel.Controls.Add(discountPriceLabel);
            }

            // Category
            Label categoryLabel = new Label();
            categoryLabel.Text = Category ?? "Danh mục";
            categoryLabel.Font = new Font("Segoe UI", 8);
            categoryLabel.Location = new Point(10, 80);
            categoryLabel.Size = new Size(180, 20);
            categoryLabel.Click += (s, e) => ItemClicked?.Invoke(this, Id);
            contentPanel.Controls.Add(categoryLabel);

            // Add to cart button
            Button btnAddToCart = new Button();
            btnAddToCart.Text = "Thêm vào giỏ";
            btnAddToCart.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnAddToCart.Size = new Size(100, 25);
            btnAddToCart.Location = new Point(10, 110);
            btnAddToCart.BackColor = Color.FromArgb(0, 174, 219);
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.FlatStyle = FlatStyle.Flat;
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Cursor = Cursors.Hand;
            btnAddToCart.Click += (s, e) =>
            {
                // Add to cart logic
                MessageBox.Show($"Đã thêm {Title} vào giỏ hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            contentPanel.Controls.Add(btnAddToCart);

            this.Controls.Add(contentPanel);
        }

        public void UpdateData(string id, string title, string description, string imageUrl, string price, string discountPrice, string category)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            DiscountPrice = discountPrice;
            Category = category;

            // Update UI
            var pictureBox = this.Controls[0] as PictureBox;
            if (pictureBox != null)
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    try
                    {
                        pictureBox.LoadAsync(imageUrl);
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
            }

            var contentPanel = this.Controls[1] as Panel;
            if (contentPanel != null)
            {
                var titleLabel = contentPanel.Controls[0] as Label;
                if (titleLabel != null) titleLabel.Text = title;

                var priceLabel = contentPanel.Controls[1] as Label;
                if (priceLabel != null) priceLabel.Text = $"Giá: {price}đ";

                // Remove existing discount price label if exists
                var existingDiscountLabel = contentPanel.Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("Giá khuyến mãi"));
                if (existingDiscountLabel != null)
                    contentPanel.Controls.Remove(existingDiscountLabel);

                // Add discount price if needed
                if (!string.IsNullOrEmpty(discountPrice) && decimal.Parse(discountPrice) > 0)
                {
                    var discountPriceLabel = new Label();
                    discountPriceLabel.Text = $"Giá khuyến mãi: {discountPrice}đ";
                    discountPriceLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    discountPriceLabel.ForeColor = Color.Red;
                    discountPriceLabel.Location = new Point(10, 55);
                    discountPriceLabel.Size = new Size(180, 20);
                    contentPanel.Controls.Add(discountPriceLabel);
                }

                var categoryLabel = contentPanel.Controls.OfType<Label>().FirstOrDefault(l => l.Text == Category || l.Text.StartsWith("Danh mục"));
                if (categoryLabel != null) categoryLabel.Text = category;
            }
        }
    }
}