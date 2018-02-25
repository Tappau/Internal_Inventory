using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ComicsInventory.DAL.DTOs;
using ComicsInventory.DAL.Repository.Interfaces;
using ComicsInventory.Services.BLLInterfaces;


namespace ComicsInventory.Services
{
    public class GeneralServices : IGeneralServices
    {
        private readonly IPublisherRepo _publisherRepo;
        private readonly ISeriesRepo _seriesRepo;
        private readonly Addition _addition;

        public GeneralServices(IPublisherRepo pRepo, ISeriesRepo sRepo)
        {

            _publisherRepo = pRepo;
            _seriesRepo = sRepo;
            _addition = new Addition();
            
        }

        public IEnumerable<SelectListItem> PublisherList()
        {
            return _publisherRepo.SelectAll()
                .ToList()
                .OrderBy(x => x.Pub_Name)
                .Select(x => new SelectListItem { Value = x.Publisher_ID.ToString(), Text = x.Pub_Name });
        }

        public IEnumerable<SelectListItem> SeriesList(int publisherId)
        {
            return _seriesRepo.GetByPublisherId(publisherId)
                    .ToList()
                    .OrderBy(s => s.Series_Name)
                    .Select(x => new SelectListItem { Value = x.Series_ID.ToString(), Text = x.Series_Name });
        }

        public List<StockTableDto> SortByColumnWithOrder(string order, string orderDir, IEnumerable<StockTableDto> data)
        {
            var ls = new List<StockTableDto>();
            try
            {
                switch (order)
                {
                    case "0":
                        ls = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase)
                            ? data.OrderByDescending(p => p.PublisherId).ToList()
                            : data.OrderBy(p => p.PublisherId).ToList();
                        break;
                    case "1":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.Publisher).ToList();
                        else ls = data.OrderBy(p => p.Publisher).ToList();
                        break;
                    case "2":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.SeriesId).ToList();
                        else ls = data.OrderBy(p => p.SeriesId).ToList();
                        break;
                    case "3":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.SeriesName).ToList();
                        else ls = data.OrderBy(p => p.SeriesName).ToList();
                        break;
                    case "4":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.IssueId).ToList();
                        else ls = data.OrderBy(p => p.IssueId).ToList();
                        break;
                    case "5":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.IssueNumber).ToList();
                        else ls = data.OrderBy(p => p.IssueNumber).ToList();
                        break;
                    case "6":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.PublishDate).ToList();
                        else ls = data.OrderBy(p => p.PublishDate).ToList();
                        break;
                    case "7":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.BoxId).ToList();
                        else ls = data.OrderBy(p => p.BoxId).ToList();
                        break;
                    case "8":
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderByDescending(p => p.Barcode).ToList();
                        else ls = data.OrderBy(p => p.Barcode).ToList();
                        break;
                    default:
                        if (orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase))
                            ls = data.OrderBy(p => p.SeriesId).ToList();
                        else ls = data.OrderBy(p => p.SeriesId).ToList();
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return ls;
        }

        public bool AddStock(int publisherId, int seriesId, int issueId, int quantity, int condition)
        {
            try
            {
                if (publisherId != 0 && seriesId != 0 && issueId != 0 && quantity != 0 && condition != 0)
                {
                   bool x = _addition.NewStock(publisherId, seriesId, issueId, quantity, condition);
                    if (!x)
                    {return false;
                        
                    }
                }
                else
                {
                    throw new ArgumentException("One or more ID's are not Valid (Zero is not a valid input)");
                    
                }
            }
            catch (Exception e)
            {
                Debug.Write("Source: "+ e.Source);
                Debug.WriteLine("Inner Exception: " + e.InnerException);
                throw;
            }
            
            return true;
            
        }
    }
}
