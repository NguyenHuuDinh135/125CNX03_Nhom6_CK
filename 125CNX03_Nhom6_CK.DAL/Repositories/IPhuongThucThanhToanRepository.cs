using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface IPhuongThucThanhToanRepository
    {
        List<PhuongThucThanhToan> GetAll();
        PhuongThucThanhToan GetById(int id);
        List<PhuongThucThanhToan> GetByNguoiDung(int maNguoiDung);
        bool Add(PhuongThucThanhToan entity);
        bool Update(PhuongThucThanhToan entity);
        bool Delete(int id);
    }
}
