using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EvidenceMVC.Models;
using EvidenceMVC.ViewModels;

namespace EvidenceMVC.ViewModels
{
    public class OrderVM
    {
        public OrderVM()
        {
            this.OrderDetails = new List<OrderDetail>();
        }
        public Order Order { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}