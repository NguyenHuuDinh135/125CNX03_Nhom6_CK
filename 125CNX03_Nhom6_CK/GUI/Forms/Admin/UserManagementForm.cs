using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.GUI.Interfaces;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class UserManagementForm : Form, ISearchableForm
    {
        private readonly INguoiDungService _userService;
        private List<XElement> _allUsers;

        public UserManagementForm()
        {
            InitializeComponent();
            _userService = new NguoiDungService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý người dùng";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create form panel
            Panel formPanel = new Panel();
            formPanel.Size = new Size(this.Width - 40, 180);
            formPanel.Location = new Point(20, 20);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;

            // ==== Họ tên ====
            Label lblFullName = new Label();
            lblFullName.Text = "Họ và tên:";
            lblFullName.Location = new Point(20, 20);
            formPanel.Controls.Add(lblFullName);

            TextBox txtFullName = new TextBox();
            txtFullName.Name = "txtFullName";
            txtFullName.Location = new Point(130, 20);
            txtFullName.Width = 200;
            formPanel.Controls.Add(txtFullName);

            // ==== Email ====
            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(20, 50);
            formPanel.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Location = new Point(130, 50);
            txtEmail.Width = 200;
            formPanel.Controls.Add(txtEmail);

            // ==== Phone ====
            Label lblPhone = new Label();
            lblPhone.Text = "Số điện thoại:";
            lblPhone.Location = new Point(20, 80);
            formPanel.Controls.Add(lblPhone);

            TextBox txtPhone = new TextBox();
            txtPhone.Name = "txtPhone";
            txtPhone.Location = new Point(130, 80);
            txtPhone.Width = 200;
            formPanel.Controls.Add(txtPhone);

            // ==== PASSWORD ====
            Label lblPassword = new Label();
            lblPassword.Text = "Mật khẩu:";
            lblPassword.Location = new Point(350, 20);
            formPanel.Controls.Add(lblPassword);

            TextBox txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            txtPassword.Location = new Point(450, 20);
            txtPassword.Width = 200;
            txtPassword.PasswordChar = '●';
            formPanel.Controls.Add(txtPassword);

            // ==== Vai trò ====
            Label lblRole = new Label();
            lblRole.Text = "Vai trò:";
            lblRole.Location = new Point(20, 110);
            formPanel.Controls.Add(lblRole);

            ComboBox cboRole = new ComboBox();
            cboRole.Name = "cboRole";
            cboRole.Items.AddRange(new object[] { "Customer", "Admin" });
            cboRole.SelectedIndex = 0;
            cboRole.Location = new Point(130, 110);
            cboRole.Width = 200;
            formPanel.Controls.Add(cboRole);

            // ==== Active ====
            CheckBox chkActive = new CheckBox();
            chkActive.Name = "chkActive";
            chkActive.Text = "Hoạt động";
            chkActive.Location = new Point(130, 140);
            chkActive.Checked = true;
            formPanel.Controls.Add(chkActive);

            // ==== Add ====
            Button btnAdd = new Button();
            btnAdd.Text = "Thêm";
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(350, 60);
            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            // ==== Update ====
            Button btnUpdate = new Button();
            btnUpdate.Text = "Cập nhật";
            btnUpdate.BackColor = Color.FromArgb(0, 174, 219);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.Location = new Point(350, 110);
            btnUpdate.Click += BtnUpdate_Click;
            formPanel.Controls.Add(btnUpdate);

            // ==== Delete ====
            Button btnDelete = new Button();
            btnDelete.Text = "Xóa";
            btnDelete.BackColor = Color.DarkRed;
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(460, 110);
            btnDelete.Click += BtnDelete_Click;
            formPanel.Controls.Add(btnDelete);

            this.Controls.Add(formPanel);

            // ==== GRID PANEL ====
            Panel gridPanel = new Panel();
            gridPanel.Size = new Size(this.Width - 40, 500);
            gridPanel.Location = new Point(20, 220);
            gridPanel.BackColor = Color.White;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;

            DataGridView dataGridView = new DataGridView();
            dataGridView.Name = "dgvUsers";
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;

            gridPanel.Controls.Add(dataGridView);
            this.Controls.Add(gridPanel);
        }

        // =====================
        // LOAD DATA
        // =====================
        private void LoadData()
        {
            _allUsers = _userService.GetAllUsers();

            var dgv = this.Controls.Find("dgvUsers", true)
                                   .FirstOrDefault() as DataGridView;

            if (dgv != null)
            {
                BindGrid(_allUsers);
            }
        }
        private void BindGrid(List<XElement> users)
        {
            var dgv = this.Controls.Find("dgvUsers", true)
                                   .FirstOrDefault() as DataGridView;

            if (dgv != null)
            {
                dgv.DataSource = null;
                dgv.DataSource = ConvertToUserTable(users);
            }
        }


        private System.Data.DataTable ConvertToUserTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Số điện thoại", typeof(string));
            dt.Columns.Add("Vai trò", typeof(string));
            dt.Columns.Add("Ngày tạo", typeof(DateTime));
            dt.Columns.Add("Trạng thái", typeof(bool));

            foreach (var el in elements)
            {
                int.TryParse(el.Element("Id")?.Value, out int id);
                DateTime.TryParse(el.Element("NgayTao")?.Value, out DateTime created);
                bool.TryParse(el.Element("TrangThai")?.Value, out bool status);

                dt.Rows.Add(
                    id,
                    el.Element("HoTen")?.Value ?? "",
                    el.Element("Email")?.Value ?? "",
                    el.Element("SoDienThoai")?.Value ?? "",
                    el.Element("VaiTro")?.Value ?? "Customer",
                    created,
                    status
                );
            }

            return dt;
        }

        // =====================
        // SELECTION CHANGED
        // =====================
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || dgv.SelectedRows.Count == 0)
                return;

            var row = dgv.SelectedRows[0];

            var fullName = this.Controls.Find("txtFullName", true).FirstOrDefault() as TextBox;
            var email = this.Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            var phone = this.Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
            var role = this.Controls.Find("cboRole", true).FirstOrDefault() as ComboBox;
            var active = this.Controls.Find("chkActive", true).FirstOrDefault() as CheckBox;
            var pass = this.Controls.Find("txtPassword", true).FirstOrDefault() as TextBox;

            // Clear password ALWAYS
            if (pass != null) pass.Clear();

            if (row.IsNewRow)
            {
                fullName?.Clear();
                email?.Clear();
                phone?.Clear();
                role.SelectedIndex = -1;
                active.Checked = false;
                return;
            }

            fullName.Text = row.Cells["Họ tên"].Value?.ToString();
            email.Text = row.Cells["Email"].Value?.ToString();
            phone.Text = row.Cells["Số điện thoại"].Value?.ToString();
            role.SelectedItem = row.Cells["Vai trò"].Value?.ToString();
            bool.TryParse(row.Cells["Trạng thái"].Value?.ToString(), out bool status);
            active.Checked = status;
        }

        // =====================
        // ADD USER
        // =====================
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var fullName = this.Controls.Find("txtFullName", true).FirstOrDefault() as TextBox;
            var email = this.Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            var phone = this.Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
            var role = this.Controls.Find("cboRole", true).FirstOrDefault() as ComboBox;
            var active = this.Controls.Find("chkActive", true).FirstOrDefault() as CheckBox;
            var pass = this.Controls.Find("txtPassword", true).FirstOrDefault() as TextBox;

            if (string.IsNullOrWhiteSpace(fullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            if (string.IsNullOrWhiteSpace(email.Text))
            {
                MessageBox.Show("Vui lòng nhập email!");
                return;
            }

            int newId = _userService.GenerateNewId();

            var newUser = new XElement("NguoiDung",
                new XElement("Id", newId),
                new XElement("HoTen", fullName.Text),
                new XElement("Email", email.Text),
                new XElement("SoDienThoai", phone.Text),
                new XElement("VaiTro", role.SelectedItem?.ToString() ?? "Customer"),
                new XElement("NgayTao", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("TrangThai", active.Checked.ToString()),
                new XElement("MatKhauHash", pass.Text ?? "")
            );

            _userService.AddUser(newUser);

            LoadData();
            MessageBox.Show("Thêm người dùng thành công!");

            fullName.Clear();
            email.Clear();
            phone.Clear();
            pass.Clear();
            role.SelectedIndex = 0;
            active.Checked = true;
        }

        // =====================
        // UPDATE USER
        // =====================
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var dgv = this.Controls.Find("dgvUsers", true).FirstOrDefault() as DataGridView;
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn user!");
                return;
            }

            var row = dgv.SelectedRows[0];
            int userId = Convert.ToInt32(row.Cells["Id"].Value);

            var user = _userService.GetUserById(userId);
            if (user == null) return;

            var fullName = this.Controls.Find("txtFullName", true).FirstOrDefault() as TextBox;
            var email = this.Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            var phone = this.Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
            var role = this.Controls.Find("cboRole", true).FirstOrDefault() as ComboBox;
            var active = this.Controls.Find("chkActive", true).FirstOrDefault() as CheckBox;
            var pass = this.Controls.Find("txtPassword", true).FirstOrDefault() as TextBox;

            user.Element("HoTen").Value = fullName.Text;
            user.Element("Email").Value = email.Text;
            user.Element("SoDienThoai").Value = phone.Text;
            user.Element("VaiTro").Value = role.SelectedItem?.ToString() ?? "Customer";
            user.Element("TrangThai").Value = active.Checked.ToString();

            if (!string.IsNullOrWhiteSpace(pass.Text))
            {
                user.Element("MatKhauHash").Value = pass.Text;
            }

            _userService.UpdateUser(user);

            LoadData();
            MessageBox.Show("Cập nhật người dùng thành công!");
        }

        // =====================
        // DELETE USER
        // =====================
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var dgv = this.Controls.Find("dgvUsers", true).FirstOrDefault() as DataGridView;

            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn user để xóa");
                return;
            }

            int userId = Convert.ToInt32(dgv.SelectedRows[0].Cells["Id"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _userService.DeleteUser(userId);
                LoadData();
                MessageBox.Show("Đã xóa user!");
            }
        }
        public void OnSearch(string keyword)
        {
            if (_allUsers == null) return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindGrid(_allUsers);
                return;
            }

            keyword = keyword.ToLower();

            var filtered = _allUsers.Where(u =>
                (u.Element("HoTen")?.Value.ToLower().Contains(keyword) ?? false) ||
                (u.Element("Email")?.Value.ToLower().Contains(keyword) ?? false) ||
                (u.Element("SoDienThoai")?.Value.ToLower().Contains(keyword) ?? false) ||
                (u.Element("DiaChi")?.Value.ToLower().Contains(keyword) ?? false) ||
                (u.Element("VaiTro")?.Value.ToLower().Contains(keyword) ?? false)
            ).ToList();

            BindGrid(filtered);
        }

    }
}
