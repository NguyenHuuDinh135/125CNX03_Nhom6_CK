using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class ContactForm : Form
    {
        private readonly ILienHeService _contactService;

        public ContactForm()
        {
            InitializeComponent();
            _contactService = new LienHeService();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var contact = new XElement("LienHe",
                new XElement("HoTen", txtName.Text),
                new XElement("Email", txtEmail.Text),
                new XElement("SoDienThoai", txtPhone.Text),
                new XElement("NoiDung", txtMessage.Text),
                new XElement("NgayGui", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("DaXem", "false")
            );

            _contactService.AddMessage(contact);
            MessageBox.Show("Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear form
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtMessage.Clear();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}