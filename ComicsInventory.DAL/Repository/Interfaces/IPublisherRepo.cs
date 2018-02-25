using System.Collections.Generic;
using ComicsInventory.DAL.Entities;

namespace ComicsInventory.DAL.Repository.Interfaces
{
    public interface IPublisherRepo
    {
        bool ChkPublisherExists(string publisherName);
        int GetId(string publisherName);
        string ConnectString { get; }
        IEnumerable<Publisher> SelectAll();
        Publisher GetById(int id);
        void Insert(Publisher entity);
        void InsertAndSubmit(Publisher entity);
        void Update(Publisher entity);
        void UpdateAndSubmit(Publisher entity);
        void Delete(int id);
        void Save();
    }
}