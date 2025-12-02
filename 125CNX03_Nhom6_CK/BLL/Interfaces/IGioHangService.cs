using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IGioHangService
    {
        XElement GetCartByUserId(int userId);
        void CreateCartForUser(int userId);
        void AddProductToCart(int userId, int productId, int quantity);
        void UpdateCartItem(int cartId, int productId, int quantity);
        void RemoveProductFromCart(int cartId, int productId);
        void ClearCart(int cartId);
        decimal GetCartTotal(int cartId);
        int GetCartItemCount(int cartId);
        List<XElement> GetCartItems(int cartId);
    }
}