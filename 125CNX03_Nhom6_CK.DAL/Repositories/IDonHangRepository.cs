using _125CNX03_Nhom6_CK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _125CNX03_Nhom6_CK.DAL
{
    public interface IDonHangRepository
    {
        List<DonHang> GetAll();
        DonHang GetById(int id);
        bool Add(DonHang entity);
        bool Update(DonHang entity);
        bool Delete(int id);
    }
}
