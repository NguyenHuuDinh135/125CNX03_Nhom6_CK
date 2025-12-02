using System;
using System.Windows.Forms;
using System.Drawing;

namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    public partial class CartItem : UserControl
    {
        public event EventHandler<CartItemEventArgs> QuantityChanged;
        public event EventHandler<CartItemEventArgs> ItemRemoved;

        public CartItem()
        {
            InitializeComponent();
        }

        public void SetCartItemInfo(int cartItemId, int productId, string productName, decimal price, int quantity, string imageUrl = null)
        {
            lblProductName.Text = productName;
            lblPrice.Text = $"{price:N0}đ";
            numericQuantity.Value = quantity;
            lblSubtotal.Text = $"{price * quantity:N0}đ";

            // Load image from URL or set default
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    pictureBoxProduct.LoadAsync(imageUrl);
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

            this.Tag = cartItemId;
            this.Name = $"CartItem_{productId}"; // Set a unique name for identification
        }

        private void numericQuantity_ValueChanged(object sender, EventArgs e)
        {
            var newQuantity = (int)numericQuantity.Value;
            var price = decimal.Parse(lblPrice.Text.Replace("đ", "").Replace(",", ""));
            lblSubtotal.Text = $"{price * newQuantity:N0}đ";

            if (Tag != null && int.TryParse(Tag.ToString(), out int cartItemId))
            {
                var args = new CartItemEventArgs(cartItemId, newQuantity);
                QuantityChanged?.Invoke(this, args);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (Tag != null && int.TryParse(Tag.ToString(), out int cartItemId))
            {
                var args = new CartItemEventArgs(cartItemId, 0);
                ItemRemoved?.Invoke(this, args);
            }
        }

        private void CartItem_Load(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(400, 80);
        }
    }

    public class CartItemEventArgs : EventArgs
    {
        public int CartItemId { get; }
        public int NewQuantity { get; }

        public CartItemEventArgs(int cartItemId, int newQuantity)
        {
            CartItemId = cartItemId;
            NewQuantity = newQuantity;
        }
    }
}