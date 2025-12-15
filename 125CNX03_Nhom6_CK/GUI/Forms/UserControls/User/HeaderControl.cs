using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.User
{
    public class HeaderControl : UserControl
    {
        public event EventHandler<string> SearchTextChanged;
        public event EventHandler CartClicked;
        public event EventHandler LogoutClicked;
        public event EventHandler<string> NavClicked;

        private readonly Color Background = ColorTranslator.FromHtml("#ffffff");
        private readonly Color Accent = ColorTranslator.FromHtml("#e53e3e");
        private readonly Color BorderColor = ColorTranslator.FromHtml("#e5e5e5");
        private TextBox _searchBox;
        private Panel _searchPanel;
        private Label _logoLabel;
        private Label _cartLabel;
        private Label _cartCountLabel;
        private Button _profileButton;
        private Panel _mainPanel;
        private readonly string _searchPlaceholder = "Tìm kiếm sản phẩm...";

        public HeaderControl()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Top;
            this.Height = 110;
            this.BackColor = Background;
            this.DoubleBuffered = true;

            // Main header panel
            _mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Background,
                Padding = new Padding(20, 15, 20, 15)
            };
            this.Controls.Add(_mainPanel);

            // Bottom border
            var bottomBorder = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = BorderColor
            };
            this.Controls.Add(bottomBorder);

            CreateHeaderContent();
            this.Resize += (s, e) => LayoutControls();
        }

        private void CreateHeaderContent()
        {
            // Logo (left)
            _logoLabel = new Label
            {
                Text = "Nhom6",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(0, 18),
                Cursor = Cursors.Hand
            };
            _logoLabel.Click += (s, e) => NavClicked?.Invoke(this, "Home");
            _mainPanel.Controls.Add(_logoLabel);

            // Navigation links
            CreateNavigationLinks();

            // Search box with better styling
            CreateSearchBox();

            // Right side controls
            CreateRightControls();

            // Ensure correct initial layout
            LayoutControls();
        }

        private void CreateNavigationLinks()
        {
            int startX = 180;
            int y = 22;
            int spacing = 80;

            var navItems = new[] { "Home", "Blog", "Contact" };
            for (int i = 0; i < navItems.Length; i++)
            {
                var navLink = CreateNavLink(navItems[i], new Point(startX + (i * spacing), y));
                _mainPanel.Controls.Add(navLink);
            }
        }

        private Label CreateNavLink(string text, Point location)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Location = location,
                Cursor = Cursors.Hand
            };

            lbl.MouseEnter += (s, e) => lbl.ForeColor = Accent;
            lbl.MouseLeave += (s, e) => lbl.ForeColor = Color.FromArgb(64, 64, 64);
            lbl.Click += (s, e) => NavClicked?.Invoke(this, text);

            return lbl;
        }

        private void CreateSearchBox()
        {
            // Search container panel with border
            _searchPanel = new Panel
            {
                Size = new Size(450, 38),
                BackColor = Color.FromArgb(248, 248, 248),
                BorderStyle = BorderStyle.None
            };
            _searchPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, _searchPanel.Width - 1, _searchPanel.Height - 1);
                }
            };

            // Search icon label
            var searchIcon = new Label
            {
                Text = "🔍",
                Font = new Font("Segoe UI", 11F),
                AutoSize = true,
                Location = new Point(12, 9),
                BackColor = Color.Transparent
            };
            _searchPanel.Controls.Add(searchIcon);

            // Search textbox
            _searchBox = new TextBox
            {
                Location = new Point(40, 8),
                Size = new Size(395, 22),
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 248, 248)
            };
            _searchBox.Text = _searchPlaceholder;
            _searchBox.ForeColor = Color.Gray;

            _searchBox.GotFocus += (s, e) =>
            {
                if (_searchBox.Text == _searchPlaceholder)
                {
                    _searchBox.Text = string.Empty;
                    _searchBox.ForeColor = Color.Black;
                }
            };

            _searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_searchBox.Text))
                {
                    _searchBox.Text = _searchPlaceholder;
                    _searchBox.ForeColor = Color.Gray;
                }
            };

            _searchBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    SearchTextChanged?.Invoke(this, _searchBox.Text == _searchPlaceholder ? string.Empty : _searchBox.Text);
            };

            _searchPanel.Controls.Add(_searchBox);
            _mainPanel.Controls.Add(_searchPanel);
        }

        private void CreateRightControls()
        {
            // Cart icon
            _cartLabel = new Label
            {
                Text = "🛒",
                Font = new Font("Segoe UI", 18F),
                AutoSize = true,
                Cursor = Cursors.Hand
            };
            _cartLabel.Click += (s, e) => CartClicked?.Invoke(this, EventArgs.Empty);
            _mainPanel.Controls.Add(_cartLabel);

            // Cart count badge
            _cartCountLabel = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Accent,
                AutoSize = false,
                Size = new Size(18, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _cartCountLabel.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Accent))
                {
                    e.Graphics.FillEllipse(brush, 0, 0, _cartCountLabel.Width - 1, _cartCountLabel.Height - 1);
                }
                TextRenderer.DrawText(e.Graphics, _cartCountLabel.Text, _cartCountLabel.Font,
                    _cartCountLabel.ClientRectangle, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
            _mainPanel.Controls.Add(_cartCountLabel);

            // Profile button
            _profileButton = new Button
            {
                Text = "Tài khoản ▾",
                Font = new Font("Segoe UI", 9F),
                Size = new Size(110, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Cursor = Cursors.Hand
            };
            _profileButton.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
            _profileButton.FlatAppearance.BorderSize = 1;
            _profileButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(250, 250, 250);
            _profileButton.Click += ProfileButton_Click;
            _mainPanel.Controls.Add(_profileButton);

            // Ensure badge is on top and layout is correct
            _cartCountLabel.BringToFront();
            LayoutControls();
        }

        private void LayoutControls()
        {
            if (_searchPanel != null && _mainPanel.Width > 0)
            {
                // Center search box within the main panel area
                _searchPanel.Location = new Point((_mainPanel.Width - _searchPanel.Width) / 2, 16);
            }

            if (_cartLabel != null && _mainPanel.Width > 0)
            {
                int rightMargin = 20;
                // place profile button at right side
                _profileButton.Location = new Point(_mainPanel.Width - _profileButton.Width - rightMargin, 14);
                // place cart icon left of profile with fixed spacing
                _cartLabel.Location = new Point(_profileButton.Left - 48, 16);
                // place badge slightly above/right of cart icon so it is not overlapped
                _cartCountLabel.Location = new Point(_cartLabel.Right - 6, _cartLabel.Top - 6);

                // ensure badge sits above other controls
                _cartCountLabel.BringToFront();
            }
        }

        private void ProfileButton_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Hồ sơ", null, (s, ev) => NavClicked?.Invoke(this, "Profile"));
            menu.Items.Add("Đơn hàng", null, (s, ev) => NavClicked?.Invoke(this, "OrderHistory"));
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Đăng xuất", null, (s, ev) => LogoutClicked?.Invoke(this, EventArgs.Empty));
            menu.Show(_profileButton, new Point(0, _profileButton.Height));
        }

        public void UpdateCartCount(int count)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => UpdateCartCount(count)));
                return;
            }
            _cartCountLabel.Text = count.ToString();
            _cartCountLabel.Visible = count > 0;
            // reposition after update
            LayoutControls();
        }

        public void SetSearchText(string text)
        {
            _searchBox.Text = text;
            _searchBox.ForeColor = Color.Black;
        }
    }
}