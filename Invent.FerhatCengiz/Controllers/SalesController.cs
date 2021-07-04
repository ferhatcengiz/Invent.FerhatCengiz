using Invent.FerhatCengiz.ADO;
using Invent.FerhatCengiz.Business.Abstract;
using Invent.FerhatCengiz.Business.Concrete;
using Invent.FerhatCengiz.Models;
using Invent.FerhatCengiz.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Invent.FerhatCengiz.Controllers
{
    public class SalesController : ApiController
    {
        private readonly IMyService _myService;

        public SalesController()
        {
            _myService = new MyService();
        }

        [HttpGet]
        [Route("getSalesAll")]
        public List<InventorySales> getSalesAll()
        {
            List<InventorySales> result = _myService.getAllSales();
            return result;
        }

        [HttpPost]
        [Route("addSales")]
        public bool addSales(InventorySalesAddDto item)
        {
            var result = _myService.addSales(item);
            return result;
        }

        [HttpPost]
        [Route("deleteSales")]
        public bool deleteSales(InventorySales item)
        {
            var result = _myService.deleteSales(item);
            return result;
        }

        [HttpPost]
        [Route("updateSales")]
        public bool updateSales(InventorySales item)
        {
            var result = _myService.updateSales(item);
            return result;
        }

        [HttpGet]
        [Route("getProfitSore")]
        public List<ReportProfitStoreDto> getProfitSore(int storeId)
        {
            List<ReportProfitStoreDto> result = _myService.getProfitSore(storeId);
            return result;
        }

        [HttpGet]
        [Route("getMostProfitableStore")]
        public List<ReportMostProfitableStore> getMostProfitableStore()
        {
            List<ReportMostProfitableStore> result = _myService.getMostProfitableStore();
            return result;
        }

        [HttpGet]
        [Route("getBestSellerProduct")]
        public List<ReportBestSellerProduct> getBestSellerProduct()
        {
            List<ReportBestSellerProduct> result = _myService.getBestSellerProduct();
            return result;
        }
    }
}

