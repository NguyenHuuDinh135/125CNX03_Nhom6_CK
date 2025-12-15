using _125CNX03_Nhom6_CK.BLL;
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
using _125CNX03_Nhom6_CK.GUI.Interfaces;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class ProductManagementForm : Form, ISearchableForm
    {
        private readonly ISanPhamService _productService;
        private readonly ILoaiSanPhamService _categoryService;
        private readonly IThuongHieuService _brandService;

        private TextBox txtProductName, txtDescription, txtPrice;
        private ComboBox cboCategory, cboBrand;
        private CheckBox chkDisplay;
        private DataGridView dgvProducts;
        private PictureBox picProductImage; // Xem trước ảnh lớn
        private List<XElement> _allProducts;
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
            this.Size = new Size(1350, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10F);

            Panel formPanel = new Panel
            {
                Size = new Size(1300, 260),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            CreateLabelAndControl(formPanel, "Tên sản phẩm:", out txtProductName, 20);
            CreateLabelAndControl(formPanel, "Mô tả:", out txtDescription, 60);
            CreateLabelAndControl(formPanel, "Giá (VNĐ):", out txtPrice, 100);

            // Danh mục
            new Label { Text = "Danh mục:", Location = new Point(20, 140), AutoSize = true, Parent = formPanel };
            cboCategory = new ComboBox
            {
                Location = new Point(130, 138),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            // Thương hiệu
            new Label { Text = "Thương hiệu:", Location = new Point(460, 140), AutoSize = true, Parent = formPanel };
            cboBrand = new ComboBox
            {
                Location = new Point(570, 138),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Parent = formPanel
            };

            // ẢNH XEM TRƯỚC - ĐỂ RIÊNG BÊN PHẢI
            new Label { Text = "Ảnh xem trước:", Location = new Point(900, 20), AutoSize = true, Parent = formPanel };
            picProductImage = new PictureBox
            {
                Size = new Size(380, 220),
                Location = new Point(900, 50),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(380, 220),
                Parent = formPanel
            };

            // CHECKBOX "HIỂN THỊ" DỜI XUỐNG DƯỚI, NGANG HÀNG VỚI CÁC NÚT
            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(800, 200),
                Size = new Size(120, 30),
                Checked = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                Parent = formPanel
            };

            // 4 NÚT + CHECKBOX NẰM CÙNG HÀNG ĐẸP MẮT
            formPanel.Controls.AddRange(new Control[]
            {
                CreateButton("Thêm mới",   new Point(30,  200), Color.FromArgb(40, 167, 69),   BtnAdd_Click),
                CreateButton("Cập nhật",  new Point(180, 200), Color.FromArgb(0, 123, 255),   BtnUpdate_Click),
                CreateButton("Xóa",       new Point(330, 200), Color.FromArgb(220, 53, 69),   BtnDelete_Click),
                CreateButton("Làm mới",   new Point(480, 200), Color.FromArgb(108, 117, 125), (s,e) => { ClearForm(); dgvProducts.ClearSelection(); }),
                CreateButton("Xuất HTML", new Point(630, 200), Color.FromArgb(23, 162, 184), BtnExportHtml_Click)
            });

            this.Controls.Add(formPanel);

            // Phần dưới (danh sách) giữ nguyên 100%
            Panel gridPanel = new Panel
            {
                Size = new Size(1300, 520),
                Location = new Point(20, 300),
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
                Size = new Size(1260, 440),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                Parent = gridPanel
            };
            dgvProducts.RowTemplate.Height = 90; // Đúng cú pháp

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
                Size = new Size(740, 30),
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

        #region Load Data
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
                        using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(8) })
                        {
                            byte[] bytes = await client.GetByteArrayAsync(url);
                            using (var ms = new MemoryStream(bytes))
                            {
                                var original = Image.FromStream(ms);
                                img = ResizeImage(original, 100, 80);
                                original.Dispose();
                            }
                        }
                    }
                    catch
                    {
                        img = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(100, 80);
                    }
                }

                row.Cells[imageColIndex].Value = img;
                row.Height = 90;
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
        private string GenerateDanhSachHtml(List<XElement> products)
        {
            var sb = new StringBuilder();

            sb.Append(@"<!DOCTYPE html>
<html lang='vi'>
<head>
<meta charset='UTF-8'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'>
<title>Danh sách sản phẩm</title>
<style>
body { font-family: Arial; background:#f4f4f4; margin:0; padding:20px; }
h1 { text-align:center; }

.product-grid {
    display:grid;
    grid-template-columns:repeat(auto-fit,minmax(280px,1fr));
    gap:20px;
    max-width:1200px;
    margin:auto;
}

.product-card {
    background:white;
    border-radius:10px;
    overflow:hidden;
    cursor:pointer;
    box-shadow:0 4px 8px rgba(0,0,0,.1);
    transition:.3s;
}
.product-card:hover { transform:translateY(-8px); }

.product-img { width:100%; height:200px; object-fit:cover; }

.product-info { padding:15px; text-align:center; }
.price { color:#e91e63; font-size:18px; font-weight:bold; }

/* DETAIL */
#detail-view {
    display:none;
    position:fixed;
    inset:0;
    background:rgba(0,0,0,.9);
    z-index:1000;
    overflow:auto;
    padding:20px;
}
.old-price { text-decoration:line-through; color:#999; }
.new-price { font-size:2em; color:#e91e63; }
</style>
</head>
<body>

<h1>DANH SÁCH SẢN PHẨM</h1>
<div class='product-grid'>");

            foreach (var p in products)
            {
                string id = p.Element("Id")?.Value ?? "";
                string ten = EscapeJsString(p.Element("TenSanPham")?.Value ?? "");
                string anh = p.Element("DuongDanAnh")?.Value ?? "";
                decimal gia = decimal.TryParse(p.Element("Gia")?.Value, out var g) ? g : 0;

                sb.Append($@"
<div class='product-card' onclick=""openDetail('{id}')"">
    <img src='{anh}' class='product-img'>
    <div class='product-info'>
        <h3>{ten}</h3>
        <p class='price'>{gia:#,##0} ₫</p>
    </div>
</div>");
            }

            sb.Append(@"</div>

<div id='detail-view'>
  <div style='background:white;max-width:1100px;margin:auto;border-radius:16px;padding:30px'>
    <a href='javascript:backToList()'>← Quay lại</a>
    <h1 id='d-ten'></h1>
    <img id='d-anh' style='max-width:400px;width:100%'>
    <p id='d-mota'></p>
    <h3>Thông tin chi tiết</h3>
    <p id='d-chitiet'></p>
    <div id='d-gia'></div>
  </div>
</div>

<script>
const products = {");

            var list = products.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var p = list[i];
                bool isLast = i == list.Count - 1;

                string id = p.Element("Id")?.Value ?? "";
                string ten = EscapeJsString(p.Element("TenSanPham")?.Value ?? "");
                string mota = EscapeJsString(p.Element("MoTa")?.Value ?? "");
                string chitiet = EscapeJsString(p.Element("ChiTiet")?.Value ?? "");
                string anh = p.Element("DuongDanAnh")?.Value ?? "";

                decimal gia = decimal.TryParse(p.Element("Gia")?.Value, out var g) ? g : 0;
                decimal giaKM = decimal.TryParse(p.Element("GiaKhuyenMai")?.Value, out var gkm) ? gkm : 0;

                sb.Append($@"
'{id}': {{
  ten:`{ten}`,
  mota:`{mota}`,
  chitiet:`{chitiet}`,
  gia:`{gia:#,##0} ₫`,
  giakm:`{(giaKM > 0 ? giaKM.ToString("#,##0") + " ₫" : "")}`,
  giaNum:{gia},
  giakmNum:{giaKM},
  anh:`{anh}`
}}{(isLast ? "" : ",")}");
            }

            sb.Append(@"
};

function openDetail(id) {
    const p = products[id];
    if (!p) {
        alert('Không tìm thấy sản phẩm ID: ' + id);
        return;
    }

    // Ẩn danh sách
    document.querySelector('.product-grid').style.display = 'none';

    // Hiện chi tiết
    const detailView = document.getElementById('detail-view');
    detailView.style.display = 'block';
    detailView.style.opacity = '1';

    // LẤY DOM
    const dTen = document.getElementById('d-ten');
    const dMota = document.getElementById('d-mota');
    const dChiTiet = document.getElementById('d-chitiet');
    const dAnh = document.getElementById('d-anh');
    const dGia = document.getElementById('d-gia');

    // ĐỔ DỮ LIỆU
    dTen.innerText = p.ten || '';
    dMota.innerText = p.mota || 'Không có mô tả';
    dChiTiet.innerHTML = (p.chitiet || '').replace(/\n/g, '<br>');
    dAnh.src = p.anh || 'https://via.placeholder.com/400x400?text=No+Image';

    // GIÁ
    if (p.giakmNum > 0 && p.giakmNum < p.giaNum) {
        dGia.innerHTML = `
            <span class=""old-price"">${p.gia}</span>
            <span class=""new-price"">${p.giakm}</span>
        `;
    } else {
        dGia.innerHTML = `<span class=""new-price"">${p.gia}</span>`;
    }
}

function backToList() {
    document.getElementById('detail-view').style.display = 'none';
    document.querySelector('.product-grid').style.display = 'grid';
}

</script>
</body>
</html>");

            return sb.ToString();
        }


        #endregion

        #region Events
        private string EscapeJsString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input
                .Replace("\\", "\\\\")
                .Replace("`", "\\`")
                .Replace("$", "\\$")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }

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
                new XElement("DuongDanAnh", ""),
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

            txtProductName.Text = row.Cells["Tên sản phẩm"].Value?.ToString() ?? "";
            txtDescription.Text = row.Cells["Mô tả"].Value?.ToString() ?? "";

            if (row.Cells["Giá"].Value != null && decimal.TryParse(row.Cells["Giá"].Value.ToString(), out decimal gia))
                txtPrice.Text = gia.ToString("#,##0");
            else
                txtPrice.Text = "0";

            if (row.Cells["Ảnh"].Value is Image img && img != null)
            {
                picProductImage.Image?.Dispose();
                picProductImage.Image = new Bitmap(img); // copy để tránh lỗi dispose
            }
            else
            {
                picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(200, 200);
            }

            chkDisplay.Checked = row.Cells["Hiển thị"].Value != null && bool.TryParse(row.Cells["Hiển thị"].Value.ToString(), out bool ht) ? ht : true;

            var product = _productService.GetProductById(row.Cells["Id"].Value != null ? (int)row.Cells["Id"].Value : 0);
            if (product != null)
            {
                if (int.TryParse(product.Element("MaLoai")?.Value, out int categoryId))
                    cboCategory.SelectedValue = categoryId;
                if (int.TryParse(product.Element("MaThuongHieu")?.Value, out int brandId))
                    cboBrand.SelectedValue = brandId;
            }
        }

        private void BtnExportHtml_Click(object sender, EventArgs e)
        {
            var products = _productService.GetAllProducts();

            string html = GenerateDanhSachHtml(products);

            // Tạo file HTML tạm
            string tempPath = Path.Combine(
                Path.GetTempPath(),
                $"SanPham_{DateTime.Now:yyyyMMddHHmmss}.html"
            );

            File.WriteAllText(tempPath, html);

            // Mở trình duyệt mặc định
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = tempPath,
                UseShellExecute = true
            });
        }


        private void ClearForm()
        {
            txtProductName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            chkDisplay.Checked = true;
            cboCategory.SelectedIndex = 0;
            cboBrand.SelectedIndex = 0;
            picProductImage.Image = Properties.Resources.DefaultProductImage ?? CreatePlaceholderImage(200, 200);
            dgvProducts.ClearSelection();
        }
        #endregion

        private string GetCategoryName(int id) =>
            _categoryService.GetCategoryById(id)?.Element("TenLoai")?.Value ?? "Chưa xác định";

        private string GetBrandName(int id) =>
            _brandService.GetBrandById(id)?.Element("TenThuongHieu")?.Value ?? "Chưa xác định";
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

    }
}
