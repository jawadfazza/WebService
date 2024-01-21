
using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ITableEntity = Azure.Data.Tables.ITableEntity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IConfiguration configuration) : base(configuration)
        {

        }


        // public ProductsController()
        // {


        //string guid = Guid.NewGuid().ToString();
        //var group = CodeGroups.Query<Group>().ToList()[new Random().Next(0, 16)];
        //var groupLan = CodeGroupLanguages.Query<GroupLanguage>().Where(x => x.GroupRowKey == group.RowKey).ToList();

        //var subGroups = CodeSubGroups.Query<SubGroup>().Where(x => x.GroupRowKey == group.RowKey).ToList();
        //var subGroup = subGroups[new Random().Next(0, subGroups.Count - 1)];
        //var subGroupLan = CodeSubGroupLanguages.Query<SubGroupLanguage>().Where(x => x.SubGroupRowKey == subGroup.RowKey).ToList();
        //DataProducts.AddEntity<Product>(new Product()
        //{
        //    PartitionKey = "1",
        //    RowKey = guid,
        //    GroupRowKey = group.RowKey,
        //    SubGroupRowKey = subGroup.RowKey,
        //    StoreRowKey = Guid.NewGuid().ToString(),
        //    Seq = 1,
        //    ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
        //    Price = new Random().Next(1, 1000),
        //    ProductQuantity = new Random().Next(1, 100),
        //    ProductAvailability = true,
        //    ProductRating = 3.5,
        //    ProductReviews = "I bought this product a week ago and it's been amazing! The quality is top-notch, and it works exactly as described. I highly recommend it to others." +
        //    "Please note that the actual content of ProductReviews will vary depending on the individual experiences and feedback from customers who have used the specific product",
        //    ProductBrand = "XYZ Electronics",
        //    ProductWeight = new Random().NextDouble(),
        //    ProductDimensions = "10 x 5 x 2 inches",
        //    Active = true,
        //    Timestamp = DateTime.Now
        //});
        //string groupNameEN = groupLan.Where(x => x.LanguageID == "EN").FirstOrDefault().Name + " " + subGroupLan.Where(x => x.LanguageID == "EN").FirstOrDefault().Name + " " + new Random().Next(1, 1000);
        //DataProductLanguages.AddEntity<ProductLanguage>(new ProductLanguage()
        //{
        //    PartitionKey = "1",
        //    RowKey = Guid.NewGuid().ToString(),
        //    ProductRowKey = guid,
        //    Name = groupNameEN,
        //    Description = groupNameEN,
        //    ProductSpecifications = "Display: 6.7-inch Super AMOLED, Resolution: 1080 x 2400 pixels, Processor: Octa-core Snapdragon 765G, RAM: 6GB, Storage: 128GB, Rear Camera: 64MP + 12MP + 5MP + 5MP, Front Camera: 32MP, Battery: 4500mAh, Operating System: Android 11, Connectivity: 5G, Wi-Fi, Bluetooth, NFC, Sensors: Fingerprint (under display), accelerometer, gyro, proximity, compass, Color Options: Black, Blue, White.",

        //    LanguageID = "EN",
        //    Active = true,
        //    Timestamp = DateTime.Now
        //});
        //string groupNameAR = groupLan.Where(x => x.LanguageID == "AR").FirstOrDefault().Name + " " + subGroupLan.Where(x => x.LanguageID == "AR").FirstOrDefault().Name + " " + new Random().Next(1, 1000);

        //DataProductLanguages.AddEntity<ProductLanguage>(new ProductLanguage()
        //{
        //    PartitionKey = "1",
        //    RowKey = Guid.NewGuid().ToString(),
        //    ProductRowKey = guid,
        //    Name = groupNameAR,
        //    Description = groupNameAR,
        //    ProductSpecifications = "الشاشة: Super AMOLED مقاس 6.7 بوصة ، الدقة: 1080 × 2400 بكسل ، المعالج: ثماني النواة Snapdragon 765G ، ذاكرة الوصول العشوائي: 6 جيجابايت ، التخزين: 128 جيجابايت ، الكاميرا الخلفية: 64 ميجابكسل + 12 ميجابكسل + 5 ميجابكسل + 5 ميجابكسل ، الكاميرا الأمامية: 32 ميجابكسل ، البطارية: 4500 مللي أمبير ، نظام التشغيل: Android 11 ، الاتصال: 5G ، Wi-Fi ، Bluetooth ، NFC ، مستشعرات: ، أبيض.",
        //    LanguageID = "AR",
        //    Active = true,
        //    Timestamp = DateTime.Now
        //}); ;
        // }
        // GET: api/<ProductsController>
        [HttpGet, Route("/api/products/LoadPartialData")]
        public IEnumerable<ProductView> LoadPartialData(int pageSize, int pageNumber,string Lan,string groupOptions, string subGroupOptions)
        {
            var products = (from a in DataProducts.Query<Product>().Where(x => x.Active && (x.GroupRowKey == groupOptions || groupOptions == null) && (x.SubGroupRowKey == subGroupOptions || subGroupOptions == null))
                            join b in DataProductLanguages.Query<ProductLanguage>().Where(x => x.LanguageID == Lan &&x.Active) on a.RowKey equals b.ProductRowKey select new
                             ProductView {
                                RowKey = a.RowKey,
                                Description = b.Description,
                                GroupRowKey = a.GroupRowKey,
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Price = a.Price,
                                Seq = a.Seq,
                                ProductAvailability=a.ProductAvailability,
                                ProductBrand=a.ProductBrand,
                                ProductDimensions=a.ProductDimensions,
                                ProductQuantity=a.ProductQuantity,
                                ProductRating=a.ProductRating,
                                ProductReviews=a.ProductReviews,
                                ProductSpecifications=b.ProductSpecifications,
                                ProductWeight=a.ProductWeight,
                                StoreRowKey=a.StoreRowKey,
                                Active=a.Active
                            }).OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return  products;
        }

        [HttpGet, Route("/api/products/LoadPartialDataWithSearch")]
        public IEnumerable<ProductView> LoadPartialData(int pageSize, int pageNumber, string searchQuery, string Lan, string groupOptions, string subGroupOptions)
        {
            // Split the search query into individual words
            string[] searchKeywords = searchQuery.Split(' ');

            var products = (from a in DataProducts.Query<Product>().Where(x => x.Active && (x.GroupRowKey == groupOptions || groupOptions == null) && (x.SubGroupRowKey == subGroupOptions || subGroupOptions == null))
                            join b in DataProductLanguages.Query<ProductLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                                .Where(x => searchKeywords.All(keyword => x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || x.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))) on a.RowKey equals b.ProductRowKey
                            select new ProductView
                            {
                                RowKey = a.RowKey,
                                Description = b.Description,
                                GroupRowKey = a.GroupRowKey,
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Price = a.Price,
                                Seq = a.Seq,
                                ProductAvailability = a.ProductAvailability,
                                ProductBrand = a.ProductBrand,
                                ProductDimensions = a.ProductDimensions,
                                ProductQuantity = a.ProductQuantity,
                                ProductRating = a.ProductRating,
                                ProductReviews = a.ProductReviews,
                                ProductSpecifications = b.ProductSpecifications,
                                ProductWeight = a.ProductWeight,
                                StoreRowKey = a.StoreRowKey,
                                Active = a.Active,
                            })
                            .OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            return products;
        }


        // GET: api/<ProductsController>
        [HttpGet, Route("/api/products/LoadAllData")]
        public IEnumerable<Product> LoadAllData()
        {
            var products = DataProducts.Query<Product>().OrderBy(x => x.Seq).ToList();
            return products;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public Product Get(string RowKey)
        {
            var product = DataProducts.Query<Product>(x=>x.RowKey==RowKey).FirstOrDefault();

            return product;
        }

        // POST api/<ProductsController>
        [HttpPost, Route("/api/products/Create")]
        public void Create([FromBody] Product value)
        {
            DataProducts.AddEntityAsync<Product>(value);
        }


        // PUT api/<ProductsController>/5
        [HttpPost, Route("/api/products/Update")]
        public async Task UpdateAsync([FromBody] ProductView value,string Lan)
        {


            // Retrieve the Product entity you want to update
            Product productEntity = await DataProducts.GetEntityAsync<Product>(value.PartitionKey, value.RowKey);

            //Mapper.Map(value, productEntity);


            // Update the Product entity in the table
            await DataProducts.UpdateEntityAsync<Product>(productEntity, productEntity.ETag);

            // Retrieve the ProductLanguage entity you want to update
           // ProductLanguage productLanguageEntity = await DataProductLanguages.GetEntityAsync<ProductLanguage>(value.PartitionKey, value.RowKey);

            // Update properties of the ProductLanguage entity
           // productLanguageEntity.Name = value.Name;
            // Update other properties as needed...

            // Update the ProductLanguage entity in the table
           // await DataProductLanguages.UpdateEntityAsync<ProductLanguage>(productLanguageEntity, productLanguageEntity.ETag);
        }


        // DELETE api/<ProductsController>/5
        [HttpDelete("{RowKey}"),Route("/api/products/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var products = DataProducts.DeleteEntityAsync("1",RowKey);
        }
    }

    public class ProductView 
    {
        public ProductView()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StoreRowKey { get; set; }
        public string GroupRowKey { get; set; }
        public string SubGroupRowKey { get; set; }
        public string ImageURL { get; set; }
        public int Price { get; set; }
        public int ProductQuantity { get; set; }
        public bool ProductAvailability { get; set; }
        public double ProductRating { get; set; }
        public string ProductReviews { get; set; }
        public string ProductSpecifications { get; set; }
        public string ProductBrand { get; set; }
        public double ProductWeight { get; set; }
        public string ProductDimensions { get; set; }
        public bool Active { get; set; }
    }
    public class Product : ITableEntity
    {
        public Product()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ShopRowKey { get; set; }
        public string GroupRowKey { get; set; }
        public string SubGroupRowKey { get; set; }
        public string StoreRowKey { get; set; }
        public int Seq { get; set; }
        public string ImageURL { get; set; }
        public int Price { get; set; }
        public int ProductQuantity { get; set; }
        public bool ProductAvailability { get; set; }
        public double ProductRating { get; set; }
        public string ProductReviews { get; set; }
        
        public string ProductBrand { get; set; }
        public double ProductWeight { get; set; }
        public string ProductDimensions { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;


    }
    public class ProductLanguage : ITableEntity
    {
        public ProductLanguage()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ProductRowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductSpecifications { get; set; }
        public string LanguageID { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

}
