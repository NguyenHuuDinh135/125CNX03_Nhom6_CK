using _125CNX03_Nhom6_CK.BLL;
using _125CNX03_Nhom6_CK.GUI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class BlogManagementForm : Form, ISearchableForm
    {
        private readonly IBaiVietService _articleService;
        private List<XElement> _allArticles;
        private TextBox txtTitle, txtSummary, txtImageUrl, txtContent;
        private CheckBox chkDisplay;
        private DataGridView dgvArticles;

        public BlogManagementForm()
        {
            InitializeComponent();
            _articleService = new BaiVietService();
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            this.Text = "Quản lý bài viết";
            this.Size = new Size(1050, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            Panel formPanel = new Panel
            {
                Size = new Size(1000, 350),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            CreateControl("Tiêu đề:", out txtTitle, 25, formPanel);
            CreateControl("Tóm tắt:", out txtSummary, 65, formPanel);
            CreateControl("Đường dẫn ảnh:", out txtImageUrl, 105, formPanel);

            new Label
            {
                Text = "Nội dung:",
                Location = new Point(20, 145),
                Parent = formPanel,
                Size = new Size(100, 23)
            };

            // Tăng kích thước textbox nội dung
            txtContent = new TextBox
            {
                Location = new Point(130, 145),
                Size = new Size(600, 170),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true,
                Parent = formPanel,
                Font = new Font("Segoe UI", 10),
            };

            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(130, 320),
                Checked = true,
                Parent = formPanel
            };

            var btnAdd = CreateButton("Thêm", new Point(780, 30), Color.FromArgb(0, 174, 219), BtnAdd_Click);
            var btnUpdate = CreateButton("Cập nhật", new Point(780, 90), Color.FromArgb(0, 174, 219), BtnUpdate_Click);
            var btnDelete = CreateButton("Xóa", new Point(780, 150), Color.FromArgb(220, 20, 60), BtnDelete_Click);
            var btnPreview = CreateButton("📖 Xem HTML", new Point(780, 210), Color.FromArgb(76, 175, 80), BtnPreview_Click);

            formPanel.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete, btnPreview });
            this.Controls.Add(formPanel);

            // Grid panel
            Panel gridPanel = new Panel
            {
                Size = new Size(1000, 380),
                Location = new Point(20, 390),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            new Label
            {
                Text = "Danh sách bài viết",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                Parent = gridPanel
            };

            dgvArticles = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(960, 300),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 40 },
                AllowUserToAddRows = false,
                ReadOnly = true,
                Parent = gridPanel
            };
            dgvArticles.SelectionChanged += DgvArticles_SelectionChanged;
            gridPanel.Controls.Add(dgvArticles);
            this.Controls.Add(gridPanel);
        }

        private void BindGrid(List<XElement> articles)
        {
            dgvArticles.DataSource = null;
            dgvArticles.DataSource = ToArticleTable(articles);
        }

        private void CreateControl(string label, out TextBox tb, int y, Panel panel)
        {
            new Label
            {
                Text = label,
                Location = new Point(20, y + 3),
                Size = new Size(100, 23),
                Parent = panel
            };

            tb = new TextBox
            {
                Location = new Point(130, y),
                Size = new Size(600, 28),
                Parent = panel
            };
        }

        private Button CreateButton(string text, Point loc, Color color, EventHandler click)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(120, 45),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };

            btn.Click += click;
            return btn;
        }

        private void LoadData()
        {
            try
            {
                _allArticles = _articleService.GetAllArticles();
                BindGrid(_allArticles);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private DataTable ToArticleTable(List<XElement> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tiêu đề", typeof(string));
            dt.Columns.Add("Tóm tắt", typeof(string));
            dt.Columns.Add("Hình ảnh", typeof(string));
            dt.Columns.Add("Ngày đăng", typeof(DateTime));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var el in list)
            {
                dt.Rows.Add(
                    (int?)el.Element("Id") ?? 0,
                    (string)el.Element("TieuDe") ?? "",
                    (string)el.Element("TomTat") ?? "",
                    (string)el.Element("HinhAnh") ?? "",
                    DateTime.TryParse(el.Element("NgayDang")?.Value, out DateTime date) ? date : DateTime.Now,
                    bool.TryParse(el.Element("HienThi")?.Value, out bool show) && show
                );
            }

            return dt;
        }

        private void DgvArticles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticles.SelectedRows.Count == 0) return;

            var row = dgvArticles.SelectedRows[0];

            txtTitle.Text = row.Cells["Tiêu đề"].Value?.ToString() ?? "";
            txtSummary.Text = row.Cells["Tóm tắt"].Value?.ToString() ?? "";
            txtImageUrl.Text = row.Cells["Hình ảnh"].Value?.ToString() ?? "";

            // Lấy nội dung từ XML gốc
            int id = (int)row.Cells["Id"].Value;
            var article = _articleService.GetArticleById(id);
            txtContent.Text = article?.Element("NoiDung")?.Value ?? "";

            chkDisplay.Checked = row.Cells["Hiển thị"].Value is bool b && b;
        }

        private void BtnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề để xem trước!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tạo XElement tạm để preview
                var previewArticle = new XElement("BaiViet",
                    new XElement("Id", 0),
                    new XElement("TieuDe", txtTitle.Text.Trim()),
                    new XElement("TomTat", txtSummary.Text.Trim()),
                    new XElement("NoiDung", txtContent.Text),
                    new XElement("HinhAnh", txtImageUrl.Text.Trim()),
                    new XElement("NgayDang", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                    new XElement("TacGia", "Admin")
                );

                ShowArticleHTML(previewArticle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo xem trước: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowArticleHTML(XElement article)
        {
            try
            {
                string htmlContent = GenerateArticleHTML(article);
                string fileName = $"Preview_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string tempPath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllText(tempPath, htmlContent, System.Text.Encoding.UTF8);

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở bài viết: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateArticleHTML(XElement article)
        {
            var title = article.Element("TieuDe")?.Value ?? "Không có tiêu đề";
            var summary = article.Element("TomTat")?.Value ?? "";
            var content = article.Element("NoiDung")?.Value ?? "";
            var imageUrl = article.Element("HinhAnh")?.Value ?? "";
            var author = article.Element("TacGia")?.Value ?? "Admin";

            DateTime publishDate = DateTime.Now;
            try
            {
                var dateStr = article.Element("NgayDang")?.Value;
                if (!string.IsNullOrEmpty(dateStr))
                    publishDate = DateTime.Parse(dateStr);
            }
            catch { }

            string formattedContent = content
                .Replace("\r\n", "<br>")
                .Replace("\n", "<br>")
                .Replace("\r", "<br>");

            int readTime = EstimateReadTime(content);

            return $@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        * {{ margin: 0; padding: 0; box-sizing: border-box; }}
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%); min-height: 100vh; padding: 40px 20px; line-height: 1.8; }}
        .container {{ max-width: 900px; margin: 0 auto; background: white; border-radius: 20px; box-shadow: 0 20px 60px rgba(0,0,0,0.15); overflow: hidden; animation: fadeIn 0.6s ease-in; }}
        @keyframes fadeIn {{ from {{ opacity: 0; transform: translateY(20px); }} to {{ opacity: 1; transform: translateY(0); }} }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 50px 40px; position: relative; overflow: hidden; }}
        .header::before {{ content: ''; position: absolute; top: -50%; right: -50%; width: 200%; height: 200%; background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%); animation: pulse 15s infinite; }}
        @keyframes pulse {{ 0%, 100% {{ transform: scale(1); }} 50% {{ transform: scale(1.1); }} }}
        .header-content {{ position: relative; z-index: 1; }}
        .category-badge {{ display: inline-block; background: rgba(255,255,255,0.2); padding: 8px 20px; border-radius: 20px; font-size: 12px; font-weight: 600; letter-spacing: 1px; text-transform: uppercase; margin-bottom: 20px; backdrop-filter: blur(10px); }}
        h1 {{ font-size: 42px; font-weight: 800; line-height: 1.3; margin-bottom: 20px; text-shadow: 2px 2px 4px rgba(0,0,0,0.1); }}
        .meta {{ display: flex; gap: 30px; font-size: 14px; opacity: 0.95; flex-wrap: wrap; }}
        .meta-item {{ display: flex; align-items: center; gap: 8px; }}
        .summary {{ padding: 40px; background: linear-gradient(to bottom, #f8f9fa, white); border-bottom: 3px solid #667eea; }}
        .summary-text {{ font-size: 20px; color: #495057; font-weight: 500; line-height: 1.7; font-style: italic; }}
        .featured-image {{ width: 100%; max-height: 500px; object-fit: cover; display: block; }}
        .content {{ padding: 50px 40px; }}
        .content-text {{ font-size: 18px; color: #333; line-height: 1.9; margin-bottom: 20px; }}
        .content-text br {{ content: ''; display: block; margin: 12px 0; }}
        .footer {{ background: #f8f9fa; padding: 30px 40px; text-align: center; border-top: 1px solid #e9ecef; }}
        .footer-text {{ color: #6c757d; font-size: 14px; }}
        .share-buttons {{ margin-top: 20px; display: flex; gap: 15px; justify-content: center; }}
        .share-btn {{ padding: 10px 20px; border-radius: 25px; border: none; font-weight: 600; cursor: pointer; transition: all 0.3s ease; text-decoration: none; display: inline-block; }}
        .share-facebook {{ background: #1877f2; color: white; }}
        .share-twitter {{ background: #1da1f2; color: white; }}
        .share-btn:hover {{ transform: translateY(-2px); box-shadow: 0 5px 15px rgba(0,0,0,0.2); }}
        @media (max-width: 768px) {{ body {{ padding: 20px 10px; }} .header {{ padding: 30px 20px; }} h1 {{ font-size: 28px; }} .summary, .content {{ padding: 30px 20px; }} .summary-text {{ font-size: 16px; }} .content-text {{ font-size: 16px; }} }}
        @media print {{ body {{ background: white; padding: 0; }} .container {{ box-shadow: none; }} .share-buttons {{ display: none; }} }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='header-content'>
                <div class='category-badge'>📰 Bài viết</div>
                <h1>{title}</h1>
                <div class='meta'>
                    <div class='meta-item'><span>✍️</span><span>{author}</span></div>
                    <div class='meta-item'><span>📅</span><span>{publishDate:dd/MM/yyyy}</span></div>
                    <div class='meta-item'><span>⏱️</span><span>{readTime} phút đọc</span></div>
                </div>
            </div>
        </div>
        {(!string.IsNullOrEmpty(summary) ? $"<div class='summary'><div class='summary-text'>{summary}</div></div>" : "")}
        {(!string.IsNullOrEmpty(imageUrl) ? $"<img src='{imageUrl}' alt='{title}' class='featured-image' onerror=\"this.style.display='none'\">" : "")}
        <div class='content'><div class='content-text'>{formattedContent}</div></div>
        <div class='footer'>
            <div class='footer-text'><p>Cảm ơn bạn đã đọc bài viết! 💙</p><p style='margin-top: 10px;'>© 2025 - Hệ thống quản lý bán hàng</p></div>
            <div class='share-buttons'>
                <button class='share-btn share-facebook' onclick='window.print()'>🖨️ In bài viết</button>
                <button class='share-btn share-twitter' onclick='window.close()'>✖️ Đóng</button>
            </div>
        </div>
    </div>
</body>
</html>";
        }

        private int EstimateReadTime(string content)
        {
            if (string.IsNullOrEmpty(content)) return 1;
            int wordCount = content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            return Math.Max(1, wordCount / 200);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tiêu đề bài viết!", "Thiếu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int newId = _articleService.GenerateNewId();

            var newArticle = new XElement("BaiViet",
                new XElement("Id", newId),
                new XElement("TieuDe", txtTitle.Text.Trim()),
                new XElement("TomTat", txtSummary.Text.Trim()),
                new XElement("NoiDung", txtContent.Text),
                new XElement("HinhAnh", txtImageUrl.Text.Trim()),
                new XElement("NgayDang", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("MaNguoiViet", 1),
                new XElement("TacGia", "Admin"),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _articleService.AddArticle(newArticle);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm bài viết thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvArticles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bài viết cần sửa!");
                return;
            }

            int id = (int)dgvArticles.SelectedRows[0].Cells["Id"].Value;
            var article = _articleService.GetArticleById(id);

            if (article != null)
            {
                article.Element("TieuDe").Value = txtTitle.Text.Trim();
                article.Element("TomTat").Value = txtSummary.Text.Trim();
                article.Element("NoiDung").Value = txtContent.Text;
                article.Element("HinhAnh").Value = txtImageUrl.Text.Trim();
                article.Element("HienThi").Value = chkDisplay.Checked.ToString();

                _articleService.UpdateArticle(article);
                LoadData();
                ClearForm();
                MessageBox.Show("Cập nhật thành công!");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvArticles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn bài viết cần xóa!");
                return;
            }

            if (MessageBox.Show("Xóa bài viết này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = (int)dgvArticles.SelectedRows[0].Cells["Id"].Value;
                _articleService.DeleteArticle(id);
                LoadData();
                ClearForm();
                MessageBox.Show("Xóa thành công!");
            }
        }

        private void ClearForm()
        {
            txtTitle.Clear();
            txtSummary.Clear();
            txtImageUrl.Clear();
            txtContent.Clear();
            chkDisplay.Checked = true;
            txtTitle.Focus();
        }

        public void OnSearch(string keyword)
        {
            if (_allArticles == null) return;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                BindGrid(_allArticles);
                return;
            }

            keyword = keyword.Trim().ToLower();

            var filtered = _allArticles.Where(a =>
                a.Elements().Any(e =>
                    !string.IsNullOrEmpty(e.Value) &&
                    e.Value.ToLower().Contains(keyword)
                )
            ).ToList();

            BindGrid(filtered);
        }
    }
}