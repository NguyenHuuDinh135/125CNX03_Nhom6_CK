using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ChiTietGioHangRepository : IChiTietGioHangRepository
    {
        private readonly string _filePath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ChiTietGioHang.xml");

        private readonly string _tableName = "ChiTietGioHang";

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

        public void Add(XElement entity)
        {
            try
            {
                var doc = XDocument.Load(_filePath);

                // ✅ ĐẢM BẢO ROOT TỒN TẠI
                if (doc.Root == null)
                {
                    doc.Add(new XElement("NewDataSet"));
                }

                var root = doc.Root;

                // ✅ TỰ SINH ID NẾU THIẾU
                if (entity.Element("Id") == null)
                {
                    int nextId = root.Elements(_tableName)
                        .Select(e => (int?)e.Element("Id"))
                        .Max() ?? 0;

                    entity.AddFirst(new XElement("Id", nextId + 1));
                }

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

        public List<XElement> GetByCartId(int cartId)
        {
            return GetAll().Where(ct =>
                ct.Element("MaGioHang") != null &&
                int.TryParse(ct.Element("MaGioHang").Value, out var cartIdValue) &&
                cartIdValue == cartId).ToList();
        }

        public void RemoveByCartId(int cartId)
        {
            var items = GetByCartId(cartId);
            foreach (var item in items)
            {
                Delete(int.Parse(item.Element("Id").Value));
            }
        }

        public XElement GetByCartAndProduct(int cartId, int productId)
        {
            return GetAll().FirstOrDefault(ct =>
                ct.Element("MaGioHang") != null &&
                int.TryParse(ct.Element("MaGioHang").Value, out var cartIdValue) &&
                cartIdValue == cartId &&
                ct.Element("MaSanPham") != null &&
                int.TryParse(ct.Element("MaSanPham").Value, out var productIdValue) &&
                productIdValue == productId);
        }
    }
}