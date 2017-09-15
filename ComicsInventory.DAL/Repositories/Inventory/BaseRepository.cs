using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repositories.Interfaces;

namespace ComicsInventory.DAL.Repositories.Inventory
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly InventoryContext _db;
        private readonly DbSet<T> _tbl;

        protected BaseRepository()
        {
            _db = new InventoryContext();
            _tbl = _db.Set<T>();
            ConnectString = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString;
        }

        public string ConnectString { get; private set; }


        public IEnumerable<T> SelectAll()
        {
            var x = _tbl.ToList();
            return x;
        }

        public T GetById(int id)
        {
            return _tbl.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _tbl.FindAsync(id);
        }
        

        public void Insert(T entity)
        {
            _tbl.Add(entity);
        }

        public void InsertAndSubmit(T entity)
        {
            _tbl.Add(entity);
            SaveChanges();
        }

        public void Update(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateAndSubmit(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete(int id)
        {
            var existing = _tbl.Find(id);
            if (existing != null) _tbl.Remove(existing);
        }

        public void Save()
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}