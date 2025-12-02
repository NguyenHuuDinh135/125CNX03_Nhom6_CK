using System.Collections.Generic;
using _125CNX03_Nhom6_CK.DTO;

namespace _125CNX03_Nhom6_CK.DAL.Repositories.Interfaces
{
    public interface IChiTietGioHangRepository
    {
        List<ChiTietGioHang> GetAll();
        ChiTietGioHang GetById(int id);
        List<ChiTietGioHang> GetByGioHang(int maGioHang);
        bool Add(ChiTietGioHang entity);
        bool Update(ChiTietGioHang entity);
        bool Delete(int id);
        bool DeleteByGioHang(int maGioHang);
    }
}
