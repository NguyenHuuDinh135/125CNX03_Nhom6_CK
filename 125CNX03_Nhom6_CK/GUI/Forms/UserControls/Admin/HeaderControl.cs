using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.Admin
{
    public partial class HeaderControl : UserControl
    {
        public event EventHandler<string> SearchTextChanged;
        public event EventHandler AddNewItemClicked;

        private readonly Color Primary = ColorTranslator.FromHtml("#4361ee");
        private readonly Color PrimaryHover = ColorTranslator.FromHtml("#3f37c9");
        private readonly Color Background = ColorTranslator.FromHtml("#f8fafc");
        private readonly Color InputFocusBg = ColorTranslator.FromHtml("#f0f9ff");
        private readonly Color TextPrimary = ColorTranslator.FromHtml("#1e293b");
        private readonly Color Border = ColorTranslator.FromHtml("#e2e8f0");
        private readonly Color TextMuted = ColorTranslator.FromHtml("#64748b");

        private Label _titleLabel;
        private RoundedTextBox _searchBox;
        private FloatingButton _fabAdd;

        public string Title
        {
            get => _titleLabel.Text;
            set => _titleLabel.Text = value;
        }

        public HeaderControl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Top;
            this.Height = 88;
            this.BackColor = Background;
            this.Padding = new Padding(24, 12, 24, 12);
            this.DoubleBuffered = true;

            // Tiêu đề - sẽ thay đổi theo trang
            _titleLabel = new Label
            {
                Text = "Bảng điều khiển",
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(24, 22)
            };
            this.Controls.Add(_titleLabel);

            // Search box
            _searchBox = new RoundedTextBox(InputFocusBg, TextMuted)
            {
                Size = new Size(420, 44),
                Location = new Point(360, 22),
                Placeholder = "Tìm kiếm...",
                IconGlyph = "\uE721"
            };
            _searchBox.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, _searchBox.Text);
            _searchBox.EnterPressed += (s, __) => SearchTextChanged?.Invoke(this, _searchBox.Text);
            this.Controls.Add(_searchBox);

            // FAB Add
            _fabAdd = new FloatingButton(Primary, PrimaryHover)
            {
                Size = new Size(60, 60),
                IconGlyph = "+",
                Cursor = Cursors.Hand
            };
            _fabAdd.Click += (s, e) => AddNewItemClicked?.Invoke(this, EventArgs.Empty);
            this.Controls.Add(_fabAdd);

            // Responsive
            this.Resize += (s, e) =>
            {
                _fabAdd.Location = new Point(this.ClientSize.Width - _fabAdd.Width - 24, 14);
                int space = _titleLabel.Right + 40;
                int available = this.ClientSize.Width - space - 100;
                int x = space + (available - _searchBox.Width) / 2;
                _searchBox.Location = new Point(Math.Max(space, x), 22);
            };
        }

        // =====================================================================
        // RoundedTextBox – Ô nhập liệu bo tròn có icon và placeholder
        // =====================================================================
        private class RoundedTextBox : Control
        {
            public string Placeholder { get; set; } = "";
            public string IconGlyph { get; set; } = "";
            public Color BorderColor { get; set; } = ColorTranslator.FromHtml("#e2e8f0");

            private readonly Color _focusBg;
            private readonly Color _iconColor;
            private readonly TextBox _innerTextBox = new TextBox();
            private bool _focused;

            public override string Text
            {
                get => _innerTextBox.Text;
                set => _innerTextBox.Text = value;
            }

            public event EventHandler TextChanged;
            public event EventHandler EnterPressed;

            public RoundedTextBox(Color focusBg, Color iconColor)
            {
                _focusBg = focusBg;
                _iconColor = iconColor;

                this.Height = 44;
              

                // With this line:
                this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
                this.BackColor = Color.White;

                _innerTextBox.BorderStyle = BorderStyle.None;
                _innerTextBox.Location = new Point(48, 11);
                _innerTextBox.Font = new Font("Segoe UI", 10F);

                _innerTextBox.TextChanged += (s, e) => { TextChanged?.Invoke(this, EventArgs.Empty); Invalidate(); };
                _innerTextBox.GotFocus += (s, e) => { _focused = true; Invalidate(); };
                _innerTextBox.LostFocus += (s, e) => { _focused = false; Invalidate(); };
                _innerTextBox.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        EnterPressed?.Invoke(this, EventArgs.Empty);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                };

                this.Controls.Add(_innerTextBox);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                _innerTextBox.Width = this.Width - 64;
                _innerTextBox.Location = new Point(48, (this.Height - _innerTextBox.Height) / 2 + 2);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Background
                using (var brush = new SolidBrush(_focused ? _focusBg : Color.White))
                using (var gp = CreateRoundedRect(0, 0, Width, Height, 12))
                    e.Graphics.FillPath(brush, gp);

                // Border
                using (var pen = new Pen(BorderColor, 1.4f))
                using (var gp = CreateRoundedRect(0, 0, Width - 1, Height - 1, 12))
                    e.Graphics.DrawPath(pen, gp);

                // Icon
                if (!string.IsNullOrEmpty(IconGlyph))
                {
                    using (var f = new Font("Segoe MDL2 Assets", 16F))
                    using (var b = new SolidBrush(_iconColor))
                    {
                        e.Graphics.DrawString(IconGlyph, f, b, new RectangleF(12, 0, 32, Height),
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }

                // Placeholder
                if (string.IsNullOrEmpty(_innerTextBox.Text) && !string.IsNullOrEmpty(Placeholder))
                {
                    using (var b = new SolidBrush(ColorTranslator.FromHtml("#94a3b8")))
                    using (var f = new Font("Segoe UI", 10F))
                    {
                        var rect = new RectangleF(48, 0, Width - 56, Height);
                        e.Graphics.DrawString(Placeholder, f, b, rect,
                            new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                    }
                }
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                _innerTextBox.Focus();
            }

            protected override bool IsInputKey(Keys keyData) => true;

            private GraphicsPath CreateRoundedRect(float x, float y, float width, float height, float radius)
            {
                var gp = new GraphicsPath();
                float d = radius * 2;
                gp.AddArc(x, y, d, d, 180, 90);
                gp.AddArc(x + width - d, y, d, d, 270, 90);
                gp.AddArc(x + width - d, y + height - d, d, d, 0, 90);
                gp.AddArc(x, y + height - d, d, d, 90, 90);
                gp.CloseFigure();
                return gp;
            }
        }

        // =====================================================================
        // FloatingButton – Nút tròn nổi (FAB)
        // =====================================================================
        private class FloatingButton : Control
        {
            public string IconGlyph { get; set; } = "+";

            private readonly Color _normalColor;
            private readonly Color _hoverColor;
            private bool _isHover;
            private readonly Font _glyphFont = new Font("Segoe UI", 28F, FontStyle.Bold);

            public FloatingButton(Color normal, Color hover)
            {
                _normalColor = normal;
                _hoverColor = hover;
                this.Size = new Size(60, 60);
                this.Cursor = Cursors.Hand;
                this.DoubleBuffered = true;

                this.MouseEnter += (s, e) => { _isHover = true; Invalidate(); };
                this.MouseLeave += (s, e) => { _isHover = false; Invalidate(); };
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // Bóng đổ nhẹ
                using (var shadow = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                    e.Graphics.FillEllipse(shadow, 8, 10, Width - 16, Height - 16);

                // Nền nút
                Color back = _isHover ? _hoverColor : _normalColor;
                using (var brush = new SolidBrush(back))
                    e.Graphics.FillEllipse(brush, 0, 0, Width - 1, Height - 1);

                // Icon +
                using (var brush = new SolidBrush(Color.White))
                {
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    e.Graphics.DrawString(IconGlyph, _glyphFont, brush, ClientRectangle, sf);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing) _glyphFont.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}