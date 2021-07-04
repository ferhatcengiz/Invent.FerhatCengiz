using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Invent.FerhatCengiz.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public float Cost { get; set; }
        public float SalesPrice { get; set; }

    }
}