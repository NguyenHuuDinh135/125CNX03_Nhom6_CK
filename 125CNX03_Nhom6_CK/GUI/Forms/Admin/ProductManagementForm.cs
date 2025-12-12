using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Globalization;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ProductManagementForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IThuongHieuService _brandService;

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

        #region UI

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
                BorderStyle = BorderStyle.FixedSingle
            };

            CreateLabelAndControl(formPanel, "Tên sản phẩm:", out txtProductName, 20);
            CreateLabelAndControl(formPanel, "Mô tả:", out txtDescription, 55);
            CreateLabelAndControl(formPanel, "Giá:", out txtPrice, 90);

            new Label { Text = "Danh mục:", Location = new Point(20, 125), Parent = formPanel };
            cboCategory = new ComboBox
            {
                Location = new Point(130, 123),
                Size = new Size(300, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            new Label { Text = "Thương hiệu:", Location = new Point(20, 160), Parent = formPanel };
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

            formPanel.Controls.AddRange(new Control[]
            {
                CreateButton("Thêm", new Point(600,20), Color.FromArgb(0,174,219), BtnAdd_Click),
                CreateButton("Cập nhật", new Point(720,20), Color.FromArgb(0,174,219), BtnUpdate_Click),
                CreateButton("Xóa", new Point(840,20), Color.FromArgb(220,20,60), BtnDelete_Click)
            });

            this.Controls.Add(formPanel);

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
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Parent = gridPanel
            };

            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;
        }

        private void CreateLabelAndControl(Panel panel, string label, out TextBox txt, int y)
        {
            new Label { Text = label, Location = new Point(20, y + 2), Parent = panel };
            txt = new TextBox { Location = new Point(130, y), Size = new Size(300, 28), Parent = panel };
        }

        private Button CreateButton(string text, Point loc, Color color, EventHandler click)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(100, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += click;
            return btn;
        }

        #endregion

        #region Load Data

        private void LoadCombos()
        {
            cboCategory.DataSource = _categoryService.GetAllCategories()
                .Select(c => new { Id = (int)c.Element("Id"), TenLoai = (string)c.Element("TenLoai") })
                .ToList();

            cboCategory.DisplayMember = "TenLoai";
            cboCategory.ValueMember = "Id";

            cboBrand.DataSource = _brandService.GetAllBrands()
                .Select(b => new { Id = (int)b.Element("Id"), TenThuongHieu = (string)b.Element("TenThuongHieu") })
                .ToList();

            cboBrand.DisplayMember = "TenThuongHieu";
            cboBrand.ValueMember = "Id";

            cboCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
        }

        private void LoadData()
        {
            dgvProducts.DataSource = ConvertToTable(_productService.GetAllProducts());
            dgvProducts.Columns["Id"].Visible = false;
        }

        private DataTable ConvertToTable(List<XElement> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên sản phẩm");
            dt.Columns.Add("Mô tả");
            dt.Columns.Add("Giá", typeof(decimal));
            dt.Columns.Add("Danh mục");
            dt.Columns.Add("Thương hiệu");
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var p in list)
            {
                dt.Rows.Add(
                    (int)p.Element("Id"),
                    (string)p.Element("TenSanPham"),
                    (string)p.Element("MoTa"),
                    decimal.Parse(p.Element("Gia").Value, CultureInfo.InvariantCulture),
                    GetCategoryName((int)p.Element("MaLoai")),
                    GetBrandName((int)p.Element("MaThuongHieu")),
                    (bool)p.Element("HienThi")
                );
            }
            return dt;
        }

        #endregion

        #region Events

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Giá không hợp lệ"); return;
            }

            int newId = _productService.GetAllProducts()
                .Select(p => (int)p.Element("Id"))
                .DefaultIfEmpty(0)
                .Max() + 1;

            var product = new XElement("SanPham",
                new XElement("Id", newId),
                new XElement("TenSanPham", txtProductName.Text.Trim()),
                new XElement("MoTa", txtDescription.Text.Trim()),
                new XElement("ChiTiet", ""),
                new XElement("Gia", price.ToString(CultureInfo.InvariantCulture)),
                new XElement("GiaKhuyenMai", 0),
                new XElement("DuongDanAnh", ""),
                new XElement("SoLuongTon", 0),
                new XElement("MaLoai", cboCategory.SelectedValue),
                new XElement("MaThuongHieu", cboBrand.SelectedValue),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _productService.AddProduct(product);
            LoadData();
            ClearForm();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            var product = _productService.GetProductById(id);

            if (product == null) return;

            product.Element("TenSanPham").Value = txtProductName.Text.Trim();
            product.Element("MoTa").Value = txtDescription.Text.Trim();
            product.Element("Gia").Value = txtPrice.Text;
            product.Element("MaLoai").Value = cboCategory.SelectedValue.ToString();
            product.Element("MaThuongHieu").Value = cboBrand.SelectedValue.ToString();
            product.Element("HienThi").Value = chkDisplay.Checked.ToString();

            _productService.UpdateProduct(product);
            LoadData();
            ClearForm();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            _productService.DeleteProduct(id);
            LoadData();
            ClearForm();
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            var r = dgvProducts.SelectedRows[0];
            txtProductName.Text = r.Cells["Tên sản phẩm"].Value.ToString();
            txtDescription.Text = r.Cells["Mô tả"].Value.ToString();
            txtPrice.Text = r.Cells["Giá"].Value.ToString();
            chkDisplay.Checked = (bool)r.Cells["Hiển thị"].Value;
        }

        #endregion

        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            chkDisplay.Checked = true;
            cboCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
        }

        private string GetCategoryName(int id)
            => _categoryService.GetCategoryById(id)?.Element("TenLoai")?.Value ?? "N/A";

        private string GetBrandName(int id)
            => _brandService.GetBrandById(id)?.Element("TenThuongHieu")?.Value ?? "N/A";
    }
}
