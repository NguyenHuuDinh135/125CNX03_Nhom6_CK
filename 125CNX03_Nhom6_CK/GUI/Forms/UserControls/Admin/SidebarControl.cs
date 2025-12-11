using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.Admin
{
    public partial class SidebarControl : UserControl
    {
        public event EventHandler<string> MenuItemClicked;

        // Color palette
        private readonly Color Primary = ColorTranslator.FromHtml("#4361ee");
        private readonly Color PrimaryHover = ColorTranslator.FromHtml("#3f37c9");
        private readonly Color SidebarBg = ColorTranslator.FromHtml("#1e293b");
        private readonly Color SidebarHover = ColorTranslator.FromHtml("#334155");

        private readonly List<SidebarButton> _buttons = new List<SidebarButton>();
        private SidebarButton _activeButton;

        public SidebarControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Left;
            this.Width = 260;
            this.BackColor = SidebarBg;
            this.DoubleBuffered = true;

            // Logo area
            var topPanel = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Color.Transparent };
            this.Controls.Add(topPanel);

            var logo = new Panel
            {
                Size = new Size(56, 56),
                Location = new Point(20, 18),
                BackColor = Primary,
                Cursor = Cursors.Hand
            };
            logo.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var b = new SolidBrush(Primary))
                    e.Graphics.FillEllipse(b, 0, 0, 55, 55);

                using (var f = new Font("Segoe UI", 20F, FontStyle.Bold))
                using (var b = new SolidBrush(Color.White))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString("6", f, b, new RectangleF(0, 0, 56, 56), sf);
            };
            topPanel.Controls.Add(logo);

            var nameLabel = new Label
            {
                Text = "NHÓM 6",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                Location = new Point(88, 20),
                AutoSize = true
            };
            topPanel.Controls.Add(nameLabel);

            var subLabel = new Label
            {
                Text = "Admin Dashboard",
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 10F),
                Location = new Point(88, 46),
                AutoSize = true
            };
            topPanel.Controls.Add(subLabel);

            // Separator
            this.Controls.Add(new Panel
            {
                Height = 1,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(50, 255, 255, 255),
                Margin = new Padding(0, 10, 0, 10)
            });

            // Menu container
            var menuPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                Padding = new Padding(8, 0, 8, 20)
            };
            this.Controls.Add(menuPanel);

            // Thêm menu (dùng font an toàn + fallback)
            AddMenuItem(menuPanel, "Dashboard", "Dashboard");
            AddMenuItem(menuPanel, "Sản phẩm", "ProductManagement");
            AddMenuItem(menuPanel, "Danh mục", "CategoryManagement");
            AddMenuItem(menuPanel, "Thương hiệu", "BrandManagement");
            AddMenuItem(menuPanel, "Người dùng", "UserManagement");
            AddMenuItem(menuPanel, "Đơn hàng", "OrderManagement");
            AddMenuItem(menuPanel, "Banner", "BannerManagement");
            AddMenuItem(menuPanel, "Bài viết", "Blog");
            AddMenuItem(menuPanel, "Liên hệ", "Contact");
            AddMenuItem(menuPanel, "Giỏ hàng", "Cart");

            // Active mặc định
            if (_buttons.Count > 0)
                SetActive(_buttons[0]);
        }

        private void AddMenuItem(Panel parent, string text, string tag)
        {
            var btn = new SidebarButton(text)
            {
                Tag = tag,
                Dock = DockStyle.Top,
                Height = 52
            };

            btn.Click += (s, e) =>
            {
                SetActive(btn);
                MenuItemClicked?.Invoke(this, tag);
            };

            parent.Controls.Add(btn);
            parent.Controls.SetChildIndex(btn, 0); // thêm lên trên cùng
            _buttons.Add(btn);
        }

        private void SetActive(SidebarButton btn)
        {
            _activeButton?.SetActive(false);
            _activeButton = btn;
            _activeButton?.SetActive(true);
        }

        // ===================================================================
        // SidebarButton – ĐÃ SỬA LỖI FONT, KHÔNG BAO GIỜ LỖI NỖI NỮA
        // ===================================================================
        private class SidebarButton : Control
        {
            private readonly string _text;
            private bool _hover;
            private bool _active;

            // Font an toàn 100%, có sẵn trên mọi Windows 10/11
            private static readonly Font IconFont = new Font("Segoe UI Symbol", 16F);
            private static readonly Font TextFont = new Font("Segoe UI", 11F);

            // Biểu tượng thay thế (nếu máy không có MDL2)
            private static readonly Dictionary<string, string> IconMap = new Dictionary<string, string>
            {
                { "Dashboard", "" },
                { "ProductManagement", "" },
                { "CategoryManagement", "" },
                { "BrandManagement", "" },
                { "UserManagement", "" },
                { "OrderManagement", "" },
                { "BannerManagement", "" },
                { "Blog", "" },
                { "Contact", "" },
                { "Cart", "" }
            };

            public SidebarButton(string text)
            {
                _text = text;
                this.DoubleBuffered = true;
                this.Cursor = Cursors.Hand;
                this.Padding = new Padding(16, 0, 16, 0);

                this.MouseEnter += (s, e) => { _hover = true; Invalidate(); };
                this.MouseLeave += (s, e) => { _hover = false; Invalidate(); };
            }

            public void SetActive(bool active)
            {
                _active = active;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Background khi hover hoặc active
                if (_active || _hover)
                {
                    Color bg = _active ? ColorTranslator.FromHtml("#4361ee") : ColorTranslator.FromHtml("#334155");
                    using (var brush = new SolidBrush(bg))
                    using (var path = GetRoundedPath(ClientRectangle, 10))
                        e.Graphics.FillPath(brush, path);
                }

                // Thanh trắng bên trái khi active
                if (_active)
                {
                    using (var b = new SolidBrush(Color.White))
                        e.Graphics.FillRectangle(b, 0, 8, 4, Height - 16);
                }

                // Icon
                string icon = GetIcon(_text);
                if (!string.IsNullOrEmpty(icon))
                {
                    Color iconColor = _active ? Color.White : Color.FromArgb(200, 200, 200);
                    using (var brush = new SolidBrush(iconColor))
                    {
                        var iconRect = new RectangleF(28, 0, 32, Height);
                        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        e.Graphics.DrawString(icon, IconFont, brush, iconRect, sf);
                    }
                }

                // Text
                Color textColor = _active ? Color.White : Color.Black;
                using (var brush = new SolidBrush(textColor))
                {
                    var textRect = new RectangleF(72, 0, Width - 80, Height);
                    var sf = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString(_text, TextFont, brush, textRect, sf);
                }
            }

            // Replace the switch expression in GetIcon with a switch statement for C# 7.3 compatibility
            private string GetIcon(string menuText)
            {
                switch (menuText)
                {
                    case "Dashboard": return "";
                    case "Sản phẩm": return "";
                    case "Danh mục": return "";
                    case "Thương hiệu": return "";
                    case "Người dùng": return "";
                    case "Đơn hàng": return "";
                    case "Banner": return "";
                    case "Bài viết": return "";
                    case "Liên hệ": return "";
                    case "Giỏ hàng": return "";
                    default: return "";
                }
            }

            private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
            {
                int d = radius * 2;
                var path = new GraphicsPath();
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    // Không dispose font static (dùng chung)
                }
                base.Dispose(disposing);
            }
        }
    }
}