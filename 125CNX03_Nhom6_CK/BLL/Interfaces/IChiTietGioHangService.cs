using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IChiTietGioHangService
    {
        List<XElement> GetAllCartItems();
        XElement GetCartItemById(int id);
        void AddCartItem(XElement cartItem);
        void UpdateCartItem(XElement cartItem);
        void DeleteCartItem(int id);
        List<XElement> GetCartItemsByCartId(int cartId);
    }
}