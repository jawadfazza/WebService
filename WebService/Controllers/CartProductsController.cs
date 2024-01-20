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
    [Route("api/CartProducts")]
    [ApiController]
    public class CartProductsController : BaseController
    {
        public CartProductsController(IConfiguration configuration) : base(configuration)
        {
        }

        // GET: api/<CartProductsController>
        [HttpGet, Route("/api/CartProducts/LoadPartialData")]
        public IEnumerable<CartProducts> LoadPartialData(int pageSize, int pageNumber)
        {
            var CartProducts = DataCartProducts.Query<CartProducts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return CartProducts;
        }

        [HttpGet, Route("/api/CartProducts/LoadPartialDataWithSearch")]
        public IEnumerable<CartProducts> LoadPartialData(int pageSize, int pageNumber, string searchQuery)
        {
            var CartProducts = DataCartProducts.Query<CartProducts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return CartProducts;
        }


        // GET: api/<CartProductsController>
        [HttpGet, Route("/api/CartProducts/LoadAllData")]
        public IEnumerable<CartProducts> LoadAllData()
        {

            var CartProducts = DataCartProducts.Query<CartProducts>().OrderBy(x => x.Seq).ToList();
            return null;
        }

        // GET api/<CartProductsController>/5
        [HttpGet("{id}")]
        public CartProducts Get(string RowKey)
        {
            var CartProducts = DataCartProducts.Query<CartProducts>(x => x.RowKey == RowKey).FirstOrDefault();

            return CartProducts;
        }

        // POST api/<CartProductsController>
        [HttpPost, Route("/api/CartProducts/Create")]
        public CartProducts Create([FromBody] CartProducts value)
        {
            value.RowKey = Guid.NewGuid().ToString();
            DataCartProducts.AddEntity<CartProducts>(new CartProducts()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = value.RowKey,
              
                Timestamp = DateTime.Now
            }); 

            return value;
        }


        // PUT api/<CartProductsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] CartProducts value)
        {
            // Retrieve the entity you want to update
            CartProducts entity = await DataCartProducts.GetEntityAsync<CartProducts>(value.PartitionKey, value.RowKey);

            entity.Quantity = value.Quantity;
          

            // Update the entity in the table
            await DataCartProducts.UpdateEntityAsync<CartProducts>(entity, entity.ETag);
        }

        // DELETE api/<CartProductsController>/5
        [HttpDelete("{RowKey}"), Route("/api/CartProducts/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var CartProducts = DataCartProducts.DeleteEntityAsync("1", RowKey);
        }


    }

    public class CartProducts : ITableEntity
    {
        public CartProducts()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string CartRowKey { get; set; }
        public string ProductRowKey { get; set; }
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
