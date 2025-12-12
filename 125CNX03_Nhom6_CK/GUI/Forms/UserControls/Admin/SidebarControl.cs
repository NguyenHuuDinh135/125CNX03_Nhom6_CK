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

        private readonly Color SidebarBg = ColorTranslator.FromHtml("#0f172a");
        private readonly Color ItemHover = ColorTranslator.FromHtml("#1e293b");
        private readonly Color ItemActive = ColorTranslator.FromHtml("#2563eb");
        private readonly Color TextNormal = ColorTranslator.FromHtml("#cbd5f5");
        private readonly Color TextActive = Color.White;

        private readonly List<SidebarItem> _items = new List<SidebarItem>();
        private SidebarItem _activeItem;

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

            // ================= ROOT LAYOUT =================
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 96));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(layout);

            // ================= HEADER =================
            Panel header = new Panel
            {
                Dock = DockStyle.Fill
            };

            Label logo = new Label
            {
                Text = "ADMIN",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(24, 20),
                AutoSize = true
            };

            Label sub = new Label
            {
                Text = "Laptop Store",
                Font = new Font("Segoe UI", 10),
                ForeColor = ColorTranslator.FromHtml("#94a3b8"),
                Location = new Point(26, 50),
                AutoSize = true
            };

            header.Controls.Add(logo);
            header.Controls.Add(sub);

            // ================= MENU =================
            Panel menuPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(8)
            };

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(menuPanel, 0, 1);

            AddMenu(menuPanel, "Dashboard", "\uE80F", "Dashboard");
            AddMenu(menuPanel, "Sản phẩm", "\uE719", "ProductManagement");
            AddMenu(menuPanel, "Danh mục", "\uE8B7", "CategoryManagement");
            AddMenu(menuPanel, "Thương hiệu", "\uE7C3", "BrandManagement");
            AddMenu(menuPanel, "Người dùng", "\uE77B", "UserManagement");
            AddMenu(menuPanel, "Đơn hàng", "\uE14C", "OrderManagement");
            AddMenu(menuPanel, "Banner", "\uE7AD", "BannerManagement");
            AddMenu(menuPanel, "Bài viết", "\uE8A5", "Blog");
            AddMenu(menuPanel, "Liên hệ", "\uE715", "Contact");

            if (_items.Count > 0)
                SetActive(_items[0]);
        }

        private void AddMenu(Panel parent, string text, string icon, string tag)
        {
            var item = new SidebarItem(text, icon)
            {
                Tag = tag,
                Dock = DockStyle.Top,
                Height = 48
            };

            item.Click += (s, e) =>
            {
                SetActive(item);
                MenuItemClicked?.Invoke(this, tag);
            };

            parent.Controls.Add(item);
            parent.Controls.SetChildIndex(item, 0);
            _items.Add(item);
        }

        private void SetActive(SidebarItem item)
        {
            _activeItem?.SetActive(false);
            _activeItem = item;
            _activeItem.SetActive(true);
        }

        // ===================================================
        // SIDEBAR ITEM
        // ===================================================
        private class SidebarItem : Control
        {
            private bool _hover;
            private bool _active;
            private readonly string _text;
            private readonly string _icon;

            private static readonly Font IconFont = new Font("Segoe MDL2 Assets", 16F);
            private static readonly Font TextFont = new Font("Segoe UI", 11F);

            public SidebarItem(string text, string icon)
            {
                _text = text;
                _icon = icon;

                this.Cursor = Cursors.Hand;
                this.DoubleBuffered = true;
                this.Padding = new Padding(12, 0, 12, 0);

                this.SetStyle(ControlStyles.Selectable, false);
                this.TabStop = false;

                this.MouseEnter += (s, e) => { _hover = true; Invalidate(); };
                this.MouseLeave += (s, e) => { _hover = false; Invalidate(); };
            }

            public void SetActive(bool active)
            {
                _active = active;
                Invalidate();
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);
                this.OnClick(EventArgs.Empty);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                if (_active || _hover)
                {
                    Color bg = _active
                        ? ColorTranslator.FromHtml("#2563eb")
                        : ColorTranslator.FromHtml("#1e293b");

                    using (var b = new SolidBrush(bg))
                        e.Graphics.FillRectangle(b, ClientRectangle);
                }

                if (_active)
                {
                    using (var bar = new SolidBrush(Color.White))
                        e.Graphics.FillRectangle(bar, 0, 8, 4, Height - 16);
                }

                // Icon
                using (var b = new SolidBrush(_active ? Color.White : ColorTranslator.FromHtml("#cbd5f5")))
                {
                    e.Graphics.DrawString(
                        _icon,
                        IconFont,
                        b,
                        new RectangleF(24, 0, 32, Height),
                        new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        });
                }

                // Text
                using (var b = new SolidBrush(_active ? Color.White : ColorTranslator.FromHtml("#cbd5f5")))
                {
                    e.Graphics.DrawString(
                        _text,
                        TextFont,
                        b,
                        new RectangleF(64, 0, Width - 64, Height),
                        new StringFormat
                        {
                            Alignment = StringAlignment.Near,
                            LineAlignment = StringAlignment.Center
                        });
                }
            }
        }
    }
}
