using System;
using System.Drawing;
using System.Windows.Forms;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class CartForm : Form
    {
        public CartForm()
        {
            InitializeComponent();
            InitializeUI();
            LoadCart();
        }

        private void InitializeUI()
        {
            this.Text = "Giỏ hàng";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create cart header
            Panel cartHeader = new Panel();
            cartHeader.Size = new Size(this.Width - 40, 60);
            cartHeader.Location = new Point(20, 20);
            cartHeader.BackColor = Color.White;
            cartHeader.BorderStyle = BorderStyle.FixedSingle;

            Label cartTitle = new Label();
            cartTitle.Text = "Giỏ hàng của bạn";
            cartTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            cartTitle.Location = new Point(20, 15);
            cartTitle.Size = new Size(200, 30);
            cartHeader.Controls.Add(cartTitle);

            this.Controls.Add(cartHeader);

            // Create cart items panel
            Panel cartItemsPanel = new Panel();
            cartItemsPanel.Size = new Size(this.Width - 40, 500);
            cartItemsPanel.Location = new Point(20, 100);
            cartItemsPanel.BackColor = Color.White;
            cartItemsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label itemsTitle = new Label();
            itemsTitle.Text = "Sản phẩm trong giỏ hàng";
            itemsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            itemsTitle.Location = new Point(20, 10);
            itemsTitle.Size = new Size(200, 20);
            cartItemsPanel.Controls.Add(itemsTitle);

            // Create flow layout for cart items
            FlowLayoutPanel cartItemsFlow = new FlowLayoutPanel();
            cartItemsFlow.Size = new Size(cartItemsPanel.Width - 40, cartItemsPanel.Height - 60);
            cartItemsFlow.Location = new Point(20, 40);
            cartItemsFlow.FlowDirection = FlowDirection.TopDown;
            cartItemsFlow.WrapContents = true;
            cartItemsFlow.AutoScroll = true;
            cartItemsFlow.BackColor = Color.White;

            // Add sample cart items
            for (int i = 0; i < 3; i++)
            {
                var cartItem = new CartItem();
                cartItem.ProductName = $"Sản phẩm mẫu {i + 1}";
                cartItem.Price = "1000000";
                cartItem.Quantity = 1;
                cartItem.TotalPrice = "1000000";

                cartItemsFlow.Controls.Add(cartItem);
            }

            cartItemsPanel.Controls.Add(cartItemsFlow);

            this.Controls.Add(cartItemsPanel);

            // Create checkout panel
            Panel checkoutPanel = new Panel();
            checkoutPanel.Size = new Size(this.Width - 40, 100);
            checkoutPanel.Location = new Point(20, 620);
            checkoutPanel.BackColor = Color.White;
            checkoutPanel.BorderStyle = BorderStyle.FixedSingle;

            Label totalLabel = new Label();
            totalLabel.Text = "Tổng tiền: 3,000,000đ";
            totalLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            totalLabel.Location = new Point(20, 20);
            totalLabel.Size = new Size(200, 30);
            checkoutPanel.Controls.Add(totalLabel);

            Button btnCheckout = new Button();
            btnCheckout.Text = "Thanh toán";
            btnCheckout.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCheckout.Size = new Size(100, 30);
            btnCheckout.Location = new Point(750, 20);
            btnCheckout.BackColor = Color.FromArgb(0, 174, 219);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Click += (s, e) =>
            {
                MessageBox.Show("Chức năng thanh toán đang phát triển.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };
            checkoutPanel.Controls.Add(btnCheckout);

            this.Controls.Add(checkoutPanel);
        }

        private void LoadCart()
        {
            // Load cart data from session or database
            // For now, we're using sample data
        }
    }
}