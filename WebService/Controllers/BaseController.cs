using AutoMapper;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public TableClient DataProducts;
        public TableClient DataProductLanguages;
        public TableClient DataStores;
        public TableClient DataStoreLanguages;
        public TableClient CodeGroups;
        public TableClient CodeSubGroups;
        public TableClient CodeGroupLanguages;
        public TableClient CodeSubGroupLanguages;
        public TableClient DataAccounts;
        public TableClient DataAccountCarts;
        public TableClient DataAccountLocations;

        IConfiguration _configuration { get; }
        //public readonly IMapper _mapper;

        public BaseController(IConfiguration configuration)
        {
           // _configuration = configuration;
            //_mapper = mapper;

            // New instance of the TableClient class
            TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
            // New instance of TableClient class referencing the server-side table

            tableServiceClient.CreateTableIfNotExists(tableName: "DataProducts");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataProductLanguages");
            tableServiceClient.CreateTableIfNotExists(tableName: "CodeGroups");
            tableServiceClient.CreateTableIfNotExists(tableName: "CodeGroupLanguages");
            tableServiceClient.CreateTableIfNotExists(tableName: "CodeSubGroups");
            tableServiceClient.CreateTableIfNotExists(tableName: "CodeSubGroupLanguages");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataStores");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataStoreLanguages");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataAccounts");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataAccountCarts");
            tableServiceClient.CreateTableIfNotExists(tableName: "DataAccountLocations");

           
            DataProducts = tableServiceClient.GetTableClient(
                tableName: "DataProducts"
            );
            DataProductLanguages = tableServiceClient.GetTableClient(
               tableName: "DataProductLanguages"
            );

            CodeGroups = tableServiceClient.GetTableClient(
               tableName: "CodeGroups"
            );
            CodeGroupLanguages = tableServiceClient.GetTableClient(
              tableName: "CodeGroupLanguages"
            );

            CodeSubGroups = tableServiceClient.GetTableClient(
                tableName: "CodeSubGroups"
            );
            CodeSubGroupLanguages = tableServiceClient.GetTableClient(
                tableName: "CodeSubGroupLanguages"
             );

            DataStores = tableServiceClient.GetTableClient(
               tableName: "DataStores"
           );
            DataStoreLanguages = tableServiceClient.GetTableClient(
                tableName: "DataStoreLanguages"
             );

            DataAccounts = tableServiceClient.GetTableClient(
               tableName: "DataAccounts"
            );
            DataAccountLocations = tableServiceClient.GetTableClient(
               tableName: "DataAccountLocations"
           );
            DataAccountCarts = tableServiceClient.GetTableClient(
                tableName: "DataAccountCarts"
            );
        }

       
    }
}
