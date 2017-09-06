using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Repositories.Interfaces;
using ComicsInventory.DAL.Repositories.Inventory;
using ComicsInventory.Services.BLLInterfaces;

namespace ComicsInventory.Services
{
    public class IssueService : IIssueService
    {
        private readonly IIssueRepo _issueRepo;

        public IssueService(IIssueRepo issue)
        {
            _issueRepo = issue;
        }

        /// <summary>
        ///     Gets the newest additions requested, Default returned is 3
        /// </summary>
        /// <param name="numberToReturn">The number to return.</param>
        /// <returns></returns>
        public IEnumerable<LatestAdditionsDto> GetNewestAdditions(int? numberToReturn)
        {
            if (numberToReturn != null)
            {
                return _issueRepo.GetLastestAdditions().Take(numberToReturn.Value);
            }
            return _issueRepo.GetLastestAdditions().Take(3);
        }

        /// <summary>
        ///     Remove the issues in array from the box
        /// </summary>
        /// <param name="issueIdArray">The issue identifier array.</param>
        /// <returns>bool true or false, false if anything goes wrong</returns>
        public void RemoveIssueFromBox(int?[] issueIdArray)
        {
            if (IsArrayEmpty(issueIdArray))
            {
                throw new ArgumentOutOfRangeException(nameof(issueIdArray), "Issue Array is empty");
            }
            //array of issue ids in box are sent, to remove them from box set the issues box id to null
            foreach (var i in issueIdArray)
            {
                try
                {
                    var x = _issueRepo.GetById(Convert.ToInt32(i));
                    x.Box_ID = null;
                    _issueRepo.UpdateAndSubmit(x);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                    throw;
                }
            }
        }

        /// <summary>
        ///     Adds the issues in array to box specified.
        /// </summary>
        /// <param name="issueIdArray">The issue identifier array.</param>
        /// <param name="boxId">The box identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">issueIdArray - Issue Array is empty</exception>
        /// <exception cref="System.ArgumentException">Invalid Box ID received on call</exception>
        public bool AddIssuesToBox(int?[] issueIdArray, int boxId)
        {
            if (IsArrayEmpty(issueIdArray))
            {
                throw new ArgumentOutOfRangeException(nameof(issueIdArray), "Issue Array is empty");
            }

            if (string.IsNullOrWhiteSpace(boxId.ToString()))
            {
                throw new ArgumentException("Invalid Box ID received on execution", nameof(boxId));
            }

            foreach (var i in issueIdArray)
            {
                try
                {
                    var x = _issueRepo.GetById(Convert.ToInt32(i));
                    x.Box_ID = boxId;
                    _issueRepo.UpdateAndSubmit(x);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e);
                    throw;
                }
            }
            return true;
        }

        public bool UpdateQuantityOfIssuesInBox(int[] issueIds, int[] gradeIds, int quantityToSet)
        {
            //Combine the two arrays into a single array with each item being a object of both issue & grade id within
            var combined = issueIds.Zip(gradeIds, (a, b) => new {issue = a, grade = b})
                .ToArray();

            foreach (var i in combined)
            {
                try
                {
                    var conditionRepo = new IssueConditionRepo();

                    var icRecord = conditionRepo.GetByIssueAndCondition(i.issue, Convert.ToInt32(i.grade));
                    icRecord.quantity = quantityToSet;
                    conditionRepo.UpdateAndSubmit(icRecord);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e);
                    throw;
                }
            }
            return true;
        }


        private bool IsArrayEmpty(int?[] arrayToCheck)
        {
            if (arrayToCheck == null || arrayToCheck.Length == 0)
            {
                Console.WriteLine("Array is empty, cannot utilise");
                return false;
            }

            return true;
        }
    }
}