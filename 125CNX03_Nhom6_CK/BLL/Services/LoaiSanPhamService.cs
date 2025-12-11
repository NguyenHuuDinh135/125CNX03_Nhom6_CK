using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;            
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class LoaiSanPhamService : ILoaiSanPhamService
    {
        private readonly ILoaiSanPhamRepository _categoryRepository;

        public LoaiSanPhamService()
        {
            _categoryRepository = new LoaiSanPhamRepository();
        }

        public List<XElement> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public XElement GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public void AddCategory(XElement category)
        {
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(XElement category)
        {
            _categoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
        }

        public List<XElement> GetActiveCategories()
        {
            return _categoryRepository.GetAll();
        }

        public int GenerateNewId()
        {
            var all = _categoryRepository.GetAll();

            if (all == null || all.Count == 0)
                return 1;

            return all.Max(x =>
            {
                int.TryParse(x.Element("Id")?.Value, out int id);
                return id;
            }) + 1;
        }
    }
}
