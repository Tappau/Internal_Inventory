using System.Collections.Generic;
using System.Web.Mvc;
using ComicsInventory.DAL.DTOs;

namespace ComicsInventory.ViewModels
{
    public class HomeIndexViewModels
    {
        public IEnumerable<SelectListItem> PublisherList { get; set; }
        public int SelectedPublisherId { get; set; }
    }

    public class LatestAdditionViewModel
    {
        public IEnumerable<LatestAdditionsDto> Additions { get; set; }
        public IEnumerable<IssueConDto> Conditions { get; set; }
    }
}