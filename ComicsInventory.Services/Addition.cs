using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicsInventory.DAL;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Entities;
using ComicsInventory.DAL.Repository.Interfaces;

namespace ComicsInventory.Services
{
    //Rewriting of the StockManageLogic class
    internal class Addition
    {
        private readonly IPublisherRepo _pubRepo;
        private readonly ISeriesRepo _seriesRepo;
        private readonly IIssueRepo _issueRepo;
        private readonly IIssueConditionRepo _conditionRepo;

        public Addition()
        {
            
        }

        public Addition(IPublisherRepo pRepo, ISeriesRepo sRepo, IIssueRepo iRepo, IIssueConditionRepo cRepo)
        {
            _pubRepo = pRepo;
            _seriesRepo = sRepo;
            _issueRepo = iRepo;
            _conditionRepo = cRepo;
        }


        public bool NewStock(int? publisherId, int?seriesId, int?issueId, int? quantity, int?gradeId )
        {
            // ReSharper disable once InconsistentNaming
            int QUANTITY_TO_ADD= Validator(quantity, gradeId).Item1;
            // ReSharper disable once InconsistentNaming
            int GRADE_TO_ADD = Validator(quantity, gradeId).Item2;
            int pubExistingId = 0, seriesExistingId = 0, issueExistingId = 0, conditionExistId=0;
            
            List<GcdPublisherDto> gcdPublisherDtos = null;
            List<GcdSeriesDto> gcdSeriesDtos = null;
            List<GcdIssueDto> gcdIssueDtos=null;

            if (publisherId !=null)
            {
                gcdPublisherDtos = GetGcdData<GcdPublisherDto>(publisherId.Value);
                if (gcdPublisherDtos != null && _pubRepo.ChkPublisherExists(gcdPublisherDtos.FirstOrDefault()?.Publisher))
                {
                    // if Publisher already exists, retrive the ID #
                    pubExistingId = _pubRepo.GetId(gcdPublisherDtos.First().Publisher);
                }
            }
            if (seriesId !=null)
            {
                gcdSeriesDtos = GetGcdData<GcdSeriesDto>(seriesId.Value);
                // if Series List<> is not empty and series exists.
                bool chkSeriesExists = gcdSeriesDtos != null &&
                                       _seriesRepo.ChkSeriesExists(gcdSeriesDtos.First().SeriesName,
                                           gcdSeriesDtos.First().SeriesBeginYear);
                if (chkSeriesExists)
                {
                    seriesExistingId = _seriesRepo.GetId(gcdSeriesDtos.First().SeriesName, pubExistingId,
                        gcdSeriesDtos.First().SeriesBeginYear);
                }
            }
            if (issueId != null)
            {
                gcdIssueDtos = GetGcdData<GcdIssueDto>(issueId.Value);
                if (gcdIssueDtos != null && _issueRepo.ChkIssueExists(gcdIssueDtos.First().IssueNumber, seriesExistingId))
                {
                    issueExistingId = _issueRepo.GetId(gcdIssueDtos.First().IssueNumber, seriesExistingId);
                }
            }
            if (issueExistingId !=0&& _conditionRepo.ChkIssueConditionExists(issueExistingId, GRADE_TO_ADD))
            {
                conditionExistId = _conditionRepo.GetId(issueExistingId, GRADE_TO_ADD);
            }

            //INSERT the Items to INVENTORY DB
            //if existing ID == 0 IT DOES NOT EXIST!!!!!
            //any number other than 0 DOES EXIST

            if (pubExistingId == 0)
            {
                //if publisher not exist then add new publisher, Series, Issue & IssueCondition
                try
                {
                    var newPub = new Publisher
                    {
                        Pub_Name = gcdPublisherDtos.First().Publisher,
                        Year_Began = gcdPublisherDtos.First().PublisherStartYear,
                        URL = gcdPublisherDtos.First().PubUrl
                    };
                    var srs = new Series
                    {
                        Series_Name = gcdSeriesDtos.First().SeriesName,
                        Year_Began = gcdSeriesDtos.First().SeriesBeginYear,
                        Year_End = gcdSeriesDtos.First().YearEnd,
                        dimensions = gcdSeriesDtos.First().Dimensions,
                        paperStock = gcdSeriesDtos.First().PaperStock,
                        Publisher_ID = newPub.Publisher_ID
                    };


                    var nwIssue = new Issue
                    {
                        Series_ID = srs.Series_ID,
                        Number = gcdIssueDtos.First().IssueNumber,
                        publication_date = gcdIssueDtos.First().PublishDate,
                        page_count = Convert.ToDecimal(gcdIssueDtos.First().PageCount),
                        frequency = gcdIssueDtos.First().Frequency,
                        editor = gcdIssueDtos.First().Editor,
                        ISBN = gcdIssueDtos.First().Isbn,
                        barcode = gcdIssueDtos.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };

                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = nwIssue.Issue_ID,
                        quantity = QUANTITY_TO_ADD
                    };

                    _pubRepo.Insert(newPub);
                    _seriesRepo.Insert(srs);
                   _issueRepo.Insert(nwIssue);
                   _conditionRepo.Insert(condition); 
                    _pubRepo.Save();
                    
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Inner Exception: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);

                    throw;
                }
            }
            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId == 0)
            {
                //publisher exists & series exists THEN ADD 
                // New ISSUE & IssueCondition

                try
                {
                    var newIssue = new Issue
                    {
                        Series_ID = seriesExistingId,
                        Number = gcdIssueDtos.First().IssueNumber,
                        publication_date = gcdIssueDtos.First().PublishDate,
                        page_count = Convert.ToDecimal(gcdIssueDtos.First().PageCount),
                        frequency = gcdIssueDtos.First().Frequency,
                        editor = gcdIssueDtos.First().Editor,
                        ISBN = gcdIssueDtos.First().Isbn,
                        barcode = gcdIssueDtos.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };
                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = newIssue.Issue_ID,
                        quantity = QUANTITY_TO_ADD
                    };

                    _issueRepo.Insert(newIssue);
                   _conditionRepo.Insert(condition);
                    _conditionRepo.Save();

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Message: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);
                    throw;
                }
            }

            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId != 0 && conditionExistId == 0)
            {
                //IF publisher, Series  & Issue Exists
                //ADD New version of Issue to IssueCondition Tbl
                try
                {
                    var existingIssue = new IssueCondition
                    {
                        Issue_ID = issueExistingId,
                        Grade_ID = gradeId,
                        quantity = QUANTITY_TO_ADD
                    };

                    _conditionRepo.InsertAndSubmit(existingIssue);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error Status: " + e.Data);
                    Debug.WriteLine("Error Message: " + e.InnerException);
                    Debug.WriteLine("Error Source: " + e.Source);
                    throw;
                }
            }

            if (pubExistingId != 0 && seriesExistingId != 0 && issueExistingId != 0 && conditionExistId != 0)
            {
                //IF Publisher, Series, Issue & IssueCondition Exists
                //THEN UPDATE the quantity on record.
                try
                {
                    var editIssueCondition = _conditionRepo.GetById(conditionExistId);

                        editIssueCondition.quantity = QUANTITY_TO_ADD;
                        _conditionRepo.Save();
                    
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error Status: " + e.Data);
                    Debug.WriteLine("Error Inner Exception: " + e.InnerException);
                    Debug.WriteLine("Error Source: " + e.Source);
                    throw;
                }
            }

            if (pubExistingId != 0 && seriesExistingId == 0)
            {
                //IF PUBLISHER EXISTS && Series NOT Exist
                //ADD NEW Series & Issue & Condition
                try
                {
                    var newSeries = new Series
                    {
                        Series_Name = gcdSeriesDtos.First().SeriesName,
                        Publisher_ID = pubExistingId,
                        Year_Began = gcdSeriesDtos.First().SeriesBeginYear,
                        Year_End = gcdSeriesDtos.First().YearEnd,
                        paperStock = gcdSeriesDtos.First().PaperStock,
                        dimensions = gcdSeriesDtos.First().Dimensions
                    };

                    var iss = new Issue
                    {
                        Series_ID = newSeries.Series_ID,
                        Number = gcdIssueDtos.First().IssueNumber,
                        publication_date = gcdIssueDtos.First().PublishDate,
                        page_count = Convert.ToDecimal(gcdIssueDtos.First().PageCount),
                        frequency = gcdIssueDtos.First().Frequency,
                        editor = gcdIssueDtos.First().Editor,
                        ISBN = gcdIssueDtos.First().Isbn,
                        barcode = gcdIssueDtos.First().Barcode,
                        IsActive = true,
                        AddedOn = DateTime.Now,
                        GCDIssueNumber = issueId
                    };

                    var condition = new IssueCondition
                    {
                        Grade_ID = gradeId,
                        Issue_ID = iss.Issue_ID,
                        quantity = QUANTITY_TO_ADD
                    };

                    _seriesRepo.Insert(newSeries);
                    _issueRepo.Insert(iss);
                    _conditionRepo.Insert(condition);
                    _issueRepo.Save();
                  

                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error Status: " + ex.Data);
                    Debug.WriteLine("Error Inner Exception: " + ex.InnerException);
                    Debug.WriteLine("Error Source: " + ex.Source);

                    throw;
                }
            }

            return true;
        }

       
        
        

        private List<T> GetGcdData<T>(int idToGet)
        {
            var repo = new ComicDataRepo();
            
            List<T> output = new List<T>();
            if (typeof(T) == typeof(GcdPublisherDto))
            {
                output.Add((T) repo.GetPublisherDetails(idToGet));
            }
            if (typeof(T) == typeof(GcdSeriesDto))
            {
                output.Add((T) repo.GetSeriesDetails(idToGet));
            }
            if (typeof(T) == typeof(GcdIssueDto))
            {
                output.Add((T) repo.GetIssueDetails(idToGet));
            }
            return output;

        }  
        
        /// <summary>
        /// Validates the specified to quantity. Item1 = Quantity
        /// Item2 = Grade/Condition
        /// </summary>
        /// <param name="toQuantity">To quantity.</param>
        /// <param name="toCondition">To condition.</param>
        /// <returns></returns>
        private static Tuple<int, int> Validator(int? toQuantity, int? toCondition)
        {
            int q, c;

            if (toQuantity == null)
            {
                q = 1;
            }
            else
            {
                q = toQuantity.Value;
            }
            if (toCondition != null)
            {
                c = toCondition.Value;
            }
            else
            {
                c = 1;
            }
            return new Tuple<int, int>(q, c);
        }
    }
}
