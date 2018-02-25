using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Interfaces;
using System.Linq;
using Dapper;

namespace ComicsInventory.DAL.Repository.Inventory
{
    public class IssueConditionRepo : BaseRepository<IssueCondition>, IIssueConditionRepo
    {
        private readonly InventoryContext _db;
        private IDbConnection _dapper;

        public IssueConditionRepo()
        {
            _db = new InventoryContext();
        }

        public IssueConditionRepo(InventoryContext context)
        {
            _db = context;
        }


        public IEnumerable<IssueCondition> GetByIssueId(int issueId)
        {
            return _db.IssueConditions.Where(x => x.Issue_ID == issueId).ToList();
        }

        public IssueCondition GetByIssueAndCondition(int issueid, int gradeid)
        {
            var x = _db.IssueConditions.FirstOrDefault(m => m.Issue_ID == issueid && m.Grade_ID == gradeid);
            return x;
        }

        public bool ChkIssueConditionExists(int issueId, int gradeId)
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string chk = "SELECT COUNT(*) FROM IssueCondition ic " +
                                       "WHERE ic.Issue_ID = @issueId AND ic.Grade_ID = @gradeId";
                    var exist = _dapper.ExecuteScalar<bool>(chk, new {issueId, gradeId});
                    return exist;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                    throw;
                }
            }
        }

        public int GetId(int issueId, int gradeId)
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                try
                {
                    const string sql = "SELECT ic.IssueCondition_ID FROM IssueCondition ic " +
                                       "WHERE ic.Issue_ID = @issueId AND ic.Grade_ID = @gradeId";
                    var issueConditionId = _dapper.ExecuteScalar<int>(sql, new {issueId, gradeId});
                    return issueConditionId;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
        }

        public bool RemoveIssueCondition(int issueId)
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                const string sql = "DELETE ic FROM IssueCondition ic " +
                                   "WHERE ic.Issue_ID = @issueId";
                var affectedRows = _dapper.Execute(sql, new {issueId});
                if (affectedRows <= 0) return false;
                return true;
            }
        }


        public IEnumerable<IssueConDto> GetIssueConditions(int issueId)
        {
            using (_dapper = new SqlConnection(ConnectString))
            {
                const string sql = "SELECT ic.IssueCondition_ID AS Id," +
                                   "ic.Issue_ID AS IssueId," +
                                   "ic.Grade_ID AS GradeId," +
                                   "ic.quantity AS Quantity," +
                                   "g.Name AS GradeName " +
                                   "FROM IssueCondition ic " +
                                   "INNER JOIN Grade g ON ic.Grade_ID = g.GradeID " +
                                   "WHERE ic.Issue_ID = @issueId " +
                                   "ORDER BY ic.Grade_ID";
                var data = _dapper.Query<IssueConDto>(sql, new {issueId});
                return data;
            }
        }
    }
}