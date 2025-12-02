using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class ChiTietGioHangService : IChiTietGioHangService
    {
        private readonly IChiTietGioHangRepository _cartItemRepository;

        public ChiTietGioHangService()
        {
            _cartItemRepository = new ChiTietGioHangRepository();
        }

        public List<XElement> GetAllCartItems()
        {
            return _cartItemRepository.GetAll();
        }

        public XElement GetCartItemById(int id)
        {
            return _cartItemRepository.GetById(id);
        }

        public void AddCartItem(XElement cartItem)
        {
            _cartItemRepository.Add(cartItem);
        }

        public void UpdateCartItem(XElement cartItem)
        {
            _cartItemRepository.Update(cartItem);
        }

        public void DeleteCartItem(int id)
        {
            _cartItemRepository.Delete(id);
        }

        public List<XElement> GetCartItemsByCartId(int cartId)
        {
            return _cartItemRepository.GetByCartId(cartId);
        }
    }
}