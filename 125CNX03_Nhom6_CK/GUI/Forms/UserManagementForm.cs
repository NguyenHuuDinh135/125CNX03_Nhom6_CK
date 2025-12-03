using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class UserManagementForm : Form
    {
        private readonly INguoiDungService _userService;

        public UserManagementForm()
        {
            InitializeComponent();
            _userService = new NguoiDungService();
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewUsers.DataSource = null;
            var users = _userService.GetAllUsers();
            dataGridViewUsers.DataSource = ConvertToDataTable(users);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Họ tên", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Điện thoại", typeof(string));
            dt.Columns.Add("Địa chỉ", typeof(string));
            dt.Columns.Add("Vai trò", typeof(string));
            dt.Columns.Add("Ngày tạo", typeof(DateTime));
            dt.Columns.Add("Trạng thái", typeof(bool));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("HoTen")?.Value ?? "",
                    element.Element("Email")?.Value ?? "",
                    element.Element("SoDienThoai")?.Value ?? "",
                    element.Element("DiaChi")?.Value ?? "",
                    element.Element("VaiTro")?.Value ?? "",
                    DateTime.Parse(element.Element("NgayTao")?.Value ?? DateTime.Now.ToString()),
                    bool.Parse(element.Element("TrangThai")?.Value ?? "false")
                );
            }

            return dt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                if (string.IsNullOrEmpty(txtFullName.Text) || string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên và email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewUsers.SelectedRows[0];
                var userId = (int)selectedRow.Cells["Id"].Value;

                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    user.Element("HoTen").Value = txtFullName.Text;
                    user.Element("Email").Value = txtEmail.Text;
                    user.Element("SoDienThoai").Value = txtPhone.Text;
                    user.Element("DiaChi").Value = txtAddress.Text;
                    user.Element("VaiTro").Value = cboRole.SelectedItem?.ToString() ?? "Customer";
                    user.Element("TrangThai").Value = chkActive.Checked.ToString();

                    _userService.UpdateUser(user);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewUsers.SelectedRows[0];
                var userId = (int)selectedRow.Cells["Id"].Value;

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _userService.DeleteUser(userId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsers.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewUsers.SelectedRows[0];
                var userId = (int)selectedRow.Cells["Id"].Value;

                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    txtFullName.Text = user.Element("HoTen").Value;
                    txtEmail.Text = user.Element("Email").Value;
                    txtPhone.Text = user.Element("SoDienThoai").Value;
                    txtAddress.Text = user.Element("DiaChi").Value;
                    cboRole.SelectedItem = user.Element("VaiTro").Value;
                    chkActive.Checked = bool.Parse(user.Element("TrangThai").Value);
                }
            }
        }

        private void ClearForm()
        {
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            cboRole.SelectedIndex = 0;
            chkActive.Checked = true;
        }
    }
}