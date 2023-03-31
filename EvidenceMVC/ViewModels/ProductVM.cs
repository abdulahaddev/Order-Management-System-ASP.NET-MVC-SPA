using EvidenceMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvidenceMVC.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int SKUCode { get; set; }
        [Required]
        public System.DateTime EntryDate { get; set; }
        public string PicturePath { get; set; }
        public bool InStock { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}