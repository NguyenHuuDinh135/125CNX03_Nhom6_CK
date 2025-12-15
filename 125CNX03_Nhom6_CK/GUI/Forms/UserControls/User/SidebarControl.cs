using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.User
{
    public class SidebarControl : UserControl
    {
        public event EventHandler<string> MenuItemClicked;

        private readonly Color Bg = ColorTranslator.FromHtml("#f8f9fa");
        private readonly Color HoverColor = ColorTranslator.FromHtml("#e9ecef");
        private readonly Color ActiveColor = ColorTranslator.FromHtml("#e53e3e");
        private readonly Color TextColor = ColorTranslator.FromHtml("#495057");
        private readonly List<Button> _buttons = new List<Button>();
        private Button _activeButton = null;

        public SidebarControl()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Left;
            this.Width = 200;
            this.BackColor = Bg;
            this.DoubleBuffered = true;

            // Logo area
            var logoPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.White
            };

            var logo = new Label
            {
                Text = "Nhom6",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.Black,
                Location = new Point(20, 25),
                AutoSize = true
            };
            logoPanel.Controls.Add(logo);
            this.Controls.Add(logoPanel);

            // Menu container
            var menuPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Bg,
                Padding = new Padding(10, 20, 10, 10),
                AutoScroll = true
            };

            int yPos = 0;
            AddMenuButton(menuPanel, "🏠  Home", ref yPos);
            AddMenuButton(menuPanel, "📂  Danh mục", ref yPos);
            AddMenuButton(menuPanel, "💻  Sản phẩm", ref yPos);
            AddMenuButton(menuPanel, "📝  Blog", ref yPos);
            AddMenuButton(menuPanel, "📞  Liên hệ", ref yPos);
            AddMenuButton(menuPanel, "🛒  Giỏ hàng", ref yPos);

            this.Controls.Add(menuPanel);

            // Bottom logout button
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Bg,
                Padding = new Padding(10)
            };

            var btnLogout = CreateStyledButton("🚪  Đăng xuất");
            btnLogout.Dock = DockStyle.Fill;
            btnLogout.BackColor = Color.FromArgb(220, 53, 69);
            btnLogout.ForeColor = Color.White;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 35, 51);
            btnLogout.Click += (s, e) => MenuItemClicked?.Invoke(this, "Logout");

            bottomPanel.Controls.Add(btnLogout);
            this.Controls.Add(bottomPanel);
        }

        private void AddMenuButton(Panel container, string text, ref int yPos)
        {
            var btn = CreateStyledButton(text);
            btn.Location = new Point(0, yPos);
            btn.Width = container.Width - 20;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            string menuKey = ExtractMenuKey(text);
            btn.Click += (s, e) =>
            {
                SetActiveButton(btn);
                MenuItemClicked?.Invoke(this, menuKey);
            };

            container.Controls.Add(btn);
            _buttons.Add(btn);
            yPos += 48;
        }

        private Button CreateStyledButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(180, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Transparent,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = HoverColor;

            btn.MouseEnter += (s, e) =>
            {
                if (btn != _activeButton)
                    btn.BackColor = HoverColor;
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn != _activeButton)
                    btn.BackColor = Color.Transparent;
            };

            return btn;
        }

        private void SetActiveButton(Button button)
        {
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.Transparent;
                _activeButton.ForeColor = TextColor;
            }

            _activeButton = button;
            _activeButton.BackColor = Color.FromArgb(255, 235, 235);
            _activeButton.ForeColor = ActiveColor;
        }

        private string ExtractMenuKey(string text)
        {
            // Remove emoji and extract menu key
            string cleaned = text.Trim();
            if (cleaned.Contains("Home")) return "Home";
            if (cleaned.Contains("Danh mục")) return "Danh mục";
            if (cleaned.Contains("Sản phẩm")) return "Sản phẩm";
            if (cleaned.Contains("Blog")) return "Blog";
            if (cleaned.Contains("Liên hệ")) return "Liên hệ";
            if (cleaned.Contains("Giỏ hàng")) return "Giỏ hàng";
            return text;
        }
    }
}