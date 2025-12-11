using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class OrderManagementForm : Form
    {
        private readonly IDonHangService _orderService;
        private readonly IChiTietDonHangService _orderItemService;

        private DataGridView dgvOrders;
        private DataGridView dgvOrderItems;

        private TextBox txtCustomer;
        private TextBox txtPhone;
        private TextBox txtNote;
        private TextBox txtTotal;
        private TextBox txtDate;

        private ComboBox cboStatus;
        private Button btnUpdateStatus;

        public OrderManagementForm()
        {
            InitializeComponent();
            _orderService = new DonHangService();
            _orderItemService = new ChiTietDonHangService();

            InitializeUI();
            LoadData();
        }

        // ======================================
        // UI
        // ======================================
        private void InitializeUI()
        {
            this.Text = "Quản lý đơn hàng";
            this.Size = new Size(1300, 800);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // ================== PANEL LIST (LEFT) ==================
            Panel leftPanel = new Panel()
            {
                Size = new Size(700, 720),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(leftPanel);

            Label lblOrders = new Label()
            {
                Text = "Danh sách đơn hàng",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            leftPanel.Controls.Add(lblOrders);


            dgvOrders = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(650, 630),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 30 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                ReadOnly = true
            };
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;
            leftPanel.Controls.Add(dgvOrders);

            // ================== PANEL DETAILS (RIGHT) ==================
            Panel rightPanel = new Panel()
            {
                Size = new Size(500, 720),
                Location = new Point(740, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(rightPanel);

            Label lblDetails = new Label()
            {
                Text = "Chi tiết đơn hàng",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            rightPanel.Controls.Add(lblDetails);

            // ==== Customer Name ====
            rightPanel.Controls.Add(new Label() { Text = "Người nhận:", Location = new Point(20, 70) });
            txtCustomer = new TextBox() { Location = new Point(150, 70), Width = 300, ReadOnly = true };
            rightPanel.Controls.Add(txtCustomer);

            // ==== Phone ====
            rightPanel.Controls.Add(new Label() { Text = "SĐT:", Location = new Point(20, 110) });
            txtPhone = new TextBox() { Location = new Point(150, 110), Width = 300, ReadOnly = true };
            rightPanel.Controls.Add(txtPhone);

            // ==== Date ====
            rightPanel.Controls.Add(new Label() { Text = "Ngày đặt:", Location = new Point(20, 150) });
            txtDate = new TextBox() { Location = new Point(150, 150), Width = 300, ReadOnly = true };
            rightPanel.Controls.Add(txtDate);

            // ==== Total ====
            rightPanel.Controls.Add(new Label() { Text = "Tổng tiền:", Location = new Point(20, 190) });
            txtTotal = new TextBox() { Location = new Point(150, 190), Width = 300, ReadOnly = true };
            rightPanel.Controls.Add(txtTotal);

            // ==== Note ====
            rightPanel.Controls.Add(new Label() { Text = "Ghi chú:", Location = new Point(20, 230) });
            txtNote = new TextBox() { Location = new Point(150, 230), Width = 300, ReadOnly = true };
            rightPanel.Controls.Add(txtNote);

            // ==== Status ====
            rightPanel.Controls.Add(new Label() { Text = "Trạng thái:", Location = new Point(20, 270) });
            cboStatus = new ComboBox()
            {
                Location = new Point(150, 270),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[]
            {
                "0 - Chưa xử lý",
                "1 - Đang xử lý",
                "2 - Đang giao",
                "3 - Đã giao",
                "4 - Đã hủy"
            });
            rightPanel.Controls.Add(cboStatus);

            // ==== Update button ====
            btnUpdateStatus = new Button()
            {
                Text = "Cập nhật trạng thái",
                Location = new Point(150, 310),
                BackColor = Color.FromArgb(0, 150, 0),
                ForeColor = Color.White,
                Width = 200
            };
            btnUpdateStatus.Click += BtnUpdateStatus_Click;
            rightPanel.Controls.Add(btnUpdateStatus);

            // ==== Order items table ====
            Label lblItems = new Label()
            {
                Text = "Sản phẩm trong đơn",
                Location = new Point(20, 360),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            rightPanel.Controls.Add(lblItems);

            dgvOrderItems = new DataGridView()
            {
                Location = new Point(20, 400),
                Size = new Size(450, 290),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            rightPanel.Controls.Add(dgvOrderItems);
        }

        // ======================================
        // LOAD LIST OF ORDERS
        // ======================================
        private void LoadData()
        {
            var orders = _orderService.GetAllOrders();
            dgvOrders.DataSource = ConvertToOrderTable(orders);
        }

        private System.Data.DataTable ConvertToOrderTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();

            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("UserId", typeof(int));
            dt.Columns.Add("Ngày đặt", typeof(string));
            dt.Columns.Add("Trạng thái", typeof(string));
            dt.Columns.Add("Người nhận", typeof(string));
            dt.Columns.Add("SĐT", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(string));

            foreach (var el in elements)
            {
                int.TryParse(el.Element("Id")?.Value, out int id);
                int.TryParse(el.Element("MaNguoiDung")?.Value, out int userId);
                int.TryParse(el.Element("TrangThaiDonHang")?.Value, out int status);

                decimal.TryParse(el.Element("TongTien")?.Value, out decimal total);

                dt.Rows.Add(
                    id,
                    userId,
                    el.Element("NgayDatHang")?.Value ?? "",
                    GetOrderStatusText(status),
                    el.Element("NguoiNhan_Ten")?.Value ?? "",
                    el.Element("NguoiNhan_SDT")?.Value ?? "",
                    $"{total:N0} đ"
                );
            }

            return dt;
        }


        // ======================================
        // WHEN ORDER IS SELECTED
        // ======================================
        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
                return;

            var row = dgvOrders.SelectedRows[0];

            // ❗ BỎ QUA HÀNG RỖNG
            if (row.IsNewRow || row.Cells["Id"].Value == null)
                return;

            // ❗ KIỂM TRA GIÁ TRỊ ID HỢP LỆ
            if (!int.TryParse(row.Cells["Id"].Value.ToString(), out int orderId))
                return;

            var order = _orderService.GetOrderById(orderId);
            if (order == null) return;

            // Fill order info (all safe)
            txtCustomer.Text = order.Element("NguoiNhan_Ten")?.Value ?? "";
            txtPhone.Text = order.Element("NguoiNhan_SDT")?.Value ?? "";
            txtDate.Text = order.Element("NgayDatHang")?.Value ?? "";
            txtNote.Text = order.Element("GhiChu")?.Value ?? "";

            if (decimal.TryParse(order.Element("TongTien")?.Value, out decimal total))
                txtTotal.Text = $"{total:N0} đ";
            else
                txtTotal.Text = "0 đ";

            // Status
            if (int.TryParse(order.Element("TrangThaiDonHang")?.Value, out int st))
                cboStatus.SelectedIndex = st;
            else
                cboStatus.SelectedIndex = 0;

            LoadOrderItems(orderId);
        }


        private void LoadOrderItems(int orderId)
        {
            var items = _orderItemService.GetOrderItemsByOrderId(orderId);

            var dt = new System.Data.DataTable();
            dt.Columns.Add("Sản phẩm");
            dt.Columns.Add("Đơn giá");
            dt.Columns.Add("Số lượng");
            dt.Columns.Add("Thành tiền");

            foreach (var item in items)
            {
                string name = item.Element("ItemOrdered_TenSanPham")?.Value ?? "";

                decimal.TryParse(item.Element("DonGia")?.Value, out decimal price);
                int.TryParse(item.Element("SoLuong")?.Value, out int qty);

                dt.Rows.Add(name, $"{price:N0} đ", qty, $"{price * qty:N0} đ");
            }

            dgvOrderItems.DataSource = dt;
        }


        // ======================================
        // UPDATE ORDER STATUS
        // ======================================
        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0)
                return;

            var row = dgvOrders.SelectedRows[0];
            if (!int.TryParse(row.Cells["Id"].Value?.ToString(), out int orderId))
                return;

            int newStatus = cboStatus.SelectedIndex;

            _orderService.UpdateOrderStatus(orderId, newStatus);

            MessageBox.Show("Đã cập nhật trạng thái đơn hàng!");

            LoadData();
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
