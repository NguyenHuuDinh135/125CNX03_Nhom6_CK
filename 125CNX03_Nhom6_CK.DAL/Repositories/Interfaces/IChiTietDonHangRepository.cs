using _125CNX03_Nhom6_CK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _125CNX03_Nhom6_CK.DAL
{
    public interface IChiTietDonHangRepository
    {
        List<ChiTietDonHang> GetAll();
        ChiTietDonHang GetById(int id);
        bool Add(ChiTietDonHang entity);
        bool Update(ChiTietDonHang entity);
        bool Delete(int id);
    }
}
