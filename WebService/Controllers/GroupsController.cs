
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

    [Route("api/Groups")]
    [ApiController]
    public class GroupsController : BaseController
    {
        public GroupsController(IConfiguration configuration) : base(configuration)
        {
        }




        // GET: api/<GroupsController>
        [HttpGet, Route("/api/Groups/LoadPartialData")]
        public IEnumerable<GroupView> LoadPartialData(int pageSize, int pageNumber,string Lan)
        {

            var Groups = (from a in CodeGroups.Query<Group>().Where(x =>  x.Active)
                            join b in CodeGroupLanguages.Query<GroupLanguage>().Where(x => x.LanguageID == Lan &&x.Active) on a.RowKey equals b.GroupRowKey select new
                             GroupView {
                                RowKey = a.RowKey,
                                Description = b.Description,
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                Active=a.Active,
                               
                            }).OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return  Groups;
        }

        [HttpGet, Route("/api/Groups/LoadPartialDataWithSearch")]
        public IEnumerable<GroupView> LoadPartialData(int pageSize, int pageNumber,string searchQuery, string Lan)
        {
            var Groups = (from a in CodeGroups.Query<Group>().Where(x => x.Active)
                            join b in CodeGroupLanguages.Query<GroupLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                            .Where(x => x.Name.Contains(searchQuery) || x.Description.Contains(searchQuery)) on a.RowKey equals b.GroupRowKey
                            select new
                            GroupView
                            {
                                
                                RowKey = a.RowKey,
                                Description = b.Description,
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                Active = a.Active,
                            })
                .OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return Groups;
        }


        // GET: api/<GroupsController>
        [HttpGet, Route("/api/Groups/LoadAllData")]
        public IEnumerable<GroupView> LoadAllData(string Lan)
        {
            var Groups = (from a in CodeGroups.Query<Group>().Where(x => x.Active)
                          join b in CodeGroupLanguages.Query<GroupLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                           on a.RowKey equals b.GroupRowKey
                          select new
                          GroupView
                          {

                              RowKey = a.RowKey,
                              Description = b.Description,
                              ImageURL = a.ImageURL,
                              Name = b.Name,
                              PartitionKey = a.PartitionKey,
                              Seq = a.Seq,
                              Active = a.Active,
                          })
                .OrderBy(x => x.Seq).ToList();
            return Groups;
        }

        // GET api/<GroupsController>/5
        [HttpGet("{id}")]
        public Group Get(string RowKey)
        {
            var Group = CodeGroups.Query<Group>(x=>x.RowKey==RowKey).FirstOrDefault();

            return Group;
        }

        // POST api/<GroupsController>
        [HttpPost]
        public void Create([FromBody] Group value)
        {
           
            CodeGroups.AddEntityAsync<Group>(value);
           
        }


        // PUT api/<GroupsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] GroupLanguage value)
        {
            // Retrieve the entity you want to update
            GroupLanguage entity = await CodeGroups.GetEntityAsync<GroupLanguage>(value.PartitionKey, value.RowKey);

            entity.Name = value.Name;

            // Update the entity in the table
            await CodeGroups.UpdateEntityAsync<GroupLanguage>(entity, entity.ETag);
        }

        // DELETE api/<GroupsController>/5
        [HttpDelete("{RowKey}"),Route("/api/Groups/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var Groups = CodeGroups.DeleteEntityAsync("1",RowKey);
        }

        private void CreateGroup1()
        {
            // Electronics
            string electronicsGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 1,
                PartitionKey = "1",
                RowKey = electronicsGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = electronicsGuid,
                Name = "Electronics",
                Description = "Electronics include a wide range of gadgets and devices such as smartphones, laptops, cameras, and audio equipment.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = electronicsGuid,
                Name = "الإلكترونيات",
                Description = "الإلكترونيات تشمل مجموعة متنوعة من الأجهزة والأدوات مثل الهواتف الذكية وأجهزة الكمبيوتر المحمولة والكاميرات والأجهزة الصوتية.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Clothing
            string clothingGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 2,
                PartitionKey = "1",
                RowKey = clothingGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = clothingGuid,
                Name = "Clothing",
                Description = "Clothing includes various types of apparel for men, women, and children, including shirts, pants, dresses, and accessories.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = clothingGuid,
                Name = "الملابس",
                Description = "الملابس تشمل أنواعًا مختلفة من الملابس للرجال والنساء والأطفال، بما في ذلك القمصان والبناطيل والفساتين والإكسسوارات.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Books
            string booksGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 3,
                PartitionKey = "1",
                RowKey = booksGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = booksGuid,
                Name = "Books",
                Description = "Books include literary works, educational materials, and informational resources in physical and digital formats.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = booksGuid,
                Name = "الكتب",
                Description = "الكتب تضم الأعمال الأدبية والمواد التعليمية والمصادر المعلوماتية في صيغ مادية ورقمية.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Home Appliances
            string homeAppliancesGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 4,
                PartitionKey = "1",
                RowKey = homeAppliancesGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = homeAppliancesGuid,
                Name = "Home Appliances",
                Description = "Home appliances include various electronic devices used for household purposes, such as refrigerators, washing machines, and ovens.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = homeAppliancesGuid,
                Name = "أجهزة منزلية",
                Description = "تشمل الأجهزة المنزلية مجموعة متنوعة من الأجهزة الإلكترونية المستخدمة لأغراض المنزل، مثل الثلاجات والغسالات والأفران.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Sports & Outdoors
            string sportsOutdoorsGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 5,
                PartitionKey = "1",
                RowKey = sportsOutdoorsGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = sportsOutdoorsGuid,
                Name = "Sports & Outdoors",
                Description = "Sports & outdoors products include equipment and gear for various sports and outdoor activities, such as camping, hiking, and cycling.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = sportsOutdoorsGuid,
                Name = "الرياضة والهواء الطلق",
                Description = "تشمل منتجات الرياضة والهواء الطلق معدات وأدوات لمختلف الرياضات والأنشطة الخارجية، مثل التخييم والمشي لمسافات طويلة وركوب الدراجات.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Beauty & Personal Care
            string beautyPersonalCareGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 6,
                PartitionKey = "1",
                RowKey = beautyPersonalCareGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = beautyPersonalCareGuid,
                Name = "Beauty & Personal Care",
                Description = "Beauty & personal care products include cosmetics, skincare items, and personal hygiene products for grooming and self-care.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = beautyPersonalCareGuid,
                Name = "الجمال والعناية الشخصية",
                Description = "تشمل منتجات الجمال والعناية الشخصية مستحضرات التجميل ومنتجات العناية بالبشرة والنظافة الشخصية للحلاقة والعناية بالذات.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });


            // Furniture
            string furnitureGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 7,
                PartitionKey = "1",
                RowKey = furnitureGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = furnitureGuid,
                Name = "Furniture",
                Description = "Furniture includes various pieces of equipment and accessories used for interior decoration and furnishing homes and offices.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = furnitureGuid,
                Name = "الأثاث",
                Description = "يشمل الأثاث مجموعة متنوعة من الأجهزة والإكسسوارات المستخدمة لتزيين الداخلية وتأثيث المنازل والمكاتب.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Toys & Games
            string toysGamesGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 8,
                PartitionKey = "1",
                RowKey = toysGamesGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = toysGamesGuid,
                Name = "Toys & Games",
                Description = "Toys & games include a wide range of playthings and entertainment products for children and adults.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = toysGamesGuid,
                Name = "الألعاب والألعاب",
                Description = "تشمل الألعاب والألعاب مجموعة متنوعة من اللعب ومنتجات الترفيه للأطفال والكبار.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Food & Beverages
            string foodBeveragesGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 9,
                PartitionKey = "1",
                RowKey = foodBeveragesGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = foodBeveragesGuid,
                Name = "Food & Beverages",
                Description = "Food & beverages include various edibles and drinks for consumption.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = foodBeveragesGuid,
                Name = "الطعام والمشروبات",
                Description = "تشمل الطعام والمشروبات مجموعة متنوعة من المواد الغذائية والمشروبات للاستهلاك.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Continue adding the rest of the product code groups in a similar fashion.
            // Health & Wellness
            string healthWellnessGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 10,
                PartitionKey = "1",
                RowKey = healthWellnessGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = healthWellnessGuid,
                Name = "Health & Wellness",
                Description = "Health & wellness products include items related to personal health and fitness.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = healthWellnessGuid,
                Name = "الصحة والعافية",
                Description = "تشمل منتجات الصحة والعافية العناصر المتعلقة بالصحة الشخصية واللياقة البدنية.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Automotive
            string automotiveGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 11,
                PartitionKey = "1",
                RowKey = automotiveGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = automotiveGuid,
                Name = "Automotive",
                Description = "Automotive products include items related to automobiles and vehicles.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = automotiveGuid,
                Name = "السيارات",
                Description = "تشمل منتجات السيارات العناصر المتعلقة بالسيارات والمركبات.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Office Supplies
            string officeSuppliesGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 12,
                PartitionKey = "1",
                RowKey = officeSuppliesGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = officeSuppliesGuid,
                Name = "Office Supplies",
                Description = "Office supplies include items used in offices and workspaces.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = officeSuppliesGuid,
                Name = "لوازم المكتب",
                Description = "تشمل لوازم المكتب العناصر المستخدمة في المكاتب ومساحات العمل.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Home Decor
            string homeDecorGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 13,
                PartitionKey = "1",
                RowKey = homeDecorGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = homeDecorGuid,
                Name = "Home Decor",
                Description = "Home decor includes items such as furniture, wall art, and decorative accessories.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = homeDecorGuid,
                Name = "ديكور المنزل",
                Description = "تشمل ديكورات المنزل العناصر مثل الأثاث وفن الجدران والإكسسوارات الزخرفية.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Garden & Outdoor
            string gardenOutdoorGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 14,
                PartitionKey = "1",
                RowKey = gardenOutdoorGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = gardenOutdoorGuid,
                Name = "Garden & Outdoor",
                Description = "Garden and outdoor products include gardening tools, patio furniture, and outdoor equipment.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = gardenOutdoorGuid,
                Name = "الحدائق والهواء الطلق",
                Description = "تشمل منتجات الحدائق والهواء الطلق أدوات الحدائق وأثاث الفناء ومعدات الهواء الطلق.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Jewelry & Accessories
            string jewelryAccessoriesGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 15,
                PartitionKey = "1",
                RowKey = jewelryAccessoriesGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = jewelryAccessoriesGuid,
                Name = "Jewelry & Accessories",
                Description = "Jewelry and accessories include items such as necklaces, earrings, and handbags.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = jewelryAccessoriesGuid,
                Name = "المجوهرات والاكسسوارات",
                Description = "تشمل المجوهرات والاكسسوارات العناصر مثل العقود والأقراط والحقائب اليدوية.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Baby & Kids
            string babyKidsGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 16,
                PartitionKey = "1",
                RowKey = babyKidsGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = babyKidsGuid,
                Name = "Baby & Kids",
                Description = "Baby and kids products include clothing, toys, and accessories for infants and children.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = babyKidsGuid,
                Name = "الأطفال والرضع",
                Description = "تشمل منتجات الأطفال والرضع الملابس والألعاب والاكسسوارات للرضع والأطفال.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Travel & Luggage
            string travelLuggageGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 17,
                PartitionKey = "1",
                RowKey = travelLuggageGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = travelLuggageGuid,
                Name = "Travel & Luggage",
                Description = "Travel and luggage products include suitcases, backpacks, and travel accessories.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = travelLuggageGuid,
                Name = "السفر والأمتعة",
                Description = "تشمل منتجات السفر والأمتعة الحقائب السفر والحقائب الظهر ومستلزمات السفر.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });

            // Fitness & Exercise
            string fitnessExerciseGuid = Guid.NewGuid().ToString();
            CodeGroups.AddEntity<Group>(new Group()
            {
                Seq = 18,
                PartitionKey = "1",
                RowKey = fitnessExerciseGuid,
                ImageURL = "https://portalapps.azurewebsites.net/img/download.png",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = fitnessExerciseGuid,
                Name = "Fitness & Exercise",
                Description = "Fitness and exercise products include gym equipment, sportswear, and accessories.",
                LanguageID = "EN",
                Active = true,
                Timestamp = DateTime.Now
            });
            CodeGroupLanguages.AddEntity<GroupLanguage>(new GroupLanguage()
            {
                PartitionKey = "1",
                RowKey = Guid.NewGuid().ToString(),
                GroupRowKey = fitnessExerciseGuid,
                Name = "لياقة وتمرين",
                Description = "تشمل منتجات اللياقة والتمرين معدات الجيم وملابس الرياضة والاكسسوارات.",
                LanguageID = "AR",
                Active = true,
                Timestamp = DateTime.Now
            });


        }
    }

    public class GroupView 
    {
        public GroupView()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool Active { get; set; }
    }
    public class Group : ITableEntity
    {
        public Group()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Seq { get; set; }
        public string ImageURL { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;


    }
    public class GroupLanguage : ITableEntity
    {
        public GroupLanguage()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string GroupRowKey { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LanguageID { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

}
