using System.Collections.Generic;
using ComicsInventory.DAL.Entities;

namespace ComicsInventory.DAL.Repository.Interfaces
{
    public interface ISeriesRepo
    {
        IEnumerable<Series> GetByPublisherId(int publisherId);
        bool ChkSeriesExists(string seriesName, int beginYear);
        int GetId(string seriesName, int publisherId, int seriesBeginYear);
        string ConnectString { get; }
        IEnumerable<Series> SelectAll();
        Series GetById(int id);
        void Insert(Series entity);
        void InsertAndSubmit(Series entity);
        void Update(Series entity);
        void UpdateAndSubmit(Series entity);
        void Delete(int id);
        void Save();
    }
}