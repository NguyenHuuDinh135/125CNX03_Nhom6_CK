using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public interface IGioHangRepository
    {
        List<XElement> GetAll();
        XElement GetById(int id);
        void Add(XElement entity);
        void Update(XElement entity);
        void Delete(int id);
        void Save();
        XElement GetByUserId(int userId);
    }
}