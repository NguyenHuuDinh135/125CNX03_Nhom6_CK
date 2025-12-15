using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderHistoryForm : UserBaseForm
    {
        private readonly IDonHangService _orderService;
        private readonly XElement _currentUser;
        private FlowLayoutPanel _ordersFlowLayout;

        public OrderHistoryForm(XElement user)
        {
            InitializeComponent();
            _orderService = new DonHangService();
            _currentUser = user;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Lịch sử đơn hàng";
            this.Size = new Size(950, 720);
            this.AutoScroll = true;

            var headerPanel = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            headerPanel.Controls.Add(new Label
            {
                Text = "Lịch sử đơn hàng",
                Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(300, 30)
            });
            this.Controls.Add(headerPanel);

            var ordersPanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, 600));

            var ordersTitle = new Label
            {
                Text = "Các đơn hàng đã đặt",
                Font = new Font(BaseFont.FontFamily, 12F, FontStyle.Bold),
                Location = new Point(20, 10),
                Size = new Size(300, 24)
            };
            ordersPanel.Controls.Add(ordersTitle);

            _ordersFlowLayout = new FlowLayoutPanel
            {
                Name = "ordersFlowLayout",
                Size = new Size(ordersPanel.Width - 40, ordersPanel.Height - 60),
                Location = new Point(20, 40),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Surface
            };
            ordersPanel.Controls.Add(_ordersFlowLayout);
            this.Controls.Add(ordersPanel);
        }

        private void LoadData()
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _ordersFlowLayout.Controls.Clear();

            try
            {
                var allOrders = _orderService.GetAllOrders();

                if (allOrders == null || !allOrders.Any())
                {
                    ShowEmptyMessage("Không có đơn hàng nào trong hệ thống");
                    return;
                }

                var userId = _currentUser.Element("Id")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    MessageBox.Show("ID người dùng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Debug: Hiển thị thông tin
                System.Diagnostics.Debug.WriteLine($"Current User ID: {userId}");
                System.Diagnostics.Debug.WriteLine($"Total Orders: {allOrders.Count}");

                // Lọc đơn hàng của user hiện tại
                var userOrders = allOrders
                    .Where(o =>
                    {
                        var orderUserId = o.Element("MaNguoiDung")?.Value;
                        System.Diagnostics.Debug.WriteLine($"Order {o.Element("Id")?.Value} - User ID: {orderUserId}");
                        return orderUserId == userId;
                    })
                    .OrderByDescending(o =>
                    {
                        try
                        {
                            return DateTime.Parse(o.Element("NgayDatHang").Value);
                        }
                        catch
                        {
                            return DateTime.MinValue;
                        }
                    })
                    .ToList();

                System.Diagnostics.Debug.WriteLine($"User Orders Found: {userOrders.Count}");

                if (!userOrders.Any())
                {
                    ShowEmptyMessage("Bạn chưa có đơn hàng nào");
                    return;
                }

                // Tạo và thêm OrderItem cho mỗi đơn hàng
                foreach (var order in userOrders)
                {
                    try
                    {
                        var orderItem = new OrderItem();

                        // Set properties trước khi add vào control
                        orderItem.OrderId = int.Parse(order.Element("Id").Value);
                        orderItem.OrderDate = DateTime.Parse(order.Element("NgayDatHang").Value);
                        orderItem.TotalAmount = decimal.Parse(order.Element("TongTien").Value);
                        orderItem.Status = int.Parse(order.Element("TrangThaiDonHang").Value);

                        // Debug
                        System.Diagnostics.Debug.WriteLine($"Adding Order Item: ID={orderItem.OrderId}, Date={orderItem.OrderDate}, Total={orderItem.TotalAmount}");

                        _ordersFlowLayout.Controls.Add(orderItem);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating order item: {ex.Message}");
                        MessageBox.Show($"Lỗi khi hiển thị đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                _ordersFlowLayout.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn hàng: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowEmptyMessage(string message)
        {
            Label emptyLabel = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            _ordersFlowLayout.Controls.Add(emptyLabel);
        }
    }
}