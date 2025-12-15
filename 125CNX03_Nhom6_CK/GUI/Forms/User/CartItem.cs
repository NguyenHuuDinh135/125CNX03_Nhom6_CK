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
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }

        // Events to notify parent form
        public event Action<int> RemoveClicked; // productId
        public event Action<int, int> QuantityChanged; // productId, newQuantity

        private NumericUpDown _numericUpDown;
        private Label _totalLabel;

        public CartItem()
        {
            InitializeComponent();
            // UI will be configured when properties are set; still initialize generic UI
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(this.Parent?.Width ?? 900, 80);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Product image
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(60, 60);
            pictureBox.Location = new Point(10, 10);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.BackColor = Color.FromArgb(245, 245, 245);
            pictureBox.Image = Properties.Resources.DefaultProduct;
            this.Controls.Add(pictureBox);

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

            // Remove button
            Button btnRemove = new Button();
            btnRemove.Text = "Xóa";
            btnRemove.Font = new Font("Segoe UI", 8);
            btnRemove.Size = new Size(60, 26);
            btnRemove.Location = new Point(this.Width - 80, 27);
            btnRemove.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRemove.BackColor = Color.FromArgb(219, 0, 0);
            btnRemove.ForeColor = Color.White;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += (s, e) =>
            {
                RemoveClicked?.Invoke(ProductId);
            };
            this.Controls.Add(btnRemove);
        }

        public void RefreshUI()
        {
            var nameLabel = this.Controls.Find("lblName", false).FirstOrDefault() as Label;
            if (nameLabel != null) nameLabel.Text = ProductName;

            var priceLabel = this.Controls.Find("lblPrice", false).FirstOrDefault() as Label;
            if (priceLabel != null) priceLabel.Text = $"Giá: {Price}đ";

            if (_numericUpDown != null) _numericUpDown.Value = Quantity > 0 ? Quantity : 1;

            UpdateTotalPrice();
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