using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface IBannerRepository
    {
        List<Banner> GetAll();
        Banner GetById(int id);
        bool Add(Banner entity);
        bool Update(Banner entity);
        bool Delete(int id);
    }
}
