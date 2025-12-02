namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    partial class BrandCard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxBrand = new System.Windows.Forms.PictureBox();
            this.lblBrandName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrand)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxBrand
            // 
            this.pictureBoxBrand.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBoxBrand.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxBrand.Name = "pictureBoxBrand";
            this.pictureBoxBrand.Size = new System.Drawing.Size(120, 40);
            this.pictureBoxBrand.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxBrand.TabIndex = 0;
            this.pictureBoxBrand.TabStop = false;
            // 
            // lblBrandName
            // 
            this.lblBrandName.AutoSize = true;
            this.lblBrandName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrandName.Location = new System.Drawing.Point(3, 43);
            this.lblBrandName.Name = "lblBrandName";
            this.lblBrandName.Size = new System.Drawing.Size(45, 16);
            this.lblBrandName.TabIndex = 1;
            this.lblBrandName.Text = "Name";
            this.lblBrandName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BrandCard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBrandName);
            this.Controls.Add(this.pictureBoxBrand);
            this.Name = "BrandCard";
            this.Size = new System.Drawing.Size(120, 65);
            this.Load += new System.EventHandler(this.BrandCard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBrand;
        private System.Windows.Forms.Label lblBrandName;
    }
}