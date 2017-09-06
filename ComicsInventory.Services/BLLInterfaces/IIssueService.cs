using System.Collections.Generic;
using ComicsInventory.DAL.DTOs;

namespace ComicsInventory.Services.BLLInterfaces
{
    public interface IIssueService
    {
        /// <summary>
        /// Gets the newest additions requested, Default returned is 3
        /// </summary>
        /// <param name="numberToReturn">The number to return.</param>
        /// <returns></returns>
        IEnumerable<LatestAdditionsDto> GetNewestAdditions(int? numberToReturn);

        /// <summary>
        ///     Remove the issues in array from the box
        /// </summary>
        /// <param name="issueIdArray">The issue identifier array.</param>
        /// <returns>bool true or false, false if anything goes wrong</returns>
        void RemoveIssueFromBox(int?[] issueIdArray);

        /// <summary>
        ///     Adds the issues in array to box specified.
        /// </summary>
        /// <param name="issueIdArray">The issue identifier array.</param>
        /// <param name="boxId">The box identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">issueIdArray - Issue Array is empty</exception>
        /// <exception cref="System.ArgumentException">Invalid Box ID received on call</exception>
        bool AddIssuesToBox(int?[] issueIdArray, int boxId);

        bool UpdateQuantityOfIssuesInBox(int[] issueIds, int[] gradeIds, int quantityToSet);
    }
}