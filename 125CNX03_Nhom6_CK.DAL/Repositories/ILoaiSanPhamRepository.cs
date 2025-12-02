using _125CNX03_Nhom6_CK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _125CNX03_Nhom6_CK.DAL
{
    public interface ILoaiSanPhamRepository
    {
        List<LoaiSanPham> GetAll();
        LoaiSanPham GetById(int id);
        bool Add(LoaiSanPham entity);
        bool Update(LoaiSanPham entity);
        bool Delete(int id);
    }
}
