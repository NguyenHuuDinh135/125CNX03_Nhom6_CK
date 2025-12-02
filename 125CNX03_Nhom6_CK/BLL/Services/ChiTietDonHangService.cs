using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class ChiTietDonHangService : IChiTietDonHangService
    {
        private readonly IChiTietDonHangRepository _orderItemRepository;

        public ChiTietDonHangService()
        {
            _orderItemRepository = new ChiTietDonHangRepository();
        }

        public List<XElement> GetAllOrderItems()
        {
            return _orderItemRepository.GetAll();
        }

        public XElement GetOrderItemById(int id)
        {
            return _orderItemRepository.GetById(id);
        }

        public void AddOrderItem(XElement orderItem)
        {
            _orderItemRepository.Add(orderItem);
        }

        public void UpdateOrderItem(XElement orderItem)
        {
            _orderItemRepository.Update(orderItem);
        }

        public void DeleteOrderItem(int id)
        {
            _orderItemRepository.Delete(id);
        }

        public List<XElement> GetOrderItemsByOrderId(int orderId)
        {
            return _orderItemRepository.GetByOrderId(orderId);
        }
    }
}