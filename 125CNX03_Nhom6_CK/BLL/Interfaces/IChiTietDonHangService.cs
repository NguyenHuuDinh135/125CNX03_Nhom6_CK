using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IChiTietDonHangService
    {
        List<XElement> GetAllOrderItems();
        XElement GetOrderItemById(int id);
        void AddOrderItem(XElement orderItem);
        void UpdateOrderItem(XElement orderItem);
        void DeleteOrderItem(int id);
        List<XElement> GetOrderItemsByOrderId(int orderId);
    }
}