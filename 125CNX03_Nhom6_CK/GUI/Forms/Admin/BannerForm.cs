using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.Collections.Generic;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class BannerForm : Form
    {
        private readonly IBannerService _bannerService;

        public BannerForm()
        {
            InitializeComponent();
            _bannerService = new BannerService();

            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý banner";
            this.Size = new Size(950, 720);
            this.BackColor = Color.White;
            this.AutoScroll = true;

            // Create form panel
            Panel formPanel = new Panel();
            formPanel.Size = new Size(this.Width - 40, 165);
            formPanel.Location = new Point(20, 20);
            formPanel.BackColor = Color.White;
            formPanel.BorderStyle = BorderStyle.FixedSingle;

            // Form controls
            Label lblBannerName = new Label();
            lblBannerName.Text = "Tên banner:";
            lblBannerName.Font = new Font("Segoe UI", 9);
            lblBannerName.Location = new Point(20, 20);
            lblBannerName.Size = new Size(100, 20);
            formPanel.Controls.Add(lblBannerName);

            TextBox txtBannerName = new TextBox();
            txtBannerName.Name = "txtBannerName";
            txtBannerName.Font = new Font("Segoe UI", 10);
            txtBannerName.Size = new Size(200, 20);
            txtBannerName.Location = new Point(130, 20);
            txtBannerName.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtBannerName);

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

            Label lblLink = new Label();
            lblLink.Text = "Liên kết:";
            lblLink.Font = new Font("Segoe UI", 9);
            lblLink.Location = new Point(20, 80);
            lblLink.Size = new Size(100, 20);
            formPanel.Controls.Add(lblLink);

            TextBox txtLink = new TextBox();
            txtLink.Name = "txtLink";
            txtLink.Font = new Font("Segoe UI", 10);
            txtLink.Size = new Size(200, 20);
            txtLink.Location = new Point(130, 80);
            txtLink.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(txtLink);

            Label lblOrder = new Label();
            lblOrder.Text = "Thứ tự:";
            lblOrder.Font = new Font("Segoe UI", 9);
            lblOrder.Location = new Point(20, 110);
            lblOrder.Size = new Size(100, 20);
            formPanel.Controls.Add(lblOrder);

            TextBox txtOrder = new TextBox();
            txtOrder.Name = "txtOrder";
            txtOrder.Font = new Font("Segoe UI", 10);
            txtOrder.Size = new Size(200, 20);
            txtOrder.Location = new Point(130, 110);
            txtOrder.BorderStyle = BorderStyle.FixedSingle;
            txtOrder.Text = "0";
            formPanel.Controls.Add(txtOrder);

            CheckBox chkDisplay = new CheckBox();
            chkDisplay.Name = "chkDisplay";
            chkDisplay.Text = "Hiển thị";
            chkDisplay.Font = new Font("Segoe UI", 9);
            chkDisplay.Location = new Point(130, 140);
            chkDisplay.Size = new Size(100, 20);
            chkDisplay.Checked = true;
            formPanel.Controls.Add(chkDisplay);

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
            gridTitle.Text = "Danh sách banner";
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
            var banners = _bannerService.GetAllBanners();
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null)
            {
                dataGridView.DataSource = null;
                dataGridView.DataSource = ConvertToBannerTable(banners);
            }
            ClearForm();
        }

        private System.Data.DataTable ConvertToBannerTable(List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên banner", typeof(string));
            dt.Columns.Add("Đường dẫn ảnh", typeof(string));
            dt.Columns.Add("Liên kết", typeof(string));
            dt.Columns.Add("Thứ tự", typeof(int));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id").Value),
                    element.Element("TenBanner").Value,
                    element.Element("HinhAnh").Value,
                    element.Element("LienKet").Value,
                    int.Parse(element.Element("ThuTu").Value),
                    bool.Parse(element.Element("HienThi").Value)
                );
            }

            return dt;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView == null) return;
            if (dataGridView.SelectedRows.Count == 0) return;

            var selectedRow = dataGridView.SelectedRows[0];

            if (selectedRow.IsNewRow)
            {
                ClearForm();
                return;
            }


            var bannerNameControl = this.Controls[0].Controls.Find("txtBannerName", true)[0] as TextBox;
            var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;
            var linkControl = this.Controls[0].Controls.Find("txtLink", true)[0] as TextBox;
            var orderControl = this.Controls[0].Controls.Find("txtOrder", true)[0] as TextBox;
            var displayControl = this.Controls[0].Controls.Find("chkDisplay", true)[0] as CheckBox;

            bannerNameControl.Text = selectedRow.Cells["Tên banner"].Value?.ToString() ?? "";
            imageUrlControl.Text = selectedRow.Cells["Đường dẫn ảnh"].Value?.ToString() ?? "";
            linkControl.Text = selectedRow.Cells["Liên kết"].Value?.ToString() ?? "";
            orderControl.Text = selectedRow.Cells["Thứ tự"].Value?.ToString() ?? "0";

            bool display = false;
            bool.TryParse(selectedRow.Cells["Hiển thị"].Value?.ToString(), out display);
            displayControl.Checked = display;
        }


        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var bannerNameControl = this.Controls[0].Controls.Find("txtBannerName", true)[0] as TextBox;
            var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;
            var linkControl = this.Controls[0].Controls.Find("txtLink", true)[0] as TextBox;
            var orderControl = this.Controls[0].Controls.Find("txtOrder", true)[0] as TextBox;
            var displayControl = this.Controls[0].Controls.Find("chkDisplay", true)[0] as CheckBox;

            if (string.IsNullOrWhiteSpace(bannerNameControl?.Text) ||
                string.IsNullOrWhiteSpace(imageUrlControl?.Text))
            {
                MessageBox.Show("Vui lòng nhập tên banner và đường dẫn ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = _bannerService.GenerateNewId();

            var newBanner = new XElement("Banner",
                new XElement("Id", newId),
                new XElement("TenBanner", bannerNameControl.Text),
                new XElement("HinhAnh", imageUrlControl.Text),
                new XElement("LienKet", linkControl?.Text ?? ""),
                new XElement("ThuTu", int.TryParse(orderControl?.Text, out var tt) ? tt : 0),
                new XElement("HienThi", displayControl?.Checked ?? true)
            );

            _bannerService.AddBanner(newBanner);
            LoadData();
            ClearForm();

            MessageBox.Show("Thêm banner thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var bannerId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var bannerNameControl = this.Controls[0].Controls.Find("txtBannerName", true)[0] as TextBox;
                var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;
                var linkControl = this.Controls[0].Controls.Find("txtLink", true)[0] as TextBox;
                var orderControl = this.Controls[0].Controls.Find("txtOrder", true)[0] as TextBox;
                var displayControl = this.Controls[0].Controls.Find("chkDisplay", true)[0] as CheckBox;

                var banner = _bannerService.GetBannerById(bannerId);
                if (banner != null)
                {
                    banner.Element("TenBanner").Value = bannerNameControl?.Text ?? "";
                    banner.Element("HinhAnh").Value = imageUrlControl?.Text ?? "";
                    banner.Element("LienKet").Value = linkControl?.Text ?? "";
                    banner.Element("ThuTu").Value = int.Parse(orderControl?.Text ?? "0").ToString();
                    banner.Element("HienThi").Value = displayControl?.Checked.ToString() ?? "false";

                    _bannerService.UpdateBanner(banner);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật banner thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn banner để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var dataGridView = this.Controls[1].Controls[1] as DataGridView;
            if (dataGridView != null && dataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView.SelectedRows[0];
                var bannerId = int.Parse(selectedRow.Cells["Id"].Value?.ToString() ?? "0");

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa banner này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _bannerService.DeleteBanner(bannerId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa banner thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn banner để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ClearForm()
        {
            var bannerNameControl = this.Controls[0].Controls.Find("txtBannerName", true)[0] as TextBox;
            var imageUrlControl = this.Controls[0].Controls.Find("txtImageUrl", true)[0] as TextBox;
            var linkControl = this.Controls[0].Controls.Find("txtLink", true)[0] as TextBox;
            var orderControl = this.Controls[0].Controls.Find("txtOrder", true)[0] as TextBox;
            var displayControl = this.Controls[0].Controls.Find("chkDisplay", true)[0] as CheckBox;

            if (bannerNameControl != null) bannerNameControl.Clear();
            if (imageUrlControl != null) imageUrlControl.Clear();
            if (linkControl != null) linkControl.Clear();
            if (orderControl != null) orderControl.Text = "0";
            if (displayControl != null) displayControl.Checked = true;
        }
    }
}