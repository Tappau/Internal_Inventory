using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Interfaces;
using Dapper;

namespace ComicsInventory.DAL.Repository.Inventory
{
    public class SeriesRepo : BaseRepository<Series>, ISeriesRepo
    {
        private readonly InventoryContext _db;
        private IDbConnection _dapper;

        public SeriesRepo()
        {
            _db = new InventoryContext();
            _dapper = new SqlConnection(ConnectString);
        }

        public SeriesRepo(InventoryContext context)
        {
            _db = context;
        }


        public IEnumerable<Series> GetByPublisherId(int publisherId)
        {
            return _db.Series.Where(x => x.Publisher_ID == publisherId).ToList();
        }

        public bool ChkSeriesExists(string seriesName, int beginYear)
        {
            if (string.IsNullOrWhiteSpace(seriesName))
            {
                throw new ArgumentException("Series Name is invalid, empty or white space", nameof(seriesName));
            }
            if (beginYear.ToString().Length != 4)
            {
                throw new ArgumentException("Year integer is not 4 characters long", nameof(beginYear));
            }
            using (
                _dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string chk =
                        "SELECT COUNT(*) FROM Series s WHERE s.Series_Name = @seriesName AND s.Year_Began = @beginYear";
                    var exists = _dapper.ExecuteScalar<bool>(chk, new {seriesName, beginYear});
                    return exists;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
        }

        public int GetId(string seriesName, int publisherId, int seriesBeginYear)
        {
            if (string.IsNullOrWhiteSpace(seriesName))
            {
                throw new ArgumentException("Publisher Name is invalid, empty or white space", nameof(seriesName));
            }

            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT s.Series_ID FROM Series s " + "WHERE s.Series_Name = @seriesName " +
                                       "AND s.Publisher_ID = @publisherId " +
                                       "AND s.Year_Began = @seriesBeginYear";
                    var seriesId = _dapper.ExecuteScalar<int>(sql, new {seriesName, publisherId, seriesBeginYear});
                    return seriesId;
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