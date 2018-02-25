using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ComicsInventory.DAL;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Inventory;

// ReSharper disable AssignNullToNotNullAttribute

namespace ComicsInventory.Services
{/// <summary>
/// Original from project, runs and does as expected
/// 
/// DONT use as it is not in the new services system
/// </summary>
    public class StockManageLogic
    {
        private readonly ComicDataRepo _gcdRepo = new ComicDataRepo();
        private readonly PublisherRepo _publisherRepo = new PublisherRepo();
        private readonly IssueRepo _issueRepo = new IssueRepo();
        private readonly SeriesRepo _seriesRepo = new SeriesRepo();
        private readonly IssueConditionRepo _issueConditionRepo = new IssueConditionRepo();
        

        private readonly InventoryContext _db = new InventoryContext();


        public bool AddStock(int? seriesId, int? publisherId, int? issueId, int quantity, int gradeId)
        {
            List<GcdPublisherDto> publisherDetails = null;
            List<GcdSeriesDto> seriesDetails = null;
            List<GcdIssueDto> issueDetails = null;

            bool success;

            if (quantity.Equals(0))
            {
                quantity = 1;
            }
            if (gradeId.Equals(0))
            {
                gradeId = 1;
            }
            //Get all item details from GCD repo using the passed ID numbers from Search
            if (publisherId != null)
            {
                publisherDetails = _gcdRepo.GetPublisherDetails((int) publisherId).ToList();
            }

            if (seriesId != null)
            {
                seriesDetails = _gcdRepo.GetSeriesDetails((int) seriesId).ToList();
            }
            if (issueId != null)
            {
                issueDetails = _gcdRepo.GetIssueDetails((int) issueId).ToList();
            }
            //Check that the related Series, Publisher And/Or Issue does not exist.

            //Check StockDB M$ SQL Server for preexisitng Records
            

            var chkPubExists = publisherDetails != null &&
                               _publisherRepo.ChkPublisherExists(publisherDetails.FirstOrDefault().Publisher);
            var pubExistingId = 0;
            if (chkPubExists)
            {
                //publisher already exists get the ID of that publisher from Stock DB

                pubExistingId = _publisherRepo.GetId(publisherDetails.First().Publisher);
            }

            var chkSeriesExists = seriesDetails != null &&
                                  _seriesRepo.ChkSeriesExists(seriesDetails.First().SeriesName,
                                      seriesDetails.First().SeriesBeginYear);
            var seriesExistingId = 0;
            if (chkSeriesExists)
            {
                //if Series already exists in Stock DB get the Stock DB id number
                seriesExistingId = _seriesRepo.GetId(seriesDetails.First().SeriesName, pubExistingId,seriesDetails.First().SeriesBeginYear);
            }


            //If issueExists then get the ID from Stock DB
            var chkIssueExists = issueDetails != null &&
                                 _issueRepo.ChkIssueExists(issueDetails.First().IssueNumber, seriesExistingId);
            var issueExistingId = 0;
            if (chkIssueExists)
            {
                issueExistingId = _issueRepo.GetId(issueDetails.First().IssueNumber, seriesExistingId);
            }

            //IF issueCondition Exists then get the ID and Store as issueConId.
            var chkIssueConditionExists = issueExistingId != 0 &&
                                          _issueConditionRepo.ChkIssueConditionExists(issueExistingId, gradeId);
            var issueConId = 0;
            if (chkIssueConditionExists)
            {
                issueConId = _issueConditionRepo.GetId(issueExistingId, gradeId);
            }

            //insert the items in to StockDB
            //if a exisiting ID variable == 0 means it Does NOT exist!
            //if an existing ID Variable IS NOT 0 then the record DOES Exist!


            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId == 0)
            {
                //publisher exists & series exists THEN ADD 
                // New ISSUE & IssueCondition

                try
                {
                    var newIssue = new Issue
                    {
                        Series_ID = seriesExistingId,
                        Number = issueDetails.First().IssueNumber,
                        publication_date = issueDetails.First().PublishDate,
                        page_count = Convert.ToDecimal(issueDetails.First().PageCount),
                        frequency = issueDetails.First().Frequency,
                        editor = issueDetails.First().Editor,
                        ISBN = issueDetails.First().Isbn,
                        barcode = issueDetails.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };
                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = newIssue.Issue_ID,
                        quantity = quantity
                    };

                    _db.Issues.Add(newIssue);
                    _db.IssueConditions.Add(condition);
                    _db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Message: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);
                    success = false;
                    return success;
                }
            }


            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId != 0 && issueConId == 0)
            {
                //IF publisher, Series  & Issue Exists
                //ADD New version of Issue to IssueCondition Tbl
                try
                {
                    var existingIssue = new IssueCondition
                    {
                        Issue_ID = issueExistingId,
                        Grade_ID = gradeId,
                        quantity = quantity
                    };

                    _db.IssueConditions.Add(existingIssue);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error Status: " + e.Data);
                    Debug.WriteLine("Error Message: " + e.InnerException);
                    Debug.WriteLine("Error Source: " + e.Source);
                    success = false;
                    return success;
                }
            }

            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId != 0 && issueConId != 0)
            {
                //IF Publisher, Series, Issue & IssueCondition Exists
                //THEN UPDATE the quantity on record.
                var editIssueCondition = _db.IssueConditions.Find(issueConId);
                try
                {
                    if (editIssueCondition != null)
                    {
                        editIssueCondition.quantity = quantity;
                        _db.SaveChanges();
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error Status: " + e.Data);
                    Debug.WriteLine("Error Inner Exception: " + e.InnerException);
                    Debug.WriteLine("Error Source: " + e.Source);
                    success = false;
                    return success;
                }
            }

            if (pubExistingId != 0 && seriesExistingId == 0)
            {
                //IF PUBLISHER EXISTS && Series NOT Exist
                //ADD NEW Series & Issue
                try
                {
                    var newSeries = new Series
                    {
                        Series_Name = seriesDetails.First().SeriesName,
                        Publisher_ID = pubExistingId,
                        Year_Began = seriesDetails.First().SeriesBeginYear,
                        Year_End = seriesDetails.First().YearEnd,
                        paperStock = seriesDetails.First().PaperStock,
                        dimensions = seriesDetails.First().Dimensions
                    };

                    var iss = new Issue
                    {
                        Series_ID = newSeries.Series_ID,
                        Number = issueDetails.First().IssueNumber,
                        publication_date = issueDetails.First().PublishDate,
                        page_count = Convert.ToDecimal(issueDetails.First().PageCount),
                        frequency = issueDetails.First().Frequency,
                        editor = issueDetails.First().Editor,
                        ISBN = issueDetails.First().Isbn,
                        barcode = issueDetails.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };

                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = iss.Issue_ID,
                        quantity = quantity
                    };

                    _db.Series.Add(newSeries);
                    _db.Issues.Add(iss);
                    _db.IssueConditions.Add(condition);
                    _db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Inner Exception: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);
                    success = false;
                    return success;
                }
            }

            if (pubExistingId == 0)
            {
                //if publisher not exist then add new publisher, Series, Issue & IssueCondition
                try
                {
                    var newPub = new Publisher
                    {
                        Pub_Name = publisherDetails.First().Publisher,
                        Year_Began = publisherDetails.First().PublisherStartYear,
                        URL = publisherDetails.First().PubUrl
                    };
                    var srs = new Series
                    {
                        Series_Name = seriesDetails.First().SeriesName,
                        Year_Began = seriesDetails.First().SeriesBeginYear,
                        Year_End = seriesDetails.First().YearEnd,
                        dimensions = seriesDetails.First().Dimensions,
                        paperStock = seriesDetails.First().PaperStock,
                        Publisher_ID = newPub.Publisher_ID
                    };


                    var nwIssue = new Issue
                    {
                        Series_ID = srs.Series_ID,
                        Number = issueDetails.First().IssueNumber,
                        publication_date = issueDetails.First().PublishDate,
                        page_count = Convert.ToDecimal(issueDetails.First().PageCount),
                        frequency = issueDetails.First().Frequency,
                        editor = issueDetails.First().Editor,
                        ISBN = issueDetails.First().Isbn,
                        barcode = issueDetails.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };

                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = nwIssue.Issue_ID,
                        quantity = quantity
                    };

                    _db.Publishers.Add(newPub);
                    _db.Series.Add(srs);
                    _db.Issues.Add(nwIssue);
                    _db.IssueConditions.Add(condition);
                    _db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Inner Exception: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);
                    success = false;
                    return success;
                }
            }

            return true;
        }
        

        
    }
}