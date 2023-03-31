using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvidenceMVC.Models;
using EvidenceMVC.ViewModels;

namespace EvidenceMVC.Controllers
{
    public class OrdersController : Controller
    {
        SellMangementDBContext db = new SellMangementDBContext();
        // GET: Orders
        public ActionResult Index()
        {
            return PartialView(db.Orders.OrderByDescending(x => x.OrderId).ToList());
        }
        public ActionResult Create()
        {
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Order order, int[] singleProductId, int[] SingleProductQuantity)
        {

            if (ModelState.IsValid)
            {
                if (order.Customer.CustomerId != 0 && singleProductId.Count() > 0 && SingleProductQuantity.Count() > 0)
                {
                    Customer c = order.Customer;
                    Order or = new Order()
                    {
                        CustomerId = order.Customer.CustomerId

                    };
                    db.Orders.Add(or);
                    for (int i = 0; i < singleProductId.Length; i++)
                    {
                        OrderDetail od = new OrderDetail()
                        {
                            OrderId = or.OrderId,
                            ProductId = singleProductId[i],
                            Price = db.Products.Find(singleProductId[i]).Price,
                            Quantity = SingleProductQuantity[i]
                        };
                        db.OrderDetails.Add(od);
                    }
                    db.SaveChanges();
                    return PartialView("_success");
                }
            }
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            return PartialView("_error");
        }
        public ActionResult Edit(int id)
        {
            Order currentOrder = db.Orders.First(x => x.OrderId == id);
            var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == currentOrder.OrderId);

            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = db.Products.ToList();

            OrderVM orderVM = new OrderVM() { Order = currentOrder };

            foreach (var item in orderDetailsList)
            {
                orderVM.OrderDetails.Add(new OrderDetail()
                {
                    OrderDetailsId = item.OrderDetailsId,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            return PartialView(orderVM);
        }

        [HttpPost]
        public ActionResult Edit(OrderVM orderVM, int[] ProductId, int[] Quantity)
        {
            if (orderVM.OrderDetails.Count() >= 0 || (ProductId != null && Quantity != null))
            {
                if (orderVM.OrderDetails.Count() == 0)
                {
                    var orderDetailsList = db.OrderDetails.Where(x => x.OrderId == orderVM.Order.OrderId);
                    foreach (var item in orderDetailsList)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                    db.SaveChanges();
                }
                if (orderVM.OrderDetails.Count() > 0)
                {
                    //Update productDetails if exists
                    foreach (var item in orderVM.OrderDetails)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    //delete removed items
                    var removedItem = db.OrderDetails.Where(x => x.OrderId == orderVM.Order.OrderId).ToList().Except(orderVM.OrderDetails).ToList();

                    foreach (var item in removedItem)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }

                    db.SaveChanges();
                }

                //Add newly added items
                if (ProductId != null)
                {
                    for (int i = 0; i < ProductId.Length; i++)
                    {
                        if (String.IsNullOrEmpty(ProductId[i].ToString()) && String.IsNullOrEmpty(Quantity[i].ToString()))
                        {
                            continue;
                        }
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = orderVM.Order.OrderId,
                            ProductId = ProductId[i],
                            Quantity = Quantity[i],
                            Price = db.Products.Find(ProductId[i]).Price,
                        };

                        db.OrderDetails.Add(orderDetail);;
                    }
                    db.SaveChanges();
                }
                return PartialView("_success");
            }
            return PartialView("_error");
        }
        public ActionResult Delete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
            {
                var orderDetail = db.OrderDetails.Where(x => x.OrderId == id);
                if ((orderDetail != null) && (orderDetail.Count() > 0))
                {
                    foreach (var item in orderDetail)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                    db.SaveChanges();

                    db.Entry(order).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return PartialView("_success");
                }
                return PartialView("_error");
            }
            return PartialView("_error");
        }
        public ActionResult SingleProductEntry()
        {
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");
            return PartialView();
        }
        public ActionResult SingleProductEditEntry()
        {
            ViewBag.customers = new SelectList(db.Customers, "CustomerId", "CustomerName");
            ViewBag.products = new SelectList(db.Products, "ProductId", "ProductName");

            return PartialView(new OrderDetail());
        }
        public JsonResult GetFee(int id)
        {
            var t = db.Products.FirstOrDefault(x => x.ProductId == id);
            return Json(t == null ? 0 : t.Price);
        }
    }
}