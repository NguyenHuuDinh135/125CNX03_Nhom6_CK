using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.BLL;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class CartForm : UserBaseForm
    {
        private readonly IGioHangService _cartService;
        private readonly ISanPhamService _productService;
        private readonly IDonHangService _orderService;
        private XElement _currentUser;
        private int _cartId;

        private FlowLayoutPanel _cartItemsFlow;
        private Label _totalLabel;

        public CartForm(XElement currentUser)
        {
            InitializeComponent();
            _cartService = new GioHangService();
            _productService = new SanPhamService();
            _orderService = new DonHangService();
            _currentUser = currentUser;

            InitializeUI();
            LoadCart();
        }

        private void InitializeUI()
        {
            this.Text = "Giỏ hàng";
            this.Size = new Size(950, 720);
            this.AutoScroll = true;

            // Create cart header
            Panel cartHeader = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            var cartTitle = new Label { Text = "Giỏ hàng của bạn", Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 30) };
            cartHeader.Controls.Add(cartTitle);
            this.Controls.Add(cartHeader);

            // Create cart items panel
            Panel cartItemsPanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, 500));

            Label itemsTitle = new Label { Text = "Sản phẩm trong giỏ hàng", Font = new Font(BaseFont.FontFamily, 12F, FontStyle.Bold), Location = new Point(20, 10), Size = new Size(300, 24) };
            cartItemsPanel.Controls.Add(itemsTitle);

            _cartItemsFlow = new FlowLayoutPanel
            {
                Size = new Size(cartItemsPanel.Width - 40, cartItemsPanel.Height - 60),
                Location = new Point(20, 40),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = true,
                AutoScroll = true,
                BackColor = Surface
            };

            cartItemsPanel.Controls.Add(_cartItemsFlow);
            this.Controls.Add(cartItemsPanel);

            // Create checkout panel
            Panel checkoutPanel = CreateSectionPanel(new Point(20, 620), new Size(this.Width - 40, 100));

            _totalLabel = new Label { Text = "Tổng tiền: 0đ", Font = new Font(BaseFont.FontFamily, 12F, FontStyle.Bold), Location = new Point(20, 20), Size = new Size(300, 30) };
            checkoutPanel.Controls.Add(_totalLabel);

            Button btnCheckout = CreateButton("Thanh toán", new Point(750, 20), new Size(140, 36), Primary, BtnCheckout_Click);
            checkoutPanel.Controls.Add(btnCheckout);

            this.Controls.Add(checkoutPanel);
        }

        private void LoadCart()
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để xem giỏ hàng.", "Yêu cầu đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = int.Parse(_currentUser.Element("Id").Value);
            var cart = _cartService.GetCartByUserId(userId);
            if (cart == null)
            {
                _cartService.CreateCartForUser(userId);
                cart = _cartService.GetCartByUserId(userId);
            }

            _cartId = int.Parse(cart.Element("Id").Value);

            // Load items
            _cartItemsFlow.Controls.Clear();
            var items = _cartService.GetCartItems(_cartId);
            foreach (var item in items)
            {
                var product = _productService.GetProductById(int.Parse(item.Element("MaSanPham").Value));
                if (product == null) continue;

                var cartItem = new CartItem();
                cartItem.ProductId = int.Parse(item.Element("MaSanPham").Value);
                cartItem.ProductName = product.Element("TenSanPham").Value;
                cartItem.Price = item.Element("DonGia").Value;
                cartItem.Quantity = int.Parse(item.Element("SoLuong").Value);
                cartItem.RefreshUI();

                // subscribe to events
                cartItem.RemoveClicked += (pid) => CartItem_RemoveClicked(pid);
                cartItem.QuantityChanged += (pid, qty) => CartItem_QuantityChanged(pid, qty);

                _cartItemsFlow.Controls.Add(cartItem);
            }

            UpdateTotal();
        }

        private void UpdateTotal()
        {
            var total = _cartService.GetCartTotal(_cartId);
            _totalLabel.Text = $"Tổng tiền: {total:N0}đ";
        }

        private void CartItem_RemoveClicked(int productId)
        {
            _cartService.RemoveProductFromCart(_cartId, productId);
            LoadCart();
        }

        private void CartItem_QuantityChanged(int productId, int quantity)
        {
            _cartService.UpdateCartItem(_cartId, productId, quantity);
            UpdateTotal();
        }

        private void BtnCheckout_Click(object sender, EventArgs e)
        {
            // Basic checkout: create order from cart items
            var items = _cartService.GetCartItems(_cartId);
            if (items == null || !items.Any())
            {
                MessageBox.Show("Giỏ hàng rỗng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var order = new XElement("DonHang",
                new XElement("MaNguoiDung", int.Parse(_currentUser.Element("Id").Value)),
                new XElement("NgayDatHang", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("TongTien", _cartService.GetCartTotal(_cartId).ToString()),
                new XElement("TrangThaiDonHang", 0),
                new XElement("NguoiNhan_Ten", _currentUser.Element("HoTen")?.Value ?? ""),
                new XElement("NguoiNhan_DiaChi", _currentUser.Element("DiaChi")?.Value ?? ""),
                new XElement("NguoiNhan_SDT", _currentUser.Element("SoDienThoai")?.Value ?? "")
            );

            var orderItems = items.Select(i => new XElement("ChiTietDonHang",
                new XElement("MaSanPham", int.Parse(i.Element("MaSanPham").Value)),
                new XElement("DonGia", decimal.Parse(i.Element("DonGia").Value)),
                new XElement("SoLuong", int.Parse(i.Element("SoLuong").Value))
            )).ToList();

            try
            {
                _orderService.CreateOrder(order, orderItems);
                _cartService.ClearCart(_cartId);
                MessageBox.Show("Đặt hàng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}