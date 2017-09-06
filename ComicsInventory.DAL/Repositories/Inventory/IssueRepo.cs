using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repositories.Interfaces;
using Dapper;

namespace ComicsInventory.DAL.Repositories.Inventory
{
    public class IssueRepo : BaseRepository<Issue>, IIssueRepo
    {
        private readonly string _connect = ConfigurationManager.ConnectionStrings["Inventory"].ConnectionString;
        private readonly InventoryContext _db;
        private IDbConnection _dapper;

        public IssueRepo()
        {
            _db = new InventoryContext();
        }

        public IssueRepo(InventoryContext context)
        {
            _db = context;
        }

        public void SetBoxIdToNull(int boxId)
        {
            var qry = SelectAll().Where(i => i.Box_ID == boxId);
            foreach (var box in qry)
            {
                box.Box_ID = null;
            }
            Save();
        }

        public bool ChkIssueExists(string issueNumber, int seriesId)
        {
            using (
                _dapper = new SqlConnection(_connect))
            {
                const string chk =
                    "SELECT COUNT(*) FROM Issue i WHERE i.Number = @issueNumber AND i.Series_ID = @seriesId";
                var exists = _dapper.ExecuteScalar<bool>(chk, new {issueNumber, seriesId});
                return exists;
            }
        }

        public IEnumerable<Issue> GetBySeriesId(int seriesId)
        {
            return _db.Issues.Where(x => x.Series_ID == seriesId).ToList();
        }

        public IEnumerable<Issue> GetByBoxId(int boxId)
        {
            return _db.Issues.Where(x => x.Box_ID == boxId).ToList();
        }

        public int GetId(string issueNumber, int seriesId)
        {
            if (string.IsNullOrWhiteSpace(issueNumber))
            {
                throw new ArgumentException("Invalid String, empty, null or Whitespace", nameof(issueNumber));
            }
            using (_dapper = new SqlConnection(_connect))
            {
                try
                {
                    const string sql =
                        "SELECT i.Issue_ID FROM Issue i WHERE i.Number = @issueNumber AND i.Series_ID = @seriesId";
                    var issueId = _dapper.ExecuteScalar<int>(sql, new {issueNumber, seriesId});
                    return issueId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error:" + ex);
                    throw;
                }
            }
        }

        /// <summary>
        ///     Gets the GCD data issue identifier for GCD.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns></returns>
        public int GetGcdIdNumber(int issueId)
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT i.GCDIssueNumber FROM Issue i " +
                                       "WHERE i.Issue_ID = @issueId";
                    var gcdId = _dapper.ExecuteScalar<int>(sql, new {issueId});
                    return gcdId;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
        }


        public IEnumerable<LatestAdditionsDto> GetLastestAdditions()
        {
            using (_dapper = new SqlConnection(_connect))
            {
                const string qry = "SELECT " +
                                   "i.Issue_ID AS IssueId, " +
                                   "i.Box_ID AS BoxId," +
                                   "p.Pub_Name AS Publisher," +
                                   "s.Series_Name AS SeriesName," +
                                   "i.Number AS IssueNumber," +
                                   "i.publication_date AS PublishedDate," +
                                   "i.page_count AS PageCount," +
                                   "i.frequency AS Frequency," +
                                   "i.AddedOn AS AddedOn, " +
                                   "i.GCDIssueNumber AS GcdId " +
                                   "FROM Issue i " +
                                   "INNER JOIN Series s ON i.Series_ID = s.Series_ID " +
                                   "INNER JOIN Publisher p ON s.Publisher_ID = p.Publisher_ID " +
                                   "ORDER BY i.AddedOn DESC";

                var result = _dapper.Query<LatestAdditionsDto>(qry);
                return result;
            }
        }

        public IEnumerable<IssuesWithoutBoxesDto> GetNonBoxedIssues()
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT i.Issue_ID AS IssueId," +
                                       "p.Pub_Name AS Publisher," +
                                       "s.Series_Name AS SeriesName," +
                                       "i.Number AS IssueNumber," +
                                       "i.barcode AS Barcode " +
                                       "" +
                                       "FROM Issue i " +
                                       "INNER JOIN Series s ON i.Series_ID = s.Series_ID " +
                                       "INNER JOIN Publisher p ON s.Publisher_ID = p.Publisher_ID " +
                                       "WHERE i.Box_ID IS NULL " +
                                       "ORDER BY i.Issue_ID ASC";

                    return _dapper.Query<IssuesWithoutBoxesDto>(sql);
                }
                catch (Exception e)
                {
                    Console.Write("Error: " + e);
                    throw;
                }
            }
        }
    }
}