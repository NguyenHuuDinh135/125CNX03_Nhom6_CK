using System.Collections.Generic;

namespace _125CNX03_Nhom6_CK.DAL.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();
    }
}