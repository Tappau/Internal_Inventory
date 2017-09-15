using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicsInventory.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        IEnumerable<T> SelectAll();
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        void Insert(T entity);
        void InsertAndSubmit(T entity);
        void Update(T entity);
        void UpdateAndSubmit(T entity);
        void Delete(int id);
        void Save();
    }
}