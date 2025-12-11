using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ContactForm : Form
    {
        private readonly ILienHeService _contactService;

        public ContactForm()
        {
            InitializeComponent();
            _contactService = new LienHeService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Liên hệ";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create data grid panel
            Panel gridPanel = new Panel();
            gridPanel.Size = new Size(this.Width - 40, this.Height - 60);
            gridPanel.Location = new Point(20, 20);
            gridPanel.BackColor = Color.White;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;

            Label gridTitle = new Label();
            gridTitle.Text = "Danh sách liên hệ";
            gridTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            gridTitle.Location = new Point(20, 20);
            gridTitle.Size = new Size(200, 30);
            gridPanel.Controls.Add(gridTitle);

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size(gridPanel.Width - 40, gridPanel.Height - 60);
            dataGridView.Location = new Point(20, 60);
            dataGridView.Font = new Font("Segoe UI", 10);
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 30;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
            gridPanel.Controls.Add(dataGridView);

            this.Controls.Add(gridPanel);
        }

        private void LoadData()
        {
            var contacts = _contactService.GetAllMessages();
            var dataGridView = this.Controls[0].Controls[1] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = null;
                dataGridView.DataSource = ConvertToContactTable(contacts);
            }
        }

        private System.Data.DataTable ConvertToContactTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Nội dung", typeof(string));
            dt.Columns.Add("Ngày gửi", typeof(DateTime));
            dt.Columns.Add("Đã xem", typeof(bool));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    element.Element("HoTen").Value,
                    element.Element("Email").Value,
                    element.Element("SoDienThoai").Value,
                    element.Element("NoiDung").Value,
                    DateTime.Parse(element.Element("NgayGui").Value),
                    bool.Parse(element.Element("DaXem").Value)
                );
            }

            return dt;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];

                // You can add code here to show contact details in a separate panel
                // For now, we just mark as read
                var contactId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");
                var contact = _contactService.GetMessageById(contactId);

                if (contact != null && !bool.Parse(contact.Element("DaXem").Value))
                {
                    contact.Element("DaXem").Value = "true";
                    _contactService.UpdateMessage(contact);
                    LoadData(); // Refresh the grid
                }
            }
        }
    }
}