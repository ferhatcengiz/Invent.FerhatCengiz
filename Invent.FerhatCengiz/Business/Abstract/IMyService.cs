using Invent.FerhatCengiz.Models;
using Invent.FerhatCengiz.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invent.FerhatCengiz.Business.Abstract
{
    interface IMyService
    {
        List<InventorySales> getAllSales();

        bool addSales(InventorySalesAddDto item);

        bool deleteSales(InventorySales item);

        bool updateSales(InventorySales item);

        List<ReportProfitStoreDto> getProfitSore(int storeId);

        List<ReportMostProfitableStore> getMostProfitableStore();

        List<ReportBestSellerProduct> getBestSellerProduct();
    }
}
