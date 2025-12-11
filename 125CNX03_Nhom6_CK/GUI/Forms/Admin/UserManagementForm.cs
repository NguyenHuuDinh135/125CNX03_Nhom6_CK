using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class UserManagementForm : Form
    {
        private readonly INguoiDungService _userService;

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
            formPanel.Size = new Size(this.Width - 40, 150);
            formPanel.Location = new Point(20, 20);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;

            // Form controls
            Label lblFullName = new Label();
            lblFullName.Text = "Họ và tên:";
            lblFullName.Font = new Font("Segoe UI", 9);
            lblFullName.Location = new Point(20, 20);
            lblFullName.Size = new Size(100, 20);
            formPanel.Controls.Add(lblFullName);

            TextBox txtFullName = new TextBox();
            txtFullName.Name = "txtFullName";
            txtFullName.Font = new Font("Segoe UI", 10);
            txtFullName.Size = new Size(200, 20);
            txtFullName.Location = new Point(130, 20);
            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtFullName);

            Label lblEmail = new Label();
            lblEmail.Text = "Email:";
            lblEmail.Font = new Font("Segoe UI", 9);
            lblEmail.Location = new Point(20, 50);
            lblEmail.Size = new Size(100, 20);
            formPanel.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            txtEmail.Font = new Font("Segoe UI", 10);
            txtEmail.Size = new Size(200, 20);
            txtEmail.Location = new Point(130, 50);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtEmail);

            Label lblPhone = new Label();
            lblPhone.Text = "Số điện thoại:";
            lblPhone.Font = new Font("Segoe UI", 9);
            lblPhone.Location = new Point(20, 80);
            lblPhone.Size = new Size(100, 20);
            formPanel.Controls.Add(lblPhone);

            TextBox txtPhone = new TextBox();
            txtPhone.Name = "txtPhone";
            txtPhone.Font = new Font("Segoe UI", 10);
            txtPhone.Size = new Size(200, 20);
            txtPhone.Location = new Point(130, 80);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtPhone);

            Label lblRole = new Label();
            lblRole.Text = "Vai trò:";
            lblRole.Font = new Font("Segoe UI", 9);
            lblRole.Location = new Point(20, 110);
            lblRole.Size = new Size(100, 20);
            formPanel.Controls.Add(lblRole);

            ComboBox cboRole = new ComboBox();
            cboRole.Name = "cboRole";
            cboRole.Font = new Font("Segoe UI", 10);
            cboRole.Size = new Size(200, 20);
            cboRole.Location = new Point(130, 110);
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Items.AddRange(new object[] { "Customer", "Admin" });
            cboRole.SelectedIndex = 0;
            formPanel.Controls.Add(cboRole);

            CheckBox chkActive = new CheckBox();
            chkActive.Name = "chkActive";
            chkActive.Text = "Hoạt động";
            chkActive.Font = new Font("Segoe UI", 9);
            chkActive.Location = new Point(130, 140);
            chkActive.Size = new Size(100, 20);
            chkActive.Checked = true;
            formPanel.Controls.Add(chkActive);

            // Action buttons
            Button btnUpdate = new Button();
            btnUpdate.Text = "Cập nhật";
            btnUpdate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnUpdate.Size = new Size(100, 30);
            btnUpdate.Location = new Point(350, 20);
            btnUpdate.BackColor = Color.FromArgb(0, 174, 219);
            btnUpdate.ForeColor = Color.White;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.FlatAppearance.BorderSize = 0;
            btnUpdate.Cursor = Cursors.Hand;
            btnUpdate.Click += BtnUpdate_Click;
            formPanel.Controls.Add(btnUpdate);

            Button btnDelete = new Button();
            btnDelete.Text = "Xóa";
            btnDelete.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDelete.Size = new Size(100, 30);
            btnDelete.Location = new Point(460, 20);
            btnDelete.BackColor = Color.FromArgb(219, 0, 0);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += BtnDelete_Click;
            formPanel.Controls.Add(btnDelete);

            this.Controls.Add(formPanel);

            // Create data grid panel
            Panel gridPanel = new Panel();
            gridPanel.Size = new Size(this.Width - 40, 500);
            gridPanel.Location = new Point(20, 190);
            gridPanel.BackColor = Color.White;
            gridPanel.BorderStyle = BorderStyle.FixedSingle;

            Label gridTitle = new Label();
            gridTitle.Text = "Danh sách người dùng";
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
            var users = _userService.GetAllUsers();
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = null;
                dataGridView.DataSource = ConvertToUserTable(users);
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

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    element.Element("HoTen").Value,
                    element.Element("Email").Value,
                    element.Element("SoDienThoai").Value,
                    element.Element("VaiTro").Value,
                    DateTime.Parse(element.Element("NgayTao").Value),
                    bool.Parse(element.Element("TrangThai").Value)
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

                var fullNameControl = this.Controls[0].Controls.Find("txtFullName", true)[0] as TextBox;
                var emailControl = this.Controls[0].Controls.Find("txtEmail", true)[0] as TextBox;
                var phoneControl = this.Controls[0].Controls.Find("txtPhone", true)[0] as TextBox;
                var roleControl = this.Controls[0].Controls.Find("cboRole", true)[0] as ComboBox;
                var activeControl = this.Controls[0].Controls.Find("chkActive", true)[0] as CheckBox;

                if (fullNameControl != null) fullNameControl.Text = selectedRow.Cells["Họ tên"].Value?.ToString() ?? "";
                if (emailControl != null) emailControl.Text = selectedRow.Cells["Email"].Value?.ToString() ?? "";
                if (phoneControl != null) phoneControl.Text = selectedRow.Cells["Số điện thoại"].Value?.ToString() ?? "";
                if (roleControl != null) roleControl.SelectedItem = selectedRow.Cells["Vai trò"].Value?.ToString() ?? "Customer";
                if (activeControl != null) activeControl.Checked = bool.Parse(selectedRow.Cells["Trạng thái"].Value?.ToString() ?? "false");
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var userId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var fullNameControl = this.Controls[0].Controls.Find("txtFullName", true)[0] as TextBox;
                var emailControl = this.Controls[0].Controls.Find("txtEmail", true)[0] as TextBox;
                var phoneControl = this.Controls[0].Controls.Find("txtPhone", true)[0] as TextBox;
                var roleControl = this.Controls[0].Controls.Find("cboRole", true)[0] as ComboBox;
                var activeControl = this.Controls[0].Controls.Find("chkActive", true)[0] as CheckBox;

                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    user.Element("HoTen").Value = fullNameControl?.Text ?? "";
                    user.Element("Email").Value = emailControl?.Text ?? "";
                    user.Element("SoDienThoai").Value = phoneControl?.Text ?? "";
                    user.Element("VaiTro").Value = roleControl?.SelectedItem?.ToString() ?? "Customer";
                    user.Element("TrangThai").Value = activeControl?.Checked.ToString() ?? "false";

                    _userService.UpdateUser(user);
                    LoadData();
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var userId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _userService.DeleteUser(userId);
                    LoadData();
                    MessageBox.Show("Xóa người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}