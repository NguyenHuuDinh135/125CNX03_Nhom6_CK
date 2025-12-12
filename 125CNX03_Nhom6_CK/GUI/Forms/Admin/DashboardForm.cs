using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class DashboardForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly INguoiDungService _userService;
        private readonly IDonHangService _orderService;

        // Tổng quan
        private Label _lblTotalProducts;
        private Label _lblTotalUsers;
        private Label _lblTotalOrders;
        private Label _lblTodayRevenue;

        // Thống kê
        private Label _lblProductVisible;
        private Label _lblUserActive;
        private Label _lblOrderShipping;
        private Label _lblOrderDone;

        // Đơn hàng gần đây
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

            // =====================================================
            // 1) TỔNG QUAN
            // =====================================================
            Panel statsPanel = new Panel
            {
                Size = new Size(this.Width - 40, 120),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            statsPanel.Controls.Add(new Label
            {
                Text = "Tổng quan",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10)
            });

            _lblTotalProducts = CreateStatCard(statsPanel, "Tổng sản phẩm", "0", Color.FromArgb(220, 255, 240), 20, 50);
            _lblTotalUsers = CreateStatCard(statsPanel, "Tổng người dùng", "0", Color.FromArgb(255, 240, 240), 250, 50);
            _lblTotalOrders = CreateStatCard(statsPanel, "Tổng đơn hàng", "0", Color.FromArgb(240, 240, 255), 480, 50);
            _lblTodayRevenue = CreateStatCard(statsPanel, "Doanh thu hôm nay", "0đ", Color.FromArgb(255, 255, 240), 710, 50);

            this.Controls.Add(statsPanel);

            // =====================================================
            // 2) THỐNG KÊ (ĐÃ HOÀN THIỆN)
            // =====================================================
            Panel chartsPanel = new Panel
            {
                Size = new Size(this.Width - 40, 260),
                Location = new Point(20, 160),
                BorderStyle = BorderStyle.FixedSingle
            };

            chartsPanel.Controls.Add(new Label
            {
                Text = "Thống kê",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10)
            });

            CreateStatisticRow(chartsPanel, "Sản phẩm đang hiển thị", out _lblProductVisible, 60);
            CreateStatisticRow(chartsPanel, "Người dùng hoạt động", out _lblUserActive, 100);
            CreateStatisticRow(chartsPanel, "Đơn hàng đang giao", out _lblOrderShipping, 140);
            CreateStatisticRow(chartsPanel, "Đơn hàng đã giao", out _lblOrderDone, 180);

            this.Controls.Add(chartsPanel);

            // =====================================================
            // 3) ĐƠN HÀNG GẦN ĐÂY
            // =====================================================
            Panel ordersPanel = new Panel
            {
                Size = new Size(this.Width - 40, 250),
                Location = new Point(20, 440),
                BorderStyle = BorderStyle.FixedSingle
            };

            ordersPanel.Controls.Add(new Label
            {
                Text = "Đơn hàng gần đây",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10)
            });

            _dgvRecentOrders = new DataGridView
            {
                Size = new Size(ordersPanel.Width - 40, ordersPanel.Height - 60),
                Location = new Point(20, 50),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowTemplate = { Height = 30 }
            };

            StyleGrid(_dgvRecentOrders);
            ordersPanel.Controls.Add(_dgvRecentOrders);
            this.Controls.Add(ordersPanel);
        }

        // =====================================================
        // UI HELPERS
        // =====================================================
        private Label CreateStatCard(Panel parent, string title, string value, Color color, int x, int y)
        {
            Panel card = new Panel
            {
                Size = new Size(200, 60),
                Location = new Point(x, y),
                BackColor = color,
                BorderStyle = BorderStyle.FixedSingle
            };

            card.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, 8)
            });

            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 30)
            };

            card.Controls.Add(valueLabel);
            parent.Controls.Add(card);
            return valueLabel;
        }

        private void CreateStatisticRow(Panel parent, string title, out Label valueLabel, int y)
        {
            parent.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10),
                Location = new Point(40, y)
            });

            ProgressBar progress = new ProgressBar
            {
                Location = new Point(270, y + 3),
                Size = new Size(500, 20),
                Style = ProgressBarStyle.Continuous,
                Maximum = 100
            };
            parent.Controls.Add(progress);

            valueLabel = new Label
            {
                Text = "0 / 0",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(790, y)
            };
            valueLabel.Tag = progress;
            parent.Controls.Add(valueLabel);
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 242, 245);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        // =====================================================
        // LOAD DATA
        // =====================================================
        private void LoadData()
        {
            var products = _productService.GetAllProducts();
            var users = _userService.GetAllUsers();
            var orders = _orderService.GetAllOrders();

            // Tổng quan
            _lblTotalProducts.Text = products.Count.ToString("N0");
            _lblTotalUsers.Text = users.Count.ToString("N0");
            _lblTotalOrders.Text = orders.Count.ToString("N0");

            decimal todayRevenue = orders
                .Where(o => DateTime.Parse(o.Element("NgayDatHang").Value).Date == DateTime.Today)
                .Sum(o => decimal.Parse(o.Element("TongTien").Value));

            _lblTodayRevenue.Text = todayRevenue.ToString("N0") + "đ";

            // Thống kê
            int visibleProducts = products.Count(p => bool.Parse(p.Element("HienThi").Value));
            int shippingOrders = orders.Count(o => int.Parse(o.Element("TrangThaiDonHang").Value) == 2);
            int doneOrders = orders.Count(o => int.Parse(o.Element("TrangThaiDonHang").Value) == 3);

            UpdateStatistic(_lblProductVisible, visibleProducts, products.Count);
            UpdateStatistic(_lblUserActive, users.Count, users.Count);
            UpdateStatistic(_lblOrderShipping, shippingOrders, orders.Count);
            UpdateStatistic(_lblOrderDone, doneOrders, orders.Count);

            // Đơn hàng gần đây
            var recentOrders = orders
                .OrderByDescending(o => DateTime.Parse(o.Element("NgayDatHang").Value))
                .Take(5)
                .ToList();

            _dgvRecentOrders.DataSource = ConvertToOrderTable(recentOrders);
        }

        private void UpdateStatistic(Label label, int value, int total)
        {
            label.Text = $"{value} / {total}";
            if (label.Tag is ProgressBar progress)
            {
                progress.Value = total == 0 ? 0 : Math.Min(100, value * 100 / total);
            }
        }

        private DataTable ConvertToOrderTable(List<XElement> elements)
        {
            var dt = new DataTable();
            dt.Columns.Add("Mã đơn", typeof(int));
            dt.Columns.Add("Khách hàng");
            dt.Columns.Add("Ngày đặt");
            dt.Columns.Add("Tổng tiền", typeof(decimal));
            dt.Columns.Add("Trạng thái");

            foreach (var e in elements)
            {
                dt.Rows.Add(
                    int.Parse(e.Element("Id").Value),
                    e.Element("NguoiNhan_Ten").Value,
                    DateTime.Parse(e.Element("NgayDatHang").Value),
                    decimal.Parse(e.Element("TongTien").Value),
                    GetOrderStatusText(int.Parse(e.Element("TrangThaiDonHang").Value))
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
