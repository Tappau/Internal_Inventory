using System.Collections.Generic;

namespace ComicsInventory.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T GetById(int id);
        void Insert(T entity);
        void InsertAndSubmit(T entity);
        void Update(T entity);
        void UpdateAndSubmit(T entity);
        void Delete(int id);
        void Save();
    }
}