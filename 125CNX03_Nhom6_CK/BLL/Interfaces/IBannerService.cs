using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public interface IBannerService
    {
        List<XElement> GetAllBanners();
        XElement GetBannerById(int id);
        void AddBanner(XElement banner);
        void UpdateBanner(XElement banner);
        void DeleteBanner(int id);
        List<XElement> GetActiveBanners();
        List<XElement> GetBannersByOrder();
        int GenerateNewId();

    }
}