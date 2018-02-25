using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Interfaces;
using Dapper;

namespace ComicsInventory.DAL.Repository.Inventory
{
    public class BoxRepo : BaseRepository<BoxStore>, IBoxRepo
    {
        private readonly InventoryContext _db;
        private IDbConnection _dapper;

        public BoxRepo()
        {
            _dapper = new SqlConnection(ConnectString);
            _db = new InventoryContext();
        }

        public BoxRepo(InventoryContext dbContext)
        {
            _db = dbContext;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Retires the specified box identifier.
        /// </summary>
        /// <param name="boxId">The box identifier.</param>
        public void Retire(int boxId)
        {
            var ext = GetById(boxId);
            ext.isActive = false;
            Save();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets highest BoxID and adds one to work out what the next box number will be
        ///     This is purely for displaying of information during creation of a new box
        /// </summary>
        /// <returns></returns>
        public int NextBoxId()
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                var rslt = _dapper.ExecuteScalar<int>("SELECT MAX(BoxID) FROM BoxStore;");

                return rslt + 1;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Retrives Select group of data related to given Box ID
        /// </summary>
        /// <param name="boxId">The box identifier.</param>
        /// <returns></returns>
        public IEnumerable<BoxContentDto> GetBoxContents(int boxId)
        {
            //Used in GetBoxContents Method of BoxController
            //Move to Box service
            using (_dapper = new SqlConnection(ConnectString))
            {
                const string sql = "SELECT i.Issue_ID AS IssueId," +
                                   "p.Pub_Name AS Publisher," +
                                   "s.Series_Name AS SeriesName," +
                                   "i.Number AS IssueNumber," +
                                   "issuecon.quantity AS Qty," +
                                   "con.Name AS Condition," +
                                   "issueCon.Grade_ID AS GradeId " +
                                   "FROM Issue i " +
                                   "INNER JOIN Series s ON i.Series_ID = s.Series_ID " +
                                   "INNER JOIN Publisher p ON s.Publisher_ID = p.Publisher_ID " +
                                   "INNER JOIN IssueCondition issueCon ON i.Issue_ID = issueCon.Issue_ID " +
                                   "INNER JOIN Grade con ON issueCon.Grade_ID = con.GradeID " +
                                   "INNER JOIN BoxStore b ON i.Box_ID = b.BoxID " +
                                   "WHERE b.BoxID = @id " +
                                   "ORDER BY i.Issue_ID ASC";

                var data = _dapper.Query<BoxContentDto>(sql, new {id = boxId});
                return data;
            }
        }
    }
}