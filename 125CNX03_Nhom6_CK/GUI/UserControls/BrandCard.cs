using System;
using System.Windows.Forms;
using System.Drawing;

namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    public partial class BrandCard : UserControl
    {
        public event EventHandler<BrandCardEventArgs> BrandSelected;

        public BrandCard()
        {
            InitializeComponent();
        }

        public void SetBrandInfo(int brandId, string name, string imageUrl = null)
        {
            lblBrandName.Text = name;

            // Load image from URL or set default
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    pictureBoxBrand.LoadAsync(imageUrl);
                }
                catch
                {
                    pictureBoxBrand.Image = Properties.Resources.DefaultBrandImage; // Assuming you have a default image resource
                }
            }
            else
            {
                pictureBoxBrand.Image = Properties.Resources.DefaultBrandImage;
            }

            this.Tag = brandId;
        }

        private void BrandCard_Click(object sender, EventArgs e)
        {
            if (Tag != null && int.TryParse(Tag.ToString(), out int brandId))
            {
                var args = new BrandCardEventArgs(brandId, lblBrandName.Text);
                BrandSelected?.Invoke(this, args);
            }
        }

        private void BrandCard_Load(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(120, 80);
            this.Click += BrandCard_Click;
        }
    }

    public class BrandCardEventArgs : EventArgs
    {
        public int BrandId { get; }
        public string BrandName { get; }

        public BrandCardEventArgs(int brandId, string brandName)
        {
            BrandId = brandId;
            BrandName = brandName;
        }
    }
}