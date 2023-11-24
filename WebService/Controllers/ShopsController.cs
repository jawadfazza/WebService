using Azure.Data.Tables;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WebService.Controllers
{
    [Route("api/Shops")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        TableClient DataShops;
        TableClient DataShopLanguages;
        public ShopsController(IConfiguration configuration)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
            // New instance of TableClient class referencing the server-side table

            tableServiceClient.CreateTableIfNotExists(tableName: "DataShops");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataShopLanguages");

            DataShops = tableServiceClient.GetTableClient(
                tableName: "DataShops"
            );
            DataShopLanguages = tableServiceClient.GetTableClient(
               tableName: "DataShopLanguages"
           );
        }
        // GET: api/<ShopsController>
        [HttpGet, Route("/api/Shops/LoadPartialData")]
        public IEnumerable<ShopView> LoadPartialData(int pageSize, int pageNumber, string Lan)
        {
            var Shops = (from a in DataShops.Query<Shop>().Where(x => x.Active)
                            join b in DataShopLanguages.Query<ShopLanguage>().Where(x => x.LanguageID == Lan && x.Active) on a.RowKey equals b.ShopRowKey
                            select new
                             ShopView
                            {
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                // Assigning properties from Shop class to ShopView class
                                Name = a.Name,
                                Description = a.Description,
                                Location = a.Location,
                                Active = a.Active,
                                OpeningHours = a.OpeningHours,
                                ClosingHours = a.ClosingHours,
                                ContactNumber = a.ContactNumber,
                                Email = a.Email,
                                Website = a.Website,
                                Rating = a.Rating,
                                NumberOfRatings = a.NumberOfRatings,
                                Tags = a.Tags,
                                Longitude = a.Longitude,
                                Latitude = a.Latitude
                            }).OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Shops;
        }

        [HttpGet, Route("/api/Shops/LoadPartialDataWithSearch")]
        public IEnumerable<ShopView> LoadPartialData(int pageSize, int pageNumber, string searchQuery, string Lan)
        {
            var Shops = (from a in DataShops.Query<Shop>().Where(x => x.Active)
                            join b in DataShopLanguages.Query<ShopLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                            .Where(x => x.Name.Contains(searchQuery) || x.Description.Contains(searchQuery)) on a.RowKey equals b.ShopRowKey
                            select new
                            ShopView
                            {
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                // Assigning properties from Shop class to ShopView class
                                Name = a.Name,
                                Description = a.Description,
                                Location = a.Location,
                                Active = a.Active,
                                OpeningHours = a.OpeningHours,
                                ClosingHours = a.ClosingHours,
                                ContactNumber = a.ContactNumber,
                                Email = a.Email,
                                Website = a.Website,
                                Rating = a.Rating,
                                NumberOfRatings = a.NumberOfRatings,
                                Tags = a.Tags,
                                Longitude = a.Longitude,
                                Latitude = a.Latitude

                            })
                .OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Shops;
        }


        // GET: api/<ShopsController>
        [HttpGet, Route("/api/Shops/LoadAllData")]
        public IEnumerable<Shop> LoadAllData()
        {
            var Shops = DataShops.Query<Shop>().OrderBy(x => x.Seq).ToList();
            return Shops;
        }

        // GET api/<ShopsController>/5
        [HttpGet("{id}")]
        public Shop Get(string RowKey)
        {
            var Shop = DataShops.Query<Shop>(x => x.RowKey == RowKey).FirstOrDefault();

            return Shop;
        }

        // POST api/<ShopsController>
        [HttpPost]
        public void Create([FromBody] Shop value)
        {

            DataShops.AddEntityAsync<Shop>(value);

        }


        // PUT api/<ShopsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] ShopLanguage value)
        {
            // Retrieve the entity you want to update
            ShopLanguage entity = await DataShops.GetEntityAsync<ShopLanguage>(value.PartitionKey, value.RowKey);

            entity.Name = value.Name;

            // Update the entity in the table
            await DataShops.UpdateEntityAsync<ShopLanguage>(entity, entity.ETag);
        }

        // DELETE api/<ShopsController>/5
        [HttpDelete("{RowKey}"), Route("/api/Shops/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var Shops = DataShops.DeleteEntityAsync("1", RowKey);
        }
    }


    public class ShopView
    {
        public ShopView()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public DateTime OpeningHours { get; set; }
        public DateTime ClosingHours { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ImageURL { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public string[] Tags { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }

        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
    public class Shop : ITableEntity
    {
        public Shop()
        {
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public DateTime OpeningHours { get; set; }
        public DateTime ClosingHours { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string ImageURL { get; set; }
        public double Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public string[] Tags { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }

        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
    public class ShopLanguage : ITableEntity
    {
        public ShopLanguage()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ShopRowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageID { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

}
