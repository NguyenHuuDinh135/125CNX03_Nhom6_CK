using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ProductManagementForm : Form, ISearchableForm
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IThuongHieuService _brandService;

        private TextBox txtProductName, txtDescription, txtPrice, txtImageUrl;
        private ComboBox cboCategory, cboBrand;
        private CheckBox chkDisplay;
        private DataGridView dgvProducts;
        private PictureBox picProductImage;
        private List<XElement> _allProducts;

        public ProductManagementForm()
        {
            // InitializeComponent(); // Bỏ comment nếu dùng Designer

            _productService = new SanPhamService();
            _categoryService = new LoaiSanPhamService();
            _brandService = new ThuongHieuService();

            InitializeUI();
            LoadCombos();
            LoadData();
        }

        #region UI Initialization
        private void InitializeUI()
        {
            this.Text = "Quản lý sản phẩm";
            this.Size = new Size(1350, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Panel formPanel = new Panel
            {
                Size = new Size(1300, 320),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            CreateLabelAndControl(formPanel, "Tên sản phẩm:", out txtProductName, 20);
            CreateLabelAndControl(formPanel, "Mô tả:", out txtDescription, 60);
            CreateLabelAndControl(formPanel, "Giá (VNĐ):", out txtPrice, 100);

            new Label { Text = "Danh mục:", Location = new Point(20, 140), AutoSize = true, Parent = formPanel };
            cboCategory = new ComboBox
            {
                Location = new Point(130, 138),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            new Label { Text = "Thương hiệu:", Location = new Point(460, 140), AutoSize = true, Parent = formPanel };
            cboBrand = new ComboBox
            {
                Location = new Point(570, 138),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            CreateLabelAndControl(formPanel, "Link ảnh:", out txtImageUrl, 180);
            txtImageUrl.Leave += async (s, e) => await LoadPreviewImage(txtImageUrl.Text);

            new Label { Text = "Ảnh xem trước:", Location = new Point(900, 20), AutoSize = true, Parent = formPanel };
            picProductImage = new PictureBox
            {
                Size = new Size(380, 240),
                Location = new Point(900, 50),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 240),
                Parent = formPanel
            };

            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(800, 240),
                Size = new Size(120, 30),
                Checked = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                Parent = formPanel
            };

            formPanel.Controls.AddRange(new Control[]
            {
                CreateButton("Thêm mới",   new Point(30,  240), Color.FromArgb(40, 167, 69),    BtnAdd_Click),
                CreateButton("Cập nhật",  new Point(180, 240), Color.FromArgb(0, 123, 255),    BtnUpdate_Click),
                CreateButton("Xóa",       new Point(330, 240), Color.FromArgb(220, 53, 69),    BtnDelete_Click),
                CreateButton("Làm mới",   new Point(480, 240), Color.FromArgb(108, 117, 125), (s,e) => { ClearForm(); }),
                CreateButton("Xuất HTML", new Point(630, 240), Color.FromArgb(23, 162, 184), BtnExportHtml_Click)
            });

            this.Controls.Add(formPanel);

            Panel gridPanel = new Panel
            {
                Size = new Size(1300, 460),
                Location = new Point(20, 360),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            new Label
            {
                Text = "Danh sách sản phẩm",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                Location = new Point(20, 15),
                AutoSize = true,
                Parent = gridPanel
            };

            dgvProducts = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(1260, 380),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                Parent = gridPanel
            };
            dgvProducts.RowTemplate.Height = 90;

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            this.Controls.Add(gridPanel);
        }

        private void CreateLabelAndControl(Panel panel, string labelText, out TextBox txt, int y)
        {
            new Label
            {
                Text = labelText,
                Location = new Point(20, y + 3),
                AutoSize = true,
                Parent = panel
            };

            txt = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(660, 30), // Giảm width một chút để tránh đè layout
                Parent = panel
            };
        }

        private Button CreateButton(string text, Point location, Color backColor, EventHandler click)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(120, 45),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += click;
            return btn;
        }
        #endregion

        #region Load Data Logic
        private void LoadCombos()
        {
            var categories = _categoryService.GetAllCategories()
                .Select(c => new { Id = (int)c.Element("Id"), TenLoai = (string)c.Element("TenLoai") })
                .ToList();
            cboCategory.DataSource = categories;
            cboCategory.DisplayMember = "TenLoai";
            cboCategory.ValueMember = "Id";

            var brands = _brandService.GetAllBrands()
                .Select(b => new { Id = (int)b.Element("Id"), TenThuongHieu = (string)b.Element("TenThuongHieu") })
                .ToList();
            cboBrand.DataSource = brands;
            cboBrand.DisplayMember = "TenThuongHieu";
            cboBrand.ValueMember = "Id";
        }

        private async void LoadData()
        {
            _allProducts = _productService.GetAllProducts();
            await BindGrid(_allProducts);
        }

        private async Task BindGrid(List<XElement> products)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Ảnh", typeof(Image));
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Mô tả", typeof(string));
            dt.Columns.Add("Giá", typeof(decimal));
            dt.Columns.Add("Giá KM", typeof(decimal));
            dt.Columns.Add("Tồn kho", typeof(int));
            dt.Columns.Add("Danh mục", typeof(string));
            dt.Columns.Add("Thương hiệu", typeof(string));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var p in products)
            {
                decimal gia = decimal.Parse(p.Element("Gia").Value, CultureInfo.InvariantCulture);
                decimal giaKM = p.Element("GiaKhuyenMai") != null &&
                                decimal.TryParse(p.Element("GiaKhuyenMai").Value, out decimal km)
                                ? km : 0;

                dt.Rows.Add(
                    (int)p.Element("Id"),
                    null,
                    p.Element("TenSanPham")?.Value,
                    p.Element("MoTa")?.Value,
                    gia,
                    giaKM,
                    (int?)p.Element("SoLuongTon") ?? 0,
                    GetCategoryName((int)p.Element("MaLoai")),
                    GetBrandName((int)p.Element("MaThuongHieu")),
                    (bool)p.Element("HienThi")
                );
            }

            dgvProducts.DataSource = dt;
            dgvProducts.DataBindingComplete -= DgvProducts_DataBindingComplete;
            dgvProducts.DataBindingComplete += DgvProducts_DataBindingComplete;

            await LoadImagesAsync(products);
        }

        private void DgvProducts_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvProducts.Columns.Contains("Id")) dgvProducts.Columns["Id"].Visible = false;
            if (dgvProducts.Columns.Contains("Ảnh"))
            {
                dgvProducts.Columns["Ảnh"].Width = 110;
                dgvProducts.Columns["Ảnh"].HeaderText = "Ảnh";
            }
            if (dgvProducts.Columns.Contains("Giá"))
                dgvProducts.Columns["Giá"].DefaultCellStyle.Format = "#,##0 ₫";
            if (dgvProducts.Columns.Contains("Giá KM"))
            {
                dgvProducts.Columns["Giá KM"].DefaultCellStyle.Format = "#,##0 ₫";
                dgvProducts.Columns["Giá KM"].DefaultCellStyle.ForeColor = Color.Red;
                dgvProducts.Columns["Giá KM"].DefaultCellStyle.Font = new Font(dgvProducts.Font, FontStyle.Bold);
            }
        }

        private async Task LoadImagesAsync(List<XElement> products)
        {
            if (!dgvProducts.Columns.Contains("Ảnh")) return;
            int imageColIndex = dgvProducts.Columns["Ảnh"].Index;

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.IsNewRow) continue;

                int id = (int)row.Cells["Id"].Value;
                var product = products.FirstOrDefault(x => (int)x.Element("Id") == id);
                if (product == null) continue;

                string url = product.Element("DuongDanAnh")?.Value?.Trim();
                Image img = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(100, 80);

                if (!string.IsNullOrEmpty(url))
                {
                    try
                    {
                        using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                        {
                            byte[] bytes = await client.GetByteArrayAsync(url);
                            using (var ms = new MemoryStream(bytes))
                            {
                                var original = Image.FromStream(ms);
                                img = ResizeImage(original, 100, 80);
                            }
                        }
                    }
                    catch { }
                }
                row.Cells[imageColIndex].Value = img;
            }
        }
        #endregion

        #region CRUD Events & Logic
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Replace(",", ""), out decimal price) || price <= 0)
            {
                MessageBox.Show("Giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newId = (_productService.GetAllProducts().Max(p => (int?)p.Element("Id")) ?? 0) + 1;

            var newProduct = new XElement("SanPham",
                new XElement("Id", newId),
                new XElement("TenSanPham", txtProductName.Text.Trim()),
                new XElement("MoTa", txtDescription.Text.Trim()),
                new XElement("ChiTiet", ""),
                new XElement("Gia", price.ToString(CultureInfo.InvariantCulture)),
                new XElement("GiaKhuyenMai", 0),
                new XElement("DuongDanAnh", txtImageUrl.Text.Trim()),
                new XElement("SoLuongTon", 0),
                new XElement("MaLoai", cboCategory.SelectedValue),
                new XElement("MaThuongHieu", cboBrand.SelectedValue),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _productService.AddProduct(newProduct);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm sản phẩm thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần sửa!");
                return;
            }

            int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
            var product = _productService.GetProductById(id);
            if (product == null) return;

            product.Element("TenSanPham").Value = txtProductName.Text.Trim();
            product.Element("MoTa").Value = txtDescription.Text.Trim();
            product.Element("DuongDanAnh").Value = txtImageUrl.Text.Trim();
            product.Element("Gia").Value = decimal.Parse(txtPrice.Text.Replace(",", ""), CultureInfo.InvariantCulture)
                .ToString(CultureInfo.InvariantCulture);
            product.Element("MaLoai").Value = cboCategory.SelectedValue.ToString();
            product.Element("MaThuongHieu").Value = cboBrand.SelectedValue.ToString();
            product.Element("HienThi").Value = chkDisplay.Checked.ToString();

            _productService.UpdateProduct(product);
            LoadData();
            MessageBox.Show("Cập nhật thành công!");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
                _productService.DeleteProduct(id);
                LoadData();
                ClearForm();
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) return;

            var row = dgvProducts.SelectedRows[0];
            int id = row.Cells["Id"].Value != null ? (int)row.Cells["Id"].Value : 0;
            var product = _productService.GetProductById(id);

            if (product != null)
            {
                txtProductName.Text = product.Element("TenSanPham")?.Value ?? "";
                txtDescription.Text = product.Element("MoTa")?.Value ?? "";
                txtImageUrl.Text = product.Element("DuongDanAnh")?.Value ?? "";

                if (decimal.TryParse(product.Element("Gia")?.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal gia))
                    txtPrice.Text = gia.ToString("#,##0");
                else
                    txtPrice.Text = "0";

                chkDisplay.Checked = bool.TryParse(product.Element("HienThi")?.Value, out bool ht) ? ht : true;

                if (int.TryParse(product.Element("MaLoai")?.Value, out int categoryId))
                    cboCategory.SelectedValue = categoryId;
                if (int.TryParse(product.Element("MaThuongHieu")?.Value, out int brandId))
                    cboBrand.SelectedValue = brandId;
            }

            if (row.Cells["Ảnh"].Value is Image img && img != null)
                picProductImage.Image = new Bitmap(img);
            else
                picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 240);
        }

        // ==========================================
        // KHU VỰC XỬ LÝ XUẤT HTML ĐẸP (BOOTSTRAP)
        // ==========================================
        private void BtnExportHtml_Click(object sender, EventArgs e)
        {
            try
            {
                var products = _productService.GetAllProducts();
                string htmlContent = GenerateDanhSachHtml(products);

                // Lưu file vào thư mục Temp và mở ngay
                string fileName = $"BaoCaoSanPham_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string filePath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllText(filePath, htmlContent);

                // Mở file bằng trình duyệt mặc định
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateDanhSachHtml(List<XElement> products)
        {
            // Phần đầu của HTML (Bootstrap CDN, FontAwesome, CSS tùy chỉnh)
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Danh Sách Sản Phẩm</title>
    <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css' rel='stylesheet'>
    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css'>
    <style>
        body { background-color: #f8f9fa; font-family: 'Segoe UI', sans-serif; }
        .header-bg { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 40px 0; margin-bottom: 30px; }
        .card { border: none; border-radius: 12px; transition: transform 0.3s; box-shadow: 0 4px 6px rgba(0,0,0,0.1); background: white; }
        .card:hover { transform: translateY(-5px); box-shadow: 0 10px 20px rgba(0,0,0,0.15); }
        .card-img-top { height: 200px; object-fit: cover; border-top-left-radius: 12px; border-top-right-radius: 12px; background-color: #eee; }
        .price-tag { color: #dc3545; font-size: 1.1rem; font-weight: bold; }
        .old-price { text-decoration: line-through; color: #6c757d; font-size: 0.9rem; margin-right: 5px; }
        .modal-header { background-color: #f8f9fa; }
    </style>
</head>
<body>

<div class='header-bg text-center'>
    <div class='container'>
        <h1 class='display-4 fw-bold'><i class='fas fa-laptop me-2'></i>SẢN PHẨM CÔNG NGHỆ</h1>
        <p class='lead'>Báo cáo danh sách sản phẩm cập nhật ngày " + DateTime.Now.ToString("dd/MM/yyyy") + @"</p>
    </div>
</div>

<div class='container mb-5'>
    <div class='row g-4'>");

            // Loop tạo Card sản phẩm
            foreach (var p in products)
            {
                string id = p.Element("Id")?.Value;
                string ten = p.Element("TenSanPham")?.Value;
                string anh = p.Element("DuongDanAnh")?.Value;
                decimal gia = decimal.Parse(p.Element("Gia")?.Value ?? "0", CultureInfo.InvariantCulture);

                sb.AppendFormat(@"
        <div class='col-12 col-sm-6 col-md-4 col-lg-3'>
            <div class='card h-100' style='cursor:pointer' onclick=""showDetail('{0}')"">
                <img src='{1}' class='card-img-top' onerror=""this.src='https://via.placeholder.com/300x200?text=No+Image'"">
                <div class='card-body d-flex flex-column'>
                    <h5 class='card-title text-truncate' title='{2}'>{2}</h5>
                    <div class='mt-auto pt-2 border-top'>
                        <div class='d-flex justify-content-between align-items-center'>
                            <span class='price-tag'>{3:N0} ₫</span>
                            <button class='btn btn-sm btn-outline-primary rounded-pill'><i class='fas fa-eye'></i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>", id, anh, System.Net.WebUtility.HtmlEncode(ten), gia);
            }

            sb.Append(@"
    </div>
</div>

<div class='modal fade' id='productModal' tabindex='-1' aria-hidden='true'>
  <div class='modal-dialog modal-lg modal-dialog-centered'>
    <div class='modal-content border-0 shadow-lg'>
      <div class='modal-header'>
        <h5 class='modal-title fw-bold text-primary' id='m-title'>Chi tiết sản phẩm</h5>
        <button type='button' class='btn-close' data-bs-dismiss='modal'></button>
      </div>
      <div class='modal-body'>
        <div class='row'>
            <div class='col-md-5'>
                <img id='m-img' src='' class='img-fluid rounded shadow-sm w-100' onerror=""this.src='https://via.placeholder.com/400?text=No+Image'"">
            </div>
            <div class='col-md-7'>
                <h3 id='m-name' class='fw-bold'></h3>
                <h4 class='text-danger fw-bold mb-3' id='m-price'></h4>
                <div class='mb-3'>
                    <strong><i class='fas fa-align-left me-2'></i>Mô tả:</strong>
                    <p id='m-desc' class='text-muted mt-1'></p>
                </div>
                <div class='bg-light p-3 rounded'>
                    <strong><i class='fas fa-info-circle me-2'></i>Thông số kỹ thuật:</strong>
                    <div id='m-detail' style='white-space: pre-wrap;'></div>
                </div>
            </div>
        </div>
      </div>
      <div class='modal-footer'>
        <button type='button' class='btn btn-secondary' data-bs-dismiss='modal'>Đóng</button>
      </div>
    </div>
  </div>
</div>

<script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js'></script>
<script>
    // Dữ liệu JSON được render từ C#
    const products = {");

            // Loop tạo JSON Data
            var list = products.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                string id = p.Element("Id")?.Value;
                string ten = EscapeJsString(p.Element("TenSanPham")?.Value);
                string mota = EscapeJsString(p.Element("MoTa")?.Value);
                string chitiet = EscapeJsString(p.Element("ChiTiet")?.Value);
                string anh = p.Element("DuongDanAnh")?.Value;
                decimal gia = decimal.Parse(p.Element("Gia")?.Value ?? "0", CultureInfo.InvariantCulture);

                sb.AppendFormat(@"
        '{0}': {{ 
            name: `{1}`, 
            desc: `{2}`, 
            detail: `{3}`, 
            img: `{4}`, 
            price: `{5:N0} ₫` 
        }}{6}", id, ten, mota, chitiet, anh, gia, (i < list.Count - 1 ? "," : ""));
            }

            sb.Append(@"
    };

    const myModal = new bootstrap.Modal(document.getElementById('productModal'));

    function showDetail(id) {
        const p = products[id];
        if(!p) return;

        document.getElementById('m-title').innerText = p.name;
        document.getElementById('m-name').innerText = p.name;
        document.getElementById('m-price').innerText = p.price;
        document.getElementById('m-desc').innerText = p.desc || 'Chưa có mô tả ngắn';
        document.getElementById('m-detail').innerText = p.detail || 'Đang cập nhật...';
        document.getElementById('m-img').src = p.img;
        
        myModal.show();
    }
</script>
</body>
</html>");

            return sb.ToString();
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            txtImageUrl.Clear();
            chkDisplay.Checked = true;
            cboCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
            picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 240);
            dgvProducts.ClearSelection();
        }
        #endregion

        #region Helpers & Image Handling
        private async Task LoadPreviewImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 240);
                return;
            }

            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                {
                    byte[] bytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(bytes))
                    {
                        var original = Image.FromStream(ms);
                        picProductImage.Image = ResizeImage(original, 380, 240);
                    }
                }
            }
            catch
            {
                picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 240);
            }
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var ratioX = (double)width / image.Width;
            var ratioY = (double)height / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(width, height);
            using (var g = Graphics.FromImage(newImage))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.White);

                var destRect = new Rectangle((width - newWidth) / 2, (height - newHeight) / 2, newWidth, newHeight);
                g.DrawImage(image, destRect);
            }
            return newImage;
        }

        private Image CreatePlaceholderImage(int width, int height)
        {
            var bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(240, 240, 240));
                g.DrawRectangle(Pens.Gray, 0, 0, width - 1, height - 1);
                string text = "No Image";
                var font = new Font("Segoe UI", 9);
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.DarkGray,
                    (width - size.Width) / 2, (height - size.Height) / 2);
            }
            return bmp;
        }

        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProducts.Columns[e.ColumnIndex].Name == "Giá KM" && e.Value is decimal d && d == 0)
            {
                e.Value = "";
                e.FormattingApplied = true;
            }
        }

        private string GetCategoryName(int id) =>
            _categoryService.GetCategoryById(id)?.Element("TenLoai")?.Value ?? "Chưa xác định";

        private string GetBrandName(int id) =>
            _brandService.GetBrandById(id)?.Element("TenThuongHieu")?.Value ?? "Chưa xác định";

        private string EscapeJsString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Xử lý các ký tự đặc biệt để không làm vỡ chuỗi JSON/JS
            return input.Replace("\\", "\\\\")
                        .Replace("`", "\\`")
                        .Replace("\"", "\\\"")
                        .Replace("$", "\\$")
                        .Replace("\r\n", "<br>")
                        .Replace("\n", "<br>");
        }

        public async void OnSearch(string keyword)
        {
            if (_allProducts == null) return;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                await BindGrid(_allProducts);
                return;
            }

            keyword = keyword.ToLower();
            var filtered = _allProducts.Where(p =>
                (p.Element("TenSanPham")?.Value.ToLower().Contains(keyword) ?? false) ||
                (p.Element("MoTa")?.Value.ToLower().Contains(keyword) ?? false) ||
                (GetCategoryName((int)p.Element("MaLoai")).ToLower().Contains(keyword)) ||
                (GetBrandName((int)p.Element("MaThuongHieu")).ToLower().Contains(keyword)) ||
                (p.Element("Gia")?.Value.Contains(keyword) ?? false) ||
                (p.Element("GiaKhuyenMai")?.Value.Contains(keyword) ?? false)
            ).ToList();

            await BindGrid(filtered);
        }
        #endregion
    }
}