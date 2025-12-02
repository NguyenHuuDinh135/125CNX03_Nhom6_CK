using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public interface IDonHangRepository
    {
        List<XElement> GetAll();
        XElement GetById(int id);
        void Add(XElement entity);
        void Update(XElement entity);
        void Delete(int id);
        void Save();
        List<XElement> GetByUserId(int userId);
        List<XElement> GetByStatus(int status);
        List<XElement> GetRecentOrders(int count);
    }
}