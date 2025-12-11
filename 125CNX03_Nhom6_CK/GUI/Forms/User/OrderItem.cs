using System;
using System.Drawing;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderItem : UserControl
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }

        public OrderItem()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(this.Parent?.Width ?? 900, 100);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Order ID
            Label lblOrderId = new Label();
            lblOrderId.Text = $"Đơn hàng #{OrderId}";
            lblOrderId.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblOrderId.Location = new Point(20, 10);
            lblOrderId.Size = new Size(150, 20);
            this.Controls.Add(lblOrderId);

            // Order date
            Label lblOrderDate = new Label();
            lblOrderDate.Text = $"Ngày đặt: {OrderDate:dd/MM/yyyy HH:mm}";
            lblOrderDate.Font = new Font("Segoe UI", 9);
            lblOrderDate.Location = new Point(20, 35);
            lblOrderDate.Size = new Size(200, 20);
            this.Controls.Add(lblOrderDate);

            // Total amount
            Label lblTotal = new Label();
            lblTotal.Text = $"Tổng tiền: {TotalAmount:N0}đ";
            lblTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotal.Location = new Point(250, 35);
            lblTotal.Size = new Size(200, 20);
            this.Controls.Add(lblTotal);

            // Status
            Label lblStatus = new Label();
            lblStatus.Text = $"Trạng thái: {GetStatusText(Status)}";
            lblStatus.Font = new Font("Segoe UI", 9);
            lblStatus.Location = new Point(480, 35);
            lblStatus.Size = new Size(200, 20);
            this.Controls.Add(lblStatus);

            // View details button
            Button btnViewDetails = new Button();
            btnViewDetails.Text = "Xem chi tiết";
            btnViewDetails.Font = new Font("Segoe UI", 8);
            btnViewDetails.Size = new Size(80, 25);
            btnViewDetails.Location = new Point(800, 35);
            btnViewDetails.BackColor = Color.FromArgb(0, 174, 219);
            btnViewDetails.ForeColor = Color.White;
            btnViewDetails.FlatStyle = FlatStyle.Flat;
            btnViewDetails.FlatAppearance.BorderSize = 0;
            btnViewDetails.Cursor = Cursors.Hand;
            btnViewDetails.Click += BtnViewDetails_Click;
            this.Controls.Add(btnViewDetails);
        }

        private string GetStatusText(int status)
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

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            // Show order details form
            var orderDetailForm = new OrderDetailForm(OrderId);
            orderDetailForm.ShowDialog();
        }
    }
}