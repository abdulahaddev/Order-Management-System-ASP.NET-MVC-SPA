using EvidenceMVC.Models;
using EvidenceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidenceMVC.Controllers
{
    public class ProductsController : Controller
    {
        SellMangementDBContext db = new SellMangementDBContext();
        // GET: Products
        public ActionResult Index()
        {
            ViewBag.orderDetails = db.OrderDetails.ToList();
            return PartialView(db.Products.OrderByDescending(x=>x.ProductId).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return PartialView( new ProductVM() );
        }
        [HttpPost]
        public ActionResult Create(ProductVM pvm)
        {
            if (ModelState.IsValid)
            {
                if (pvm.Picture != null)
                {
                    string ext = Path.GetExtension(pvm.Picture.FileName);
                    var filePath = Path.Combine("~/Images/", Guid.NewGuid().ToString() + ext);
                    pvm.Picture.SaveAs(Server.MapPath(filePath));
                    Product pr = new Product()
                    {
                        ProductName = pvm.ProductName,
                        CategoryId = pvm.CategoryId,
                        Price = pvm.Price,
                        SKUCode = pvm.SKUCode,
                        EntryDate = pvm.EntryDate,
                        PicturePath = filePath,
                        InStock = pvm.InStock
                    };
                    db.Products.Add(pr);
                    db.SaveChanges();
                    return PartialView("_success");
                }
            }
            return PartialView("_error");
        }
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                Product pr = db.Products.Find(id);

                ProductVM pvm = new ProductVM()
                {
                    ProductId = pr.ProductId,
                    ProductName = pr.ProductName,
                    CategoryId = pr.CategoryId,
                    Price = pr.Price,
                    SKUCode = pr.SKUCode,
                    EntryDate = pr.EntryDate,
                    PicturePath = pr.PicturePath,
                    InStock = pr.InStock
                };

                ViewBag.categories = new SelectList(db.Categories, "CategoryId", "CategoryName");
                return PartialView(pvm);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Picture != null)
                {
                    var ext = Path.GetExtension(productVM.Picture.FileName);
                    var fileName = Path.Combine("~/Images/", Guid.NewGuid().ToString() + ext);
                    productVM.Picture.SaveAs(Server.MapPath(fileName));
                    Product pr = new Product()
                    {
                        ProductId = productVM.ProductId,
                        ProductName = productVM.ProductName,
                        CategoryId = productVM.CategoryId,
                        Price = productVM.Price,
                        SKUCode = productVM.SKUCode,
                        EntryDate = productVM.EntryDate,
                        PicturePath = fileName,
                        InStock = productVM.InStock
                    };
                    db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return PartialView("_success");
                }
                else
                {
                    Product pr = new Product()
                    {
                        ProductId = productVM.ProductId,
                        ProductName = productVM.ProductName,
                        CategoryId = productVM.CategoryId,
                        Price = productVM.Price,
                        SKUCode = productVM.SKUCode,
                        EntryDate = productVM.EntryDate,
                        PicturePath = productVM.PicturePath,
                        InStock = productVM.InStock
                    };
                    db.Entry(pr).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return PartialView("_success");
                }
            }
            return PartialView("_error");
        }

        public ActionResult Delete(int id)
        {
            var pr = db.Products.Find(id);
            if (pr != null)
            {
                db.Entry(pr).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_success");
            }
            return PartialView("_error");
        }


    }
}