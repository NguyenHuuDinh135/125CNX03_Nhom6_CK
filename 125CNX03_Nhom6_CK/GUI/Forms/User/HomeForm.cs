using _125CNX03_Nhom6_CK.BLL;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class HomeForm : UserBaseForm
    {
        private readonly ISanPhamService _productService;
        private Panel _mainContainer;

        public HomeForm()
        {
            InitializeComponent();
            _productService = new SanPhamService();

            ConfigureMdiChild();
            InitializeUI();
            LoadData();
        }

        private void ConfigureMdiChild()
        {
            // Remove form border and maximize
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = "";
            this.WindowState = FormWindowState.Maximized;
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#f5f5f5");
        }

        private void InitializeUI()
        {
            // Main scrollable container
            _mainContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ColorTranslator.FromHtml("#f5f5f5"),
                Padding = new Padding(30, 20, 30, 20)
            };
            this.Controls.Add(_mainContainer);

            // Use TableLayoutPanel for better layout management
            var layoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                AutoSize = true,
                BackColor = Color.Transparent
            };
            layoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 360F)); // Banner
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F)); // Header
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Products
            layoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F)); // Categories

            _mainContainer.Controls.Add(layoutPanel);

            // Add sections
            layoutPanel.Controls.Add(CreateBannerSection(), 0, 0);
            layoutPanel.Controls.Add(CreateFeaturedHeader(), 0, 1);
            layoutPanel.Controls.Add(CreateFeaturedProductsSection(), 0, 2);
            layoutPanel.Controls.Add(CreateCategoriesSection(), 0, 3);
        }

        private Panel CreateBannerSection()
        {
            var bannerContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 0, 0, 20)
            };

            var bannerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ColorTranslator.FromHtml("#1a1a2e")
            };
            
            bannerPanel.Paint += (s, e) =>
            {
                var rect = bannerPanel.ClientRectangle;
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    rect,
                    ColorTranslator.FromHtml("#1a1a2e"),
                    ColorTranslator.FromHtml("#16213e"),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, rect);
                }
            };

            // Banner text
            var titleLabel = new Label
            {
                Text = "GIẢM GIÁ LÊN ĐẾN 50%",
                Font = new Font("Segoe UI", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(50, 80)
            };

            var subtitleLabel = new Label
            {
                Text = "Laptop cao cấp - Giá tốt nhất thị trường",
                Font = new Font("Segoe UI", 16F),
                ForeColor = Color.FromArgb(200, 200, 200),
                AutoSize = true,
                Location = new Point(50, 140)
            };

            var shopButton = new Button
            {
                Text = "MUA NGAY",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                Size = new Size(160, 50),
                Location = new Point(50, 200),
                BackColor = ColorTranslator.FromHtml("#e53e3e"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            shopButton.FlatAppearance.BorderSize = 0;
            shopButton.Click += (s, e) => NavigateToProducts();

            bannerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, shopButton });
            bannerContainer.Controls.Add(bannerPanel);

            return bannerContainer;
        }

        private Panel CreateFeaturedHeader()
        {
            var headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var titleLabel = new Label
            {
                Text = "SẢN PHẨM NỔI BẬT",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                AutoSize = true,
                Location = new Point(0, 20)
            };

            var viewAllLink = new LinkLabel
            {
                Text = "Xem tất cả →",
                Font = new Font("Segoe UI", 11F),
                LinkColor = ColorTranslator.FromHtml("#e53e3e"),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Cursor = Cursors.Hand
            };
            viewAllLink.Location = new Point(headerPanel.Width - 120, 25);
            viewAllLink.LinkClicked += (s, e) => NavigateToProducts();

            headerPanel.Controls.AddRange(new Control[] { titleLabel, viewAllLink });
            headerPanel.Resize += (s, e) => viewAllLink.Location = new Point(headerPanel.Width - 120, 25);

            return headerPanel;
        }

        private FlowLayoutPanel CreateFeaturedProductsSection()
        {
            var productsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(0, 10, 0, 10)
            };

            // Load products
            try
            {
                var products = _productService.GetAllProducts().Take(6).ToList();
                foreach (var product in products)
                {
                    var productCard = CreateModernProductCard(product);
                    productsPanel.Controls.Add(productCard);
                }
            }
            catch (Exception ex)
            {
                var errorLabel = new Label
                {
                    Text = "Không thể tải sản phẩm: " + ex.Message,
                    ForeColor = Color.Red,
                    AutoSize = true
                };
                productsPanel.Controls.Add(errorLabel);
            }

            return productsPanel;
        }

        private Panel CreateModernProductCard(System.Xml.Linq.XElement product)
        {
            var cardPanel = new Panel
            {
                Size = new Size(280, 380),
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            // Add border
            cardPanel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 230), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, cardPanel.Width - 1, cardPanel.Height - 1);
                }
            };

            // Product image
            var imagePanel = new Panel
            {
                Size = new Size(260, 200),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(248, 248, 248)
            };

            var pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(248, 248, 248)
            };

            try
            {
                string imageUrl = product.Element("DuongDanAnh")?.Value;

                if (!string.IsNullOrWhiteSpace(imageUrl)
                    && Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                {
                    pictureBox.LoadAsync(imageUrl);
                }
                else
                {
                    pictureBox.Image = Properties.Resources.DefaultProduct;
                }

            }
            catch { }

            imagePanel.Controls.Add(pictureBox);
            cardPanel.Controls.Add(imagePanel);

            // Product name
            var nameLabel = new Label
            {
                Text = product.Element("TenSanPham")?.Value ?? "Sản phẩm",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Location = new Point(10, 220),
                Size = new Size(260, 50),
                ForeColor = Color.FromArgb(33, 33, 33)
            };
            cardPanel.Controls.Add(nameLabel);

            // Category
            var categoryLabel = new Label
            {
                Text = GetCategoryName(int.Parse(product.Element("MaLoai")?.Value ?? "0")),
                Font = new Font("Segoe UI", 9F),
                Location = new Point(10, 275),
                AutoSize = true,
                ForeColor = Color.Gray
            };
            cardPanel.Controls.Add(categoryLabel);

            // Price
            AddPriceLabels(cardPanel, product);

            // Click events
            string productId = product.Element("Id")?.Value ?? "0";
            AddCardClickEvents(cardPanel, productId);

            // Hover effect
            cardPanel.MouseEnter += (s, e) => cardPanel.BackColor = Color.FromArgb(250, 250, 250);
            cardPanel.MouseLeave += (s, e) => cardPanel.BackColor = Color.White;

            return cardPanel;
        }

        private void AddPriceLabels(Panel cardPanel, System.Xml.Linq.XElement product)
        {
            string price = product.Element("Gia")?.Value ?? "0";
            string discountPrice = product.Element("GiaKhuyenMai")?.Value ?? "";

            var pricePanel = new Panel
            {
                Location = new Point(10, 300),
                Size = new Size(260, 35),
                BackColor = Color.Transparent
            };

            if (!string.IsNullOrEmpty(discountPrice) && decimal.Parse(discountPrice) > 0)
            {
                var oldPriceLabel = new Label
                {
                    Text = decimal.Parse(price).ToString("N0") + "₫",
                    Font = new Font("Segoe UI", 10F, FontStyle.Strikeout),
                    ForeColor = Color.Gray,
                    Location = new Point(0, 0),
                    AutoSize = true
                };
                pricePanel.Controls.Add(oldPriceLabel);

                var newPriceLabel = new Label
                {
                    Text = decimal.Parse(discountPrice).ToString("N0") + "₫",
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#e53e3e"),
                    Location = new Point(0, 15),
                    AutoSize = true
                };
                pricePanel.Controls.Add(newPriceLabel);
            }
            else
            {
                var priceLabel = new Label
                {
                    Text = decimal.Parse(price).ToString("N0") + "₫",
                    Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                    ForeColor = ColorTranslator.FromHtml("#e53e3e"),
                    Location = new Point(0, 5),
                    AutoSize = true
                };
                pricePanel.Controls.Add(priceLabel);
            }

            cardPanel.Controls.Add(pricePanel);
        }

        private void AddCardClickEvents(Panel cardPanel, string productId)
        {
            cardPanel.Click += (s, e) => ProductCard_ItemClicked(s, productId);
            
            foreach (Control ctrl in cardPanel.Controls)
            {
                ctrl.Click += (s, e) => ProductCard_ItemClicked(s, productId);
                foreach (Control subCtrl in ctrl.Controls)
                {
                    subCtrl.Click += (s, e) => ProductCard_ItemClicked(s, productId);
                }
            }
        }

        private Panel CreateCategoriesSection()
        {
            var sectionPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };

            var headerLabel = new Label
            {
                Text = "DANH MỤC SẢN PHẨM",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            sectionPanel.Controls.Add(headerLabel);

            var categoriesPanel = new FlowLayoutPanel
            {
                Size = new Size(sectionPanel.Width - 20, 120),
                Location = new Point(0, 60),
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            try
            {
                var categoryService = new LoaiSanPhamService();
                var categories = categoryService.GetAllCategories().Take(4).ToList();

                foreach (var category in categories)
                {
                    var catButton = new Button
                    {
                        Text = category.Element("TenLoai")?.Value ?? "Category",
                        Size = new Size(200, 100),
                        Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                        BackColor = Color.White,
                        ForeColor = Color.FromArgb(33, 33, 33),
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(5),
                        Cursor = Cursors.Hand
                    };
                    catButton.FlatAppearance.BorderColor = Color.FromArgb(220, 220, 220);
                    catButton.Click += (s, e) => NavigateToProducts();

                    categoriesPanel.Controls.Add(catButton);
                }
            }
            catch { }

            sectionPanel.Controls.Add(categoriesPanel);
            return sectionPanel;
        }

        private void NavigateToProducts()
        {
            try
            {
                var mainForm = this.MdiParent as MainForm;
                if (mainForm != null)
                {
                    var method = mainForm.GetType().GetMethod("Sidebar_MenuItemClicked",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    method?.Invoke(mainForm, new object[] { mainForm, "Sản phẩm" });
                }
            }
            catch { }
        }

        private void LoadData()
        {
            // Data loaded in InitializeUI
        }

        private string GetCategoryName(int categoryId)
        {
            try
            {
                var categoryService = new LoaiSanPhamService();
                var category = categoryService.GetCategoryById(categoryId);
                return category?.Element("TenLoai")?.Value ?? "N/A";
            }
            catch
            {
                return "N/A";
            }
        }

        private void ProductCard_ItemClicked(object sender, string productId)
        {
            try
            {
                var productDetailForm = new ProductDetailForm(productId);
                productDetailForm.MdiParent = this.MdiParent;
                productDetailForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể mở chi tiết sản phẩm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}