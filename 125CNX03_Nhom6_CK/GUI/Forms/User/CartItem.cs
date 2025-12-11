using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class CartItem : UserControl
    {
        public string ProductName { get; set; }
        public string Price { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }

        public CartItem()
        {
            InitializeComponent();
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
            pictureBox.Image = Properties.Resources.DefaultProductImage;
            this.Controls.Add(pictureBox);

            // Product details
            Label nameLabel = new Label();
            nameLabel.Text = ProductName ?? "Tên sản phẩm";
            nameLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            nameLabel.Location = new Point(80, 10);
            nameLabel.Size = new Size(200, 20);
            this.Controls.Add(nameLabel);

            Label priceLabel = new Label();
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

            NumericUpDown numericUpDown = new NumericUpDown();
            numericUpDown.Value = Quantity;
            numericUpDown.Minimum = 1;
            numericUpDown.Maximum = 99;
            numericUpDown.Size = new Size(50, 20);
            numericUpDown.Location = new Point(260, 35);
            numericUpDown.Font = new Font("Segoe UI", 9);
            numericUpDown.ValueChanged += (s, e) =>
            {
                Quantity = (int)numericUpDown.Value;
                UpdateTotalPrice();
            };
            this.Controls.Add(numericUpDown);

            // Total price
            Label totalLabel = new Label();
            totalLabel.Text = $"Tổng: {TotalPrice}đ";
            totalLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            totalLabel.Location = new Point(350, 35);
            totalLabel.Size = new Size(100, 20);
            this.Controls.Add(totalLabel);

            // Remove button
            Button btnRemove = new Button();
            btnRemove.Text = "Xóa";
            btnRemove.Font = new Font("Segoe UI", 8);
            btnRemove.Size = new Size(50, 20);
            btnRemove.Location = new Point(850, 35);
            btnRemove.BackColor = Color.FromArgb(219, 0, 0);
            btnRemove.ForeColor = Color.White;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += (s, e) =>
            {
                // Remove item from cart
                this.Parent?.Controls.Remove(this);
            };
            this.Controls.Add(btnRemove);
        }

        private void UpdateTotalPrice()
        {
            TotalPrice = (decimal.Parse(Price) * Quantity).ToString();
            var totalLabel = this.Controls.OfType<Label>().FirstOrDefault(l => l.Text.Contains("Tổng:"));
            if (totalLabel != null)
                totalLabel.Text = $"Tổng: {TotalPrice}đ";
        }
    }
}