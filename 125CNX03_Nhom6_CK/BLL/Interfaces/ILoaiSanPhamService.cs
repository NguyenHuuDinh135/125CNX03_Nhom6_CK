using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface ILoaiSanPhamService
    {
        List<XElement> GetAllCategories();
        XElement GetCategoryById(int id);
        void AddCategory(XElement category);
        void UpdateCategory(XElement category);
        void DeleteCategory(int id);
        List<XElement> GetActiveCategories();
    }
}