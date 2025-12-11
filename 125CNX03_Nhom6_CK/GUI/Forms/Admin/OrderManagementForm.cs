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

        public OrderManagementForm()
        {
            InitializeComponent();
            _orderService = new DonHangService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý đơn hàng";
            this.Size = new Size(1200, 800);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create data grid panel
            Panel gridPanel = new Panel();
            gridPanel.Size = new Size(this.Width - 40, this.Height - 60);
            gridPanel.Location = new Point(20, 20);
            gridPanel.BackColor = Color.White;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;

            Label gridTitle = new Label();
            gridTitle.Text = "Danh sách đơn hàng";
            gridTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            gridTitle.Location = new Point(20, 20);
            gridTitle.Size = new Size(200, 30);
            gridPanel.Controls.Add(gridTitle);

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size(gridPanel.Width - 40, gridPanel.Height - 60);
            dataGridView.Location = new Point(20, 60);
            dataGridView.Font = new Font("Segoe UI", 10);
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 30;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            gridPanel.Controls.Add(dataGridView);

            this.Controls.Add(gridPanel);
        }

        private void LoadData()
        {
            var orders = _orderService.GetAllOrders();
            var dataGridView = this.Controls[0].Controls[1] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = null;
                dataGridView.DataSource = ConvertToOrderTable(orders);
            }
        }

        private System.Data.DataTable ConvertToOrderTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Mã người dùng", typeof(int));
            dt.Columns.Add("Ngày đặt hàng", typeof(DateTime));
            dt.Columns.Add("Trạng thái", typeof(int));
            dt.Columns.Add("Người nhận", typeof(string));
            dt.Columns.Add("SĐT người nhận", typeof(string));
            dt.Columns.Add("Tổng tiền", typeof(decimal));
            dt.Columns.Add("Ghi chú", typeof(string));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    int.Parse(element.Element("MaNguoiDung").Value),
                    DateTime.Parse(element.Element("NgayDatHang").Value),
                    int.Parse(element.Element("TrangThaiDonHang").Value),
                    element.Element("NguoiNhan_Ten").Value,
                    element.Element("NguoiNhan_SDT").Value,
                    decimal.Parse(element.Element("TongTien").Value),
                    element.Element("GhiChu").Value
                );
            }

            return dt;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];

                // You can add code here to show order details in a separate panel
                // For now, we just show a message
                var orderId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");
                var order = _orderService.GetOrderById(orderId);

                if (order != null)
                {
                    var details = $"Đơn hàng #{orderId}\n" +
                                  $"Khách hàng: {order.Element("NguoiNhan_Ten").Value}\n" +
                                  $"Tổng tiền: {decimal.Parse(order.Element("TongTien").Value):N0}đ\n" +
                                  $"Trạng thái: {GetOrderStatusText(int.Parse(order.Element("TrangThaiDonHang").Value))}";

                    MessageBox.Show(details, "Chi tiết đơn hàng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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