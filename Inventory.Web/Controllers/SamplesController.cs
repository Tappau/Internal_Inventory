using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Web.Controllers
{
    public class SamplesController : Controller
    {
        // GET: Samples
        public ActionResult Blank()
        {
            return View();
        }

        public ActionResult Buttons()
        {
            return View();
        }

        public ActionResult Forms()
        {
            return View();
        }
        public ActionResult Grid()
        {
            return View();
        }
        public ActionResult Icons()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Notifications()
        {
            return View();
        }
        public ActionResult PanelsWells()
        {
            return View("panels-wells");
        }
        public ActionResult Tables()
        {
            return View();
        }

    }
}