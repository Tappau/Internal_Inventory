using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;
using ComicsInventory.DAL.DTOs;

namespace ComicsInventory.Services.BLLInterfaces
{
    public interface IGeneralServices
    {
        IEnumerable<SelectListItem> PublisherList();
        IEnumerable<SelectListItem> SeriesList(int publisherId);

        List<StockTableDto> SortByColumnWithOrder(string order, string orderDir,
            IEnumerable<StockTableDto> data);

        bool AddStock(int publisherId, int seriesId, int issueId, int quantity, int condition);



    }
}