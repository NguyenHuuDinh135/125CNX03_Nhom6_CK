using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.IO;
using System.Diagnostics;

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
            this.Text = "Bài viết";
            this.Size = new Size(1000, 700);
            this.BackColor = Surface;
            this.AutoScroll = true;

            var header = CreateSectionPanel(new Point(20, 20), new Size(this.Width - 40, 60));
            var title = new Label
            {
                Text = "Bài viết",
                Font = new Font(BaseFont.FontFamily, 14F, FontStyle.Bold),
                Location = new Point(20, 15),
                Size = new Size(300, 30)
            };
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

            if (articles == null || !articles.Any())
            {
                Label emptyLabel = new Label
                {
                    Text = "Chưa có bài viết nào",
                    Font = new Font("Segoe UI", 12, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Location = new Point(20, 20)
                };
                _flow.Controls.Add(emptyLabel);
                return;
            }

            foreach (var a in articles)
            {
                var card = CreateArticleCard(a);
                _flow.Controls.Add(card);
            }
        }

        private Control CreateArticleCard(XElement article)
        {
            var panel = new Panel
            {
                Width = _flow.ClientSize.Width - 25,
                Height = 160,
                BackColor = Color.White,
                Margin = new Padding(6),
                BorderStyle = BorderStyle.FixedSingle
            };

            var img = new PictureBox
            {
                Size = new Size(220, 140),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            try
            {
                var imgUrl = article.Element("HinhAnh")?.Value;
                if (!string.IsNullOrWhiteSpace(imgUrl))
                {
                    img.LoadAsync(imgUrl);
                }
            }
            catch { /* ignore image load errors */ }
            panel.Controls.Add(img);

            var lblTitle = new Label
            {
                Text = article.Element("TieuDe")?.Value ?? "Không có tiêu đề",
                Font = new Font(BaseFont.FontFamily, 13F, FontStyle.Bold),
                Location = new Point(245, 15),
                Size = new Size(panel.Width - 270, 30),
                AutoSize = false
            };
            panel.Controls.Add(lblTitle);

            var lblSummary = new Label
            {
                Text = article.Element("TomTat")?.Value ?? "",
                Font = new Font(BaseFont.FontFamily, 9.5F),
                Location = new Point(245, 50),
                Size = new Size(panel.Width - 270, 65),
                AutoEllipsis = true
            };
            panel.Controls.Add(lblSummary);

            var btnRead = CreateButton(
                "📖 Đọc bài viết",
                new Point(panel.Width - 140, 115),
                new Size(120, 32),
                Primary,
                (s, e) => ShowArticleDetail(article)
            );
            panel.Controls.Add(btnRead);

            return panel;
        }

        private void ShowArticleDetail(XElement article)
        {
            try
            {
                string htmlContent = GenerateArticleHTML(article);

                // Tạo file HTML trong thư mục Temp
                string articleId = article.Element("Id")?.Value ?? Guid.NewGuid().ToString();
                string fileName = $"BaiViet_{articleId}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string tempPath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllText(tempPath, htmlContent, System.Text.Encoding.UTF8);

                // Mở file HTML trong trình duyệt
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

            // Format nội dung: thay \n thành <br>, giữ nguyên các đoạn văn
            string formattedContent = content
                .Replace("\r\n", "<br>")
                .Replace("\n", "<br>")
                .Replace("\r", "<br>");

            return $@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            min-height: 100vh;
            padding: 40px 20px;
            line-height: 1.8;
        }}
        
        .container {{
            max-width: 900px;
            margin: 0 auto;
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.15);
            overflow: hidden;
            animation: fadeIn 0.6s ease-in;
        }}
        
        @keyframes fadeIn {{
            from {{
                opacity: 0;
                transform: translateY(20px);
            }}
            to {{
                opacity: 1;
                transform: translateY(0);
            }}
        }}
        
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 50px 40px;
            position: relative;
            overflow: hidden;
        }}
        
        .header::before {{
            content: '';
            position: absolute;
            top: -50%;
            right: -50%;
            width: 200%;
            height: 200%;
            background: radial-gradient(circle, rgba(255,255,255,0.1) 0%, transparent 70%);
            animation: pulse 15s infinite;
        }}
        
        @keyframes pulse {{
            0%, 100% {{ transform: scale(1); }}
            50% {{ transform: scale(1.1); }}
        }}
        
        .header-content {{
            position: relative;
            z-index: 1;
        }}
        
        .category-badge {{
            display: inline-block;
            background: rgba(255,255,255,0.2);
            padding: 8px 20px;
            border-radius: 20px;
            font-size: 12px;
            font-weight: 600;
            letter-spacing: 1px;
            text-transform: uppercase;
            margin-bottom: 20px;
            backdrop-filter: blur(10px);
        }}
        
        h1 {{
            font-size: 42px;
            font-weight: 800;
            line-height: 1.3;
            margin-bottom: 20px;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.1);
        }}
        
        .meta {{
            display: flex;
            gap: 30px;
            font-size: 14px;
            opacity: 0.95;
            flex-wrap: wrap;
        }}
        
        .meta-item {{
            display: flex;
            align-items: center;
            gap: 8px;
        }}
        
        .summary {{
            padding: 40px;
            background: linear-gradient(to bottom, #f8f9fa, white);
            border-bottom: 3px solid #667eea;
        }}
        
        .summary-text {{
            font-size: 20px;
            color: #495057;
            font-weight: 500;
            line-height: 1.7;
            font-style: italic;
        }}
        
        .featured-image {{
            width: 100%;
            max-height: 500px;
            object-fit: cover;
            display: block;
        }}
        
        .content {{
            padding: 50px 40px;
        }}
        
        .content-text {{
            font-size: 18px;
            color: #333;
            line-height: 1.9;
            margin-bottom: 20px;
        }}
        
        .content-text br {{
            content: '';
            display: block;
            margin: 12px 0;
        }}
        
        .footer {{
            background: #f8f9fa;
            padding: 30px 40px;
            text-align: center;
            border-top: 1px solid #e9ecef;
        }}
        
        .footer-text {{
            color: #6c757d;
            font-size: 14px;
        }}
        
        .share-buttons {{
            margin-top: 20px;
            display: flex;
            gap: 15px;
            justify-content: center;
        }}
        
        .share-btn {{
            padding: 10px 20px;
            border-radius: 25px;
            border: none;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }}
        
        .share-facebook {{
            background: #1877f2;
            color: white;
        }}
        
        .share-twitter {{
            background: #1da1f2;
            color: white;
        }}
        
        .share-btn:hover {{
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(0,0,0,0.2);
        }}
        
        /* Responsive */
        @media (max-width: 768px) {{
            body {{
                padding: 20px 10px;
            }}
            
            .header {{
                padding: 30px 20px;
            }}
            
            h1 {{
                font-size: 28px;
            }}
            
            .summary, .content {{
                padding: 30px 20px;
            }}
            
            .summary-text {{
                font-size: 16px;
            }}
            
            .content-text {{
                font-size: 16px;
            }}
        }}
        
        /* Print styles */
        @media print {{
            body {{
                background: white;
                padding: 0;
            }}
            
            .container {{
                box-shadow: none;
            }}
            
            .share-buttons {{
                display: none;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div class='header-content'>
                <div class='category-badge'>📰 Bài viết</div>
                <h1>{title}</h1>
                <div class='meta'>
                    <div class='meta-item'>
                        <span>✍️</span>
                        <span>{author}</span>
                    </div>
                    <div class='meta-item'>
                        <span>📅</span>
                        <span>{publishDate:dd/MM/yyyy}</span>
                    </div>
                    <div class='meta-item'>
                        <span>⏱️</span>
                        <span>{EstimateReadTime(content)} phút đọc</span>
                    </div>
                </div>
            </div>
        </div>
        
        {(!string.IsNullOrEmpty(summary) ? $@"
        <div class='summary'>
            <div class='summary-text'>{summary}</div>
        </div>" : "")}
        
        {(!string.IsNullOrEmpty(imageUrl) ? $@"
        <img src='{imageUrl}' alt='{title}' class='featured-image' onerror=""this.style.display='none'"">" : "")}
        
        <div class='content'>
            <div class='content-text'>
                {formattedContent}
            </div>
        </div>
        
        <div class='footer'>
            <div class='footer-text'>
                <p>Cảm ơn bạn đã đọc bài viết! 💙</p>
                <p style='margin-top: 10px;'>© 2025 - Hệ thống quản lý bán hàng</p>
            </div>
            <div class='share-buttons'>
                <button class='share-btn share-facebook' onclick=""window.print()"">
                    🖨️ In bài viết
                </button>
                <button class='share-btn share-twitter' onclick=""window.close()"">
                    ✖️ Đóng
                </button>
            </div>
        </div>
    </div>
</body>
</html>";
        }

        private int EstimateReadTime(string content)
        {
            // Tính thời gian đọc dựa trên số từ (trung bình 200 từ/phút)
            if (string.IsNullOrEmpty(content)) return 1;

            int wordCount = content.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int minutes = Math.Max(1, wordCount / 200);

            return minutes;
        }
    }
}