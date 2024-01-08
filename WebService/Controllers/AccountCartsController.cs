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
    [Route("api/AccountCarts")]
    [ApiController]
    public class AccountCartsController : BaseController
    {
        public AccountCartsController(IConfiguration configuration) : base(configuration)
        {
        }

        // GET: api/<AccountCartsController>
        [HttpGet, Route("/api/AccountCarts/LoadPartialData")]
        public IEnumerable<AccountCarts> LoadPartialData(int pageSize, int pageNumber)
        {
            var AccountCarts = DataAccountCarts.Query<AccountCarts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return AccountCarts;
        }

        [HttpGet, Route("/api/AccountCarts/LoadPartialDataWithSearch")]
        public IEnumerable<AccountCarts> LoadPartialData(int pageSize, int pageNumber, string searchQuery)
        {
            var AccountCarts = DataAccountCarts.Query<AccountCarts>().Where(x => x.ProductName.Contains(searchQuery) ).OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return AccountCarts;
        }


        // GET: api/<AccountCartsController>
        [HttpGet, Route("/api/AccountCarts/LoadAllData")]
        public IEnumerable<AccountCarts> LoadAllData()
        {

            var AccountCarts = DataAccountCarts.Query<AccountCarts>().OrderBy(x => x.Seq).ToList();
            return null;
        }

        // GET api/<AccountCartsController>/5
        [HttpGet("{id}")]
        public AccountCarts Get(string RowKey)
        {
            var AccountCarts = DataAccountCarts.Query<AccountCarts>(x => x.RowKey == RowKey).FirstOrDefault();

            return AccountCarts;
        }

        // POST api/<AccountCartsController>
        [HttpPost, Route("/api/AccountCarts/Create")]
        public AccountCarts Create([FromBody] AccountCarts value)
        {
            value.RowKey = Guid.NewGuid().ToString();
            DataAccountCarts.AddEntity<AccountCarts>(new AccountCarts()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = value.RowKey,
              
                Timestamp = DateTime.Now
            }); 

            return value;
        }


        // PUT api/<AccountCartsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] AccountCarts value)
        {
            // Retrieve the entity you want to update
            AccountCarts entity = await DataAccountCarts.GetEntityAsync<AccountCarts>(value.PartitionKey, value.RowKey);

            entity.Quantity = value.Quantity;
          

            // Update the entity in the table
            await DataAccountCarts.UpdateEntityAsync<AccountCarts>(entity, entity.ETag);
        }

        // DELETE api/<AccountCartsController>/5
        [HttpDelete("{RowKey}"), Route("/api/AccountCarts/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var AccountCarts = DataAccountCarts.DeleteEntityAsync("1", RowKey);
        }


    }

    public class AccountCarts : ITableEntity
    {
        public AccountCarts()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ProductRowKey { get; set; }
        public string ProductName { get; set; }
        public int Seq { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }

        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
