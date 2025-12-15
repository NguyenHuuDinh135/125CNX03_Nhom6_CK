using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public class BlogForm : UserBaseForm
    {
        private readonly IBaiVietService _articleService;
        private FlowLayoutPanel _flow;

        public BlogForm()
        {
            _articleService = new BaiVietService();
            InitializeUI();
            LoadArticles();
        }

        private void InitializeUI()
        {
            this.Text = "Bài vi?t";
            this.Size = new Size(1000, 700);
            this.BackColor = Surface;
            this.AutoScroll = true;

            var header = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            var title = new Label { Text = "Bài vi?t", Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold), Location = new Point(20, 15), Size = new Size(300, 30) };
            header.Controls.Add(title);
            this.Controls.Add(header);

            _flow = new FlowLayoutPanel
            {
                Location = new Point(20, 100),
                Size = new Size(this.ClientSize.Width - 60, this.ClientSize.Height - 160),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            _flow.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(_flow);
        }

        private void LoadArticles()
        {
            _flow.Controls.Clear();
            List<XElement> articles;
            try
            {
                // Try to get active articles first
                articles = _articleService.GetActiveArticles();
            }
            catch
            {
                articles = _articleService.GetAllArticles();
            }

            foreach (var a in articles)
            {
                var card = CreateArticleCard(a);
                _flow.Controls.Add(card);
            }
        }

        private Control CreateArticleCard(XElement article)
        {
            var panel = new Panel { Width = _flow.ClientSize.Width - 25, Height = 140, BackColor = Color.White, Margin = new Padding(6) };

            var img = new PictureBox { Size = new Size(200, 120), Location = new Point(10, 10), SizeMode = PictureBoxSizeMode.Zoom };
            try
            {
                var imgUrl = article.Element("HinhAnh")?.Value;
                if (!string.IsNullOrWhiteSpace(imgUrl)) img.LoadAsync(imgUrl);
            }
            catch { /* ignore image load errors */ }
            panel.Controls.Add(img);

            var lblTitle = new Label { Text = article.Element("TieuDe")?.Value ?? "", Font = new Font(BaseFont.FontFamily, 12F, FontStyle.Bold), Location = new Point(220, 10), Size = new Size(panel.Width - 240, 28) };
            panel.Controls.Add(lblTitle);

            var lblSummary = new Label { Text = article.Element("TomTat")?.Value ?? "", Font = new Font(BaseFont.FontFamily, 9F), Location = new Point(220, 42), Size = new Size(panel.Width - 240, 56), AutoEllipsis = true };
            panel.Controls.Add(lblSummary);

            var btnRead = CreateButton("Xem chi ti?t", new Point(panel.Width - 120, 96), new Size(100, 30), Primary, (s, e) => ShowArticleDetail(article));
            panel.Controls.Add(btnRead);

            return panel;
        }

        private void ShowArticleDetail(XElement article)
        {
            var detail = new Form { Text = article.Element("TieuDe")?.Value ?? "Bài vi?t", Size = new Size(800, 600), StartPosition = FormStartPosition.CenterParent };
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };
            detail.Controls.Add(pnl);

            var lbl = new Label { Text = article.Element("TieuDe")?.Value ?? "", Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            pnl.Controls.Add(lbl);

            var img = new PictureBox { Location = new Point(20, 60), Size = new Size(740, 240), SizeMode = PictureBoxSizeMode.Zoom };
            try { var url = article.Element("HinhAnh")?.Value; if (!string.IsNullOrWhiteSpace(url)) img.LoadAsync(url); } catch { }
            pnl.Controls.Add(img);

            var content = new TextBox { Location = new Point(20, 320), Size = new Size(740, 200), Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Text = article.Element("NoiDung")?.Value ?? "" };
            pnl.Controls.Add(content);

            detail.ShowDialog();
        }
    }
}
