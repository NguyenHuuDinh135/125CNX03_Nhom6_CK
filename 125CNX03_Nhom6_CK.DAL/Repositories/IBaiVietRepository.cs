using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface IBaiVietRepository
    {
        List<BaiViet> GetAll();
        BaiViet GetById(int id);
        bool Add(BaiViet entity);
        bool Update(BaiViet entity);
        bool Delete(int id);
    }
}
