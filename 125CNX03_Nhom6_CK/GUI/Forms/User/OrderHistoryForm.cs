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
            headerPanel.Controls.Add(new Label { Text = "Lịch sử đơn hàng", Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 30) });
            this.Controls.Add(headerPanel);

            var ordersPanel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, 600));
            var ordersTitle = new Label { Text = "Các đơn hàng đã đặt", Font = new Font(BaseFont.FontFamily, 12F, FontStyle.Bold), Location = new Point(20, 10), Size = new Size(300, 24) };
            ordersPanel.Controls.Add(ordersTitle);

            FlowLayoutPanel ordersFlowLayout = new FlowLayoutPanel
            {
                Name = "ordersFlowLayout",
                Size = new Size(ordersPanel.Width - 40, ordersPanel.Height - 60),
                Location = new Point(20, 40),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = true,
                AutoScroll = true,
                BackColor = Surface
            };

            ordersPanel.Controls.Add(ordersFlowLayout);
            this.Controls.Add(ordersPanel);
        }

        private void LoadData()
        {
            var allOrders = _orderService.GetAllOrders();
            var userId = _currentUser.Element("Id").Value;
            var userOrders = allOrders.Where(o => o.Element("MaNguoiDung")?.Value == userId).ToList();

            var ordersPanel = this.Controls[this.Controls.Count - 1] as Panel;
            if (ordersPanel == null) return;
            var ordersFlowLayout = ordersPanel.Controls.OfType<FlowLayoutPanel>().FirstOrDefault(f => f.Name == "ordersFlowLayout");
            if (ordersFlowLayout == null) return;

            ordersFlowLayout.Controls.Clear();

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