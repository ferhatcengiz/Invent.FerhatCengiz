using Invent.FerhatCengiz.ADO;
using Invent.FerhatCengiz.Business.Abstract;
using Invent.FerhatCengiz.Models;
using Invent.FerhatCengiz.Models.Dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Invent.FerhatCengiz.Business.Concrete
{
    public class MyService : IMyService
    {
        public List<InventorySales> getAllSales()
        {
            using (var connection = DbCon.GetConnection())
            {
                var query = "SELECT InventorySalesId,ProductId,StoreId,Date,SalesQuantity,Stock FROM InventorySales";
                var command = new SqlCommand(query, connection);
                List<InventorySales> isList = new List<InventorySales>();
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        InventorySales inventorySales = new InventorySales();
                        inventorySales.InventorySalesId = reader.GetInt32(0);
                        inventorySales.ProductId = reader.GetInt32(1);
                        inventorySales.StoreId = reader.GetInt32(2);
                        inventorySales.Date = reader.GetDateTime(3);
                        inventorySales.SalesQuantity = reader.GetInt32(4);
                        inventorySales.Stock = reader.GetInt32(5);
                        isList.Add(inventorySales);
                    }
                }
                connection.Close();
                return isList;
            }
        }

        public bool addSales(InventorySalesAddDto item)
        {
            using (var connection = DbCon.GetConnection())
            {
                bool result = true;
                var query = @"SET @STOCK = (
		                        SELECT Stock - @SALESQUANTITY
		                        FROM InventorySales
		                        WHERE ProductId = @PRODUCTID
			                        AND StoreId	= @STOREID
	                        )

	                        INSERT INTO InventorySales (ProductId,StoreId,Date,SalesQuantity,Stock) 
	                        VALUES (@PRODUCTID,@STOREID,GETDATE(),@SALESQUANTITY,@STOCK)";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("SALESQUANTITY", item.SalesQuantity));
                command.Parameters.Add(new SqlParameter("PRODUCTID", item.ProductId));
                command.Parameters.Add(new SqlParameter("STOREID", item.StoreId));
                connection.Open();
                if (command.ExecuteNonQuery() != -1)
                {
                    result = true;
                }
                connection.Close();
                return result;
            }
        }

        public bool deleteSales(InventorySales item)
        {
            bool result = true;
            using (var connection = DbCon.GetConnection())
            {
                var query = @"SET @SALESQUANTITY = (SELECT SalesQuantity FROM InventorySales WHERE InventorySalesId = @INVENTORYSALESID) 

                            UPDATE InventorySales SET
                                SalesQuantity = 0,
                                Stock = Stock + @SALESQUANTITY
                            WHERE InventorySalesId = @INVENTORYSALESID

                            UPDATE UI SET
                                Stock = UI.Stock + @SALESQUANTITY
                            FROM InventorySales I
                            JOIN InventorySales UI  ON UI.Stock < I.Stock
                                                    AND UI.ProductId = I.ProductId
                                                    AND UI.StoreId = I.StoreId
                            WHERE I.InventorySalesId = @INVENTORYSALESID";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("SALESQUANTITY", item.SalesQuantity));
                command.Parameters.Add(new SqlParameter("INVENTORYSALESID", item.InventorySalesId));
                connection.Open();
                if (command.ExecuteNonQuery() != -1)
                {
                    result = true;
                }
                connection.Close();
                return result;
            }
        }

        public bool updateSales(InventorySales item)
        {
            bool result = true;
            using (var connection = DbCon.GetConnection())
            {
                var query = @"UPDATE InventorySales SET  
		                         ProductId		= @PRODUCTID
		                        ,StoreId		= @STOREID
		                        ,Date			= @DATE
		                        ,SalesQuantity	= @SALESQUANTITY
		                        ,Stock			= @STOCK
	                        WHERE InventorySalesId = @INVENTORYSALESID";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("PRODUCTID", item.SalesQuantity));
                command.Parameters.Add(new SqlParameter("STOREID", item.StoreId));
                command.Parameters.Add(new SqlParameter("DATE", item.Date));
                command.Parameters.Add(new SqlParameter("SALESQUANTITY", item.SalesQuantity));
                command.Parameters.Add(new SqlParameter("STOCK", item.Stock));
                command.Parameters.Add(new SqlParameter("INVENTORYSALESID", item.InventorySalesId));
                connection.Open();
                if (command.ExecuteNonQuery() != -1)
                {
                    result = true;
                }
                connection.Close();
            }
            return result;
        }

        public List<ReportProfitStoreDto> getProfitSore(int storeId)
        {
            using (var connection = DbCon.GetConnection())
            {
                var query = @"SELECT SUM((P.SalesPrice-P.Cost)*I.SalesQuantity)
                            FROM InventorySales I
                            JOIN Products P ON P.Id = I.ProductId
                            WHERE StoreId = @STOREID";

                var command = new SqlCommand(query, connection);
                command.Parameters.Add(new SqlParameter("STOREID", storeId));
                List<ReportProfitStoreDto> isList = new List<ReportProfitStoreDto>();
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReportProfitStoreDto reportDto = new ReportProfitStoreDto();
                        reportDto.SumProfit = reader.GetInt32(0);
                        isList.Add(reportDto);
                    }
                }
                connection.Close();
                return isList;
            }
        }

        public List<ReportMostProfitableStore> getMostProfitableStore()
        {
            using (var connection = DbCon.GetConnection())
            {
                var query = @"SELECT I.StoreId,S.StoreName,Profit = SUM((P.SalesPrice-P.Cost)*I.SalesQuantity)
                            INTO #TMP
	                        FROM InventorySales I
                            JOIN Stores         S ON S.Id = I.StoreId
                            JOIN Products       P ON P.Id = I.ProductId
                            GROUP BY StoreId,S.StoreName

                            SELECT TOP 1 StoreId,StoreName,MaxProfit = Profit
                            FROM #TMP 
	                        ORDER BY MaxProfit DESC

                            DROP TABLE #TMP";

                var command = new SqlCommand(query, connection);
                List<ReportMostProfitableStore> isList = new List<ReportMostProfitableStore>();
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReportMostProfitableStore reportDto = new ReportMostProfitableStore();
                        reportDto.StoreName = reader.GetString(1);
                        reportDto.MaxProfit = reader.GetInt32(2);
                        isList.Add(reportDto);
                    }
                }
                connection.Close();
                return isList;
            }
        }

        public List<ReportBestSellerProduct> getBestSellerProduct()
        {
            using (var connection = DbCon.GetConnection())
            {
                var query = @"SELECT TOP 1 P.ProductName,SumSeller = SUM(I.SalesQuantity)
	                        FROM InventorySales I
	                        JOIN Products P ON P.Id = I.ProductId
	                        GROUP BY P.ProductName
	                        ORDER BY SumSeller DESC";

                var command = new SqlCommand(query, connection);
                List<ReportBestSellerProduct> isList = new List<ReportBestSellerProduct>();
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReportBestSellerProduct reportDto = new ReportBestSellerProduct();
                        reportDto.ProductName = reader.GetString(0);
                        reportDto.SumSeller = reader.GetInt32(1);
                        isList.Add(reportDto);
                    }
                }
                connection.Close();
                return isList;
            }
        }

    }
}