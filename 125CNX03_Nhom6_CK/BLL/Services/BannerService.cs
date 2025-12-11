using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService()
        {
            _bannerRepository = new BannerRepository();
        }

        public List<XElement> GetAllBanners()
        {
            return _bannerRepository.GetAll();
        }

        public XElement GetBannerById(int id)
        {
            return _bannerRepository.GetById(id);
        }

        public void AddBanner(XElement banner)
        {
            _bannerRepository.Add(banner);
        }

        public void UpdateBanner(XElement banner)
        {
            _bannerRepository.Update(banner);
        }

        public void DeleteBanner(int id)
        {
            _bannerRepository.Delete(id);
        }

        public List<XElement> GetActiveBanners()
        {
            return _bannerRepository.GetActiveBanners();
        }

        public List<XElement> GetBannersByOrder()
        {
            return _bannerRepository.GetBannersByOrder();
        }
        public int GenerateNewId()
        {
            var all = _bannerRepository.GetAll();

            if (all == null || all.Count == 0)
                return 1;

            return all.Max(x => (int?)x.Element("Id") ?? 0) + 1;
        }

    }
}