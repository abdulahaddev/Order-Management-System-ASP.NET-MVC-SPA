using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvidenceMVC.Models;

namespace EvidenceMVC.Controllers
{
    public class CategoriesController : Controller
    {
        SellMangementDBContext db = new SellMangementDBContext();
        // GET: Categories

        public ActionResult Index()
        {
            ViewBag.products = db.Products.ToList();
            return PartialView(db.Categories.OrderByDescending(x => x.CategoryId).ToList());
        }
        public ActionResult Create()
        {
            return PartialView(new Category() );
        }
        [HttpPost]
        public ActionResult Create(Category c)
        {
            if (ModelState.IsValid && (c.CategoryName != null))
            {
                db.Categories.Add(c);
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Edit(int id)
        {
            return PartialView(db.Categories.First(x=> x.CategoryId == id));
        }
        [HttpPost]
        public ActionResult Edit(Category c)
        {
            if (ModelState.IsValid && (c.CategoryName != null))
            {
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                Category c = new Category() {CategoryId = (int)id };
                db.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
    }
}