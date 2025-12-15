using _125CNX03_Nhom6_CK.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class DonHangService : IDonHangService
    {
        private readonly IDonHangRepository _orderRepository;
        private readonly IChiTietDonHangRepository _orderItemRepository;
        private readonly ISanPhamRepository _productRepository;
        private readonly IGioHangService _cartService;

        public DonHangService()
        {
            _orderRepository = new DonHangRepository();
            _orderItemRepository = new ChiTietDonHangRepository();
            _productRepository = new SanPhamRepository();
            _cartService = new GioHangService();
        }

        public List<XElement> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        public XElement GetOrderById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public void CreateOrder(XElement order, List<XElement> orderItems)
        {
            order.Element("NgayDatHang").Value = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            decimal total = 0;
            foreach (var item in orderItems)
            {
                var price = decimal.Parse(item.Element("DonGia").Value);
                var quantity = int.Parse(item.Element("SoLuong").Value);
                total += price * quantity;
            }
            order.Element("TongTien").Value = total.ToString();

            _orderRepository.Add(order);

            // Update product quantities
            foreach (var item in orderItems)
            {
                var productId = int.Parse(item.Element("MaSanPham").Value);
                var quantity = int.Parse(item.Element("SoLuong").Value);
                var product = _productRepository.GetById(productId);
                if (product != null)
                {
                    var currentQuantity = int.Parse(product.Element("SoLuongTon").Value);
                    product.Element("SoLuongTon").Value = (currentQuantity - quantity).ToString();
                    _productRepository.Update(product);
                }
            }

            // Save order items
            foreach (var item in orderItems)
            {
                item.SetElementValue("MaDonHang", order.Element("Id")?.Value);
                _orderItemRepository.Add(item);
            }
        }

        public void UpdateOrder(XElement order)
        {
            _orderRepository.Update(order);
        }

        public void DeleteOrder(int id)
        {
            _orderItemRepository.RemoveByOrderId(id);
            _orderRepository.Delete(id);
        }

        public List<XElement> GetOrdersByUserId(int userId)
        {
            return _orderRepository.GetByUserId(userId);
        }

        public List<XElement> GetOrdersByStatus(int status)
        {
            return _orderRepository.GetByStatus(status);
        }

        public List<XElement> GetRecentOrders(int count)
        {
            return _orderRepository.GetRecentOrders(count);
        }

        public void UpdateOrderStatus(int orderId, int newStatus)
        {
            var order = GetOrderById(orderId);
            if (order != null)
            {
                order.Element("TrangThaiDonHang").Value = newStatus.ToString();
                UpdateOrder(order);
            }
        }
    }
}