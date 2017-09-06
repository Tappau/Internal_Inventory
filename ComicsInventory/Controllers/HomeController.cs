using System.Web.Mvc;
using ComicsInventory.Services.BLLInterfaces;
using ComicsInventory.ViewModels;

namespace ComicsInventory.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGeneralServices _helpers;
        private readonly IIssueService _issueService;

        public HomeController(IIssueService issueService, IGeneralServices genService)
        {
            _helpers = genService;
            _issueService = issueService;
        }


        public ActionResult Index()
        {
            var model = new HomeIndexViewModels {PublisherList = _helpers.PublisherList()};
            return View(model);
        }

        public ActionResult LatestAddition()
        {
            var recently = _issueService.GetNewestAdditions(null);
            var model = new LatestAdditionViewModel
            {
                Additions = recently
            };
            return PartialView("_LatestAddition", model);
        }
    }
}