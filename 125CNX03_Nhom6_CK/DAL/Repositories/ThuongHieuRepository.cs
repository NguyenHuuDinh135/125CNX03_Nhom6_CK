using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        private readonly string _filePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "ThuongHieu.xml");
        private readonly string _tableName = "ThuongHieu";

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
        private int GenerateNewId(List<XElement> all)
        {
            if (all == null || all.Count == 0)
                return 1;

            return all.Max(el =>
            {
                int.TryParse(el.Element("Id")?.Value, out int id);
                return id;
            }) + 1;
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
            var doc = XDocument.Load(_filePath);
            var root = doc.Root;

            var all = root.Elements("ThuongHieu").ToList();

            // Tự sinh Id nếu không có
            if (entity.Element("Id") == null)
            {
                int newId = GenerateNewId(all);
                entity.AddFirst(new XElement("Id", newId));
            }

            root.Add(entity);
            doc.Save(_filePath);
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
    }
}