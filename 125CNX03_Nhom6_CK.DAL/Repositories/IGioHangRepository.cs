using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface IGioHangRepository
    {
        List<GioHang> GetAll();
        GioHang GetById(int id);
        GioHang GetByNguoiDung(int maNguoiDung);
        bool Add(GioHang entity);
        bool Update(GioHang entity);
        bool Delete(int id);
    }
}
