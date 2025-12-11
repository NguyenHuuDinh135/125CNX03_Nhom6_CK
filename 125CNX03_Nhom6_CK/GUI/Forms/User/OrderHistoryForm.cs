using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderHistoryForm : Form
    {
        private readonly IDonHangService _orderService;
        private readonly XElement _currentUser;

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
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create header
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.Width - 40, 60);
            headerPanel.Location = new Point(20, 20);
            headerPanel.BackColor = Color.White;
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            Label headerTitle = new Label();
            headerTitle.Text = "Lịch sử đơn hàng";
            headerTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            headerTitle.Location = new Point(20, 15);
            headerTitle.Size = new Size(200, 30);
            headerPanel.Controls.Add(headerTitle);

            this.Controls.Add(headerPanel);
            Panel ordersPanel = new Panel();
            ordersPanel.Size = new Size(this.Width - 40, 600);
            ordersPanel.Location = new Point(20, 100);
            ordersPanel.BackColor = Color.White;
            ordersPanel.BorderStyle = BorderStyle.FixedSingle;

            Label ordersTitle = new Label();
            ordersTitle.Text = "Các đơn hàng đã đặt";
            ordersTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            ordersTitle.Location = new Point(20, 10);
            ordersTitle.Size = new Size(200, 20);
            ordersPanel.Controls.Add(ordersTitle);

            // Create orders list panel
            // Create flow layout for orders
            FlowLayoutPanel ordersFlowLayout = new FlowLayoutPanel();
            ordersFlowLayout.Name = "ordersFlowLayout"; // Add a name for easy lookup
            ordersFlowLayout.Size = new Size(ordersPanel.Width - 40, ordersPanel.Height - 60);
            ordersFlowLayout.Location = new Point(20, 40);
            ordersFlowLayout.FlowDirection = FlowDirection.TopDown;
            ordersFlowLayout.WrapContents = true;
            ordersFlowLayout.AutoScroll = true;
            ordersFlowLayout.BackColor = Color.White;

            ordersPanel.Controls.Add(ordersFlowLayout);

            this.Controls.Add(ordersPanel);
        }

        private void LoadData()
        {
            // Get orders for current user
            var allOrders = _orderService.GetAllOrders();
            var userId = _currentUser.Element("Id").Value;
            var userOrders = allOrders.Where(o =>
                o.Element("MaNguoiDung")?.Value == userId).ToList();

            // Get the flow layout panel by name
            var ordersPanel = this.Controls[1] as Panel;
            if (ordersPanel == null) return;
            var ordersFlowLayout = ordersPanel.Controls
                .OfType<FlowLayoutPanel>()
                .FirstOrDefault(f => f.Name == "ordersFlowLayout");
            if (ordersFlowLayout == null) return;

            // Clear existing items
            ordersFlowLayout.Controls.Clear();

            // Add order items
            foreach (var order in userOrders)
            {
                var orderItem = new OrderItem();
                orderItem.OrderId = int.Parse(order.Element("Id").Value);
                orderItem.OrderDate = DateTime.Parse(order.Element("NgayDatHang").Value);
                orderItem.TotalAmount = decimal.Parse(order.Element("TongTien").Value);
                orderItem.Status = int.Parse(order.Element("TrangThaiDonHang").Value);

                ordersFlowLayout.Controls.Add(orderItem);
            }
        }
    }
}