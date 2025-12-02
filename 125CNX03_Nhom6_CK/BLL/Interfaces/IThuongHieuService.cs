using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IThuongHieuService
    {
        List<XElement> GetAllBrands();
        XElement GetBrandById(int id);
        void AddBrand(XElement brand);
        void UpdateBrand(XElement brand);
        void DeleteBrand(int id);
        List<XElement> GetActiveBrands();
    }
}