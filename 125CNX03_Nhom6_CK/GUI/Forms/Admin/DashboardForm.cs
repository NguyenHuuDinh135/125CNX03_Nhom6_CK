using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class DashboardForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly INguoiDungService _userService;
        private readonly IDonHangService _orderService;

        // Stat labels
        private Label _lblTotalProducts;
        private Label _lblTotalUsers;
        private Label _lblTotalOrders;
        private Label _lblTodayRevenue;

        // Controls need references
        private DataGridView _dgvRecentOrders;

        public DashboardForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();
            _userService = new NguoiDungService();
            _orderService = new DonHangService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Bảng điều khiển - Quản trị hệ thống";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // -----------------------------------------------------
            // 1) STATS PANEL
            // -----------------------------------------------------
            Panel statsPanel = new Panel();
            statsPanel.Size = new Size(this.Width - 40, 120);
            statsPanel.Location = new Point(20, 20);
            statsPanel.BackColor = Color.White;
            statsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label statsTitle = new Label();
            statsTitle.Text = "Tổng quan";
            statsTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            statsTitle.Location = new Point(20, 10);
            statsTitle.Size = new Size(200, 30);
            statsPanel.Controls.Add(statsTitle);

            _lblTotalProducts = CreateStatCard(statsPanel, "Tổng sản phẩm", "0", Color.FromArgb(220, 255, 240), 20, 50);
            _lblTotalUsers = CreateStatCard(statsPanel, "Tổng người dùng", "0", Color.FromArgb(255, 240, 240), 250, 50);
            _lblTotalOrders = CreateStatCard(statsPanel, "Tổng đơn hàng", "0", Color.FromArgb(240, 240, 255), 480, 50);
            _lblTodayRevenue = CreateStatCard(statsPanel, "Doanh thu hôm nay", "0đ", Color.FromArgb(255, 255, 240), 710, 50);

            this.Controls.Add(statsPanel);

            // -----------------------------------------------------
            // 2) CHART PANEL
            // -----------------------------------------------------
            Panel chartsPanel = new Panel();
            chartsPanel.Size = new Size(this.Width - 40, 300);
            chartsPanel.Location = new Point(20, 160);
            chartsPanel.BackColor = Color.White;
            chartsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label chartsTitle = new Label();
            chartsTitle.Text = "Thống kê";
            chartsTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            chartsTitle.Location = new Point(20, 10);
            chartsTitle.Size = new Size(200, 30);
            chartsPanel.Controls.Add(chartsTitle);

            this.Controls.Add(chartsPanel);

            // -----------------------------------------------------
            // 3) RECENT ORDER PANEL
            // -----------------------------------------------------
            Panel ordersPanel = new Panel();
            ordersPanel.Size = new Size(this.Width - 40, 250);
            ordersPanel.Location = new Point(20, 480);
            ordersPanel.BackColor = Color.White;
            ordersPanel.BorderStyle = BorderStyle.FixedSingle;

            Label ordersTitle = new Label();
            ordersTitle.Text = "Đơn hàng gần đây";
            ordersTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            ordersTitle.Location = new Point(20, 10);
            ordersTitle.Size = new Size(200, 30);
            ordersPanel.Controls.Add(ordersTitle);

            _dgvRecentOrders = new DataGridView();
            _dgvRecentOrders.Size = new Size(ordersPanel.Width - 40, ordersPanel.Height - 60);
            _dgvRecentOrders.Location = new Point(20, 50);
            _dgvRecentOrders.Font = new Font("Segoe UI", 10);
            _dgvRecentOrders.BackgroundColor = Color.White;
            _dgvRecentOrders.BorderStyle = BorderStyle.None;
            _dgvRecentOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgvRecentOrders.MultiSelect = false;
            _dgvRecentOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _dgvRecentOrders.RowTemplate.Height = 30;

            ordersPanel.Controls.Add(_dgvRecentOrders);

            this.Controls.Add(ordersPanel);
        }

        private Label CreateStatCard(Panel parent, string title, string value, Color color, int x, int y)
        {
            Panel card = new Panel();
            card.Size = new Size(200, 60);
            card.Location = new Point(x, y);
            card.BackColor = color;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Font = new Font("Segoe UI", 9);
            titleLabel.Location = new Point(10, 10);
            titleLabel.Size = new Size(180, 20);
            card.Controls.Add(titleLabel);

            Label valueLabel = new Label();
            valueLabel.Text = value;
            valueLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            valueLabel.Location = new Point(10, 30);
            valueLabel.Size = new Size(180, 20);
            card.Controls.Add(valueLabel);

            parent.Controls.Add(card);

            return valueLabel;
        }

        private void LoadData()
        {
            var products = _productService.GetAllProducts();
            var users = _userService.GetAllUsers();
            var orders = _orderService.GetAllOrders();

            // Update stat cards
            _lblTotalProducts.Text = products.Count.ToString("N0");
            _lblTotalUsers.Text = users.Count.ToString("N0");
            _lblTotalOrders.Text = orders.Count.ToString("N0");

            // Load recent orders
            var recentOrders = orders
                .OrderByDescending(o => DateTime.Parse(o.Element("NgayDatHang").Value))
                .Take(5)
                .ToList();

            _dgvRecentOrders.DataSource = ConvertToOrderTable(recentOrders);
        }

        private System.Data.DataTable ConvertToOrderTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Mã đơn", typeof(int));
            dt.Columns.Add("Khách hàng", typeof(string));
            dt.Columns.Add("Ngày đặt", typeof(DateTime));
            dt.Columns.Add("Tổng tiền", typeof(decimal));
            dt.Columns.Add("Trạng thái", typeof(string));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    element.Element("NguoiNhan_Ten").Value,
                    DateTime.Parse(element.Element("NgayDatHang").Value),
                    decimal.Parse(element.Element("TongTien").Value),
                    GetOrderStatusText(int.Parse(element.Element("TrangThaiDonHang").Value))
                );
            }

            return dt;
        }

        private string GetOrderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "Chưa xử lý";
                case 1: return "Đang xử lý";
                case 2: return "Đang giao";
                case 3: return "Đã giao";
                case 4: return "Đã hủy";
                default: return "Không xác định";
            }
        }
    }
}
