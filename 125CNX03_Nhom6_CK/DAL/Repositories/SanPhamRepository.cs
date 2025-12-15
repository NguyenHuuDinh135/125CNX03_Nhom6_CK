using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly string _filePath = "Data/SanPham.xml";
        private readonly string _tableName = "SanPham";

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
                int idValue = int.Parse(entity.Element("Id").Value);

                var element = doc.Descendants(_tableName)
                    .FirstOrDefault(e => (int)e.Element("Id") == idValue);

                if (element != null)
                {
                    element.ReplaceWith(new XElement(entity));
                    doc.Save(_filePath);
                }
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

        public List<XElement> GetByCategory(int categoryId)
        {
            return GetAll().Where(p =>
                p.Element("MaLoai") != null &&
                int.TryParse(p.Element("MaLoai").Value, out var catId) &&
                catId == categoryId).ToList();
        }

        public List<XElement> GetByBrand(int brandId)
        {
            return GetAll().Where(p =>
                p.Element("MaThuongHieu") != null &&
                int.TryParse(p.Element("MaThuongHieu").Value, out var brand) &&
                brand == brandId).ToList();
        }

        public List<XElement> GetByCategoryAndBrand(int categoryId, int brandId)
        {
            return GetAll().Where(p =>
                p.Element("MaLoai") != null &&
                int.TryParse(p.Element("MaLoai").Value, out var catId) &&
                catId == categoryId &&
                p.Element("MaThuongHieu") != null &&
                int.TryParse(p.Element("MaThuongHieu").Value, out var brand) &&
                brand == brandId).ToList();
        }

        public List<XElement> GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return GetAll().Where(p =>
                decimal.TryParse(p.Element("Gia")?.Value ?? "0", out var price) &&
                price >= minPrice && price <= maxPrice).ToList();
        }

        public List<XElement> GetFeaturedProducts()
        {
            return GetAll().Where(p =>
                p.Element("GiaKhuyenMai") != null &&
                !string.IsNullOrEmpty(p.Element("GiaKhuyenMai").Value) &&
                p.Element("HienThi") != null &&
                bool.TryParse(p.Element("HienThi").Value, out var hienThi) &&
                hienThi).ToList();
        }

        public List<XElement> SearchProducts(string searchTerm)
        {
            return GetAll().Where(p =>
                p.Element("TenSanPham")?.Value
                    .ToLower().Contains(searchTerm.ToLower()) == true ||
                p.Element("MoTa")?.Value
                    .ToLower().Contains(searchTerm.ToLower()) == true ||
                p.Element("ChiTiet")?.Value
                    .ToLower().Contains(searchTerm.ToLower()) == true
            ).ToList();
        }
    }
}