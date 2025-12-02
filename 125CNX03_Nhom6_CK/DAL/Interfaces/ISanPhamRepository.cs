using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public interface ISanPhamRepository
    {
        List<XElement> GetAll();
        XElement GetById(int id);
        void Add(XElement entity);
        void Update(XElement entity);
        void Delete(int id);
        void Save();
        List<XElement> GetByCategory(int categoryId);
        List<XElement> GetByBrand(int brandId);
        List<XElement> GetByCategoryAndBrand(int categoryId, int brandId);
        List<XElement> GetByPriceRange(decimal minPrice, decimal maxPrice);
        List<XElement> GetFeaturedProducts();
        List<XElement> SearchProducts(string searchTerm);
    }
}