using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class ProductDetailForm : Form
    {
        private readonly ISanPhamService _productService;
        private readonly IGioHangService _cartService;
        private XElement _currentProduct;
        private int _currentUserId = 1; // This should come from the logged-in user

        public ProductDetailForm(XElement product)
        {
            InitializeComponent();
            _productService = new SanPhamService();
            _cartService = new GioHangService();
            _currentProduct = product;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            if (_currentProduct != null)
            {
                lblProductName.Text = _currentProduct.Element("TenSanPham").Value;
                lblDescription.Text = _currentProduct.Element("MoTa").Value;
                txtDetail.Text = _currentProduct.Element("ChiTiet").Value;

                var price = decimal.Parse(_currentProduct.Element("Gia").Value);
                var discountPrice = _currentProduct.Element("GiaKhuyenMai")?.Value;

                if (!string.IsNullOrEmpty(discountPrice) && decimal.Parse(discountPrice) > 0)
                {
                    lblPrice.Text = $"<s>{price:N0}đ</s> {decimal.Parse(discountPrice):N0}đ";
                    lblPrice.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblPrice.Text = $"{price:N0}đ";
                    lblPrice.ForeColor = System.Drawing.Color.Red;
                }

                lblStock.Text = $"Còn lại: {_currentProduct.Element("SoLuongTon").Value} sản phẩm";

                // Load image
                var imageUrl = _currentProduct.Element("DuongDanAnh").Value;
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    try
                    {
                        pictureBoxProduct.LoadAsync(imageUrl);
                    }
                    catch
                    {
                        pictureBoxProduct.Image = Properties.Resources.DefaultProductImage; // Assuming you have a default image resource
                    }
                }
            }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            var quantity = (int)numericQuantity.Value;
            var stock = int.Parse(_currentProduct.Element("SoLuongTon").Value);

            if (quantity > stock)
            {
                MessageBox.Show("Số lượng vượt quá số lượng tồn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _cartService.AddProductToCart(_currentUserId, int.Parse(_currentProduct.Element("Id").Value), quantity);
                MessageBox.Show("Thêm vào giỏ hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm vào giỏ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}