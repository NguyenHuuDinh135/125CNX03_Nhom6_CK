using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;
using System.Linq;
using _125CNX03_Nhom6_CK.GUI.Interfaces;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class CategoryManagementForm : Form, ISearchableForm
    {
        private readonly ILoaiSanPhamService _categoryService;

        // Khai báo các control làm field để truy cập dễ dàng, an toàn
        private TextBox txtCategoryName;
        private TextBox txtDescription;
        private CheckBox chkDisplay;
        private DataGridView dgvCategories;
        private Button btnAdd, btnUpdate, btnDelete;

        private List<XElement> _allCategories;
        public CategoryManagementForm()
        {
            InitializeComponent();
            _categoryService = new LoaiSanPhamService();
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý danh mục";
            this.Size = new Size(950, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            // === Panel nhập liệu ===
            Panel formPanel = new Panel
            {
                Size = new Size(900, 150),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Tên danh mục
            var lblCategoryName = new Label { Text = "Tên danh mục:", Location = new Point(20, 25), Size = new Size(100, 23) };
            txtCategoryName = new TextBox
            {
                Name = "txtCategoryName",
                Location = new Point(130, 23),
                Size = new Size(300, 25)
            };

            // Mô tả (bạn chưa có trường MoTa trong XSD, nhưng code cũ có dùng → tạm thêm)
            var lblDescription = new Label { Text = "Mô tả:", Location = new Point(20, 60), Size = new Size(100, 23) };
            txtDescription = new TextBox
            {
                Name = "txtDescription",
                Location = new Point(130, 58),
                Size = new Size(300, 25)
            };

            // Hiển thị
            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(130, 95),
                Size = new Size(100, 25),
                Checked = true
            };

            // Các nút
            btnAdd = CreateButton("Thêm", new Point(480, 23), Color.FromArgb(0, 174, 219), BtnAdd_Click);
            btnUpdate = CreateButton("Cập nhật", new Point(600, 23), Color.FromArgb(0, 174, 219), BtnUpdate_Click);
            btnDelete = CreateButton("Xóa", new Point(720, 23), Color.FromArgb(220, 20, 60), BtnDelete_Click);

            formPanel.Controls.AddRange(new Control[] { lblCategoryName, txtCategoryName, lblDescription, txtDescription, chkDisplay, btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(formPanel);

            // === Panel danh sách ===
            Panel gridPanel = new Panel
            {
                Size = new Size(900, 480),
                Location = new Point(20, 190),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var gridTitle = new Label
            {
                Text = "Danh sách danh mục",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(300, 30)
            };

            dgvCategories = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(860, 400),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 35 },
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            dgvCategories.SelectionChanged += DgvCategories_SelectionChanged;

            gridPanel.Controls.AddRange(new Control[] { gridTitle, dgvCategories });
            this.Controls.Add(gridPanel);
        }

        private Button CreateButton(string text, Point location, Color backColor, EventHandler clickHandler)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(100, 35),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Click += clickHandler;
            return btn;
        }

        private void LoadData()
        {
            try
            {
                _allCategories = _categoryService.GetAllCategories();
                BindGrid(_allCategories);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindGrid(List<XElement> categories)
        {
            dgvCategories.DataSource = null;
            dgvCategories.DataSource = ConvertToCategoryTable(categories);
        }
        private System.Data.DataTable ConvertToCategoryTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên loại", typeof(string));
            dt.Columns.Add("Mô tả", typeof(string));  // Nếu không có MoTa thì để trống
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var el in elements)
            {
                dt.Rows.Add(
                    (int)el.Element("Id"),
                    (string)el.Element("TenLoai") ?? "",
                    (string)el.Element("MoTa") ?? "",           // An toàn nếu không có
                    bool.TryParse(el.Element("HienThi")?.Value, out bool hthi) ? hthi : true
                );
            }
            return dt;
        }

        private void DgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count > 0)
            {
                var row = dgvCategories.SelectedRows[0];
                txtCategoryName.Text = row.Cells["Tên loại"].Value?.ToString() ?? "";
                txtDescription.Text = row.Cells["Mô tả"].Value?.ToString() ?? "";
                chkDisplay.Checked = row.Cells["Hiển thị"].Value is bool b && b;
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = _categoryService.GenerateNewId();

            var newCat = new XElement("LoaiSanPham",
                new XElement("Id", newId),
                new XElement("TenLoai", txtCategoryName.Text.Trim()),
                new XElement("MoTa", txtDescription.Text.Trim()),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _categoryService.AddCategory(newCat);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm danh mục thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvCategories.SelectedRows[0].Cells["Id"].Value;
            var cat = _categoryService.GetCategoryById(id);

            if (cat != null)
            {
                cat.Element("TenLoai").Value = txtCategoryName.Text.Trim();
                cat.Element("MoTa").Value = txtDescription.Text.Trim();
                cat.Element("HienThi").Value = chkDisplay.Checked.ToString();

                _categoryService.UpdateCategory(cat);
                LoadData();
                ClearForm();
                MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCategories.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn danh mục để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa danh mục này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = (int)dgvCategories.SelectedRows[0].Cells["Id"].Value;
                _categoryService.DeleteCategory(id);
                LoadData();
                ClearForm();
                MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ClearForm()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
            chkDisplay.Checked = true;
            txtCategoryName.Focus();
        }
        public void OnSearch(string keyword)
        {
            if (_allCategories == null) return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindGrid(_allCategories);
                return;
            }

            keyword = keyword.ToLower();

            var filtered = _allCategories.Where(c =>
                c.Element("TenLoai")?.Value
                    .ToLower()
                    .Contains(keyword) == true
            ).ToList();

            BindGrid(filtered);
        }

    }
}   