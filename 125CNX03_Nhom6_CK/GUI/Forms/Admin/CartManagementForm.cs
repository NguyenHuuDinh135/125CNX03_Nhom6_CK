using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class CartManagementForm : Form
    {
        private readonly IGioHangService _cartService;
        private readonly IChiTietGioHangService _cartItemService;

        private DataGridView dgvCarts;
        private DataGridView dgvCartItems;
        private NumericUpDown numProductId;
        private NumericUpDown numQty;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;

        public CartManagementForm()
        {
            InitializeComponent();
            _cartService = new GioHangService();
            _cartItemService = new ChiTietGioHangService();

            InitializeUI();
            LoadCarts();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý giỏ hàng";
            this.Size = new Size(1300, 800);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // ================== PANEL DANH SÁCH GIỎ HÀNG (TRÁI) ==================
            Panel leftPanel = new Panel()
            {
                Size = new Size(700, 720),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(leftPanel);

            Label lblCarts = new Label()
            {
                Text = "Danh sách giỏ hàng",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            leftPanel.Controls.Add(lblCarts);

            dgvCarts = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(650, 630),
                Font = new Font("Segoe UI", 10),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 30 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                ReadOnly = true
            };
            dgvCarts.SelectionChanged += DgvCarts_SelectionChanged;
            leftPanel.Controls.Add(dgvCarts);

            // ================== PANEL CHI TIẾT GIỎ HÀNG (PHẢI) ==================
            Panel rightPanel = new Panel()
            {
                Size = new Size(500, 720),
                Location = new Point(740, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(rightPanel);

            Label lblDetails = new Label()
            {
                Text = "Chi tiết giỏ hàng",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20)
            };
            rightPanel.Controls.Add(lblDetails);

            // DataGridView chi tiết giỏ hàng (ngắn hơn để chừa chỗ input + button)
            dgvCartItems = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(450, 500), // giảm chiều cao
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            rightPanel.Controls.Add(dgvCartItems);

            // ================== INPUT ProductId & Số lượng ==================
            Label lblProductId = new Label() { Text = "ProductId:", Location = new Point(20, 570), AutoSize = true };
            numProductId = new NumericUpDown() { Location = new Point(100, 565), Width = 80, Minimum = 1, Maximum = 99999 };
            rightPanel.Controls.Add(lblProductId);
            rightPanel.Controls.Add(numProductId);

            Label lblQty = new Label() { Text = "Số lượng:", Location = new Point(200, 570), AutoSize = true };
            numQty = new NumericUpDown() { Location = new Point(270, 565), Width = 60, Minimum = 1, Maximum = 1000 };
            rightPanel.Controls.Add(lblQty);
            rightPanel.Controls.Add(numQty);

            // ================== FLOWLAYOUTPANEL CHỨA NÚT ==================
            FlowLayoutPanel flowButtons = new FlowLayoutPanel()
            {
                Location = new Point(20, 610),
                Size = new Size(450, 35),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };
            btnAdd = new Button() { Text = "Thêm", Size = new Size(60, 30) };
            btnEdit = new Button() { Text = "Sửa", Size = new Size(60, 30) };
            btnDelete = new Button() { Text = "Xóa", Size = new Size(60, 30) };

            flowButtons.Controls.Add(btnAdd);
            flowButtons.Controls.Add(btnEdit);
            flowButtons.Controls.Add(btnDelete);

            rightPanel.Controls.Add(flowButtons);

            // ================== SỰ KIỆN NÚT ==================
            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;
        }
        private void LoadCarts()
        {
            var dt = new DataTable();
            dt.Columns.Add("CartId", typeof(int));
            dt.Columns.Add("UserId", typeof(int));
            dt.Columns.Add("Ngày cập nhật", typeof(string));
            dt.Columns.Add("Số lượng sản phẩm", typeof(int));
            dt.Columns.Add("Tổng tiền", typeof(decimal));

            var carts = _cartService.GetAllCarts();
            foreach (var cart in carts)
            {
                int cartId = int.Parse(cart.Element("Id").Value);
                int userId = int.Parse(cart.Element("MaNguoiDung").Value);
                var items = _cartItemService.GetCartItemsByCartId(cartId);
                int itemCount = items.Sum(x => int.Parse(x.Element("SoLuong").Value));
                decimal total = items.Sum(x => decimal.Parse(x.Element("DonGia").Value) * int.Parse(x.Element("SoLuong").Value));
                dt.Rows.Add(cartId, userId, cart.Element("NgayCapNhat").Value, itemCount, total);
            }

            dgvCarts.DataSource = dt;
        }

        private void DgvCarts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCarts.SelectedRows.Count == 0) return;
            int cartId = int.Parse(dgvCarts.SelectedRows[0].Cells["CartId"].Value.ToString());
            LoadCartItems(cartId);
        }

        private void LoadCartItems(int cartId)
        {
            var items = _cartItemService.GetCartItemsByCartId(cartId);
            var dt = new DataTable();
            dt.Columns.Add("ProductId", typeof(int));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Đơn giá", typeof(decimal));
            dt.Columns.Add("Số lượng", typeof(int));
            dt.Columns.Add("Thành tiền", typeof(decimal));

            foreach (var item in items)
            {
                int productId = int.Parse(item.Element("MaSanPham").Value);
                string productName = "Sản phẩm " + productId;
                decimal price = decimal.Parse(item.Element("DonGia").Value);
                int qty = int.Parse(item.Element("SoLuong").Value);
                dt.Rows.Add(productId, productName, price, qty, price * qty);
            }

            dgvCartItems.DataSource = dt;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (dgvCarts.SelectedRows.Count == 0) return;
            int cartId = int.Parse(dgvCarts.SelectedRows[0].Cells["CartId"].Value.ToString());
            int productId = (int)numProductId.Value;
            int qty = (int)numQty.Value;

            _cartItemService.AddCartItem(cartId, productId, qty);
            LoadCartItems(cartId);
            LoadCarts();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCarts.SelectedRows.Count == 0 || dgvCartItems.SelectedRows.Count == 0) return;
            int cartId = int.Parse(dgvCarts.SelectedRows[0].Cells["CartId"].Value.ToString());
            int productId = int.Parse(dgvCartItems.SelectedRows[0].Cells["ProductId"].Value.ToString());
            int qty = (int)numQty.Value;

            _cartItemService.UpdateCartItem(cartId, productId, qty);
            LoadCartItems(cartId);
            LoadCarts();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCarts.SelectedRows.Count == 0 || dgvCartItems.SelectedRows.Count == 0) return;
            int cartId = int.Parse(dgvCarts.SelectedRows[0].Cells["CartId"].Value.ToString());
            int productId = int.Parse(dgvCartItems.SelectedRows[0].Cells["ProductId"].Value.ToString());

            _cartItemService.RemoveCartItem(cartId, productId);
            LoadCartItems(cartId);
            LoadCarts();
        }
    }
}
