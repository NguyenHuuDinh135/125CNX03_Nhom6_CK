using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface ILienHeRepository
    {
        List<LienHe> GetAll();
        LienHe GetById(int id);
        bool Add(LienHe entity);
        bool Update(LienHe entity);
        bool Delete(int id);
    }
}
