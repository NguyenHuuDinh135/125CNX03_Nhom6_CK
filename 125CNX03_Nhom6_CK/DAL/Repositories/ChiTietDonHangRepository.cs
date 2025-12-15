using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ChiTietDonHangRepository : IChiTietDonHangRepository
    {
        private readonly string _filePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietDonHang.xml");

        private readonly string _tableName = "ChiTietDonHang";

        public List<XElement> GetAll()
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                return doc.Descendants(_tableName).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading data from {_filePath}: {ex.Message}", ex);
            }
        }

        public XElement GetById(int id)
        {
            var all = GetAll();
            return all.FirstOrDefault(e =>
                e.Element("Id") != null &&
                int.TryParse(e.Element("Id").Value, out var elementId) &&
                elementId == id);
        }
        private int GetNextId(XDocument doc)
        {
            var maxId = doc.Descendants(_tableName)
                .Select(e => (int?)e.Element("Id"))
                .Max();

            return (maxId ?? 0) + 1;
        }

        public void Add(XElement entity)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var root = doc.Root;

                if (root == null)
                {
                    root = new XElement("NewDataSet");
                    doc.Add(root);
                }

                // 🔒 ÉP CÓ MaDonHang
                if (entity.Element("MaDonHang") == null)
                    throw new Exception("ChiTietDonHang thiếu MaDonHang");

                // ⭐ AUTO ID
                int newId = GetNextId(doc);
                entity.AddFirst(new XElement("Id", newId));

                root.Add(entity);
                doc.Save(_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding entity to {_filePath}: {ex.Message}", ex);
            }
        }


        public void Update(XElement entity)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var idValue = int.Parse(entity.Element("Id").Value);
                var element = doc.Descendants(_tableName).FirstOrDefault(e =>
                    e.Element("Id") != null &&
                    int.TryParse(e.Element("Id").Value, out var elementId) &&
                    elementId == idValue);

                if (element != null)
                {
                    element.Remove();
                    doc.Root?.Add(entity);
                }

                doc.Save(_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating entity in {_filePath}: {ex.Message}", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var element = doc.Descendants(_tableName).FirstOrDefault(e =>
                    e.Element("Id") != null &&
                    int.TryParse(e.Element("Id").Value, out var elementId) &&
                    elementId == id);

                if (element != null)
                {
                    element.Remove();
                    doc.Save(_filePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting entity from {_filePath}: {ex.Message}", ex);
            }
        }

        public void Save()
        {
            // XML is saved immediately on each operation, so Save is a no-op
        }

        public List<XElement> GetByOrderId(int orderId)
        {
            return GetAll().Where(ct =>
                ct.Element("MaDonHang") != null &&
                int.TryParse(ct.Element("MaDonHang").Value, out var orderIdValue) &&
                orderIdValue == orderId).ToList();
        }

        public void RemoveByOrderId(int orderId)
        {
            var items = GetByOrderId(orderId);
            foreach (var item in items)
            {
                Delete(int.Parse(item.Element("Id").Value));
            }
        }
    }
}