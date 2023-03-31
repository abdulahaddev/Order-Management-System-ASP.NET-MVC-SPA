using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvidenceMVC.Models;

namespace EvidenceMVC.Controllers
{
    public class CustomersController : Controller
    {
        SellMangementDBContext db = new SellMangementDBContext();
        // GET: Customers
        public ActionResult Index()
        {
            ViewBag.orders = db.Orders.ToList();
            return PartialView(db.Customers.OrderByDescending(x=>x.CustomerId).ToList());
        }
        public ActionResult Create()
        {
            return PartialView(new Customer());
        }
        [HttpPost]
        public ActionResult Create(Customer c)
        {
            if (ModelState.IsValid && (c.CustomerName !=null && c.CustomerAddress !=null))
            {
                db.Customers.Add(c);
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Edit(int id)
        {
            return PartialView(db.Customers.First(x => x.CustomerId == id));
        }
        [HttpPost]
        public ActionResult Edit(Customer c)
        {
            if (ModelState.IsValid)
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
                Customer c = new Customer() { CustomerId = (int)id };
                db.Entry(c).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }
    }
}