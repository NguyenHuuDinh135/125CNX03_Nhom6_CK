using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.Interfaces;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ContactForm : Form, ISearchableForm
    {
        private readonly ILienHeService _contactService;
        private DataGridView dgvContacts;
        private TextBox txtHoTen, txtEmail, txtSoDienThoai, txtNoiDung;
        private Button btnAdd, btnUpdate, btnDelete;

        private List<XElement> _allContacts;

        public ContactForm()
        {
            InitializeComponent();
            _contactService = new LienHeService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý liên hệ";
            this.Size = new Size(950, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            // Panel form chi tiết
            Panel formPanel = new Panel
            {
                Size = new Size(900, 220),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            CreateControl("Họ tên:", out txtHoTen, 20, formPanel);
            CreateControl("Email:", out txtEmail, 60, formPanel);
            CreateControl("Số điện thoại:", out txtSoDienThoai, 100, formPanel);
            CreateControl("Nội dung:", out txtNoiDung, 140, formPanel, multiline: true, height: 50);

            // Buttons
            btnAdd = CreateButton("Thêm", new Point(700, 20), Color.FromArgb(0, 174, 219), BtnAdd_Click);
            btnUpdate = CreateButton("Cập nhật", new Point(700, 80), Color.FromArgb(0, 174, 219), BtnUpdate_Click);
            btnDelete = CreateButton("Xóa", new Point(700, 140), Color.FromArgb(220, 20, 60), BtnDelete_Click);

            formPanel.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(formPanel);

            // Panel DataGridView
            Panel gridPanel = new Panel
            {
                Size = new Size(900, 400),
                Location = new Point(20, 260),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            Label lblTitle = new Label
            {
                Text = "Danh sách liên hệ",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10),
                Parent = gridPanel
            };

            dgvContacts = new DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(860, 330),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 35 },
                AllowUserToAddRows = false,
                ReadOnly = true,
                Parent = gridPanel
            };
            dgvContacts.SelectionChanged += DgvContacts_SelectionChanged;

            gridPanel.Controls.Add(dgvContacts);
            this.Controls.Add(gridPanel);
        }

        private void CreateControl(string labelText, out TextBox tb, int y, Panel panel, bool multiline = false, int height = 28)
        {
            new Label
            {
                Text = labelText,
                Location = new Point(20, y + 3),
                Size = new Size(120, 23),
                Parent = panel
            };

            tb = new TextBox
            {
                Location = new Point(150, y),
                Size = new Size(500, height),
                Multiline = multiline,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None,
                Parent = panel
            };
        }

        private Button CreateButton(string text, Point loc, Color color, EventHandler click)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(120, 45),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btn.Click += click;
            return btn;
        }

        private void LoadData()
        {
            _allContacts = _contactService.GetAllMessages();
            BindGrid(_allContacts);
        }

        private void BindGrid(List<XElement> contacts)
        {
            dgvContacts.DataSource = null;
            dgvContacts.DataSource = ConvertToContactTable(contacts);
        }
        private DataTable ConvertToContactTable(List<XElement> elements)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Nội dung", typeof(string));
            dt.Columns.Add("Ngày gửi", typeof(DateTime));
            dt.Columns.Add("Đã xem", typeof(bool));

            foreach (var el in elements)
            {
                dt.Rows.Add(
                    int.Parse(el.Element("Id").Value),
                    el.Element("HoTen").Value,
                    el.Element("Email").Value,
                    el.Element("SoDienThoai").Value,
                    el.Element("NoiDung").Value,
                    DateTime.Parse(el.Element("NgayGui").Value),
                    bool.Parse(el.Element("DaXem").Value)
                );
            }

            return dt;
        }

        private void DgvContacts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0) return;

            var row = dgvContacts.SelectedRows[0];

            txtHoTen.Text = row.Cells["Họ tên"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";
            txtSoDienThoai.Text = row.Cells["Số điện thoại"].Value?.ToString() ?? "";
            txtNoiDung.Text = row.Cells["Nội dung"].Value?.ToString() ?? "";

            // Cập nhật trạng thái Đã xem
            int id = (int)row.Cells["Id"].Value;
            var contact = _contactService.GetMessageById(id);
            if (contact != null && !bool.Parse(contact.Element("DaXem").Value))
            {
                contact.Element("DaXem").Value = "true";
                _contactService.UpdateMessage(contact);
                LoadData();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ tên và Email!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = _contactService.GenerateNewId();
            var newContact = new XElement("LienHe",
                new XElement("Id", newId),
                new XElement("HoTen", txtHoTen.Text.Trim()),
                new XElement("Email", txtEmail.Text.Trim()),
                new XElement("SoDienThoai", txtSoDienThoai.Text.Trim()),
                new XElement("NoiDung", txtNoiDung.Text.Trim()),
                new XElement("NgayGui", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("DaXem", false)
            );

            _contactService.AddMessage(newContact);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm liên hệ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn liên hệ cần sửa!");
                return;
            }

            int id = (int)dgvContacts.SelectedRows[0].Cells["Id"].Value;
            var contact = _contactService.GetMessageById(id);

            if (contact != null)
            {
                contact.Element("HoTen").Value = txtHoTen.Text.Trim();
                contact.Element("Email").Value = txtEmail.Text.Trim();
                contact.Element("SoDienThoai").Value = txtSoDienThoai.Text.Trim();
                contact.Element("NoiDung").Value = txtNoiDung.Text.Trim();

                _contactService.UpdateMessage(contact);
                LoadData();
                ClearForm();
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn liên hệ cần xóa!");
                return;
            }

            int id = (int)dgvContacts.SelectedRows[0].Cells["Id"].Value;
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa liên hệ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _contactService.DeleteMessage(id);
                LoadData();
                ClearForm();
                MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearForm()
        {
            txtHoTen.Clear();
            txtEmail.Clear();
            txtSoDienThoai.Clear();
            txtNoiDung.Clear();
            txtHoTen.Focus();
        }
        public void OnSearch(string keyword)
        {
            if (_allContacts == null) return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindGrid(_allContacts);
                return;
            }

            keyword = keyword.ToLower();

            var filtered = _allContacts.Where(c =>
                c.Elements().Any(e =>
                    !string.IsNullOrEmpty(e.Value) &&
                    e.Value.ToLower().Contains(keyword)
                )
            ).ToList();

            BindGrid(filtered);
        }

    }
}
