using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public class ContactForm : UserBaseForm
    {
        private readonly ILienHeService _contactService;
        private TextBox _txtName, _txtEmail, _txtMessage;

        public ContactForm()
        {
            _contactService = new LienHeService();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Liên h?";
            this.Size = new Size(800, 600);
            this.BackColor = Surface;

            var header = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            var title = new Label { Text = "Liên h?", Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 30) };
            header.Controls.Add(title);
            this.Controls.Add(header);

            var panel = CreateSectionPanel(new Point(20, 100), new Size(this.Width - 40, 360));

            panel.Controls.Add(new Label { Text = "H? và tên:", Location = new Point(20, 20) });
            _txtName = new TextBox { Location = new Point(120, 18), Size = new Size(400, 26) };
            panel.Controls.Add(_txtName);

            panel.Controls.Add(new Label { Text = "Email:", Location = new Point(20, 60) });
            _txtEmail = new TextBox { Location = new Point(120, 58), Size = new Size(400, 26) };
            panel.Controls.Add(_txtEmail);

            panel.Controls.Add(new Label { Text = "N?i dung:", Location = new Point(20, 100) });
            _txtMessage = new TextBox { Location = new Point(120, 98), Size = new Size(600, 160), Multiline = true, ScrollBars = ScrollBars.Vertical };
            panel.Controls.Add(_txtMessage);

            var btnSend = CreateButton("G?i liên h?", new Point(120, 270), new Size(120, 36), Primary, BtnSend_Click);
            panel.Controls.Add(btnSend);

            this.Controls.Add(panel);
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtName.Text) || string.IsNullOrWhiteSpace(_txtEmail.Text) || string.IsNullOrWhiteSpace(_txtMessage.Text))
            {
                ShowWarning("Vui lòng ?i?n ??y ?? thông tin.", "Thi?u thông tin");
                return;
            }

            var msg = new XElement("LienHe",
                new XElement("HoTen", _txtName.Text.Trim()),
                new XElement("Email", _txtEmail.Text.Trim()),
                new XElement("NoiDung", _txtMessage.Text.Trim()),
                new XElement("DaXem", "false"),
                new XElement("NgayGui", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"))
            );

            try
            {
                _contactService.AddMessage(msg);
                ShowInfo("G?i liên h? thành công! Chúng tôi s? liên h? l?i s?m.", "Thành công");
                _txtName.Clear(); _txtEmail.Clear(); _txtMessage.Clear();
            }
            catch (Exception ex)
            {
                ShowError($"Không th? g?i: {ex.Message}", "L?i");
            }
        }
    }
}
