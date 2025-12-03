using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class ProductManagementForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IThuongHieuService _brandService;

        public ProductManagementForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();
            _categoryService = new LoaiSanPhamService();
            _brandService = new ThuongHieuService();
            LoadData();
            LoadCategories();
            LoadBrands();
        }

        private void LoadData()
        {
            dataGridViewProducts.DataSource = null;
            var products = _productService.GetAllProducts();
            dataGridViewProducts.DataSource = ConvertToDataTable(products);
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetAllCategories();
            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "TenLoai";
            cboCategory.ValueMember = "Id";
        }

        private void LoadBrands()
        {
            var brands = _brandService.GetAllBrands();
            cboBrand.DataSource = brands;
            cboBrand.DisplayMember = "TenThuongHieu";
            cboBrand.ValueMember = "Id";
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Mô tả", typeof(string));
            dt.Columns.Add("Giá", typeof(decimal));
            dt.Columns.Add("Giá khuyến mãi", typeof(decimal));
            dt.Columns.Add("Số lượng tồn", typeof(int));
            dt.Columns.Add("Danh mục", typeof(string));
            dt.Columns.Add("Thương hiệu", typeof(string));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("TenSanPham")?.Value ?? "",
                    element.Element("MoTa")?.Value ?? "",
                    decimal.Parse(element.Element("Gia")?.Value ?? "0"),
                    decimal.Parse(element.Element("GiaKhuyenMai")?.Value ?? "0"),
                    int.Parse(element.Element("SoLuongTon")?.Value ?? "0"),
                    GetCategoryName(int.Parse(element.Element("MaLoai")?.Value ?? "0")),
                    GetBrandName(int.Parse(element.Element("MaThuongHieu")?.Value ?? "0"))
                );
            }

            return dt;
        }

        private string GetCategoryName(int categoryId)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            return category?.Element("TenLoai")?.Value ?? "N/A";
        }

        private string GetBrandName(int brandId)
        {
            var brand = _brandService.GetBrandById(brandId);
            return brand?.Element("TenThuongHieu")?.Value ?? "N/A";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var newProduct = new XElement("SanPham",
                    new XElement("TenSanPham", txtProductName.Text),
                    new XElement("MoTa", txtDescription.Text),
                    new XElement("ChiTiet", txtDetail.Text),
                    new XElement("Gia", decimal.Parse(txtPrice.Text)),
                    new XElement("GiaKhuyenMai", string.IsNullOrEmpty(txtDiscountPrice.Text) ? "0" : decimal.Parse(txtDiscountPrice.Text).ToString()),
                    new XElement("DuongDanAnh", txtImageUrl.Text),
                    new XElement("SoLuongTon", int.Parse(txtStock.Text)),
                    new XElement("MaLoai", (int)cboCategory.SelectedValue),
                    new XElement("MaThuongHieu", (int)cboBrand.SelectedValue),
                    new XElement("HienThi", chkDisplay.Checked)
                );

                _productService.AddProduct(newProduct);
                LoadData();
                ClearForm();
                MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                if (ValidateInput())
                {
                    var selectedRow = dataGridViewProducts.SelectedRows[0];
                    var productId = (int)selectedRow.Cells["Id"].Value;

                    var product = _productService.GetProductById(productId);
                    if (product != null)
                    {
                        product.Element("TenSanPham").Value = txtProductName.Text;
                        product.Element("MoTa").Value = txtDescription.Text;
                        product.Element("ChiTiet").Value = txtDetail.Text;
                        product.Element("Gia").Value = decimal.Parse(txtPrice.Text).ToString();
                        product.Element("GiaKhuyenMai").Value = string.IsNullOrEmpty(txtDiscountPrice.Text) ? "0" : decimal.Parse(txtDiscountPrice.Text).ToString();
                        product.Element("DuongDanAnh").Value = txtImageUrl.Text;
                        product.Element("SoLuongTon").Value = txtStock.Text;
                        product.Element("MaLoai").Value = ((int)cboCategory.SelectedValue).ToString();
                        product.Element("MaThuongHieu").Value = ((int)cboBrand.SelectedValue).ToString();
                        product.Element("HienThi").Value = chkDisplay.Checked.ToString();

                        _productService.UpdateProduct(product);
                        LoadData();
                        ClearForm();
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                var productId = (int)selectedRow.Cells["Id"].Value;

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _productService.DeleteProduct(productId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewProducts.SelectedRows[0];
                var productId = (int)selectedRow.Cells["Id"].Value;

                var product = _productService.GetProductById(productId);
                if (product != null)
                {
                    txtProductName.Text = product.Element("TenSanPham").Value;
                    txtDescription.Text = product.Element("MoTa").Value;
                    txtDetail.Text = product.Element("ChiTiet").Value;
                    txtPrice.Text = product.Element("Gia").Value;
                    txtDiscountPrice.Text = product.Element("GiaKhuyenMai").Value;
                    txtImageUrl.Text = product.Element("DuongDanAnh").Value;
                    txtStock.Text = product.Element("SoLuongTon").Value;
                    cboCategory.SelectedValue = int.Parse(product.Element("MaLoai").Value);
                    cboBrand.SelectedValue = int.Parse(product.Element("MaThuongHieu").Value);
                    chkDisplay.Checked = bool.Parse(product.Element("HienThi").Value);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtProductName.Text) ||
                string.IsNullOrEmpty(txtPrice.Text) ||
                string.IsNullOrEmpty(txtStock.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out _) ||
                !int.TryParse(txtStock.Text, out _))
            {
                MessageBox.Show("Giá và số lượng phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            txtDetail.Clear();
            txtPrice.Clear();
            txtDiscountPrice.Clear();
            txtImageUrl.Clear();
            txtStock.Clear();
            chkDisplay.Checked = true;
        }
    }
}