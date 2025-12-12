using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.UserControls.Admin
{
    public partial class HeaderControl : UserControl
    {
        public event EventHandler<string> SearchTextChanged;

        private readonly Color Background = ColorTranslator.FromHtml("#ffffff");
        private readonly Color TextPrimary = ColorTranslator.FromHtml("#1e293b");
        private readonly Color TextMuted = ColorTranslator.FromHtml("#64748b");
        private readonly Color InputFocusBg = ColorTranslator.FromHtml("#f1f5f9");
        private readonly Color Border = ColorTranslator.FromHtml("#e2e8f0");

        private Label _titleLabel;
        private RoundedTextBox _searchBox;

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
            this.Height = 72;
            this.BackColor = Background;
            this.Padding = new Padding(24, 16, 24, 12);
            this.DoubleBuffered = true;

            // Title
            _titleLabel = new Label
            {
                Text = "Bảng điều khiển",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = TextPrimary,
                AutoSize = true,
                Location = new Point(24, 20)
            };
            this.Controls.Add(_titleLabel);

            // Search box (dùng cho danh sách – dashboard thì không cần)
            _searchBox = new RoundedTextBox(InputFocusBg, TextMuted)
            {
                Size = new Size(360, 40),
                Placeholder = "Tìm kiếm...",
                IconGlyph = "\uE721"
            };
            _searchBox.TextChanged += (s, e) => SearchTextChanged?.Invoke(this, _searchBox.Text);
            this.Controls.Add(_searchBox);

            // Responsive
            this.Resize += (s, e) =>
            {
                _searchBox.Location = new Point(
                    this.ClientSize.Width - _searchBox.Width - 24,
                    16
                );
            };
        }

        // ================= RoundedTextBox =================
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

            public RoundedTextBox(Color focusBg, Color iconColor)
            {
                _focusBg = focusBg;
                _iconColor = iconColor;

                this.Height = 40;
                this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);
                this.BackColor = Color.White;

                _innerTextBox.BorderStyle = BorderStyle.None;
                _innerTextBox.Font = new Font("Segoe UI", 10F);
                _innerTextBox.Location = new Point(44, 11);

                _innerTextBox.TextChanged += (s, e) => { TextChanged?.Invoke(this, EventArgs.Empty); Invalidate(); };
                _innerTextBox.GotFocus += (s, e) => { _focused = true; Invalidate(); };
                _innerTextBox.LostFocus += (s, e) => { _focused = false; Invalidate(); };

                this.Controls.Add(_innerTextBox);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                _innerTextBox.Width = this.Width - 56;
                _innerTextBox.Location = new Point(44, (this.Height - _innerTextBox.Height) / 2);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                using (var bg = new SolidBrush(_focused ? _focusBg : Color.White))
                using (var gp = RoundedRect(0, 0, Width, Height, 10))
                    e.Graphics.FillPath(bg, gp);

                using (var pen = new Pen(BorderColor))
                using (var gp = RoundedRect(0, 0, Width - 1, Height - 1, 10))
                    e.Graphics.DrawPath(pen, gp);

                if (!string.IsNullOrEmpty(IconGlyph))
                {
                    var f = new Font("Segoe MDL2 Assets", 14F);
                    var b = new SolidBrush(_iconColor);
                    e.Graphics.DrawString(IconGlyph, f, b, new RectangleF(10, 0, 28, Height),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }

                if (string.IsNullOrEmpty(_innerTextBox.Text))
                {
                     var b = new SolidBrush(ColorTranslator.FromHtml("#94a3b8"));
                    var f = new Font("Segoe UI", 9F);
                    e.Graphics.DrawString(Placeholder, f, b, new RectangleF(44, 0, Width - 50, Height),
                        new StringFormat { LineAlignment = StringAlignment.Center });
                }
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                _innerTextBox.Focus();
            }

            private GraphicsPath RoundedRect(float x, float y, float w, float h, float r)
            {
                var gp = new GraphicsPath();
                float d = r * 2;
                gp.AddArc(x, y, d, d, 180, 90);
                gp.AddArc(x + w - d, y, d, d, 270, 90);
                gp.AddArc(x + w - d, y + h - d, d, d, 0, 90);
                gp.AddArc(x, y + h - d, d, d, 90, 90);
                gp.CloseFigure();
                return gp;
            }
        }
    }
}
