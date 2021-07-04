using System;

namespace Invent.FerhatCengiz.Models.Dto
{
    public class InventorySalesAddDto
    {
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public int SalesQuantity { get; set; }
    }

}