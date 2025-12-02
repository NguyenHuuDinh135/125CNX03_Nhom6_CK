using System;
using System.Windows.Forms;
using System.Drawing;

namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    public partial class OrderCard : UserControl
    {
        public event EventHandler<OrderCardEventArgs> OrderSelected;

        public OrderCard()
        {
            InitializeComponent();
        }

        public void SetOrderInfo(int orderId, DateTime orderDate, decimal totalAmount, int status, string customerName)
        {
            lblOrderId.Text = $"Mã đơn: #{orderId}";
            lblOrderDate.Text = $"Ngày: {orderDate:dd/MM/yyyy}";
            lblTotalAmount.Text = $"Tổng: {totalAmount:N0}đ";
            lblStatus.Text = GetStatusText(status);
            lblCustomerName.Text = $"Khách hàng: {customerName}";

            SetStatusColor(status);

            this.Tag = orderId;
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

        private void SetStatusColor(int status)
        {
            switch (status)
            {
                case 0:
                    lblStatus.ForeColor = Color.Orange;
                    break;
                case 1:
                    lblStatus.ForeColor = Color.Blue;
                    break;
                case 2:
                    lblStatus.ForeColor = Color.Cyan;
                    break;
                case 3:
                    lblStatus.ForeColor = Color.Green;
                    break;
                case 4:
                    lblStatus.ForeColor = Color.Red;
                    break;
                default:
                    lblStatus.ForeColor = Color.Gray;
                    break;
            }
        }

        private void OrderCard_Click(object sender, EventArgs e)
        {
            if (Tag != null && int.TryParse(Tag.ToString(), out int orderId))
            {
                var args = new OrderCardEventArgs(orderId);
                OrderSelected?.Invoke(this, args);
            }
        }

        private void OrderCard_Load(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(300, 120);
            this.Click += OrderCard_Click;
        }
    }

    public class OrderCardEventArgs : EventArgs
    {
        public int OrderId { get; }

        public OrderCardEventArgs(int orderId)
        {
            OrderId = orderId;
        }
    }
}