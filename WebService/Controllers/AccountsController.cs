
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
    [Route("api/Accounts")]
    [ApiController]
    public class AccountsController : BaseController
    {
        public AccountsController(IConfiguration configuration) : base(configuration)
        {
        }




        //public AccountsController(IConfiguration configuration)
        //{

        //    string tableName = "DataAccounts";
        //    // New instance of the DataAccounts class
        //    TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
        //    // New instance of DataAccounts class referencing the server-side table

        //    tableServiceClient.CreateTableIfNotExists(tableName: tableName);
        //    DataAccounts = tableServiceClient.GetDataAccounts(
        //        tableName: tableName
        //    );

        //}

        // GET: api/<AccountsController>
        [HttpGet, Route("/api/Accounts/LoadPartialData")]
        public IEnumerable<Accounts> LoadPartialData(int pageSize, int pageNumber)
        {
            var Accounts = DataAccounts.Query<Accounts>().OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Accounts;
        }

        [HttpGet, Route("/api/Accounts/LoadPartialDataWithSearch")]
        public IEnumerable<Accounts> LoadPartialData(int pageSize, int pageNumber, string searchQuery)
        {
            var Accounts = DataAccounts.Query<Accounts>().Where(x => x.FullName.Contains(searchQuery) || x.Email.Contains(searchQuery)).OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Accounts;
        }


        // GET: api/<AccountsController>
        [HttpGet, Route("/api/Accounts/LoadAllData")]
        public IEnumerable<Accounts> LoadAllData()
        {

            var Accounts = DataAccounts.Query<Accounts>().OrderBy(x => x.Seq).ToList();
            return null;
        }

        // GET api/<AccountsController>/5
        [HttpGet, Route("/api/Accounts/GetByRowKey")]
        public Accounts Get(string RowKey)
        {
            var Accounts = DataAccounts.Query<Accounts>(x => x.RowKey == RowKey).FirstOrDefault();

            return Accounts;
        }

        // GET api/<AccountsController>/5
        [HttpGet, Route("/api/Accounts/GetByEmail")]
        public Accounts GetByEmail(string emailAddress)
        {
            var Accounts = DataAccounts.Query<Accounts>(x => x.Email == emailAddress).FirstOrDefault();

            return Accounts;
        }

        // POST api/<AccountsController>
        [HttpPost, Route("/api/Accounts/Create")]
        public Accounts Create([FromBody] Accounts value)
        {
            value.RowKey = Guid.NewGuid().ToString();
            DataAccounts.AddEntity<Accounts>(new Accounts()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = value.RowKey,
                FullName = value.FullName,
                PhoneNumber = value.PhoneNumber,
                Email = value.Email,
                AccountConfirmed = false,
                Password = value.Password,
                PasswordExpiredDate = DateTime.Now.ToUniversalTime(),
                Gender = value.Gender,
                PreferdLanguage = value.PreferdLanguage,
                Timestamp = DateTime.Now,
                AccountCreatedDate = DateTime.Now.ToUniversalTime(),
                AccountStatus = value.AccountStatus,
                AccountType = value.AccountType,
                LastLoginDate = DateTime.Now.ToUniversalTime(),
                ProfilePictureUrl = value.ProfilePictureUrl,
                UserRole = value.UserRole,

            });

            Mail.SendEmail(
                "Email Confirmation",
                value.Email,
                "clickShop@Support.com",
                "Email Confirmation",
                $"Dear {value.FullName},\n\nThank you for registering! Please click the following link to verify your email address:\n\n" +
                        $"https://portalapps.azurewebsites.net/Home/VerifyEmail?code={value.RowKey}");

            return value;
        }


        // PUT api/<AccountsController>/5
        [HttpPost, Route("/api/Accounts/Update")]
        public void Update([FromBody] Accounts value)
        {
            // Retrieve the entity you want to update
            Accounts entity = DataAccounts.GetEntity<Accounts>(value.PartitionKey, value.RowKey);


            //_mapper.Map(value, entity);

            //entity.FullName = value.FullName;
            //entity.PhoneNumber = value.PhoneNumber;
            //entity.Gender = value.Gender;
            //entity.PreferdLanguage = value.PreferdLanguage;

            // Update the entity in the table
            DataAccounts.UpdateEntity<Accounts>(entity, entity.ETag);
        }

        // DELETE api/<AccountsController>/5
        [HttpDelete("{RowKey}"), Route("/api/Accounts/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var Accounts = DataAccounts.DeleteEntityAsync("1", RowKey);
        }

        public void VerifyEmail(string RowKey)
        {
            // Retrieve the entity you want to update
            var entity = DataAccounts.Query<Accounts>(x => x.RowKey == RowKey).FirstOrDefault(); ;
            entity.AccountConfirmed = true;
            // Update the entity in the table
            DataAccounts.UpdateEntity<Accounts>(entity, entity.ETag);
        }

        [HttpPost, Route("/api/Accounts/Login")]
        public Accounts Login([FromBody] Accounts value)
        {
            var Accounts = DataAccounts.Query<Accounts>(x => x.Email == value.Email && x.Password == value.Password).FirstOrDefault();
            if (Accounts != null)
            {
                return Accounts;
            }
            return null;
        }
    }

    public class Accounts : ITableEntity
    {
        public Accounts()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool AccountConfirmed { get; set; }
        public string Password { get; set; }
        public DateTime PasswordExpiredDate { get; set; }
        public string Gender { get; set; }
        public string PreferdLanguage { get; set; }
        public string AccountType { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string AccountStatus { get; set; }
        public string UserRole { get; set; }

        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }
}
