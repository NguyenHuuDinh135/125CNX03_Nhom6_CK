using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class CheckoutForm : Form
    {
        private readonly IGioHangService _cartService;
        private readonly IDonHangService _orderService;
        private readonly IChiTietDonHangService _orderItemService;
        private readonly ISanPhamService _productService;
        private int _currentUserId = 1; // This should come from the logged-in user

        public CheckoutForm()
        {
            InitializeComponent();
            _cartService = new GioHangService();
            _orderService = new DonHangService();
            _orderItemService = new ChiTietDonHangService();
            _productService = new SanPhamService();
            LoadCartSummary();
        }

        private void LoadCartSummary()
        {
            var total = _cartService.GetCartTotal(_currentUserId);
            lblTotalAmount.Text = $"Tổng tiền: {total:N0}đ";
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerName.Text) ||
                string.IsNullOrEmpty(txtCustomerPhone.Text) ||
                string.IsNullOrEmpty(txtDeliveryAddress.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin giao hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Create order element
                var order = new XElement("DonHang",
                    new XElement("MaNguoiDung", _currentUserId),
                    new XElement("NgayDatHang", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new XElement("TrangThaiDonHang", "0"), // Chưa xử lý
                    new XElement("NguoiNhan_Ten", txtCustomerName.Text),
                    new XElement("NguoiNhan_SDT", txtCustomerPhone.Text),
                    new XElement("DiaChi_Duong", txtDeliveryAddress.Text),
                    new XElement("DiaChi_ThanhPho", txtDeliveryCity.Text),
                    new XElement("DiaChi_Tinh", txtDeliveryProvince.Text),
                    new XElement("TongTien", _cartService.GetCartTotal(_currentUserId)),
                    new XElement("GhiChu", txtNote.Text)
                );

                // Get cart items
                var cartItems = _cartService.GetCartItems(_currentUserId);
                var orderItems = new System.Collections.Generic.List<XElement>();

                foreach (var item in cartItems)
                {
                    var productId = int.Parse(item.Element("MaSanPham").Value);
                    var product = _productService.GetProductById(productId);

                    var orderItem = new XElement("ChiTietDonHang",
                        new XElement("ItemOrdered_MaSanPham", productId),
                        new XElement("ItemOrdered_TenSanPham", product?.Element("TenSanPham").Value ?? ""),
                        new XElement("ItemOrdered_DuongDanAnh", product?.Element("DuongDanAnh").Value ?? ""),
                        new XElement("DonGia", decimal.Parse(item.Element("DonGia").Value)),
                        new XElement("SoLuong", int.Parse(item.Element("SoLuong").Value))
                    );

                    orderItems.Add(orderItem);
                }

                // Create the order
                _orderService.CreateOrder(order, orderItems);

                // Clear the cart
                _cartService.ClearCart(_currentUserId);

                MessageBox.Show("Đặt hàng thành công! Cảm ơn bạn đã mua hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}