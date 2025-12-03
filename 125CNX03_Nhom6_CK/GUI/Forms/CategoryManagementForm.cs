using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class CategoryManagementForm : Form
    {
        private readonly ILoaiSanPhamService _categoryService;

        public CategoryManagementForm()
        {
            InitializeComponent();
            _categoryService = new LoaiSanPhamService();
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewCategories.DataSource = null;
            var categories = _categoryService.GetAllCategories();
            dataGridViewCategories.DataSource = ConvertToDataTable(categories);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên loại", typeof(string));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("TenLoai")?.Value ?? "",
                    bool.Parse(element.Element("HienThi")?.Value ?? "false")
                );
            }

            return dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCategory = new XElement("LoaiSanPham",
                new XElement("TenLoai", txtCategoryName.Text),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _categoryService.AddCategory(newCategory);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                if (string.IsNullOrEmpty(txtCategoryName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewCategories.SelectedRows[0];
                var categoryId = (int)selectedRow.Cells["Id"].Value;

                var category = _categoryService.GetCategoryById(categoryId);
                if (category != null)
                {
                    category.Element("TenLoai").Value = txtCategoryName.Text;
                    category.Element("HienThi").Value = chkDisplay.Checked.ToString();

                    _categoryService.UpdateCategory(category);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn danh mục để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewCategories.SelectedRows[0];
                var categoryId = (int)selectedRow.Cells["Id"].Value;

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa danh mục này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _categoryService.DeleteCategory(categoryId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn danh mục để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewCategories.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewCategories.SelectedRows[0];
                var categoryId = (int)selectedRow.Cells["Id"].Value;

                var category = _categoryService.GetCategoryById(categoryId);
                if (category != null)
                {
                    txtCategoryName.Text = category.Element("TenLoai").Value;
                    chkDisplay.Checked = bool.Parse(category.Element("HienThi").Value);
                }
            }
        }

        private void ClearForm()
        {
            txtCategoryName.Clear();
            chkDisplay.Checked = true;
        }
    }
}