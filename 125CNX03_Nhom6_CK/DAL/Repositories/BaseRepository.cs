using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly string _filePath;
        private readonly string _tableName;

        public BaseRepository(string filePath, string tableName)
        {
            _filePath = filePath;
            _tableName = tableName;
        }

        public List<T> GetAll()
        {
            var entities = new List<T>();
            try
            {
                var doc = XDocument.Load(_filePath);
                var tableElements = doc.Descendants(_tableName);

                foreach (var element in tableElements)
                {
                    var entity = new T();
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        var attribute = element.Element(property.Name);
                        if (attribute != null)
                        {
                            var value = ConvertValue(attribute.Value, property.PropertyType);
                            property.SetValue(entity, value);
                        }
                    }
                    entities.Add(entity);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception($"Error loading data from {_filePath}: {ex.Message}", ex);
            }

            return entities;
        }

        public T GetById(int id)
        {
            var all = GetAll();
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                return all.FirstOrDefault(e => (int)idProperty.GetValue(e) == id);
            }
            return null;
        }

        public void Add(T entity)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var root = doc.Root ?? new XElement("NewDataSet");

                var element = CreateXElement(entity);
                root.Add(element);

                doc.Save(_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding entity to {_filePath}: {ex.Message}", ex);
            }
        }

        public void Update(T entity)
        {
            try
            {
                var doc = XDocument.Load(_filePath);
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty == null) return;

                var idValue = (int)idProperty.GetValue(entity);
                var element = doc.Descendants(_tableName).FirstOrDefault(e =>
                    e.Element("Id") != null &&
                    int.TryParse(e.Element("Id").Value, out var elementId) &&
                    elementId == idValue);

                if (element != null)
                {
                    element.Remove();
                    var newElement = CreateXElement(entity);
                    doc.Root?.Add(newElement);
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

        private XElement CreateXElement(T entity)
        {
            var element = new XElement(_tableName);
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                var elementValue = value?.ToString() ?? "";
                element.Add(new XElement(property.Name, elementValue));
            }

            return element;
        }

        private object ConvertValue(string value, Type targetType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            if (targetType == typeof(int))
                return int.TryParse(value, out var intValue) ? intValue : 0;
            if (targetType == typeof(decimal))
                return decimal.TryParse(value, out var decimalValue) ? decimalValue : 0m;
            if (targetType == typeof(double))
                return double.TryParse(value, out var doubleValue) ? doubleValue : 0.0;
            if (targetType == typeof(float))
                return float.TryParse(value, out var floatValue) ? floatValue : 0.0f;
            if (targetType == typeof(bool))
                return bool.TryParse(value, out var boolValue) ? boolValue : false;
            if (targetType == typeof(DateTime))
                return DateTime.TryParse(value, out var dateValue) ? dateValue : DateTime.Now;
            if (targetType == typeof(string))
                return value;

            // Handle nullable types
            var underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                return ConvertValue(value, underlyingType);
            }

            return value;
        }
        public List<T> Search(string keyword, params string[] fields)
        {
            var result = new List<T>();

            if (string.IsNullOrWhiteSpace(keyword))
                return GetAll();

            keyword = keyword.ToLower();

            try
            {
                var doc = XDocument.Load(_filePath);

                var elements = doc.Descendants(_tableName)
                    .Where(e =>
                        fields.Any(f =>
                            e.Element(f) != null &&
                            e.Element(f).Value.ToLower().Contains(keyword)
                        )
                    );

                foreach (var element in elements)
                {
                    var entity = new T();
                    var properties = typeof(T).GetProperties();

                    foreach (var prop in properties)
                    {
                        var el = element.Element(prop.Name);
                        if (el != null)
                        {
                            prop.SetValue(entity, ConvertValue(el.Value, prop.PropertyType));
                        }
                    }

                    result.Add(entity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Search error in {_filePath}: {ex.Message}", ex);
            }

            return result;
        }

    }
}