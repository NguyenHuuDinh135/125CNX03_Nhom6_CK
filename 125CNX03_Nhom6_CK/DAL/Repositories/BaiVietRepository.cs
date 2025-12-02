using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class BaiVietRepository : IBaiVietRepository
    {
        private readonly string _filePath = "Data/BaiViet.xml";
        private readonly string _tableName = "BaiViet";

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
                var root = doc.Root ?? new XElement("NewDataSet");
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

        public List<XElement> GetActiveArticles()
        {
            return GetAll().Where(a =>
                a.Element("HienThi") != null &&
                bool.TryParse(a.Element("HienThi").Value, out var hienThi) &&
                hienThi).ToList();
        }

        public List<XElement> GetLatestArticles(int count)
        {
            return GetAll().Where(a =>
                a.Element("HienThi") != null &&
                bool.TryParse(a.Element("HienThi").Value, out var hienThi) &&
                hienThi
            ).OrderByDescending(a =>
                DateTime.TryParse(a.Element("NgayDang")?.Value, out var date) ? date : DateTime.MinValue
            ).Take(count).ToList();
        }

        public List<XElement> GetByAuthor(int authorId)
        {
            return GetAll().Where(a =>
                a.Element("MaNguoiViet") != null &&
                int.TryParse(a.Element("MaNguoiViet").Value, out var authorIdValue) &&
                authorIdValue == authorId).ToList();
        }

        public List<XElement> SearchArticles(string searchTerm)
        {
            return GetAll().Where(a =>
                a.Element("HienThi") != null &&
                bool.TryParse(a.Element("HienThi").Value, out var hienThi) &&
                hienThi &&
                (a.Element("TieuDe")?.Value.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) == true ||
                 a.Element("TomTat")?.Value.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) == true ||
                 a.Element("NoiDung")?.Value.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) == true)
            ).ToList();
        }
    }
}