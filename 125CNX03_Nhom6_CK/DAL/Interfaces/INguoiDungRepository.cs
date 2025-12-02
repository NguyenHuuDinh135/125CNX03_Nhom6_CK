using _125CNX03_Nhom6_CK.DAL.Repositories;
using System.Collections.Generic;
using System.Xml.Linq;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public interface INguoiDungRepository
    {
        List<XElement> GetAll();
        XElement GetById(int id);
        void Add(XElement entity);
        void Update(XElement entity);
        void Delete(int id);
        void Save();
        XElement GetByEmail(string email);
        XElement GetByEmailAndPassword(string email, string passwordHash);
        List<XElement> GetByRole(string role);
        List<XElement> GetActiveUsers();
    }
}