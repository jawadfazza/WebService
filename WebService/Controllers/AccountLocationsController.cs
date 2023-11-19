using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebService.Library;
using ITableEntity = Azure.Data.Tables.ITableEntity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    [Route("api/AccountLocations")]
    [ApiController]
    public class AccountLocationsController : ControllerBase
    {
        TableClient tableClient;
        public AccountLocationsController(IConfiguration configuration)
        {

            string tableName = "DataAccountLocations";
            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
            // New instance of TableClient class referencing the server-side table

            tableServiceClient.CreateTableIfNotExists(tableName: tableName);
            tableClient = tableServiceClient.GetTableClient(
                tableName: tableName
            );

        }

        // GET: api/<AccountLocationsController>
        [HttpGet, Route("/api/AccountLocations/LoadPartialData")]
        public IEnumerable<AccountLocations> LoadPartialData(int pageSize, int pageNumber)
        {
            var AccountLocations = tableClient.Query<AccountLocations>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return AccountLocations;
        }

        [HttpGet, Route("/api/AccountLocations/LoadPartialDataWithSearch")]
        public IEnumerable<AccountLocations> LoadPartialData(int pageSize, int pageNumber, string searchQuery)
        {
            var AccountLocations = tableClient.Query<AccountLocations>().Where(x => x.Area.Contains(searchQuery) || x.MoreDetails.Contains(searchQuery)).OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return AccountLocations;
        }


        // GET: api/<AccountLocationsController>
        [HttpGet, Route("/api/AccountLocations/LoadAllData")]
        public IEnumerable<AccountLocations> LoadAllData()
        {

            var AccountLocations = tableClient.Query<AccountLocations>().OrderBy(x => x.Seq).ToList();
            return null;
        }

        // GET api/<AccountLocationsController>/5
        [HttpGet("{id}")]
        public AccountLocations Get(string RowKey)
        {
            var AccountLocations = tableClient.Query<AccountLocations>(x => x.RowKey == RowKey).FirstOrDefault();

            return AccountLocations;
        }

        // POST api/<AccountLocationsController>
        [HttpPost, Route("/api/AccountLocations/Create")]
        public AccountLocations Create([FromBody] AccountLocations value)
        {
            value.RowKey = Guid.NewGuid().ToString();
            tableClient.AddEntity<AccountLocations>(new AccountLocations()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = value.RowKey,
               
                Timestamp = DateTime.Now
            }); 

        

            return value;
        }


        // PUT api/<AccountLocationsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] AccountLocations value)
        {
            // Retrieve the entity you want to update
            AccountLocations entity = await tableClient.GetEntityAsync<AccountLocations>(value.PartitionKey, value.RowKey);

            entity.AddressName = value.AddressName;
        
            // Update the entity in the table
            await tableClient.UpdateEntityAsync<AccountLocations>(entity, entity.ETag);
        }

        // DELETE api/<AccountLocationsController>/5
        [HttpDelete("{RowKey}"), Route("/api/AccountLocations/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var AccountLocations = tableClient.DeleteEntityAsync("1", RowKey);
        }


    }

    public class AccountLocations : ITableEntity
    {
        public AccountLocations()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public int Longitude { get; set; }
        public int Latitude { get; set; }
        public string AddressName { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Floor { get; set; }
        public string Near { get; set; }
        public string MoreDetails { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
