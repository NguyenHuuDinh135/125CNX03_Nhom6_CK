using System;
using System.Drawing;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.Admin
{
    public partial class HeaderControl : UserControl
    {
        public event EventHandler<string> SearchTextChanged;
        public event EventHandler<string> AddNewItem;

        public string Title
        {
            get { return _titleLabel.Text; }
            set { _titleLabel.Text = value; }
        }

        private Label _titleLabel;

        public HeaderControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Size = new Size(950, 80);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;

            // Title
            _titleLabel = new Label();
            _titleLabel.Text = "Bảng điều khiển";
            _titleLabel.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            _titleLabel.Location = new Point(20, 25);
            _titleLabel.Size = new Size(300, 30);
            this.Controls.Add(_titleLabel);

            // Search box
            TextBox txtSearch = new TextBox();
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.Size = new Size(200, 30);
            txtSearch.Location = new Point(350, 25);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Padding = new Padding(5);
            txtSearch.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, txtSearch.Text);
            this.Controls.Add(txtSearch);

            // Add new item button
            Button btnAdd = new Button();
            btnAdd.Text = "Thêm mới";
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Location = new Point(750, 25);
            btnAdd.BackColor = Color.FromArgb(0, 174, 219);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += (s, e) => AddNewItem?.Invoke(this, "");
            this.Controls.Add(btnAdd);
        }
    }
}