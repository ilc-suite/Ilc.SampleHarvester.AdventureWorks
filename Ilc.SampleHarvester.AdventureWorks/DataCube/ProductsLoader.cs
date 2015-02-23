using AdventureWorks.InformaitonObjects;
using Ilc.BusinessObjects.Common;
using Ilc.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Text;
using Ilc.DataCube.Contract;
using Ilc.BusinessObjects;

namespace Ilc.SampleHarvester.AdventureWorks.DataCube
{
    public class ProductsLoader
    {
         private SqlConnection connection;

        /// <summary>
         /// Creates a ProductsLoader class.
        /// </summary>
        /// <param name="connectionString">A connection string to a AdventureWorkds database.</param>
        public ProductsLoader(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public List<Product> LoadProductByCompany(Company company)
        {
            var result = new List<Product>();            
            var sql = "select p.*, sod.OrderQty " +
                        "from [Production].[Product] p " +
                        "inner join [Sales].[SalesOrderDetail] sod " +
                        "ON p.ProductID = sod.ProductID " +
                        "inner join [Sales].[SalesOrderHeader] soh " +
                        "ON sod.SalesOrderID = soh.SalesOrderID " +
                        "inner join [Sales].[Customer] c " +
                        "ON c.CustomerID = soh.CustomerID " +
                        "where c.StoreID = @storeid ";
            var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@storeid", company.Number);
            using (SqlDataReader datareader = cmd.ExecuteReader())
            {
                while (datareader.Read())
                {
                    var product = ReadProduct(datareader);
                    result.Add(product);                    
                }
            }
            return result;
        }

        private BikeProduct ReadProduct(SqlDataReader reader)
        {
            return new BikeProduct
            {
                Id = reader.GetInt32("ProductID").ToString(),
                Name = reader.GetStringOrDefault("Name"),
                Articlenumber = reader.GetStringOrDefault("ProductNumber"),
                MakeFlag = reader.GetBoolean("MakeFlag"),
                FinishedGoodsFlag = reader.GetBoolean("FinishedGoodsFlag"),
                Color = reader.GetStringOrDefault("Color"),
                StandardCost = reader.GetDecimal("StandardCost"),
                ListPrice = reader.GetDecimal("ListPrice"),
                Size = reader.GetStringOrDefault("Size"),
                SizeMeasure = reader.GetStringOrDefault("SizeUnitMeasureCode"),
                WeightMeasure = reader.GetStringOrDefault("WeightUnitMeasureCode"),
                Weight = reader.GetDecimalOrNull("Weight"),
                DaysToManufacture = reader.GetInt32("DaysToManufacture"),
                ProductLine = reader.GetStringOrDefault("ProductLine"),
                Class = reader.GetStringOrDefault("Class"),
                Style = reader.GetStringOrDefault("Style"),
                SubCategoryId = reader.GetInt32("ProductSubCategoryID"),
                ProductModelId = reader.GetInt32("ProductModelID"),

                InstallationDate = reader.GetDateTimeOrNull("SellStartDate"),
                SellEndDate = reader.GetDateTimeOrNull("SellEndDate"),

                CustomerOrderQty = reader.GetInt32OrDefault("OrderQty")
            };
        }

        public List<ProductPhoto> GetProductPhoto(string productId)
        {
            var result = new List<ProductPhoto>();
            var sql = "SELECT * " +
                    "FROM [Production].[ProductPhoto] p " +
                    "inner join [Production].[ProductProductPhoto] ppp " +
                    "ON ppp.ProductPhotoID = p.ProductPhotoID " +
                    "WHERE ppp.ProductID = @productid " +
                    "and ppp.[Primary] = 1";
            var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@productid", productId);
            using (SqlDataReader datareader = cmd.ExecuteReader())
            {
                while (datareader.Read())
                {
                    result.Add(ReadPhoto(datareader));
                }
            }
            return result;
        }

        private ProductPhoto ReadPhoto(SqlDataReader reader)
        {
            var p = new ProductPhoto
            {
                Id = reader.GetInt32("ProductPhotoId"),
                LargePhotoFileName = reader.GetStringOrDefault("LargePhotoFileName"),
                ModifiedDate = reader.GetDateTimeOrDefault("ModifiedDate"),
            };

            var imagebytes = (byte[])reader["LargePhoto"];
            p.LargePhotoB64 = Convert.ToBase64String(imagebytes);
            return p;
        }
    }
}