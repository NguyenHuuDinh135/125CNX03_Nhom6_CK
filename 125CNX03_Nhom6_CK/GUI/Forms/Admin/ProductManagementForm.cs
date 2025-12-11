using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ProductManagementForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IThuongHieuService _brandService;

        // Khai báo control làm field – QUAN TRỌNG!
        private TextBox txtProductName, txtDescription, txtPrice;
        private ComboBox cboCategory, cboBrand;
        private CheckBox chkDisplay;
        private DataGridView dgvProducts;

        public ProductManagementForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();
            _categoryService = new LoaiSanPhamService();
            _brandService = new ThuongHieuService();
            InitializeUI();
            LoadCombos();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý sản phẩm";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Panel formPanel = new Panel
            {
                Size = new Size(1150, 220),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Các label + control
            CreateLabelAndControl(formPanel, "Tên sản phẩm:", out txtProductName, 20);
            CreateLabelAndControl(formPanel, "Mô tả:", out txtDescription, 55);
            CreateLabelAndControl(formPanel, "Giá:", out txtPrice, 90);

            new Label { Text = "Danh mục:", Location = new Point(20, 125), Parent = formPanel, Size = new Size(100, 23) };
            cboCategory = new ComboBox
            {
                Location = new Point(130, 123),
                Size = new Size(300, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            new Label { Text = "Thương hiệu:", Location = new Point(20, 160), Parent = formPanel, Size = new Size(100, 23) };
            cboBrand = new ComboBox
            {
                Location = new Point(130, 158),
                Size = new Size(300, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(460, 160),
                Checked = true,
                Parent = formPanel
            };

            // Nút
            var btnAdd = CreateButton("Thêm", new Point(600, 20), Color.FromArgb(0, 174, 219), BtnAdd_Click);
            var btnUpdate = CreateButton("Cập nhật", new Point(720, 20), Color.FromArgb(0, 174, 219), BtnUpdate_Click);
            var btnDelete = CreateButton("Xóa", new Point(840, 20), Color.FromArgb(220, 20, 60), BtnDelete_Click);

            formPanel.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(formPanel);

            // Grid
            Panel gridPanel = new Panel
            {
                Size = new Size(1150, 500),
                Location = new Point(20, 260),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            new Label
            {
                Text = "Danh sách sản phẩm",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                Parent = gridPanel
            };

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(1110, 420),
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
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
        }

        private void CreateLabelAndControl(Panel panel, string labelText, out TextBox textBox, int y)
        {
            new Label
            {
                Text = labelText,
                Location = new Point(20, y + 2),
                Size = new Size(100, 23),
                Parent = panel
            };
            textBox = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(300, 28),
                Parent = panel
            };
        }

        private Button CreateButton(string text, Point location, Color backColor, EventHandler click)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 40),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += click;
            return btn;
        }

        private void LoadCombos()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
                var catList = categories.Select(c => new
                {
                    Id = (int)c.Element("Id"),
                    TenLoai = (string)c.Element("TenLoai") ?? "Chưa có tên"
                }).ToList();

                cboCategory.DataSource = catList;
                cboCategory.DisplayMember = "TenLoai";
                cboCategory.ValueMember = "Id";

                var brands = _brandService.GetAllBrands();
                var brandList = brands.Select(b => new
                {
                    Id = (int)b.Element("Id"),
                    TenThuongHieu = (string)b.Element("TenThuongHieu") ?? "Chưa có tên"
                }).ToList();

                cboBrand.DataSource = brandList;
                cboBrand.DisplayMember = "TenThuongHieu";
                cboBrand.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục/thương hiệu: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                var products = _productService.GetAllProducts();
                dgvProducts.DataSource = ConvertToProductTable(products);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message);
            }
        }

        private DataTable ConvertToProductTable(List<XElement> elements)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Mô tả", typeof(string));
            dt.Columns.Add("Giá", typeof(decimal));
            dt.Columns.Add("Danh mục", typeof(string));
            dt.Columns.Add("Thương hiệu", typeof(string));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var p in elements)
            {
                dt.Rows.Add(
                    (int)p.Element("Id"),
                    (string)p.Element("TenSanPham") ?? "",
                    (string)p.Element("MoTa") ?? "",
                    (decimal)p.Element("Gia"),
                    GetCategoryName((int)p.Element("MaLoai")),
                    GetBrandName((int)p.Element("MaThuongHieu")),
                    (bool)p.Element("HienThi")
                );
            }
            return dt;
        }

        private string GetCategoryName(int id) => _categoryService.GetCategoryById(id)?.Element("TenLoai")?.Value ?? "N/A";
        private string GetBrandName(int id) => _brandService.GetBrandById(id)?.Element("TenThuongHieu")?.Value ?? "N/A";

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            var row = dgvProducts.SelectedRows[0];

            txtProductName.Text = row.Cells["Tên sản phẩm"].Value?.ToString() ?? "";
            txtDescription.Text = row.Cells["Mô tả"].Value?.ToString() ?? "";
            txtPrice.Text = row.Cells["Giá"].Value?.ToString() ?? "";
            chkDisplay.Checked = row.Cells["Hiển thị"].Value is bool b && b;

            // === SỬA CHỌN DANH MỤC & THƯƠNG HIỆU THEO TÊN (ĐÃ SỬA) ===
            string catName = row.Cells["Danh mục"].Value?.ToString();
            if (!string.IsNullOrEmpty(catName))
            {
                var catItem = cboCategory.Items.Cast<dynamic>().FirstOrDefault(item => item.TenLoai == catName);
                if (catItem != null)
                    cboCategory.SelectedItem = catItem;
            }

            string brandName = row.Cells["Thương hiệu"].Value?.ToString();
            if (!string.IsNullOrEmpty(brandName))
            {
                var brandItem = cboBrand.Items.Cast<dynamic>().FirstOrDefault(item => item.TenThuongHieu == brandName);
                if (brandItem != null)
                    cboBrand.SelectedItem = brandItem;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm và giá!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal gia))
            {
                MessageBox.Show("Giá không hợp lệ!", "Lỗi");
                return;
            }

            var newProduct = new XElement("SanPham",
                new XElement("TenSanPham", txtProductName.Text.Trim()),
                new XElement("MoTa", txtDescription.Text.Trim()),
                new XElement("ChiTiet", ""),
                new XElement("Gia", gia),
                new XElement("GiaKhuyenMai", 0),
                new XElement("DuongDanAnh", ""),
                new XElement("SoLuongTon", 0),
                new XElement("MaLoai", cboCategory.SelectedValue ?? 0),
                new XElement("MaThuongHieu", cboBrand.SelectedValue ?? 0),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _productService.AddProduct(newProduct);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm sản phẩm thành công!");
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chọn sản phẩm cần sửa!");
                return;
            }

            int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            var product = _productService.GetProductById(id);

            if (product != null && decimal.TryParse(txtPrice.Text, out decimal gia))
            {
                product.Element("TenSanPham").Value = txtProductName.Text.Trim();
                product.Element("MoTa").Value = txtDescription.Text.Trim();
                product.Element("Gia").Value = gia.ToString();
                product.Element("MaLoai").Value = cboCategory.SelectedValue?.ToString() ?? "0";
                product.Element("MaThuongHieu").Value = cboBrand.SelectedValue?.ToString() ?? "0";
                product.Element("HienThi").Value = chkDisplay.Checked.ToString();

                _productService.UpdateProduct(product);
                LoadData();
                ClearForm();
                MessageBox.Show("Cập nhật thành công!");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Xóa sản phẩm này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
                _productService.DeleteProduct(id);
                LoadData();
                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            chkDisplay.Checked = true;
            cboCategory.SelectedIndex = -1;
            cboBrand.SelectedIndex = -1;
            txtProductName.Focus();
        }
    }
}