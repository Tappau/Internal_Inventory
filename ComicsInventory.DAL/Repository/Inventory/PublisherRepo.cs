using System;
using System.Data;
using System.Data.SqlClient;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Interfaces;
using Dapper;

namespace ComicsInventory.DAL.Repository.Inventory
{
    public class PublisherRepo : BaseRepository<Publisher>, IPublisherRepo
    {
        private readonly InventoryContext _db;
        private IDbConnection _dapper;

        public PublisherRepo()
        {
            _dapper = new SqlConnection(ConnectString);
            _db = new InventoryContext();
        }

        public PublisherRepo(InventoryContext context, IDbConnection dapper)
        {
            _db = context;
            _dapper = dapper;
        }

        public bool ChkPublisherExists(string publisherName)
        {
            if (string.IsNullOrWhiteSpace(publisherName))
            {
                throw new ArgumentException("Publisher Name is invalid, empty or white space", nameof(publisherName));
            }
            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT COUNT(*) FROM Publisher p WHERE p.Pub_Name = @publisherName";
                    var exists = _dapper.ExecuteScalar<bool>(sql, new {publisherName});

                    return exists;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
        }

        public int GetId(string publisherName)
        {
            if (string.IsNullOrWhiteSpace(publisherName))
            {
                throw new ArgumentException("Publisher Name is invalid, empty or white space", nameof(publisherName));
            }

            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT p.Publisher_ID FROM Publisher p WHERE p.Pub_Name = @publisherName";
                    var pubId = _dapper.ExecuteScalar<int>(sql, new {publisherName});

                    return pubId;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
        }
    }
}