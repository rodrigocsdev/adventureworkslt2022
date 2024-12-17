using AdventureWorksLT2022.Models;
using AdventureWorksLT2022.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorksLT2022.Repositories
{
    public class ProductRepository
    {
        private readonly Connection _connection;

        public ProductRepository(Connection connection)
        {
            _connection = connection;
        }

        private async Task EnsureConnectionOpenAsync(IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                if (dbConnection is SqlConnection sqlConnection)
                {
                    await sqlConnection.OpenAsync();
                }
                else
                {
                    dbConnection.Open();
                }
            }
        }

        private async Task<bool> ReadAsync(IDataReader reader)
        {
            if (reader is SqlDataReader sqlReader)
            {
                return await sqlReader.ReadAsync();
            }
            else
            {
                return reader.Read(); // Síncrono como fallback
            }
        }

        public async Task<IEnumerable<IProduct>> GetAllProductsAsync()
        {
            var products = new List<IProduct>();

            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [SalesLT].[Product]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (await ReadAsync(reader))
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ProductNumber = reader.GetString(1),
                                Color = reader.GetString(1),
                                StandardCost = reader.GetDecimal(1),
                                ListPrice = reader.GetDecimal(1),
                                Size = reader.GetString(1),
                                Weight = reader.GetDecimal(1),
                                ProductCategoryID = reader.GetInt32(1),
                                ProductModelID = reader.GetInt32(1),
                                SellStartDate = reader.GetDateTime(1),
                                SellEndDate = reader.GetDateTime(1),
                                DiscontinuedDate = reader.GetDateTime(1),
                                ModifiedDate = reader.GetDateTime(1),
                            });
                        }
                    }
                }
            }

            return products;
        }

        public async Task<IProduct?> GetProductByIdAsync(int productId)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [SalesLT].[Product] WHERE ProductID = @ProductID";
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ProductID";
                    parameter.Value = productId;
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (await ReadAsync(reader))
                        {
                            return new Product
                            {
                                ProductID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                ProductNumber = reader.GetString(1),
                                Color = reader.GetString(1),
                                StandardCost = reader.GetDecimal(1),
                                ListPrice = reader.GetDecimal(1),
                                Size = reader.GetString(1),
                                Weight = reader.GetDecimal(1),
                                ProductCategoryID = reader.GetInt32(1),
                                ProductModelID = reader.GetInt32(1),
                                SellStartDate = reader.GetDateTime(1),
                                SellEndDate = reader.GetDateTime(1),
                                DiscontinuedDate = reader.GetDateTime(1),
                                ModifiedDate = reader.GetDateTime(1),
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddProductAsync(IProduct product)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = @"
                INSERT INTO [SalesLT].[Product] 
                (Name, ProductNumber, Color, StandardCost, ListPrice, Size, Weight, ProductCategoryID, ProductModelID, SellStartDate, SellEndDate, DiscontinuedDate, ThumbNailPhoto,
                ThumbnailPhotoFileName, RowGuid, ModifiedDate) 
                VALUES 
                (@Name, @ProductNumber, @Color, @StandardCost, @ListPrice, @Size, @Weight, @ProductCategoryID, @ProductModelID, @SellStartDate, @SellEndDate, @DiscontinuedDate,
                @ThumbNailPhoto, @ThumbnailPhotoFileName, @RowGuid, @ModifiedDate)";
                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@Name";
                    nameParameter.Value = product.Name;
                    command.Parameters.Add(nameParameter);

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@ProductID";
                    idParameter.Value = product.ProductID;
                    command.Parameters.Add(idParameter);

                    var productNumberParameter = command.CreateParameter();
                    productNumberParameter.ParameterName = "@ProductNumber";
                    productNumberParameter.Value = product.ProductNumber;
                    command.Parameters.Add(productNumberParameter);

                    var colorParameter = command.CreateParameter();
                    colorParameter.ParameterName = "@Color";
                    colorParameter.Value = product.Color ?? (object)DBNull.Value;
                    command.Parameters.Add(colorParameter);

                    var standardCostParameter = command.CreateParameter();
                    standardCostParameter.ParameterName = "@StandardCost";
                    standardCostParameter.Value = product.StandardCost;
                    command.Parameters.Add(standardCostParameter);

                    var listPriceParameter = command.CreateParameter();
                    listPriceParameter.ParameterName = "@ListPrice";
                    listPriceParameter.Value = product.ListPrice;
                    command.Parameters.Add(listPriceParameter);

                    var sizeParameter = command.CreateParameter();
                    sizeParameter.ParameterName = "@Size";
                    sizeParameter.Value = product.Size ?? (object)DBNull.Value;
                    command.Parameters.Add(sizeParameter);

                    var weightParameter = command.CreateParameter();
                    weightParameter.ParameterName = "@Weight";
                    weightParameter.Value = product.Weight ?? (object)DBNull.Value;
                    command.Parameters.Add(weightParameter);

                    var productCategoryIDParameter = command.CreateParameter();
                    productCategoryIDParameter.ParameterName = "@ProductCategoryID";
                    productCategoryIDParameter.Value = product.ProductCategoryID ?? (object)DBNull.Value;
                    command.Parameters.Add(productCategoryIDParameter);

                    var productModelIDParameter = command.CreateParameter();
                    productModelIDParameter.ParameterName = "@ProductModelID";
                    productModelIDParameter.Value = product.ProductModelID ?? (object)DBNull.Value;
                    command.Parameters.Add(productModelIDParameter);

                    var sellStartDateParameter = command.CreateParameter();
                    sellStartDateParameter.ParameterName = "@SellStartDate";
                    sellStartDateParameter.Value = product.SellStartDate;
                    command.Parameters.Add(sellStartDateParameter);

                    var sellEndDateParameter = command.CreateParameter();
                    sellEndDateParameter.ParameterName = "@SellEndDate";
                    sellEndDateParameter.Value = product.SellEndDate ?? (object)DBNull.Value;
                    command.Parameters.Add(sellEndDateParameter);

                    var discontinuedDateParameter = command.CreateParameter();
                    discontinuedDateParameter.ParameterName = "@DiscontinuedDate";
                    discontinuedDateParameter.Value = product.DiscontinuedDate ?? (object)DBNull.Value;
                    command.Parameters.Add(discontinuedDateParameter);

                    var thumbNailPhotoParameter = command.CreateParameter();
                    thumbNailPhotoParameter.ParameterName = "@ThumbNailPhoto";
                    thumbNailPhotoParameter.Value = product.ThumbNailPhoto ?? (object)DBNull.Value;
                    command.Parameters.Add(thumbNailPhotoParameter);

                    var thumbnailPhotoFileNameParameter = command.CreateParameter();
                    thumbnailPhotoFileNameParameter.ParameterName = "@ThumbnailPhotoFileName";
                    thumbnailPhotoFileNameParameter.Value = product.ThumbnailPhotoFileName ?? (object)DBNull.Value;
                    command.Parameters.Add(thumbnailPhotoFileNameParameter);

                    var rowGuidParameter = command.CreateParameter();
                    rowGuidParameter.ParameterName = "@RowGuid";
                    rowGuidParameter.Value = product.RowGuid;
                    command.Parameters.Add(rowGuidParameter);

                    var modifiedDateParameter = command.CreateParameter();
                    modifiedDateParameter.ParameterName = "@ModifiedDate";
                    modifiedDateParameter.Value = product.ModifiedDate;
                    command.Parameters.Add(modifiedDateParameter);                    

                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task UpdateProductAsync(IProduct product)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = @"
                UPDATE [SalesLT].[Product] 
                SET Name = @Name, ProductNumber = @ProductNumber, Color = @Color, StandardCost = @StandardCost, ListPrice = @ListPrice, Size = @Size, Weight = @Weight, 
                    ProductCategoryID = @ProductCategoryID, ProductModelID = @ProductModelID, SellStartDate = @SellStartDate, SellEndDate = @SellEndDate, 
                    DiscontinuedDate = @DiscontinuedDate, ThumbNailPhoto = @ThumbNailPhoto, ThumbnailPhotoFileName = @ThumbnailPhotoFileName, RowGuid = @RowGuid, 
                    ModifiedDate = @ModifiedDate  WHERE ProductID = @ProductID";
                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@Name";
                    nameParameter.Value = product.Name;
                    command.Parameters.Add(nameParameter);

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@ProductID";
                    idParameter.Value = product.ProductID;
                    command.Parameters.Add(idParameter);

                    var productNumberParameter = command.CreateParameter();
                    productNumberParameter.ParameterName = "@ProductNumber";
                    productNumberParameter.Value = product.ProductNumber;
                    command.Parameters.Add(productNumberParameter);

                    var colorParameter = command.CreateParameter();
                    colorParameter.ParameterName = "@Color";
                    colorParameter.Value = product.Color ?? (object)DBNull.Value;
                    command.Parameters.Add(colorParameter);

                    var standardCostParameter = command.CreateParameter();
                    standardCostParameter.ParameterName = "@StandardCost";
                    standardCostParameter.Value = product.StandardCost;
                    command.Parameters.Add(standardCostParameter);

                    var listPriceParameter = command.CreateParameter();
                    listPriceParameter.ParameterName = "@ListPrice";
                    listPriceParameter.Value = product.ListPrice;
                    command.Parameters.Add(listPriceParameter);

                    var sizeParameter = command.CreateParameter();
                    sizeParameter.ParameterName = "@Size";
                    sizeParameter.Value = product.Size ?? (object)DBNull.Value;
                    command.Parameters.Add(sizeParameter);

                    var weightParameter = command.CreateParameter();
                    weightParameter.ParameterName = "@Weight";
                    weightParameter.Value = product.Weight ?? (object)DBNull.Value;
                    command.Parameters.Add(weightParameter);

                    var productCategoryIDParameter = command.CreateParameter();
                    productCategoryIDParameter.ParameterName = "@ProductCategoryID";
                    productCategoryIDParameter.Value = product.ProductCategoryID ?? (object)DBNull.Value;
                    command.Parameters.Add(productCategoryIDParameter);

                    var productModelIDParameter = command.CreateParameter();
                    productModelIDParameter.ParameterName = "@ProductModelID";
                    productModelIDParameter.Value = product.ProductModelID ?? (object)DBNull.Value;
                    command.Parameters.Add(productModelIDParameter);

                    var sellStartDateParameter = command.CreateParameter();
                    sellStartDateParameter.ParameterName = "@SellStartDate";
                    sellStartDateParameter.Value = product.SellStartDate;
                    command.Parameters.Add(sellStartDateParameter);

                    var sellEndDateParameter = command.CreateParameter();
                    sellEndDateParameter.ParameterName = "@SellEndDate";
                    sellEndDateParameter.Value = product.SellEndDate ?? (object)DBNull.Value;
                    command.Parameters.Add(sellEndDateParameter);

                    var discontinuedDateParameter = command.CreateParameter();
                    discontinuedDateParameter.ParameterName = "@DiscontinuedDate";
                    discontinuedDateParameter.Value = product.DiscontinuedDate ?? (object)DBNull.Value;
                    command.Parameters.Add(discontinuedDateParameter);

                    var thumbNailPhotoParameter = command.CreateParameter();
                    thumbNailPhotoParameter.ParameterName = "@ThumbNailPhoto";
                    thumbNailPhotoParameter.Value = product.ThumbNailPhoto ?? (object)DBNull.Value;
                    command.Parameters.Add(thumbNailPhotoParameter);

                    var thumbnailPhotoFileNameParameter = command.CreateParameter();
                    thumbnailPhotoFileNameParameter.ParameterName = "@ThumbnailPhotoFileName";
                    thumbnailPhotoFileNameParameter.Value = product.ThumbnailPhotoFileName ?? (object)DBNull.Value;
                    command.Parameters.Add(thumbnailPhotoFileNameParameter);

                    var rowGuidParameter = command.CreateParameter();
                    rowGuidParameter.ParameterName = "@RowGuid";
                    rowGuidParameter.Value = product.RowGuid;
                    command.Parameters.Add(rowGuidParameter);

                    var modifiedDateParameter = command.CreateParameter();
                    modifiedDateParameter.ParameterName = "@ModifiedDate";
                    modifiedDateParameter.Value = product.ModifiedDate;
                    command.Parameters.Add(modifiedDateParameter);


                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [SalesLT].[Product] WHERE ProductID = @ProductID";
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ProductID";
                    parameter.Value = productId;
                    command.Parameters.Add(parameter);

                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
