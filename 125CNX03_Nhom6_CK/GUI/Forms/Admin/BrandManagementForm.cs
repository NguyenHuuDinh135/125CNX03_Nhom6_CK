using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class BrandManagementForm : Form
    {
        private readonly IThuongHieuService _brandService;

        public BrandManagementForm()
        {
            InitializeComponent();
            _brandService = new ThuongHieuService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý thương hiệu";
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
            Label lblBrandName = new Label();
            lblBrandName.Text = "Tên thương hiệu:";
            lblBrandName.Font = new Font("Segoe UI", 9);
            lblBrandName.Location = new Point(20, 20);
            lblBrandName.Size = new Size(100, 20);
            formPanel.Controls.Add(lblBrandName);

            TextBox txtBrandName = new TextBox();
            txtBrandName.Name = "txtBrandName";
            txtBrandName.Font = new Font("Segoe UI", 10);
            txtBrandName.Size = new Size(200, 20);
            txtBrandName.Location = new Point(130, 20);
            txtBrandName.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtBrandName);

            Label lblImageUrl = new Label();
            lblImageUrl.Text = "Đường dẫn ảnh:";
            lblImageUrl.Font = new Font("Segoe UI", 9);
            lblImageUrl.Location = new Point(20, 50);
            lblImageUrl.Size = new Size(100, 20);
            formPanel.Controls.Add(lblImageUrl);

            TextBox txtImageUrl = new TextBox();
            txtImageUrl.Name = "txtImageUrl";
            txtImageUrl.Font = new Font("Segoe UI", 10);
            txtImageUrl.Size = new Size(200, 20);
            txtImageUrl.Location = new Point(130, 50);
            txtImageUrl.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtImageUrl);

            // Action buttons
            Button btnAdd = new Button();
            btnAdd.Text = "Thêm";
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.Size = new Size(100, 30);
            btnAdd.Location = new Point(350, 20);
            btnAdd.BackColor = Color.FromArgb(0, 174, 219);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += BtnAdd_Click;
            formPanel.Controls.Add(btnAdd);

            Button btnUpdate = new Button();
            btnUpdate.Text = "Cập nhật";
            btnUpdate.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnUpdate.Size = new Size(100, 30);
            btnUpdate.Location = new Point(460, 20);
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
            btnDelete.Location = new Point(570, 20);
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
            gridTitle.Text = "Danh sách thương hiệu";
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
            var brands = _brandService.GetAllBrands();
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = null;
                dataGridView.DataSource = ConvertToBrandTable(brands);
            }
        }

        private System.Data.DataTable ConvertToBrandTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên thương hiệu", typeof(string));
            dt.Columns.Add("Đường dẫn ảnh", typeof(string));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    element.Element("TenThuongHieu").Value,
                    element.Element("HinhAnh").Value
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

                var brandNameControl = this.Controls[0].Controls.Find("txtBrandName", true)[0] as TextBox;
                var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;

                if (brandNameControl != null) brandNameControl.Text = selectedRow.Cells["Tên thương hiệu"].Value?.ToString() ?? "";
                if (imageUrlControl != null) imageUrlControl.Text = selectedRow.Cells["Đường dẫn ảnh"].Value?.ToString() ?? "";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var brandNameControl = this.Controls[0].Controls.Find("txtBrandName", true)[0] as TextBox;
            var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;

            if (string.IsNullOrEmpty(brandNameControl?.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thương hiệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newBrand = new XElement("ThuongHieu",
                new XElement("TenThuongHieu", brandNameControl.Text),
                new XElement("HinhAnh", imageUrlControl?.Text ?? "")
            );

            _brandService.AddBrand(newBrand);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var brandId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var brandNameControl = this.Controls[0].Controls.Find("txtBrandName", true)[0] as TextBox;
                var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;

                var brand = _brandService.GetBrandById(brandId);
                if (brand != null)
                {
                    brand.Element("TenThuongHieu").Value = brandNameControl?.Text ?? "";
                    brand.Element("HinhAnh").Value = imageUrlControl?.Text ?? "";

                    _brandService.UpdateBrand(brand);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thương hiệu để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var brandId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa thương hiệu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _brandService.DeleteBrand(brandId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa thương hiệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thương hiệu để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearForm()
        {
            var brandNameControl = this.Controls[0].Controls.Find("txtBrandName", true)[0] as TextBox;
            var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;

            if (brandNameControl != null) brandNameControl.Clear();
            if (imageUrlControl != null) imageUrlControl.Clear();
        }
    }
}