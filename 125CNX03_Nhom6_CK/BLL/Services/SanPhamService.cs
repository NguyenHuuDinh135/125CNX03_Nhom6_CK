using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamRepository _productRepository;

        public SanPhamService()
        {
            _productRepository = new SanPhamRepository();
        }

        public List<XElement> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public XElement GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public void AddProduct(XElement product)
        {
            _productRepository.Add(product);
        }

        public void UpdateProduct(XElement product)
        {
            _productRepository.Update(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.Delete(id);
        }

        public List<XElement> GetProductsByCategory(int categoryId)
        {
            return _productRepository.GetByCategory(categoryId);
        }

        public List<XElement> GetProductsByBrand(int brandId)
        {
            return _productRepository.GetByBrand(brandId);
        }

        public List<XElement> GetProductsByCategoryAndBrand(int categoryId, int brandId)
        {
            return _productRepository.GetByCategoryAndBrand(categoryId, brandId);
        }

        public List<XElement> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _productRepository.GetByPriceRange(minPrice, maxPrice);
        }

        public List<XElement> GetFeaturedProducts()
        {
            return _productRepository.GetFeaturedProducts();
        }

        public List<XElement> SearchProducts(string searchTerm)
        {
            return _productRepository.SearchProducts(searchTerm);
        }

        public void UpdateProductQuantity(int productId, int newQuantity)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product.Element("SoLuongTon").Value = newQuantity.ToString();
                UpdateProduct(product);
            }
        }
    }
}