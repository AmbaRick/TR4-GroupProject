//using DnsClient;
//using MongoDB.Driver;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CheckEventCapacity.Tests.LambdaFunctionTests
//{
//    public class MongoDBServiceUnitTest
//    {
//        private Mock<IMongoClient> mongoClient;
//        private Mock<IMongoDatabase> mongodb;
//        private Mock<IMongoCollection<Products>> productCollection;
//        private List<Products> productList;
//        private Mock<IAsyncCursor<Products>> productCursor;
//        public MongoDBServiceUnitTest()
//        {
//            this.settings = new MongoDBSettings("ecommerce-db");
//            this.mongoClient = new Mock<IMongoClient>();
//            this.productCollection = new Mock<IMongoCollection<Products>>();
//            this.mongodb = new Mock<IMongoDatabase>();
//            this.productCursor = new Mock<IAsyncCursor<Products>>();
//            var product = new Products
//            {
//                Id = "1",
//                Attributes = new Dictionary<string, string>() {
//                    {
//                        "ram",
//                        "4gb"
//                    }, {
//                        "diskSpace",
//                        "128gb"
//                    }
//                },
//                Categories = new List<string>() {
//                    "Mobiles"
//                },
//                Department = "Electronics",
//                Description = "Brand new affordable samsung mobile",
//                Item = "Samsung Galaxy M31s",
//                Sku = "sku1234567",
//                Quantity = 99,
//                Image = "url",
//                InsertedDate = DateTime.UtcNow,
//                UpdatedDate = DateTime.UtcNow,
//                SchemaVersion = 1,
//                ManufactureDetails = new ManufactureDetails()
//                {
//                    Brand = "Samsung",
//                    Model = "M31s"
//                },
//                Pricing = new Pricing()
//                {
//                    Price = 15000,
//                    Currency = "INR",
//                    Discount = 1000,
//                    DiscountExpireAt = DateTime.UtcNow.AddDays(10)
//                },
//                Rating = new Rating()
//                {
//                    AggregateRating = 4.3,
//                    TotalReviews = 10000,
//                    Stars = new List<int>() {
//                            1,
//                            2,
//                            3,
//                            4,
//                            5
//                        }
//                }
//            };
//            this.productList = new List<Products>() {
//            product
//        };
//        }
//    }
//}
