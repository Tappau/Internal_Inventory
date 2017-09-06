using System.Collections.Generic;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;

namespace ComicsInventory.DAL.Repositories.Interfaces
{
    public interface IIssueConditionRepo
    {
        IEnumerable<IssueCondition> GetByIssueId(int issueId);
        IssueCondition GetByIssueAndCondition(int issueid, int gradeid);
        bool ChkIssueConditionExists(int issueId, int gradeId);
        int GetId(int issueId, int gradeId);
        bool RemoveIssueCondition(int issueId);
        IEnumerable<IssueConDto> GetIssueConditions(int issueId);
        string ConnectString { get; }
        IEnumerable<IssueCondition> SelectAll();
        IssueCondition GetById(int id);
        void Insert(IssueCondition entity);
        void InsertAndSubmit(IssueCondition entity);
        void Update(IssueCondition entity);
        void UpdateAndSubmit(IssueCondition entity);
        void Delete(int id);
        void Save();
    }
}