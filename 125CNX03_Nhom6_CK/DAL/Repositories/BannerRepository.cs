using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly string _filePath = "Data/Banner.xml";
        private readonly string _tableName = "Banner";

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

                if (doc.Root == null)
                {
                    doc.Add(new XElement("NewDataSet"));
                }

                doc.Root.Add(entity);
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

                int idValue = int.Parse(entity.Element("Id")?.Value ?? "0");

                var element = doc.Descendants(_tableName).FirstOrDefault(e =>
                    int.TryParse(e.Element("Id")?.Value, out var elementId) &&
                    elementId == idValue);

                if (element != null)
                {
                    // Thay node cũ bằng node mới hoàn toàn
                    element.ReplaceWith(new XElement(entity));
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

        public List<XElement> GetActiveBanners()
        {
            return GetAll().Where(b =>
                b.Element("HienThi") != null &&
                bool.TryParse(b.Element("HienThi").Value, out var hienThi) &&
                hienThi).ToList();
        }

        public List<XElement> GetBannersByOrder()
        {
            return GetAll().OrderBy(b =>
                int.TryParse(b.Element("ThuTu")?.Value, out var thuTu) ? thuTu : 0
            ).ToList();
        }
    }
}