using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    // Custom EventArgs để truyền Id
    public class ProductEventArgs : EventArgs
    {
        public string ProductId { get; set; }

        public ProductEventArgs(string productId)
        {
            ProductId = productId;
        }
    }

    public partial class ProductCard : UserControl
    {
        public event EventHandler<ProductEventArgs> ItemClicked;
        public event EventHandler<ProductEventArgs> AddToCartClicked;

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
            pictureBox.Click += (s, e) => ItemClicked?.Invoke(this, new ProductEventArgs(Id));
            this.Controls.Add(pictureBox);

            // Content panel
            Panel contentPanel = new Panel();
            contentPanel.Size = new Size(200, 150);
            contentPanel.Location = new Point(0, 150);
            contentPanel.BackColor = Color.White;

            // Title
            Label titleLabel = new Label();
            titleLabel.Name = "lblTitle";
            titleLabel.Text = Title ?? "Tiêu đề sản phẩm";
            titleLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            titleLabel.Location = new Point(10, 5);
            titleLabel.Size = new Size(180, 30);
            titleLabel.MaximumSize = new Size(180, 30);
            titleLabel.AutoEllipsis = true;
            titleLabel.Click += (s, e) => ItemClicked?.Invoke(this, new ProductEventArgs(Id));
            contentPanel.Controls.Add(titleLabel);

            // Price
            Label priceLabel = new Label();
            priceLabel.Name = "lblPrice";
            priceLabel.Text = $"Giá: {Price}đ";
            priceLabel.Font = new Font("Segoe UI", 9);
            priceLabel.Location = new Point(10, 38);
            priceLabel.Size = new Size(180, 20);
            priceLabel.Click += (s, e) => ItemClicked?.Invoke(this, new ProductEventArgs(Id));
            contentPanel.Controls.Add(priceLabel);

            // Category - moved down to make room
            Label categoryLabel = new Label();
            categoryLabel.Name = "lblCategory";
            categoryLabel.Text = Category ?? "Danh mục";
            categoryLabel.Font = new Font("Segoe UI", 8);
            categoryLabel.ForeColor = Color.Gray;
            categoryLabel.Location = new Point(10, 80);
            categoryLabel.Size = new Size(180, 20);
            categoryLabel.Click += (s, e) => ItemClicked?.Invoke(this, new ProductEventArgs(Id));
            contentPanel.Controls.Add(categoryLabel);

            // Add to cart button
            Button btnAddToCart = new Button();
            btnAddToCart.Name = "btnAddToCart";
            btnAddToCart.Text = "Thêm vào giỏ";
            btnAddToCart.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            btnAddToCart.Size = new Size(100, 25);
            btnAddToCart.Location = new Point(10, 110);
            btnAddToCart.BackColor = Color.FromArgb(0, 174, 219);
            btnAddToCart.ForeColor = Color.White;
            btnAddToCart.FlatStyle = FlatStyle.Flat;
            btnAddToCart.FlatAppearance.BorderSize = 0;
            btnAddToCart.Cursor = Cursors.Hand;
            btnAddToCart.Click += (s, e) => AddToCartClicked?.Invoke(this, new ProductEventArgs(Id));
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
            var pictureBox = this.Controls.OfType<PictureBox>().FirstOrDefault();
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
                        pictureBox.Image = Properties.Resources.DefaultProduct;
                    }
                }
                else
                {
                    pictureBox.Image = Properties.Resources.DefaultProduct;
                }
            }

            var contentPanel = this.Controls.OfType<Panel>().FirstOrDefault();
            if (contentPanel != null)
            {
                var titleLabel = contentPanel.Controls.Find("lblTitle", false).FirstOrDefault() as Label;
                if (titleLabel != null) titleLabel.Text = title;

                var priceLabel = contentPanel.Controls.Find("lblPrice", false).FirstOrDefault() as Label;
                var categoryLabel = contentPanel.Controls.Find("lblCategory", false).FirstOrDefault() as Label;

                // Remove existing discount price label if exists
                var existingDiscountLabel = contentPanel.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblDiscount");
                if (existingDiscountLabel != null)
                    contentPanel.Controls.Remove(existingDiscountLabel);

                // Check if we have a discount price
                bool hasDiscount = !string.IsNullOrEmpty(discountPrice) &&
                                   decimal.TryParse(discountPrice, out var dp) &&
                                   dp > 0;

                if (hasDiscount)
                {
                    // Show original price with strikethrough
                    if (priceLabel != null)
                    {
                        priceLabel.Text = $"Giá: {price}đ";
                        priceLabel.Font = new Font("Segoe UI", 8, FontStyle.Strikeout);
                        priceLabel.ForeColor = Color.Gray;
                    }

                    // Add discount price in red
                    var discountPriceLabel = new Label();
                    discountPriceLabel.Name = "lblDiscount";
                    discountPriceLabel.Text = $"Giá khuyến mãi: {discountPrice}đ";
                    discountPriceLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    discountPriceLabel.ForeColor = Color.Red;
                    discountPriceLabel.Location = new Point(10, 58);
                    discountPriceLabel.Size = new Size(180, 20);
                    discountPriceLabel.Click += (s, e) => ItemClicked?.Invoke(this, new ProductEventArgs(Id));
                    contentPanel.Controls.Add(discountPriceLabel);
                    discountPriceLabel.BringToFront(); // Ensure it's visible
                }
                else
                {
                    // No discount, show regular price
                    if (priceLabel != null)
                    {
                        priceLabel.Text = $"Giá: {price}đ";
                        priceLabel.Font = new Font("Segoe UI", 9);
                        priceLabel.ForeColor = Color.Black;
                    }
                }

                if (categoryLabel != null)
                    categoryLabel.Text = category;
            }
        }
    }
}