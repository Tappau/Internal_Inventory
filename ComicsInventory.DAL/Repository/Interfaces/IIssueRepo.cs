using System.Collections.Generic;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;

namespace ComicsInventory.DAL.Repository.Interfaces
{
    public interface IIssueRepo
    {
        void SetBoxIdToNull(int boxId);
        bool ChkIssueExists(string issueNumber, int seriesId);
        IEnumerable<Issue> GetBySeriesId(int seriesId);
        IEnumerable<Issue> GetByBoxId(int boxId);
        int GetId(string issueNumber, int seriesId);

        /// <summary>
        ///     Gets the GCD data issue identifier for GCD.
        /// </summary>
        /// <param name="issueId">The issue identifier.</param>
        /// <returns></returns>
        int GetGcdIdNumber(int issueId);

        IEnumerable<LatestAdditionsDto> GetLastestAdditions();
        IEnumerable<IssuesWithoutBoxesDto> GetNonBoxedIssues();
        string ConnectString { get; }
        IEnumerable<Issue> SelectAll();
        Issue GetById(int id);
        void Insert(Issue entity);
        void InsertAndSubmit(Issue entity);
        void Update(Issue entity);
        void UpdateAndSubmit(Issue entity);
        void Delete(int id);
        void Save();
    }
}