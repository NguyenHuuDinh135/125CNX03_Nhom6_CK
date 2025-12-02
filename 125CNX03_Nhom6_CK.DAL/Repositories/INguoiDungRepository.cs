using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface INguoiDungRepository
    {
        List<NguoiDung> GetAll();
        NguoiDung GetById(int id);
        NguoiDung GetByEmail(string email);
        bool Add(NguoiDung entity);
        bool Update(NguoiDung entity);
        bool Delete(int id);
    }
}
