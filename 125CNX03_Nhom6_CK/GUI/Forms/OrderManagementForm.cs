using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class OrderManagementForm : Form
    {
        private readonly IDonHangService _orderService;
        private readonly INguoiDungService _userService;

        public OrderManagementForm()
        {
            InitializeComponent();
            _orderService = new DonHangService();
            _userService = new NguoiDungService();
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewOrders.DataSource = null;
            var orders = _orderService.GetAllOrders();
            dataGridViewOrders.DataSource = ConvertToDataTable(orders);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
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
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    int.Parse(element.Element("MaNguoiDung")?.Value ?? "0"),
                    DateTime.Parse(element.Element("NgayDatHang")?.Value ?? DateTime.Now.ToString()),
                    int.Parse(element.Element("TrangThaiDonHang")?.Value ?? "0"),
                    element.Element("NguoiNhan_Ten")?.Value ?? "",
                    element.Element("NguoiNhan_SDT")?.Value ?? "",
                    decimal.Parse(element.Element("TongTien")?.Value ?? "0"),
                    element.Element("GhiChu")?.Value ?? ""
                );
            }

            return dt;
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0];
                var orderId = (int)selectedRow.Cells["Id"].Value;
                var newStatus = int.Parse(cboStatus.SelectedValue.ToString());

                _orderService.UpdateOrderStatus(orderId, newStatus);
                LoadData();
                MessageBox.Show("Cập nhật trạng thái đơn hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn đơn hàng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewOrders.SelectedRows[0];
                var orderId = (int)selectedRow.Cells["Id"].Value;

                var order = _orderService.GetOrderById(orderId);
                if (order != null)
                {
                    cboStatus.SelectedValue = int.Parse(order.Element("TrangThaiDonHang").Value);
                }
            }
        }

        private void OrderManagementForm_Load(object sender, EventArgs e)
        {
            cboStatus.DataSource = new[] {
                new { Value = 0, Text = "Chưa xử lý" },
                new { Value = 1, Text = "Đang xử lý" },
                new { Value = 2, Text = "Đang giao" },
                new { Value = 3, Text = "Đã giao" },
                new { Value = 4, Text = "Đã hủy" }
            };
            cboStatus.DisplayMember = "Text";
            cboStatus.ValueMember = "Value";
        }
    }
}