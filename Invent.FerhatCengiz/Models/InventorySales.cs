using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Invent.FerhatCengiz.Models
{
    public class InventorySales
    {
        public int InventorySalesId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public DateTime Date { get; set; }
        public int SalesQuantity { get; set; }
        public int Stock { get; set; }
    }

}