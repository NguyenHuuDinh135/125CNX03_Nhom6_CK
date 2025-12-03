namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.btnProductCatalog = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCart = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBlog = new System.Windows.Forms.ToolStripMenuItem();
            this.btnContact = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnLogin = new System.Windows.Forms.ToolStripButton();
            this.btnRegister = new System.Windows.Forms.ToolStripButton();
            this.btnLogout = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.panelManagement = new System.Windows.Forms.Panel();
            this.btnBannerManagement = new System.Windows.Forms.Button();
            this.btnOrderManagement = new System.Windows.Forms.Button();
            this.btnUserManagement = new System.Windows.Forms.Button();
            this.btnBrandManagement = new System.Windows.Forms.Button();
            this.btnCategoryManagement = new System.Windows.Forms.Button();
            this.btnProductManagement = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panelManagement.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnProductCatalog,
            this.btnCart,
            this.btnBlog,
            this.btnContact});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // btnProductCatalog
            // 
            this.btnProductCatalog.Name = "btnProductCatalog";
            this.btnProductCatalog.Size = new System.Drawing.Size(113, 20);
            this.btnProductCatalog.Text = "Danh mục sản phẩm";
            this.btnProductCatalog.Click += new System.EventHandler(this.btnProductCatalog_Click);
            // 
            // btnCart
            // 
            this.btnCart.Name = "btnCart";
            this.btnCart.Size = new System.Drawing.Size(61, 20);
            this.btnCart.Text = "Giỏ hàng";
            this.btnCart.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // btnBlog
            // 
            this.btnBlog.Name = "btnBlog";
            this.btnBlog.Size = new System.Drawing.Size(46, 20);
            this.btnBlog.Text = "Blog";
            this.btnBlog.Click += new System.EventHandler(this.btnBlog_Click);
            // 
            // btnContact
            // 
            this.btnContact.Name = "btnContact";
            this.btnContact.Size = new System.Drawing.Size(67, 20);
            this.btnContact.Text = "Liên hệ";
            this.btnContact.Click += new System.EventHandler(this.btnContact_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUser});
            this.statusStrip.Location = new System.Drawing.Point(0, 639);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblUser
            // 
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(56, 17);
            this.lblUser.Text = "Chưa đăng nhập";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLogin,
            this.btnRegister,
            this.btnLogout});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1200, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // panelManagement
            // 
            this.panelManagement.Controls.Add(this.btnBannerManagement);
            this.panelManagement.Controls.Add(this.btnOrderManagement);
            this.panelManagement.Controls.Add(this.btnUserManagement);
            this.panelManagement.Controls.Add(this.btnBrandManagement);
            this.panelManagement.Controls.Add(this.btnCategoryManagement);
            this.panelManagement.Controls.Add(this.btnProductManagement);
            this.panelManagement.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelManagement.Location = new System.Drawing.Point(0, 49);
            this.panelManagement.Name = "panelManagement";
            this.panelManagement.Size = new System.Drawing.Size(1200, 40);
            this.panelManagement.TabIndex = 3;
            // 
            // btnBannerManagement
            // 
            this.btnBannerManagement.Location = new System.Drawing.Point(510, 8);
            this.btnBannerManagement.Name = "btnBannerManagement";
            this.btnBannerManagement.Size = new System.Drawing.Size(80, 23);
            this.btnBannerManagement.TabIndex = 5;
            this.btnBannerManagement.Text = "Quản lý Banner";
            this.btnBannerManagement.UseVisualStyleBackColor = true;
            this.btnBannerManagement.Click += new System.EventHandler(this.btnBannerManagement_Click);
            // 
            // btnOrderManagement
            // 
            this.btnOrderManagement.Location = new System.Drawing.Point(410, 8);
            this.btnOrderManagement.Name = "btnOrderManagement";
            this.btnOrderManagement.Size = new System.Drawing.Size(80, 23);
            this.btnOrderManagement.TabIndex = 4;
            this.btnOrderManagement.Text = "Quản lý đơn hàng";
            this.btnOrderManagement.UseVisualStyleBackColor = true;
            this.btnOrderManagement.Click += new System.EventHandler(this.btnOrderManagement_Click);
            // 
            // btnUserManagement
            // 
            this.btnUserManagement.Location = new System.Drawing.Point(310, 8);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Size = new System.Drawing.Size(80, 23);
            this.btnUserManagement.TabIndex = 3;
            this.btnUserManagement.Text = "Quản lý người dùng";
            this.btnUserManagement.UseVisualStyleBackColor = true;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);
            // 
            // btnBrandManagement
            // 
            this.btnBrandManagement.Location = new System.Drawing.Point(210, 8);
            this.btnBrandManagement.Name = "btnBrandManagement";
            this.btnBrandManagement.Size = new System.Drawing.Size(80, 23);
            this.btnBrandManagement.TabIndex = 2;
            this.btnBrandManagement.Text = "Quản lý thương hiệu";
            this.btnBrandManagement.UseVisualStyleBackColor = true;
            this.btnBrandManagement.Click += new System.EventHandler(this.btnBrandManagement_Click);
            // 
            // btnCategoryManagement
            // 
            this.btnCategoryManagement.Location = new System.Drawing.Point(110, 8);
            this.btnCategoryManagement.Name = "btnCategoryManagement";
            this.btnCategoryManagement.Size = new System.Drawing.Size(80, 23);
            this.btnCategoryManagement.TabIndex = 1;
            this.btnCategoryManagement.Text = "Quản lý danh mục";
            this.btnCategoryManagement.UseVisualStyleBackColor = true;
            this.btnCategoryManagement.Click += new System.EventHandler(this.btnCategoryManagement_Click);
            // 
            // btnProductManagement
            // 
            this.btnProductManagement.Location = new System.Drawing.Point(10, 8);
            this.btnProductManagement.Name = "btnProductManagement";
            this.btnProductManagement.Size = new System.Drawing.Size(80, 23);
            this.btnProductManagement.TabIndex = 0;
            this.btnProductManagement.Text = "Quản lý sản phẩm";
            this.btnProductManagement.UseVisualStyleBackColor = true;
            this.btnProductManagement.Click += new System.EventHandler(this.btnProductManagement_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 661);
            this.Controls.Add(this.panelManagement);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Cửa hàng bán laptop";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelManagement.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem btnProductCatalog;
        private System.Windows.Forms.ToolStripMenuItem btnCart;
        private System.Windows.Forms.ToolStripMenuItem btnBlog;
        private System.Windows.Forms.ToolStripMenuItem btnContact;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblUser;
        private System.Windows.Forms.ToolStripButton btnLogin;
        private System.Windows.Forms.ToolStripButton btnRegister;
        private System.Windows.Forms.ToolStripButton btnLogout;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.Panel panelManagement;
        private System.Windows.Forms.Button btnProductManagement;
        private System.Windows.Forms.Button btnCategoryManagement;
        private System.Windows.Forms.Button btnBrandManagement;
        private System.Windows.Forms.Button btnUserManagement;
        private System.Windows.Forms.Button btnOrderManagement;
        private System.Windows.Forms.Button btnBannerManagement;
    }
}