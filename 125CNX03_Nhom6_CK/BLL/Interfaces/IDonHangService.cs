using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IDonHangService
    {
        List<XElement> GetAllOrders();
        XElement GetOrderById(int id);
        void CreateOrder(XElement order, List<XElement> orderItems);
        void UpdateOrder(XElement order);
        void DeleteOrder(int id);
        List<XElement> GetOrdersByUserId(int userId);
        List<XElement> GetOrdersByStatus(int status);
        List<XElement> GetRecentOrders(int count);
        void UpdateOrderStatus(int orderId, int newStatus);
    }
}