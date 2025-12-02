using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.BLL
{
    public class ThuongHieuService : IThuongHieuService
    {
        private readonly IThuongHieuRepository _brandRepository;

        public ThuongHieuService()
        {
            _brandRepository = new ThuongHieuRepository();
        }

        public List<XElement> GetAllBrands()
        {
            return _brandRepository.GetAll();
        }

        public XElement GetBrandById(int id)
        {
            return _brandRepository.GetById(id);
        }

        public void AddBrand(XElement brand)
        {
            _brandRepository.Add(brand);
        }

        public void UpdateBrand(XElement brand)
        {
            _brandRepository.Update(brand);
        }

        public void DeleteBrand(int id)
        {
            _brandRepository.Delete(id);
        }

        public List<XElement> GetActiveBrands()
        {
            return _brandRepository.GetAll();
        }
    }
}