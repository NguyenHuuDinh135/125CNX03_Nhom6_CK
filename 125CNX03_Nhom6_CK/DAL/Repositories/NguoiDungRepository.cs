using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly string _filePath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "NguoiDung.xml"); 
        private readonly string _tableName = "NguoiDung";

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

        public XElement GetByEmail(string email)
        {
            return GetAll().FirstOrDefault(u =>
                u.Element("Email")?.Value == email);
        }

        public XElement GetByEmailAndPassword(string email, string passwordHash)
        {
            return GetAll().FirstOrDefault(u =>
                u.Element("Email")?.Value == email &&
                u.Element("MatKhauHash")?.Value == passwordHash);
        }

        public List<XElement> GetByRole(string role)
        {
            return GetAll().Where(u =>
                u.Element("VaiTro")?.Value == role).ToList();
        }

        public List<XElement> GetActiveUsers()
        {
            return GetAll().Where(u =>
                u.Element("TrangThai") != null &&
                bool.TryParse(u.Element("TrangThai").Value, out var trangThai) &&
                trangThai).ToList();
        }
    }
}