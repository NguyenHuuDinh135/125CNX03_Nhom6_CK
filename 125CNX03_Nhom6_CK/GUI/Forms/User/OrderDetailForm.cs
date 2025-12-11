using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderDetailForm : Form
    {
        private readonly IDonHangService _orderService;
        private readonly IChiTietDonHangService _orderItemService;

        public OrderDetailForm(int orderId)
        {
            InitializeComponent();
            _orderService = new DonHangService();
            _orderItemService = new ChiTietDonHangService();

            LoadOrderDetails(orderId);
        }

        private void LoadOrderDetails(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order != null)
            {
                InitializeUI(order);
            }
        }

        private void InitializeUI(XElement order)
        {
            this.Text = $"Chi tiết đơn hàng #{order.Element("Id").Value}";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create header
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.Width - 40, 80);
            headerPanel.Location = new Point(20, 20);
            headerPanel.BackColor = Color.White;
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            Label headerTitle = new Label();
            headerTitle.Text = $"Chi tiết đơn hàng #{order.Element("Id").Value}";
            headerTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            headerTitle.Location = new Point(20, 20);
            headerTitle.Size = new Size(300, 30);
            headerPanel.Controls.Add(headerTitle);

            Label orderDateLabel = new Label();
            orderDateLabel.Text = $"Ngày đặt: {DateTime.Parse(order.Element("NgayDatHang").Value):dd/MM/yyyy HH:mm}";
            orderDateLabel.Font = new Font("Segoe UI", 10);
            orderDateLabel.Location = new Point(20, 50);
            orderDateLabel.Size = new Size(200, 20);
            headerPanel.Controls.Add(orderDateLabel);

            this.Controls.Add(headerPanel);

            // Create order info panel
            Panel infoPanel = new Panel();
            infoPanel.Size = new Size(this.Width - 40, 120);
            infoPanel.Location = new Point(20, 120);
            infoPanel.BackColor = Color.White;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;

            Label nameLabel = new Label();
            nameLabel.Text = $"Khách hàng: {order.Element("NguoiNhan_Ten").Value}";
            nameLabel.Font = new Font("Segoe UI", 10);
            nameLabel.Location = new Point(20, 20);
            nameLabel.Size = new Size(300, 20);
            infoPanel.Controls.Add(nameLabel);

            Label phoneLabel = new Label();
            phoneLabel.Text = $"SĐT: {order.Element("NguoiNhan_SDT").Value}";
            phoneLabel.Font = new Font("Segoe UI", 10);
            phoneLabel.Location = new Point(20, 45);
            phoneLabel.Size = new Size(200, 20);
            infoPanel.Controls.Add(phoneLabel);

            Label addressLabel = new Label();
            addressLabel.Text = $"Địa chỉ: {order.Element("DiaChi_Duong").Value}, {order.Element("DiaChi_ThanhPho").Value}, {order.Element("DiaChi_Tinh").Value}";
            addressLabel.Font = new Font("Segoe UI", 10);
            addressLabel.Location = new Point(20, 70);
            addressLabel.Size = new Size(500, 20);
            infoPanel.Controls.Add(addressLabel);

            Label statusLabel = new Label();
            statusLabel.Text = $"Trạng thái: {GetOrderStatusText(int.Parse(order.Element("TrangThaiDonHang").Value))}";
            statusLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            statusLabel.Location = new Point(20, 95);
            statusLabel.Size = new Size(200, 20);
            infoPanel.Controls.Add(statusLabel);

            this.Controls.Add(infoPanel);

            // Create items panel
            Panel itemsPanel = new Panel();
            itemsPanel.Size = new Size(this.Width - 40, 250);
            itemsPanel.Location = new Point(20, 260);
            itemsPanel.BackColor = Color.White;
            itemsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label itemsTitle = new Label();
            itemsTitle.Text = "Danh sách sản phẩm";
            itemsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            itemsTitle.Location = new Point(20, 20);
            itemsTitle.Size = new Size(200, 20);
            itemsPanel.Controls.Add(itemsTitle);

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size(itemsPanel.Width - 40, itemsPanel.Height - 60);
            dataGridView.Location = new Point(20, 50);
            dataGridView.Font = new Font("Segoe UI", 10);
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 30;

            var orderItems = _orderItemService.GetOrderItemsByOrderId(int.Parse(order.Element("Id").Value));
            dataGridView.DataSource = ConvertToOrderItemTable(orderItems);

            itemsPanel.Controls.Add(dataGridView);

            this.Controls.Add(itemsPanel);

            // Create total panel
            Panel totalPanel = new Panel();
            totalPanel.Size = new Size(this.Width - 40, 60);
            totalPanel.Location = new Point(20, 530);
            totalPanel.BackColor = Color.White;
            totalPanel.BorderStyle = BorderStyle.FixedSingle;

            Label totalLabel = new Label();
            totalLabel.Text = $"Tổng tiền: {decimal.Parse(order.Element("TongTien").Value):N0}đ";
            totalLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            totalLabel.ForeColor = Color.Red;
            totalLabel.Location = new Point(20, 20);
            totalLabel.Size = new Size(300, 20);
            totalPanel.Controls.Add(totalLabel);

            this.Controls.Add(totalPanel);
        }

        private System.Data.DataTable ConvertToOrderItemTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Đơn giá", typeof(decimal));
            dt.Columns.Add("Số lượng", typeof(int));
            dt.Columns.Add("Thành tiền", typeof(decimal));

            foreach (var element in elements)
            {
                var unitPrice = decimal.Parse(element.Element("DonGia").Value);
                var quantity = int.Parse(element.Element("SoLuong").Value);
                var totalPrice = unitPrice * quantity;

                dt.Rows.Add(
                    element.Element("ItemOrdered_TenSanPham").Value,
                    unitPrice,
                    quantity,
                    totalPrice
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