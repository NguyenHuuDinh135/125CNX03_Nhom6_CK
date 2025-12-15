using System;
using System.Drawing;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderItem : UserControl
    {
        private int _orderId;
        private DateTime _orderDate;
        private decimal _totalAmount;
        private int _status;

        public int OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                UpdateOrderIdLabel();
            }
        }

        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                _orderDate = value;
                UpdateOrderDateLabel();
            }
        }

        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                UpdateTotalLabel();
            }
        }

        public int Status
        {
            get => _status;
            set
            {
                _status = value;
                UpdateStatusLabel();
            }
        }

        private Label _lblOrderId;
        private Label _lblOrderDate;
        private Label _lblTotal;
        private Label _lblStatus;
        private Button _btnViewDetails;

        public OrderItem()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(860, 110);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Margin = new Padding(0, 0, 0, 10);

            // Order ID - Left top
            _lblOrderId = new Label();
            _lblOrderId.Name = "lblOrderId";
            _lblOrderId.Text = "Đơn hàng #0";
            _lblOrderId.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            _lblOrderId.Location = new Point(20, 15);
            _lblOrderId.Size = new Size(200, 26);
            this.Controls.Add(_lblOrderId);

            // Order date - Left middle
            _lblOrderDate = new Label();
            _lblOrderDate.Name = "lblOrderDate";
            _lblOrderDate.Text = "Ngày đặt: --/--/----";
            _lblOrderDate.Font = new Font("Segoe UI", 9);
            _lblOrderDate.ForeColor = Color.FromArgb(100, 100, 100);
            _lblOrderDate.Location = new Point(20, 46);
            _lblOrderDate.Size = new Size(250, 15);
            this.Controls.Add(_lblOrderDate);

            // Total amount - Left bottom (dòng riêng để không bị cắt)
            _lblTotal = new Label();
            _lblTotal.Name = "lblTotal";
            _lblTotal.Text = "Tổng tiền: 0đ";
            _lblTotal.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblTotal.ForeColor = Color.FromArgb(219, 0, 0);
            _lblTotal.Location = new Point(20, 66);
            _lblTotal.Size = new Size(300, 22);
            _lblTotal.AutoSize = false;
            this.Controls.Add(_lblTotal);

            // Status - Center
            _lblStatus = new Label();
            _lblStatus.Name = "lblStatus";
            _lblStatus.Text = "Trạng thái: Chưa xử lý";
            _lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblStatus.Location = new Point(350, 46);
            _lblStatus.Size = new Size(260, 22);
            _lblStatus.ForeColor = GetStatusColor(0);
            this.Controls.Add(_lblStatus);

            // View details button - Right
            _btnViewDetails = new Button();
            _btnViewDetails.Text = "Xem chi tiết";
            _btnViewDetails.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnViewDetails.Size = new Size(110, 36);
            _btnViewDetails.Location = new Point(720, 37);
            _btnViewDetails.BackColor = Color.FromArgb(0, 174, 219);
            _btnViewDetails.ForeColor = Color.White;
            _btnViewDetails.FlatStyle = FlatStyle.Flat;
            _btnViewDetails.FlatAppearance.BorderSize = 0;
            _btnViewDetails.Cursor = Cursors.Hand;
            _btnViewDetails.Click += BtnViewDetails_Click;
            this.Controls.Add(_btnViewDetails);
        }

        private void UpdateOrderIdLabel()
        {
            if (_lblOrderId != null)
                _lblOrderId.Text = $"Đơn hàng #{_orderId}";
        }

        private void UpdateOrderDateLabel()
        {
            if (_lblOrderDate != null)
                _lblOrderDate.Text = $"Ngày đặt: {_orderDate:dd/MM/yyyy HH:mm}";
        }

        private void UpdateTotalLabel()
        {
            if (_lblTotal != null)
                _lblTotal.Text = $"Tổng tiền: {_totalAmount:N0}đ";
        }

        private void UpdateStatusLabel()
        {
            if (_lblStatus != null)
            {
                _lblStatus.Text = $"Trạng thái: {GetStatusText(_status)}";
                _lblStatus.ForeColor = GetStatusColor(_status);
            }
        }

        // Method để refresh toàn bộ UI (nếu cần)
        public void RefreshUI()
        {
            UpdateOrderIdLabel();
            UpdateOrderDateLabel();
            UpdateTotalLabel();
            UpdateStatusLabel();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_btnViewDetails != null)
            {
                _btnViewDetails.Location = new Point(this.Width - 140, 37);
            }
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

        private Color GetStatusColor(int status)
        {
            switch (status)
            {
                case 0: return Color.FromArgb(255, 152, 0); // Orange - Chưa xử lý
                case 1: return Color.FromArgb(33, 150, 243); // Blue - Đang xử lý
                case 2: return Color.FromArgb(156, 39, 176); // Purple - Đang giao
                case 3: return Color.FromArgb(76, 175, 80); // Green - Đã giao
                case 4: return Color.FromArgb(244, 67, 54); // Red - Đã hủy
                default: return Color.Gray;
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            // Không cho xem chi tiết nếu OrderId = 0 (invalid)
            if (_orderId <= 0)
            {
                MessageBox.Show("Đơn hàng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Show order details form
                var orderDetailForm = new OrderDetailForm(_orderId);
                orderDetailForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở chi tiết đơn hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}