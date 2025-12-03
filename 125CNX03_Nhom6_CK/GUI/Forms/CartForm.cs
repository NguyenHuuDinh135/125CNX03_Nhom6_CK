using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.UserControls;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class CartForm : Form
    {
        private readonly IGioHangService _cartService;
        private readonly ISanPhamService _productService;
        private readonly IDonHangService _orderService;
        private int _currentUserId = 1; // This should come from the logged-in user

        public CartForm()
        {
            InitializeComponent();
            _cartService = new GioHangService();
            _productService = new SanPhamService();
            _orderService = new DonHangService();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            flowLayoutPanelCart.Controls.Clear();
            var cartItems = _cartService.GetCartItems(_currentUserId); // This should get items by cart ID, not user ID

            // Get the cart ID for the current user
            var cart = _cartService.GetCartByUserId(_currentUserId);
            if (cart != null)
            {
                var cartId = int.Parse(cart.Element("Id").Value);
                cartItems = _cartService.GetCartItems(cartId);

                foreach (var item in cartItems)
                {
                    var productId = int.Parse(item.Element("MaSanPham").Value);
                    var product = _productService.GetProductById(productId);

                    var cartItemControl = new CartItem();
                    cartItemControl.SetCartItemInfo(
                        int.Parse(item.Element("Id").Value),
                        productId,
                        product?.Element("TenSanPham").Value ?? "Unknown Product",
                        decimal.Parse(item.Element("DonGia").Value),
                        int.Parse(item.Element("SoLuong").Value),
                        product?.Element("DuongDanAnh").Value
                    );

                    cartItemControl.QuantityChanged += CartItemControl_QuantityChanged;
                    cartItemControl.ItemRemoved += CartItemControl_ItemRemoved;

                    flowLayoutPanelCart.Controls.Add(cartItemControl);
                }

                UpdateTotal();
            }
        }

        private void CartItemControl_QuantityChanged(object sender, CartItemEventArgs e)
        {
            _cartService.UpdateCartItem(_currentUserId, e.CartItemId, e.NewQuantity);
            LoadCartItems(); // Reload to update the total
        }

        private void CartItemControl_ItemRemoved(object sender, CartItemEventArgs e)
        {
            _cartService.RemoveProductFromCart(_currentUserId, e.CartItemId);
            LoadCartItems(); // Reload to update the total
        }

        private void UpdateTotal()
        {
            var total = _cartService.GetCartTotal(_currentUserId);
            lblTotal.Text = $"Tổng cộng: {total:N0}đ";
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            var checkoutForm = new CheckoutForm();
            checkoutForm.MdiParent = this.MdiParent;
            checkoutForm.Show();
            this.Close();
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa toàn bộ giỏ hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _cartService.ClearCart(_currentUserId);
                LoadCartItems();
            }
        }
    }
}