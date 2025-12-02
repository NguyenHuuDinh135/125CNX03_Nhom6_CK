using _125CNX03_Nhom6_CK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace _125CNX03_Nhom6_CK.DAL
{
    public interface ISanPhamRepository
    {
        List<SanPham> GetAll();
        SanPham GetById(int id);
        bool Add(SanPham sp);
        bool Update(SanPham sp);
        bool Delete(int id);
    }
}
