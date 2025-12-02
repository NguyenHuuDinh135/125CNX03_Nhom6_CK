using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface ISanPhamService
    {
        List<XElement> GetAllProducts();
        XElement GetProductById(int id);
        void AddProduct(XElement product);
        void UpdateProduct(XElement product);
        void DeleteProduct(int id);
        List<XElement> GetProductsByCategory(int categoryId);
        List<XElement> GetProductsByBrand(int brandId);
        List<XElement> GetProductsByCategoryAndBrand(int categoryId, int brandId);
        List<XElement> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        List<XElement> GetFeaturedProducts();
        List<XElement> SearchProducts(string searchTerm);
        void UpdateProductQuantity(int productId, int newQuantity);
    }
}