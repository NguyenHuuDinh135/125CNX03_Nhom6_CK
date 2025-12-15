using System;
using System.Drawing;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public class UserBaseForm : Form
    {
        protected readonly Color Primary = ColorTranslator.FromHtml("#0ab4dd");
        protected readonly Color Accent = ColorTranslator.FromHtml("#0b87b6");
        protected readonly Color Surface = ColorTranslator.FromHtml("#ffffff");
        protected readonly Color Muted = ColorTranslator.FromHtml("#64748b");
        protected readonly Font BaseFont = new Font("Segoe UI", 10F);

        public UserBaseForm()
        {
            this.BackColor = Surface;
            this.Font = BaseFont;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        // Helper: create a label with consistent styling
        protected Label CreateLabel(string text, Point location, int width = 100)
        {
            return new Label
            {
                Text = text,
                Font = new Font(BaseFont.FontFamily, 9F),
                ForeColor = Muted,
                Location = location,
                Size = new Size(width, 20)
            };
        }

        // Helper: create textbox
        protected TextBox CreateTextBox(string name, Point location, Size size, bool isPassword = false)
        {
            return new TextBox
            {
                Name = name,
                Font = BaseFont,
                Size = size,
                Location = location,
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = isPassword
            };
        }

        // Helper: create button
        protected Button CreateButton(string text, Point location, Size size, Color backColor, EventHandler onClick, bool bold = true)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font(BaseFont.FontFamily, BaseFont.Size, bold ? FontStyle.Bold : FontStyle.Regular),
                Size = size,
                Location = location,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            if (onClick != null) btn.Click += onClick;
            return btn;
        }

        // Helper: create a panel section
        protected Panel CreateSectionPanel(Point location, Size size)
        {
            return new Panel
            {
                Location = location,
                Size = size,
                BackColor = Surface,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        protected void ShowInfo(string message, string title = "Thông tin")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string message, string title = "C?nh báo")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        protected void ShowError(string message, string title = "L?i")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
