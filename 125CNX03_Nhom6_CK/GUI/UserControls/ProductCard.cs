using System;
using System.Windows.Forms;
using System.Drawing;

namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    public partial class ProductCard : UserControl
    {
        public event EventHandler<ProductCardEventArgs> AddToCartClicked;

        public ProductCard()
        {
            InitializeComponent();
        }

        public void SetProductInfo(int productId, string name, string description, decimal price, decimal? discountPrice, string imageUrl)
        {
            lblProductName.Text = name;
            lblDescription.Text = description;
            lblPrice.Text = discountPrice.HasValue ?
                $"<s>{price:N0}đ</s> {discountPrice.Value:N0}đ" :
                $"{price:N0}đ";

            // Load image from URL or set default
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    pictureBoxProduct.LoadAsync(imageUrl.Trim());
                }
                catch
                {
                    pictureBoxProduct.Image = Properties.Resources.DefaultProductImage; // Assuming you have a default image resource
                }
            }
            else
            {
                pictureBoxProduct.Image = Properties.Resources.DefaultProductImage;
            }

            this.Tag = productId;
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (Tag != null && int.TryParse(Tag.ToString(), out int productId))
            {
                var args = new ProductCardEventArgs(productId, lblProductName.Text, decimal.Parse(lblPrice.Text.Replace("đ", "").Replace("<s>", "").Replace("</s>", "").Trim()));
                AddToCartClicked?.Invoke(this, args);
            }
        }

        private void ProductCard_Load(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(200, 250);
        }
    }

    public class ProductCardEventArgs : EventArgs
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public decimal Price { get; }

        public ProductCardEventArgs(int productId, string productName, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            Price = price;
        }
    }
}