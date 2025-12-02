using _125CNX03_Nhom6_CK.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class GioHangService : IGioHangService
    {
        private readonly IGioHangRepository _cartRepository;
        private readonly IChiTietGioHangRepository _cartItemRepository;
        private readonly ISanPhamRepository _productRepository;

        public GioHangService()
        {
            _cartRepository = new GioHangRepository();
            _cartItemRepository = new ChiTietGioHangRepository();
            _productRepository = new SanPhamRepository();
        }

        public XElement GetCartByUserId(int userId)
        {
            return _cartRepository.GetByUserId(userId);
        }

        public void CreateCartForUser(int userId)
        {
            var existingCart = GetCartByUserId(userId);
            if (existingCart == null)
            {
                var newCart = new XElement("GioHang",
                    new XElement("MaNguoiDung", userId),
                    new XElement("NgayCapNhat", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"))
                );
                _cartRepository.Add(newCart);
            }
        }

        public void AddProductToCart(int userId, int productId, int quantity)
        {
            var cart = GetCartByUserId(userId);
            if (cart == null)
            {
                CreateCartForUser(userId);
                cart = GetCartByUserId(userId);
            }

            var product = _productRepository.GetById(productId);
            if (product == null)
                throw new InvalidOperationException("Sản phẩm không tồn tại");

            var productQuantity = int.Parse(product.Element("SoLuongTon").Value);
            if (productQuantity < quantity)
                throw new InvalidOperationException("Không đủ số lượng sản phẩm");

            var existingItem = _cartItemRepository.GetByCartAndProduct(int.Parse(cart.Element("Id").Value), productId);
            if (existingItem != null)
            {
                var currentQuantity = int.Parse(existingItem.Element("SoLuong").Value);
                existingItem.Element("SoLuong").Value = (currentQuantity + quantity).ToString();
                _cartItemRepository.Update(existingItem);
            }
            else
            {
                var productPrice = decimal.Parse(product.Element("GiaKhuyenMai")?.Value ?? product.Element("Gia").Value);
                var newItem = new XElement("ChiTietGioHang",
                    new XElement("MaGioHang", cart.Element("Id").Value),
                    new XElement("MaSanPham", productId),
                    new XElement("DonGia", productPrice),
                    new XElement("SoLuong", quantity)
                );
                _cartItemRepository.Add(newItem);
            }

            cart.Element("NgayCapNhat").Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            _cartRepository.Update(cart);
        }

        public void UpdateCartItem(int cartId, int productId, int quantity)
        {
            var item = _cartItemRepository.GetByCartAndProduct(cartId, productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    _cartItemRepository.Delete(int.Parse(item.Element("Id").Value));
                }
                else
                {
                    var product = _productRepository.GetById(productId);
                    if (product != null)
                    {
                        var productQuantity = int.Parse(product.Element("SoLuongTon").Value);
                        if (productQuantity >= quantity)
                        {
                            item.Element("SoLuong").Value = quantity.ToString();
                            _cartItemRepository.Update(item);
                        }
                    }
                }
            }
        }

        public void RemoveProductFromCart(int cartId, int productId)
        {
            var item = _cartItemRepository.GetByCartAndProduct(cartId, productId);
            if (item != null)
            {
                _cartItemRepository.Delete(int.Parse(item.Element("Id").Value));
            }
        }

        public void ClearCart(int cartId)
        {
            _cartItemRepository.RemoveByCartId(cartId);
        }

        public decimal GetCartTotal(int cartId)
        {
            var items = GetCartItems(cartId);
            decimal total = 0;
            foreach (var item in items)
            {
                var price = decimal.Parse(item.Element("DonGia").Value);
                var quantity = int.Parse(item.Element("SoLuong").Value);
                total += price * quantity;
            }
            return total;
        }

        public int GetCartItemCount(int cartId)
        {
            var items = GetCartItems(cartId);
            int count = 0;
            foreach (var item in items)
            {
                count += int.Parse(item.Element("SoLuong").Value);
            }
            return count;
        }

        public List<XElement> GetCartItems(int cartId)
        {
            return _cartItemRepository.GetByCartId(cartId);
        }
    }
}