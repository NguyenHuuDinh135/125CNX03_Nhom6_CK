using _125CNX03_Nhom6_CK.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _125CNX03_Nhom6_CK.DAL
{
    public interface IThuongHieuRepository
    {
        List<ThuongHieu> GetAll();
        ThuongHieu GetById(int id);
        bool Add(ThuongHieu entity);
        bool Update(ThuongHieu entity);
        bool Delete(int id);
    }
}
