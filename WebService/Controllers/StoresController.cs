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
    [Route("api/Stores")]
    [ApiController]

    public class StoresController : ControllerBase
    {
        TableClient DataStores;
        TableClient DataStoreLanguages;
        public StoresController(IConfiguration configuration)
        {
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
            // New instance of TableClient class referencing the server-side table

            tableServiceClient.CreateTableIfNotExists(tableName: "DataStores");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataStoreLanguages");

            DataStores = tableServiceClient.GetTableClient(
                tableName: "DataStores"
            );
            DataStoreLanguages = tableServiceClient.GetTableClient(
               tableName: "DataStoreLanguages"
           );

        }
        // GET: api/<StoresController>
        [HttpGet, Route("/api/Stores/LoadPartialData")]
        public IEnumerable<StoreView> LoadPartialData(int pageSize, int pageNumber, string Lan)
        {
            var Stores = (from a in DataStores.Query<Store>().Where(x => x.Active)
                            join b in DataStoreLanguages.Query<StoreLanguage>().Where(x => x.LanguageID == Lan && x.Active) on a.RowKey equals b.StoreRowKey
                            select new
                             StoreView
                            {
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                // Assigning properties from Store class to StoreView class
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
            return Stores;
        }

        [HttpGet, Route("/api/Stores/LoadPartialDataWithSearch")]
        public IEnumerable<StoreView> LoadPartialData(int pageSize, int pageNumber, string searchQuery, string Lan)
        {
            var Stores = (from a in DataStores.Query<Store>().Where(x => x.Active)
                            join b in DataStoreLanguages.Query<StoreLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                            .Where(x => x.Name.Contains(searchQuery) || x.Description.Contains(searchQuery)) on a.RowKey equals b.StoreRowKey
                            select new
                            StoreView
                            {
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                // Assigning properties from Store class to StoreView class
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
            return Stores;
        }


        // GET: api/<StoresController>
        [HttpGet, Route("/api/Stores/LoadAllData")]
        public IEnumerable<Store> LoadAllData()
        {
            var Stores = DataStores.Query<Store>().OrderBy(x => x.Seq).ToList();
            return Stores;
        }

        // GET api/<StoresController>/5
        [HttpGet("{id}")]
        public Store Get(string RowKey)
        {
            var Store = DataStores.Query<Store>(x => x.RowKey == RowKey).FirstOrDefault();

            return Store;
        }

        // POST api/<StoresController>
        [HttpPost]
        public void Create([FromBody] Store value)
        {

            DataStores.AddEntityAsync<Store>(value);

        }


        // PUT api/<StoresController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] StoreLanguage value)
        {
            // Retrieve the entity you want to update
            StoreLanguage entity = await DataStores.GetEntityAsync<StoreLanguage>(value.PartitionKey, value.RowKey);

            entity.Name = value.Name;

            // Update the entity in the table
            await DataStores.UpdateEntityAsync<StoreLanguage>(entity, entity.ETag);
        }

        // DELETE api/<StoresController>/5
        [HttpDelete("{RowKey}"), Route("/api/Stores/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var Stores = DataStores.DeleteEntityAsync("1", RowKey);
        }
    }


    public class StoreView
    {
        public StoreView()
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
    public class Store : ITableEntity
    {
        public Store()
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
    public class StoreLanguage : ITableEntity
    {
        public StoreLanguage()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string StoreRowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageID { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

}
