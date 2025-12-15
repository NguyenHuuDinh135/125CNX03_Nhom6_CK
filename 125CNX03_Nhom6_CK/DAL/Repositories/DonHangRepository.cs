using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class DonHangRepository : IDonHangRepository
    {
        private readonly string _filePath =
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DonHang.xml");

        private readonly string _tableName = "DonHang";

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

                // ⭐ TẠO ID
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

        public List<XElement> GetByUserId(int userId)
        {
            return GetAll().Where(o =>
                o.Element("MaNguoiDung") != null &&
                int.TryParse(o.Element("MaNguoiDung").Value, out var userIdValue) &&
                userIdValue == userId).ToList();
        }

        public List<XElement> GetByStatus(int status)
        {
            return GetAll().Where(o =>
                o.Element("TrangThaiDonHang") != null &&
                int.TryParse(o.Element("TrangThaiDonHang").Value, out var statusValue) &&
                statusValue == status).ToList();
        }

        public List<XElement> GetRecentOrders(int count)
        {
            return GetAll().OrderByDescending(o =>
                DateTime.TryParse(o.Element("NgayDatHang")?.Value, out var date) ? date : DateTime.MinValue
            ).Take(count).ToList();
        }
    }
}