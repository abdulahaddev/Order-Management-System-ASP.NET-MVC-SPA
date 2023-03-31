using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvidenceMVC.Models;

namespace EvidenceMVC.Controllers
{
    public class HomeController : Controller
    {
        SellMangementDBContext db = new SellMangementDBContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}