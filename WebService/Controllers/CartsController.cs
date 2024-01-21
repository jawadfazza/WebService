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
    [Route("api/Carts")]
    [ApiController]
    public class CartsController : BaseController
    {
        public CartsController(IConfiguration configuration) : base(configuration)
        {
        }

        // GET: api/<CartsController>
        [HttpGet, Route("/api/Carts/LoadPartialData")]
        public IEnumerable<Carts> LoadPartialData(int pageSize, int pageNumber)
        {
            var Carts = DataCarts.Query<Carts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Carts;
        }

        [HttpGet, Route("/api/Carts/LoadPartialDataWithSearch")]
        public IEnumerable<Carts> LoadPartialData(int pageSize, int pageNumber, string searchQuery)
        {
            var Carts = DataCarts.Query<Carts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Carts;
        }


        // GET: api/<CartsController>
        [HttpGet, Route("/api/Carts/LoadAllData")]
        public IEnumerable<Carts> LoadAllData()
        {

            var Carts = DataCarts.Query<Carts>().OrderBy(x => x.Seq).ToList();
            return null;
        }

        // GET api/<CartsController>/5
        [HttpGet("{id}")]
        public Carts Get(string RowKey)
        {
            var Carts = DataCarts.Query<Carts>(x => x.RowKey == RowKey).FirstOrDefault();

            return Carts;
        }

        // POST api/<CartsController>
        [HttpPost, Route("/api/Carts/Create")]
        public Carts Create([FromBody] Carts value)
        {
            value.RowKey = Guid.NewGuid().ToString();
            DataCarts.AddEntity<Carts>(new Carts()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = value.RowKey,
              
                Timestamp = DateTime.Now
            }); 

            return value;
        }


        // PUT api/<CartsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] Carts value)
        {
            // Retrieve the entity you want to update
            Carts entity = await DataCarts.GetEntityAsync<Carts>(value.PartitionKey, value.RowKey);
          
            // Update the entity in the table
            await DataCarts.UpdateEntityAsync<Carts>(entity, entity.ETag);
        }

        // DELETE api/<CartsController>/5
        [HttpDelete("{RowKey}"), Route("/api/Carts/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var Carts = DataCarts.DeleteEntityAsync("1", RowKey);
        }


    }

    public class Carts : ITableEntity
    {
        public Carts()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string UserRowKey { get; set; }
        public string StoreRowKey { get; set; }
        public int Seq { get; set; }
        public double TotalPrice { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }

        // User-related properties
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserReview { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public int Rate { get; set; }

        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
