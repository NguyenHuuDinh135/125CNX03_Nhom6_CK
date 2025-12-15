using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class CartItem : UserControl
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }

        public int Quantity { get; set; }
        public string TotalPrice { get; set; }
        private PictureBox _pictureBox;

        // Events to notify parent form
        public event Action<int> RemoveClicked; // productId
        public event Action<int, int> QuantityChanged; // productId, newQuantity

        private NumericUpDown _numericUpDown;
        private Label _totalLabel;
        private Button _btnRemove; // Thêm reference để có thể điều chỉnh vị trí sau

        public CartItem()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(860, 80); // Đặt size cố định thay vì dựa vào Parent
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Product image
            _pictureBox = new PictureBox();
            _pictureBox.Size = new Size(60, 60);
            _pictureBox.Location = new Point(10, 10);
            _pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            _pictureBox.BackColor = Color.FromArgb(245, 245, 245);
            _pictureBox.Image = Properties.Resources.DefaultProduct;
            this.Controls.Add(_pictureBox);

            // Product details
            Label nameLabel = new Label();
            nameLabel.Name = "lblName";
            nameLabel.Text = ProductName ?? "Tên sản phẩm";
            nameLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            nameLabel.Location = new Point(80, 10);
            nameLabel.Size = new Size(400, 20);
            this.Controls.Add(nameLabel);

            Label priceLabel = new Label();
            priceLabel.Name = "lblPrice";
            priceLabel.Text = $"Giá: {Price}đ";
            priceLabel.Font = new Font("Segoe UI", 9);
            priceLabel.Location = new Point(80, 35);
            priceLabel.Size = new Size(100, 20);
            this.Controls.Add(priceLabel);

            // Quantity controls
            Label qtyLabel = new Label();
            qtyLabel.Text = "Số lượng:";
            qtyLabel.Font = new Font("Segoe UI", 9);
            qtyLabel.Location = new Point(200, 35);
            qtyLabel.Size = new Size(60, 20);
            this.Controls.Add(qtyLabel);

            _numericUpDown = new NumericUpDown();
            _numericUpDown.Value = Quantity > 0 ? Quantity : 1;
            _numericUpDown.Minimum = 1;
            _numericUpDown.Maximum = 9999;
            _numericUpDown.Size = new Size(60, 20);
            _numericUpDown.Location = new Point(260, 35);
            _numericUpDown.Font = new Font("Segoe UI", 9);
            _numericUpDown.ValueChanged += (s, e) =>
            {
                Quantity = (int)_numericUpDown.Value;
                UpdateTotalPrice();
                QuantityChanged?.Invoke(ProductId, Quantity);
            };
            this.Controls.Add(_numericUpDown);

            // Total price
            _totalLabel = new Label();
            _totalLabel.Name = "lblTotal";
            _totalLabel.Text = $"Tổng: {TotalPrice}đ";
            _totalLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _totalLabel.Location = new Point(350, 35);
            _totalLabel.Size = new Size(200, 20);
            this.Controls.Add(_totalLabel);

            // Remove button - Đặt vị trí cố định rõ ràng
            _btnRemove = new Button();
            _btnRemove.Text = "Xóa";
            _btnRemove.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            _btnRemove.Size = new Size(70, 30);
            _btnRemove.Location = new Point(770, 25); // Vị trí cố định
            _btnRemove.BackColor = Color.FromArgb(219, 0, 0);
            _btnRemove.ForeColor = Color.White;
            _btnRemove.FlatStyle = FlatStyle.Flat;
            _btnRemove.FlatAppearance.BorderSize = 0;
            _btnRemove.Cursor = Cursors.Hand;
            _btnRemove.Click += (s, e) =>
            {
                var result = MessageBox.Show(
                    "Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    RemoveClicked?.Invoke(ProductId);
                }
            };
            this.Controls.Add(_btnRemove);
        }

        // Override OnResize để điều chỉnh vị trí nút xóa khi resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_btnRemove != null)
            {
                _btnRemove.Location = new Point(this.Width - 90, 25);
            }
        }

        public void RefreshUI()
        {
            var nameLabel = this.Controls.Find("lblName", false).FirstOrDefault() as Label;
            if (nameLabel != null) nameLabel.Text = ProductName;

            var priceLabel = this.Controls.Find("lblPrice", false).FirstOrDefault() as Label;
            if (priceLabel != null) priceLabel.Text = $"Giá: {Price}đ";

            if (_numericUpDown != null) _numericUpDown.Value = Quantity > 0 ? Quantity : 1;
            LoadImage();
            UpdateTotalPrice();
        }

        private void LoadImage()
        {
            if (_pictureBox == null) return;

            if (!string.IsNullOrWhiteSpace(ImageUrl) &&
                Uri.IsWellFormedUriString(ImageUrl, UriKind.Absolute))
            {
                try
                {
                    _pictureBox.LoadAsync(ImageUrl);
                    _pictureBox.LoadCompleted += (s, e) =>
                    {
                        if (e.Error != null)
                            _pictureBox.Image = Properties.Resources.DefaultProduct;
                    };
                }
                catch
                {
                    _pictureBox.Image = Properties.Resources.DefaultProduct;
                }
            }
            else
            {
                _pictureBox.Image = Properties.Resources.DefaultProduct;
            }
        }

        private void UpdateTotalPrice()
        {
            decimal price = 0;
            decimal.TryParse(Price, out price);
            TotalPrice = (price * Quantity).ToString();
            if (_totalLabel != null)
                _totalLabel.Text = $"Tổng: {decimal.Parse(TotalPrice):N0}đ";
        }
    }
}