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

    public class StoresController : BaseController
    {
        public StoresController(IConfiguration configuration) : base(configuration)
        {
        }

        //public StoresController(IConfiguration configuration)
        //{



        //    string guid = Guid.NewGuid().ToString();
        //    var group = CodeGroups.Query<Group>().ToList()[new Random().Next(0, 17)];
        //    var groupLan = CodeGroupLanguages.Query<GroupLanguage>().Where(x => x.GroupRowKey == group.RowKey).ToList();

        //    DataStores.AddEntity<Store>(new Store()
        //    {
        //        PartitionKey = "1",
        //        RowKey = guid,
        //        GroupRowKey = group.RowKey,
        //        Seq = 1,
        //        ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
        //        OpeningHours= new Random().Next(5, 12),
        //        ClosingHours = new Random().Next(18, 24),
        //        ContactNumber= "05984465465",
        //        Email="email@company.com",
        //        Location="Damascues",
        //        NumberOfRatings= new Random().Next(1,1000),
        //        Rating= new Random().Next(1, 5),
        //        Website="",
        //        Tags = {},
        //        Longitude=0,
        //        Latitude=0,

        //        Active = true,
        //        Timestamp = DateTime.Now
        //    }) ;
        //    string groupNameEN = groupLan.Where(x => x.LanguageID == "EN").FirstOrDefault().Name  + " " + new Random().Next(1, 1000);
        //    DataStoreLanguages.AddEntity<StoreLanguage>(new StoreLanguage()
        //    {
        //        PartitionKey = "1",
        //        RowKey = Guid.NewGuid().ToString(),
        //        StoreRowKey = guid,
        //        Name = groupNameEN,
        //        Description = groupNameEN,

        //        LanguageID = "EN",
        //        Active = true,
        //        Timestamp = DateTime.Now

        //    });
        //    string groupNameAR = groupLan.Where(x => x.LanguageID == "AR").FirstOrDefault().Name + new Random().Next(1, 1000);

        //    DataStoreLanguages.AddEntity<StoreLanguage>(new StoreLanguage()
        //    {
        //        PartitionKey = "1",
        //        RowKey = Guid.NewGuid().ToString(),
        //        StoreRowKey = guid,
        //        Name = groupNameAR,
        //        Description = groupNameAR,
        //        LanguageID = "AR",
        //        Active = true,
        //        Timestamp = DateTime.Now
        //    }); ;
        //}
        // GET: api/<StoresController>

        [HttpGet, Route("/api/Stores/LoadPartialData")]
        public IEnumerable<StoreView> LoadPartialData(int pageSize, int pageNumber, string Lan, string groupOptions)
        {
            var Stores = (from a in DataStores.Query<Store>().Where(x => x.Active && (x.GroupRowKey == groupOptions || groupOptions == null))
                            join b in DataStoreLanguages.Query<StoreLanguage>().Where(x => x.LanguageID == Lan && x.Active) on a.RowKey equals b.StoreRowKey
                           
                            select new
                             StoreView
                            {
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                // Assigning properties from Store class to StoreView class
                                Name = b.Name,
                                Description = b.Description,
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
                                Latitude = a.Latitude,
                                GroupRowKey = a.GroupRowKey
                            }).OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Stores;
        }

        [HttpGet, Route("/api/Stores/LoadPartialDataWithSearch")]
        public IEnumerable<StoreView> LoadPartialData(int pageSize, int pageNumber, string searchQuery, string Lan, string groupOptions)
        {
            // Split the search query into individual words
            string[] searchKeywords = searchQuery.Split(' ');

            var stores = (from a in DataStores.Query<Store>().Where(x => x.Active && (x.GroupRowKey == groupOptions || groupOptions == null))
                          join b in DataStoreLanguages.Query<StoreLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                              .Where(x => searchKeywords.All(keyword => x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) || x.Description.Contains(keyword,StringComparison.OrdinalIgnoreCase))) on a.RowKey equals b.StoreRowKey
                          select new StoreView
                          {
                              RowKey = a.RowKey,
                              ImageURL = a.ImageURL,
                              PartitionKey = a.PartitionKey,
                              Seq = a.Seq,
                              Name = b.Name,
                              Description = b.Description,
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
                              Latitude = a.Latitude,
                              GroupRowKey = a.GroupRowKey
                          })
                          .OrderBy(x => x.Seq)
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToList();

            return stores;
        }


        // GET: api/<StoresController>
        [HttpGet, Route("/api/Stores/LoadAllData")]
        public IEnumerable<Store> LoadAllData()
        {
            var Stores = DataStores.Query<Store>().OrderBy(x => x.Seq).ToList();
            return Stores;
        }

        [HttpGet, Route("/api/Stores/GetByRowKey")]
        public Accounts Get(string RowKey)
        {
            var Accounts = DataStores.Query<Accounts>(x => x.RowKey == RowKey).FirstOrDefault();

            return Accounts;
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
        public string GroupRowKey { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public int OpeningHours { get; set; }
        public int ClosingHours { get; set; }
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

        public string GroupRowKey { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public int OpeningHours { get; set; }
        public int ClosingHours { get; set; }
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
