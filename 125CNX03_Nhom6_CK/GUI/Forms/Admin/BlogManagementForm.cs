using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.Admin
{
    public partial class BlogManagementForm : Form
    {
        private readonly IBaiVietService _articleService;

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
            this.Size = new Size(1050, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            Panel formPanel = new Panel
            {
                Size = new Size(1000, 280),
                Location = new Point(20, 20),
                BorderStyle = BorderStyle.FixedSingle,
                Parent = this
            };

            CreateControl("Tiêu đề:", out txtTitle, 25, formPanel);
            CreateControl("Tóm tắt:", out txtSummary, 65, formPanel);
            CreateControl("Đường dẫn ảnh:", out txtImageUrl, 105, formPanel);

            new Label { Text = "Nội dung:", Location = new Point(20, 145), Parent = formPanel, Size = new Size(100, 23) };

            txtContent = new TextBox
            {
                Location = new Point(130, 145),
                Size = new Size(600, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                AcceptsReturn = true,
                Parent = formPanel
            };

            chkDisplay = new CheckBox
            {
                Text = "Hiển thị",
                Location = new Point(130, 255),
                Checked = true,
                Parent = formPanel
            };

            var btnAdd = CreateButton("Thêm", new Point(780, 30), Color.FromArgb(0, 174, 219), BtnAdd_Click);
            var btnUpdate = CreateButton("Cập nhật", new Point(780, 90), Color.FromArgb(0, 174, 219), BtnUpdate_Click);
            var btnDelete = CreateButton("Xóa", new Point(780, 150), Color.FromArgb(220, 20, 60), BtnDelete_Click);

            formPanel.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete });
            this.Controls.Add(formPanel);

            // Grid panel
            Panel gridPanel = new Panel
            {
                Size = new Size(1000, 400),
                Location = new Point(20, 320),
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
                Size = new Size(960, 320),
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

            btn.Click += click;   // <-- QUAN TRỌNG: gán sự kiện click

            return btn;
        }


        private void LoadData()
        {
            try
            {
                var articles = _articleService.GetAllArticles();
                dgvArticles.DataSource = ToArticleTable(articles);
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

            return dt; // ĐÚNG VỊ TRÍ: ở ngoài vòng lặp
        }

        private void DgvArticles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticles.SelectedRows.Count == 0) return;

            var row = dgvArticles.SelectedRows[0];

            txtTitle.Text = row.Cells["Tiêu đề"].Value?.ToString() ?? "";
            txtSummary.Text = row.Cells["Tóm tắt"].Value?.ToString() ?? "";
            txtImageUrl.Text = row.Cells["Hình ảnh"].Value?.ToString() ?? "";
            // Nội dung không có trong grid → lấy từ XML gốc
            int id = (int)row.Cells["Id"].Value;
            var article = _articleService.GetArticleById(id);
            txtContent.Text = article?.Element("NoiDung")?.Value ?? "";

            chkDisplay.Checked = row.Cells["Hiển thị"].Value is bool b && b;
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
    }
}