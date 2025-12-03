using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class BrandManagementForm : Form
    {
        private readonly IThuongHieuService _brandService;

        public BrandManagementForm()
        {
            InitializeComponent();
            _brandService = new ThuongHieuService();
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewBrands.DataSource = null;
            var brands = _brandService.GetAllBrands();
            dataGridViewBrands.DataSource = ConvertToDataTable(brands);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên thương hiệu", typeof(string));
            dt.Columns.Add("Đường dẫn ảnh", typeof(string));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("TenThuongHieu")?.Value ?? "",
                    element.Element("HinhAnh")?.Value ?? ""
                );
            }

            return dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBrandName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thương hiệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newBrand = new XElement("ThuongHieu",
                new XElement("TenThuongHieu", txtBrandName.Text),
                new XElement("HinhAnh", txtImageUrl.Text)
            );

            _brandService.AddBrand(newBrand);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm thương hiệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewBrands.SelectedRows.Count > 0)
            {
                if (string.IsNullOrEmpty(txtBrandName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên thương hiệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewBrands.SelectedRows[0];
                var brandId = (int)selectedRow.Cells["Id"].Value;

                var brand = _brandService.GetBrandById(brandId);
                if (brand != null)
                {
                    brand.Element("TenThuongHieu").Value = txtBrandName.Text;
                    brand.Element("HinhAnh").Value = txtImageUrl.Text;

                    _brandService.UpdateBrand(brand);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật thương hiệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thương hiệu để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewBrands.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBrands.SelectedRows[0];
                var brandId = (int)selectedRow.Cells["Id"].Value;

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thương hiệu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _brandService.DeleteBrand(brandId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa thương hiệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thương hiệu để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewBrands_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBrands.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBrands.SelectedRows[0];
                var brandId = (int)selectedRow.Cells["Id"].Value;

                var brand = _brandService.GetBrandById(brandId);
                if (brand != null)
                {
                    txtBrandName.Text = brand.Element("TenThuongHieu").Value;
                    txtImageUrl.Text = brand.Element("HinhAnh").Value;
                }
            }
        }

        private void ClearForm()
        {
            txtBrandName.Clear();
            txtImageUrl.Clear();
        }
    }
}