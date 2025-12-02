using System;
using System.Windows.Forms;
using System.Drawing;

namespace _125CNX03_Nhom6_CK.GUI.UserControls
{
    public partial class CategoryCard : UserControl
    {
        public event EventHandler<CategoryCardEventArgs> CategorySelected;

        public CategoryCard()
        {
            InitializeComponent();
        }

        public void SetCategoryInfo(int categoryId, string name, string imageUrl = null)
        {
            lblCategoryName.Text = name;

            // Load image from URL or set default
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    pictureBoxCategory.LoadAsync(imageUrl);
                }
                catch
                {
                    pictureBoxCategory.Image = Properties.Resources.DefaultCategoryImage; // Assuming you have a default image resource
                }
            }
            else
            {
                pictureBoxCategory.Image = Properties.Resources.DefaultCategoryImage;
            }

            this.Tag = categoryId;
        }

        private void CategoryCard_Click(object sender, EventArgs e)
        {
            if (Tag != null && int.TryParse(Tag.ToString(), out int categoryId))
            {
                var args = new CategoryCardEventArgs(categoryId, lblCategoryName.Text);
                CategorySelected?.Invoke(this, args);
            }
        }

        private void CategoryCard_Load(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Size = new Size(150, 100);
            this.Click += CategoryCard_Click;
        }
    }

    public class CategoryCardEventArgs : EventArgs
    {
        public int CategoryId { get; }
        public string CategoryName { get; }

        public CategoryCardEventArgs(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }
    }
}