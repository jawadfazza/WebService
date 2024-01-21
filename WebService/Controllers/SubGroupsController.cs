
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
    [Route("api/SubGroups")]
    [ApiController]
    public class SubGroupsController : BaseController
    {
        
        public SubGroupsController(IConfiguration configuration) : base(configuration) {
           
        }

        //public SubGroupsController(IConfiguration configuration)
        //{

        //    // New instance of the TableClient class
        //    TableServiceClient tableServiceClient = new TableServiceClient(configuration.GetConnectionString("CosmosDB"));
        //    // New instance of TableClient class referencing the server-side table
        //    tableServiceClient.CreateTableIfNotExists(tableName: "CodeSubGroups");
        //    tableServiceClient.CreateTableIfNotExists(tableName: "CodeSubGroupLanguages");
        //    CodeSubGroups = tableServiceClient.GetTableClient(
        //        tableName: "CodeSubGroups"
        //    );
        //    CodeSubGroupLanguages = tableServiceClient.GetTableClient(
        //       tableName: "CodeSubGroupLanguages"
        //   );

        //    //CreateSubGroupElectronics();
        //    //CreateSubGroupBooks();
        //    //CreateSubGroupAutomotive();
        //    //CreateSubGroupBabyKids();
        //    //CreateSubGroupBeautyPersonalCare();
        //    //CreateSubGroupClothing();
        //    //CreateSubGroupElectronics();
        //    //CreateSubGroupFitnessExercise();
        //    //CreateSubGroupFoodBeverages();
        //    //CreateSubGroupFurniture();
        //    //CreateSubGroupGardenOutdoor();
        //    //CreateSubGroupHealthWellness();
        //    //CreateSubGroupHomeDecor();
        //    //CreateSubGroupJewelryAccessories();
        //    //CreateSubGroupOfficeSupplies();
        //    //CreateSubGroupomeAppliance();
        //    //CreateSubGroupSportsOutdoor();
        //    //CreateSubGroupToysGames();
        //    //CreateSubGroupTravelLuggage();

        //}



        // GET: api/<SubGroupsController>

        [HttpGet, Route("/api/SubGroups/LoadPartialData")]
        public IEnumerable<SubGroupView> LoadPartialData(int pageSize, int pageNumber,string Lan)
        {

            var SubGroups = (from a in CodeSubGroups.Query<SubGroup>().Where(x =>  x.Active)
                            join b in CodeSubGroupLanguages.Query<SubGroupLanguage>().Where(x => x.LanguageID == Lan &&x.Active) on a.RowKey equals b.SubGroupRowKey select new
                             SubGroupView {
                                RowKey = a.RowKey,
                               
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                Active=a.Active,
                               
                            }).OrderBy(x => x.Seq)
                            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return  SubGroups;
        }

        [HttpGet, Route("/api/SubGroups/LoadPartialDataWithSearch")]
        public IEnumerable<SubGroupView> LoadPartialData(int pageSize, int pageNumber,string searchQuery, string Lan)
        {
            var SubGroups = (from a in CodeSubGroups.Query<SubGroup>().Where(x => x.Active)
                            join b in CodeSubGroupLanguages.Query<SubGroupLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                            .Where(x => x.Name.Contains(searchQuery)) on a.RowKey equals b.SubGroupRowKey
                            select new
                            SubGroupView
                            {
                                
                                RowKey = a.RowKey,
                                ImageURL = a.ImageURL,
                                Name = b.Name,
                                PartitionKey = a.PartitionKey,
                                Seq = a.Seq,
                                Active = a.Active,
                            })
                .OrderBy(x => x.Seq).Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
            return SubGroups;
        }


        // GET: api/<SubGroupsController>
        [HttpGet, Route("/api/SubGroups/LoadAllData")]
        public IEnumerable<SubGroupView> LoadAllData(string Lan)
        {
            var SubGroups = (from a in CodeSubGroups.Query<SubGroup>().Where(x => x.Active)
                          join b in CodeSubGroupLanguages.Query<SubGroupLanguage>().Where(x => x.LanguageID == Lan && x.Active)
                           on a.RowKey equals b.SubGroupRowKey
                          select new
                          SubGroupView
                          {

                              RowKey = a.RowKey,
                              GroupRowKey=a.GroupRowKey,
                              ImageURL = a.ImageURL,
                              Name = b.Name,
                              PartitionKey = a.PartitionKey,
                              Seq = a.Seq,
                              Active = a.Active,
                          })
                .OrderBy(x => x.Seq).ToList();
            return SubGroups;
        }

        // GET api/<SubGroupsController>/5
        [HttpGet("{id}")]
        public SubGroup Get(string RowKey)
        {
            var SubGroup = CodeSubGroups.Query<SubGroup>(x=>x.RowKey==RowKey).FirstOrDefault();

            return SubGroup;
        }

        // POST api/<SubGroupsController>
        [HttpPost]
        public void Create([FromBody] SubGroup value)
        {
           
            CodeSubGroups.AddEntityAsync<SubGroup>(value);
           
        }


        // PUT api/<SubGroupsController>/5
        [HttpPost]
        public async Task UpdateAsync([FromBody] SubGroupLanguage value)
        {
            // Retrieve the entity you want to update
            SubGroupLanguage entity = await CodeSubGroups.GetEntityAsync<SubGroupLanguage>(value.PartitionKey, value.RowKey);

            entity.Name = value.Name;

            // Update the entity in the table
            await CodeSubGroups.UpdateEntityAsync<SubGroupLanguage>(entity, entity.ETag);
        }

        // DELETE api/<SubGroupsController>/5
        [HttpDelete("{RowKey}"),Route("/api/SubGroups/delete/{RowKey}")]
        public void Delete(string RowKey)
        {
            var SubGroups = CodeSubGroups.DeleteEntityAsync("1",RowKey);
        }

        private void CreateSubGroupClothing()
        {
            string clothingGuid = "18648f86-4d17-4a24-a519-ec36ebf1c5a6";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishClothingTypes = new List<string>
            {
              "Tops", "Bottoms", "Dresses", "Outerwear", "Activewear", "Sleepwear",
              "Swimwear", "Formalwear", "Underwear", "Men's Clothing", "Women's Clothing",
              "Unisex Clothing", "Children's Clothing", "Teen Clothing", "Adult Clothing",
              "Senior Clothing", "Casual Wear", "Formal Wear", "Streetwear", "Vintage Clothing",
              "Bohemian Style", "Minimalist Style", "Classic Style", "Trendy/Fashion-forward Style",
              "Workwear", "Sports and Athletic Wear", "Special Occasion Clothing", "Loungewear",
              "Maternity Clothing", "Spring/Summer Clothing", "Fall/Winter Clothing",
              "Denim Clothing", "Leather Clothing", "Woolen Clothing", "Cotton Clothing",
              "Synthetic Fabric Clothing", "Hats and Caps", "Scarves and Shawls",
              "Gloves and Mittens", "Belts", "Ties and Bowties", "Traditional Clothing",
              "Religious Clothing", "Medical Scrubs", "Military Uniforms", "Chef's Uniforms",
              "Construction Workwear"
            };

            // Arabic Clothing Types (Translate the English types to Arabic)
            List<string> arabicClothingTypes = new List<string>
            {
              "القمصان والبلوزات", "السراويل والتنانير", "الفساتين", "الملابس الخارجية", "ملابس الرياضة", "ملابس النوم",
              "ملابس السباحة", "ملابس رسمية", "الملابس الداخلية", "ملابس رجالية", "ملابس نسائية",
              "ملابس للجنسين", "ملابس أطفال", "ملابس مراهقين", "ملابس للكبار",
              "ملابس لكبار السن", "ملابس عارضة", "ملابس رسمية", "ملابس شبابية", "ملابس عتيقة",
              "أسلوب البوهيمي", "أسلوب بسيط", "أسلوب كلاسيكي", "أسلوب مواكب للموضة",
              "ملابس العمل", "ملابس الرياضة", "ملابس مناسبات خاصة", "ملابس المنزل",
              "ملابس الحمل", "ملابس ربيعية وصيفية", "ملابس خريفية وشتوية",
              "ملابس جينز", "ملابس جلدية", "ملابس صوفية", "ملابس قطنية",
              "ملابس منسوجة من مواد صناعية", "قبعات وكابات", "أوشحة وشالات",
              "قفازات وقفازات صوفية", "أحزمة", "رباطات عنق وربطات عنق", "ملابس تقليدية",
              "ملابس دينية", "ملابس طبية", "ملابس عسكرية", "ملابس الطهاة",
              "ملابس عمل في مجال البناء"
            };


            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishClothingTypes.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey= clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/"+ RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishClothingTypes[i];
                string arabicType = arabicClothingTypes[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupElectronics()
        {
            string clothingGuid = "2c0d01b9-3fb8-4028-afa9-2bdf0daf5969";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishElectronicsSubgroups = new List<string>
{
    "Computers", "Smartphones", "Tablets", "Laptops", "Desktops",
    "Wearable Devices", "Audio Devices", "Cameras", "Televisions",
    "Home Appliances", "Gaming Consoles", "Accessories",
    "Networking Devices", "Smart Home Devices", "Home Theater Systems"
};

            List<string> arabicElectronicsSubgroups = new List<string>
{
    "أجهزة الكمبيوتر", "الهواتف الذكية", "الأجهزة اللوحية", "الحواسيب المحمولة", "أجهزة الكمبيوتر الشخصية",
    "الأجهزة القابلة للارتداء", "أجهزة الصوت", "الكاميرات", "التلفزيونات", "الأجهزة المنزلية",
    "أجهزة الألعاب", "الإكسسوارات", "أجهزة الشبكات", "أجهزة المنزل الذكية", "أنظمة المسرح المنزلي"
};



            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishElectronicsSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishElectronicsSubgroups[i];
                string arabicType = arabicElectronicsSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupBooks()
        {
            string clothingGuid = "8c4b1276-0799-403b-8cbf-5a8d02d0fda3";
            // Adding clothing types in both languages
            // English Clothing Types
          // Books Subgroups
        List<string> englishBooksSubgroups = new List<string>
        {
            "Fiction", "Non-Fiction", "Mystery", "Science Fiction", "Fantasy",
            "Biography", "Self-Help", "History", "Cookbooks", "Art and Photography",
            "Children's Books", "Young Adult", "Poetry", "Travel Guides", "Religion"
        };

            List<string> arabicBooksSubgroups = new List<string>
        {
            "الروايات", "غير الخيالية", "الغموض", "الخيال العلمي", "الخيال السحري",
            "السيرة الذاتية", "التطوير الشخصي", "التاريخ", "كتب الطهي", "الفن والتصوير",
            "كتب الأطفال", "الشباب البالغ", "الشعر", "أدلة السفر", "الديانة"
        };




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishBooksSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishBooksSubgroups[i];
                string arabicType = arabicBooksSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupomeAppliance()
        {
            string clothingGuid = "4884bcf5-842d-42fa-9a8e-08053a820b8d";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishHomeApplianceSubgroups = new List<string>
{
    "Refrigerators", "Washing Machines", "Ovens", "Microwaves", "Dishwashers",
    "Vacuum Cleaners", "Air Conditioners", "Water Heaters", "Blenders", "Coffee Makers",
    "Toasters", "Juicers", "Food Processors", "Rice Cookers", "Ironing Appliances"
};

            List<string> arabicHomeApplianceSubgroups = new List<string>
{
    "ثلاجات", "غسالات ملابس", "أفران", "أفران الميكروويف", "غسالات صحون",
    "مكانس كهربائية", "مكيفات هواء", "سخانات مياه", "خلاطات", "ماكينات صنع القهوة",
    "محمصات", "عصارات", "معالجات الطعام", "مطاحن الأرز", "أجهزة الكوي"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishHomeApplianceSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishHomeApplianceSubgroups[i];
                string arabicType = arabicHomeApplianceSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupSportsOutdoor()
        {
            string clothingGuid = "862d16ef-eac5-4bc0-a70b-4ca7e1d38bc7";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishSportsOutdoorsSubgroups = new List<string>
{
    "Fitness Equipment", "Outdoor Clothing", "Sports Shoes", "Camping Gear", "Hiking Equipment",
    "Bicycles", "Exercise Accessories", "Team Sports Gear", "Water Sports Equipment", "Winter Sports Gear",
    "Athletic Accessories", "Outdoor Recreation", "Yoga and Pilates Equipment", "Hunting and Fishing Gear",
    "Skateboarding and Scooters", "Golf Equipment"
};

            List<string> arabicSportsOutdoorsSubgroups = new List<string>
{
    "معدات اللياقة البدنية", "ملابس للأماكن الخارجية", "أحذية رياضية", "معدات التخييم", "معدات التسلق",
    "دراجات هوائية", "إكسسوارات التمرين", "معدات رياضات جماعية", "معدات الرياضات المائية", "معدات رياضات الشتاء",
    "إكسسوارات رياضية", "الترفيه في الهواء الطلق", "معدات اليوغا والبيلاتيس", "معدات الصيد والصيد",
    "معدات التزلج والتوازن على الدراجة الهوائية والدراجة الهوائية", "معدات الجولف"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishSportsOutdoorsSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishSportsOutdoorsSubgroups[i];
                string arabicType = arabicSportsOutdoorsSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupBeautyPersonalCare()
        {
            string clothingGuid = "d06e4bbb-26a7-45c4-9d0e-3fc5957e4989";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishBeautyPersonalCareSubgroups = new List<string>
{
    "Skin Care", "Hair Care", "Makeup", "Fragrances", "Bath and Body",
    "Personal Care Appliances", "Oral Care", "Men's Grooming", "Beauty Accessories"
};

            List<string> arabicBeautyPersonalCareSubgroups = new List<string>
{
    "العناية بالبشرة", "العناية بالشعر", "مستحضرات التجميل", "العطور", "الاستحمام والجسم",
    "أجهزة العناية الشخصية", "العناية بالفم", "العناية بالرجال", "إكسسوارات الجمال"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishBeautyPersonalCareSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishBeautyPersonalCareSubgroups[i];
                string arabicType = arabicBeautyPersonalCareSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupFurniture()
        {
            string clothingGuid = "c87368a5-a1ed-4917-912b-3200808a0c04";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishFurnitureSubgroups = new List<string>
{
    "Living Room Furniture", "Bedroom Furniture", "Dining Room Furniture", "Kitchen Furniture", "Office Furniture",
    "Outdoor Furniture", "Bathroom Furniture", "Kids' Furniture", "Entryway Furniture", "Storage Furniture",
    "Accent Furniture", "Furniture Accessories", "Home Entertainment Furniture"
};

            List<string> arabicFurnitureSubgroups = new List<string>
{
    "أثاث غرفة المعيشة", "أثاث غرفة النوم", "أثاث غرفة الطعام", "أثاث المطبخ", "أثاث المكتب",
    "أثاث الهواء الطلق", "أثاث الحمام", "أثاث الأطفال", "أثاث المدخل", "أثاث التخزين",
    "أثاث الزخرفة", "إكسسوارات الأثاث", "أثاث ترفيهي منزلي"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishFurnitureSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishFurnitureSubgroups[i];
                string arabicType = arabicFurnitureSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupToysGames()
        {
            string clothingGuid = "dac985c6-73bf-41c0-9344-626a6b16884d";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishToysGamesSubgroups = new List<string>
{
    "Action Figures", "Board Games", "Puzzles", "Dolls", "Educational Toys",
    "Outdoor Play Equipment", "Remote Control Toys", "Arts and Crafts", "Building Blocks",
    "Video Games", "Electronic Toys", "Stuffed Animals", "Ride-On Toys", "Party Supplies"
};

            List<string> arabicToysGamesSubgroups = new List<string>
{
    "تماثيل العمل", "ألعاب اللوحة", "الألغاز", "الدمى", "ألعاب تعليمية",
    "معدات اللعب في الهواء الطلق", "ألعاب التحكم عن بعد", "الفنون والحرف اليدوية", "كتل البناء",
    "ألعاب الفيديو", "ألعاب إلكترونية", "الحيوانات المحشوة", "ألعاب مركبة", "لوازم الحفلات"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishToysGamesSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishToysGamesSubgroups[i];
                string arabicType = arabicToysGamesSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupFoodBeverages()
        {
            string clothingGuid = "47816aae-3164-4cf1-ab63-f7b169f3543d";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishFoodBeveragesSubgroups = new List<string>
{
    "Snacks", "Beverages", "Baking Ingredients", "Canned Foods", "Dairy Products",
    "Frozen Foods", "Grains and Pasta", "Sauces and Condiments", "Sweets and Desserts",
    "Cooking Oils", "Spices and Seasonings", "Tea and Coffee", "Health Foods", "Baby Food"
};

            List<string> arabicFoodBeveragesSubgroups = new List<string>
{
    "الوجبات الخفيفة", "المشروبات", "مكونات الخبز", "الأطعمة المعلبة", "منتجات الألبان",
    "الأطعمة المجمدة", "الحبوب والمعكرونة", "الصلصات والمكملات", "الحلويات والمأكولات الحلوة",
    "زيوت الطهي", "التوابل والبهارات", "الشاي والقهوة", "الأطعمة الصحية", "طعام الأطفال"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishFoodBeveragesSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishFoodBeveragesSubgroups[i];
                string arabicType = arabicFoodBeveragesSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupHealthWellness()
        {
            string clothingGuid = "e2969f69-aca4-43e2-ba03-38e706a368ba";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishHealthWellnessSubgroups = new List<string>
{
    "Vitamins and Supplements", "Personal Care", "Fitness Equipment", "Health Monitors", "Medical Supplies",
    "First Aid", "Natural and Herbal Remedies", "Skin Care", "Oral Care", "Mental Health",
    "Weight Management", "Nutrition", "Hygiene Products", "Health Books", "Wellness Accessories"
};

            List<string> arabicHealthWellnessSubgroups = new List<string>
{
    "الفيتامينات والمكملات", "العناية الشخصية", "معدات اللياقة البدنية", "مراقبة الصحة", "الإمدادات الطبية",
    "الإسعافات الأولية", "العلاجات الطبيعية والعشبية", "العناية بالبشرة", "العناية بالفم", "الصحة النفسية",
    "إدارة الوزن", "التغذية", "منتجات النظافة", "كتب الصحة", "إكسسوارات الرفاهية"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishHealthWellnessSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishHealthWellnessSubgroups[i];
                string arabicType = arabicHealthWellnessSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupAutomotive()
        {
            string clothingGuid = "1320a879-0718-48bb-b725-2e81ca6577c8";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishAutomotiveSubgroups = new List<string>
{
    "Car Parts", "Car Accessories", "Tires and Wheels", "Car Care Products", "Tools and Equipment",
    "Motorcycle Parts", "Motorcycle Accessories", "Truck Parts", "Truck Accessories", "RV and Camper Parts",
    "RV and Camper Accessories", "ATV Parts", "ATV Accessories", "Boat Parts", "Boat Accessories",
    "Marine Electronics", "Automotive Exterior", "Automotive Interior"
};

            List<string> arabicAutomotiveSubgroups = new List<string>
{
    "قطع السيارات", "إكسسوارات السيارات", "إطارات وعجلات", "منتجات العناية بالسيارة", "أدوات ومعدات",
    "قطع الدراجات النارية", "إكسسوارات الدراجات النارية", "قطع الشاحنات", "إكسسوارات الشاحنات", "قطع المركبات الترفيهية والمخيمات",
    "إكسسوارات المركبات الترفيهية والمخيمات", "قطع الدراجات الرباعية", "إكسسوارات الدراجات الرباعية", "قطع القوارب", "إكسسوارات القوارب",
    "إلكترونيات البحرية", "ملحقات السيارة الخارجية", "ملحقات السيارة الداخلية"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishAutomotiveSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishAutomotiveSubgroups[i];
                string arabicType = arabicAutomotiveSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupOfficeSupplies()
        {
            string clothingGuid = "9690d4c7-e402-4de8-85d0-2e825badd129";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishOfficeSuppliesSubgroups = new List<string>
{
    "Writing Instruments", "Paper Products", "Desk Accessories", "Filing and Organization", "Office Electronics",
    "Calendars and Planners", "Binders and Notebooks", "Presentation Supplies", "Office Furniture", "Labels and Labeling",
    "Shipping and Packaging", "Stamps and Stamp Supplies", "Envelopes and Mailers", "Whiteboards and Bulletin Boards",
    "Art Supplies", "Printers and Scanners", "Calculators", "Staplers and Punches"
};

            List<string> arabicOfficeSuppliesSubgroups = new List<string>
{
    "أدوات الكتابة", "منتجات الورق", "إكسسوارات المكتب", "التصنيف والتنظيم", "إلكترونيات المكتب",
    "التقاويم والمخططات", "المجلدات والدفاتر", "مستلزمات العروض", "أثاث المكتب", "العلامات والتسميات",
    "الشحن والتغليف", "الطوابع ولوازم الطوابع", "الظروف والمرسلات", "السبورات البيضاء ولوحات الإعلانات",
    "مستلزمات الفنون", "الطابعات والماسحات الضوئية", "الآلات الحاسبة", "الدبابيس والمثاقب"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishOfficeSuppliesSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishOfficeSuppliesSubgroups[i];
                string arabicType = arabicOfficeSuppliesSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupHomeDecor()
        {
            string clothingGuid = "72d258ef-aab1-406b-8a61-21183ba8c922";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishHomeDecorSubgroups = new List<string>
{
    "Wall Art", "Decorative Accents", "Candles and Candle Holders", "Mirrors", "Clocks",
    "Vases and Centerpieces", "Photo Frames", "Indoor Plants and Planters", "Rugs and Carpets", "Throws and Blankets",
    "Pillows and Cushions", "Curtains and Window Treatments", "Holiday Decor", "Home Fragrances", "Sculptures and Figurines"
};

            List<string> arabicHomeDecorSubgroups = new List<string>
{
    "فن الجدار", "ديكورات زخرفية", "الشموع وحاملات الشموع", "المرايا", "الساعات",
    "الزهور ومراكز الزهور", "إطارات الصور", "النباتات الداخلية وحاملات النباتات", "السجاد والسجاد", "المفارش والبطانيات",
    "الوسائد والوسائد", "الستائر ومعالجات النوافذ", "زينة العطلات", "عطور المنزل", "النحت والتماثيل"
};



            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishHomeDecorSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishHomeDecorSubgroups[i];
                string arabicType = arabicHomeDecorSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupGardenOutdoor()
        {
            string clothingGuid = "633373bf-193e-4ecf-a3ef-8183b0f7546e";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishGardenOutdoorSubgroups = new List<string>
{
    "Outdoor Furniture", "Gardening Tools", "Plants and Seeds", "Lawn Care", "Outdoor Decor",
    "Patio and Decking", "Grills and Outdoor Cooking", "Outdoor Lighting", "Pools and Spas", "Sheds and Storage",
    "Pest Control", "Landscaping Supplies", "Outdoor Power Equipment", "Garden Accessories", "Fencing and Edging"
};

            List<string> arabicGardenOutdoorSubgroups = new List<string>
{
    "أثاث الهواء الطلق", "أدوات الزراعة", "النباتات والبذور", "العناية بالحديقة", "ديكور الهواء الطلق",
    "الفناء والديكور", "أجهزة الشواء والطهي في الهواء الطلق", "إضاءة الهواء الطلق", "حمامات السباحة والجاكوزي", "السقائف والتخزين",
    "مكافحة الآفات", "لوازم تصميم المناظر الطبيعية", "معدات الطاقة الهواء الطلق", "إكسسوارات الحديقة", "السياج والحواف"
};



            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishGardenOutdoorSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishGardenOutdoorSubgroups[i];
                string arabicType = arabicGardenOutdoorSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupJewelryAccessories()
        {
            string clothingGuid = "5330a4e9-3368-4359-9be9-ab2979d21a15";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishJewelryAccessoriesSubgroups = new List<string>
{
    "Necklaces", "Earrings", "Rings", "Bracelets", "Watches",
    "Anklets", "Brooches", "Hair Accessories", "Hats and Caps", "Scarves and Shawls",
    "Gloves and Mittens", "Belts", "Ties and Bowties", "Sunglasses", "Wallets and Purses"
};

            List<string> arabicJewelryAccessoriesSubgroups = new List<string>
{
    "القلائد", "الأقراط", "الخواتم", "الأساور", "الساعات",
    "الأنكليت", "البروشات", "إكسسوارات الشعر", "القبعات والكابات", "الأوشحة والشالات",
    "القفازات والقفازات الصوفية", "الأحزمة", "الربطات وربطات العنق", "النظارات الشمسية", "المحافظ والمحافظ"
};



            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishJewelryAccessoriesSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishJewelryAccessoriesSubgroups[i];
                string arabicType = arabicJewelryAccessoriesSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupBabyKids()
        {
            string clothingGuid = "31cd1243-b9a0-4fc3-bbe8-89c2b5f1fdf2";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishBabyKidsSubgroups = new List<string>
{
    "Baby Clothing", "Baby Gear", "Baby Toys", "Diapers and Wipes", "Nursery Furniture",
    "Kids' Clothing", "Kids' Shoes", "Kids' Toys", "Kids' Accessories", "Baby Feeding",
    "Baby Health and Safety", "Baby Bath and Skincare", "Maternity Clothing", "Baby Bedding",
    "Kids' Bedding", "Baby Travel Gear", "Baby Books and Gifts", "Baby Room Decor"
};

            List<string> arabicBabyKidsSubgroups = new List<string>
{
    "ملابس الأطفال", "مستلزمات الرضع", "ألعاب الأطفال", "حفاضات ومناديل", "أثاث حضانة الأطفال",
    "ملابس الأطفال", "أحذية الأطفال", "ألعاب الأطفال", "إكسسوارات الأطفال", "تغذية الأطفال",
    "صحة وسلامة الرضع", "الاستحمام والعناية بالبشرة للرضع", "ملابس الأمومة", "مفروشات سرير الرضع",
    "مفروشات سرير الأطفال", "مستلزمات السفر للرضع", "كتب وهدايا للرضع", "ديكور غرفة الرضع"
};



            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishBabyKidsSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishBabyKidsSubgroups[i];
                string arabicType = arabicBabyKidsSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupTravelLuggage()
        {
            string clothingGuid = "1c04e7d1-fe7a-40c1-89e7-018ca80616ee";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishTravelLuggageSubgroups = new List<string>
{
    "Luggage", "Backpacks", "Travel Accessories", "Travel Electronics", "Travel Clothing",
    "Travel Toiletries", "Travel Bags", "Travel Organizers", "Travel Pillows and Blankets", "Luggage Tags",
    "Travel Locks", "Travel Adapters", "Travel Guides", "Travel Maps", "Passport Holders"
};

            List<string> arabicTravelLuggageSubgroups = new List<string>
{
    "الأمتعة", "الحقائب الظهر", "مستلزمات السفر", "إلكترونيات السفر", "ملابس السفر",
    "مستلزمات الحمام أثناء السفر", "حقائب السفر", "منظمي السفر", "وسائد وبطانيات السفر", "علامات الأمتعة",
    "أقفال السفر", "محولات السفر", "أدلة السفر", "خرائط السفر", "حاملات جوازات السفر"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishTravelLuggageSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishTravelLuggageSubgroups[i];
                string arabicType = arabicTravelLuggageSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
        private void CreateSubGroupFitnessExercise()
        {
            string clothingGuid = "73f88d46-7425-47ab-9779-91509627d701";
            // Adding clothing types in both languages
            // English Clothing Types
            List<string> englishFitnessExerciseSubgroups = new List<string>
{
    "Cardio Equipment", "Strength Training", "Yoga and Pilates", "Fitness Accessories", "Exercise Clothing",
    "Fitness Technology", "Fitness Supplements", "Home Gym Equipment", "Exercise Mats and Flooring", "Resistance Bands",
    "Fitness Recovery", "Fitness Books and Guides", "Workout DVDs", "Fitness Trackers", "Sports Nutrition"
};

            List<string> arabicFitnessExerciseSubgroups = new List<string>
{
    "معدات القلب والأوعية الدموية", "تدريب القوة", "اليوغا والبيلاتس", "إكسسوارات اللياقة البدنية", "ملابس التمرين",
    "تكنولوجيا اللياقة البدنية", "مكملات اللياقة البدنية", "معدات الصالة الرياضية المنزلية", "بسطات وأرضيات التمرين",
    "شرائط المقاومة", "تجديد اللياقة البدنية", "كتب وأدلة اللياقة البدنية", "أقراص الفيديو التمرينية", "متتبعات اللياقة البدنية", "تغذية رياضية"
};




            // Loop through and add clothing types in both languages
            for (int i = 0; i < englishFitnessExerciseSubgroups.Count; i++)
            {
                string RowKeyVal = Guid.NewGuid().ToString();
                CodeSubGroups.AddEntity<SubGroup>(new SubGroup()
                {
                    Seq = 1,
                    PartitionKey = "1",
                    RowKey = RowKeyVal,
                    GroupRowKey = clothingGuid,
                    ImageURL = "https://portalapps.azurewebsites.net/img/group/Sub/" + RowKeyVal + ".png",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                string englishType = englishFitnessExerciseSubgroups[i];
                string arabicType = arabicFitnessExerciseSubgroups[i]; // Corresponding Arabic translation

                // Adding clothing type in English
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = englishType,
                    LanguageID = "EN",
                    Active = true,
                    Timestamp = DateTime.Now
                });

                // Adding clothing type in Arabic
                CodeSubGroupLanguages.AddEntity<SubGroupLanguage>(new SubGroupLanguage()
                {
                    PartitionKey = "1",
                    RowKey = Guid.NewGuid().ToString(),
                    SubGroupRowKey = RowKeyVal,
                    Name = arabicType,
                    LanguageID = "AR",
                    Active = true,
                    Timestamp = DateTime.Now
                });
            }

        }
      
    }

    public class SubGroupView 
    {
        public SubGroupView()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string GroupRowKey { get; set; }
        public int Seq { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool Active { get; set; }
    }
    public class SubGroup : ITableEntity
    {
        public SubGroup()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string GroupRowKey { get; set; }
        public int Seq { get; set; }
        public string ImageURL { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;


    }
    public class SubGroupLanguage : ITableEntity
    {
        public SubGroupLanguage()
        {
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string SubGroupRowKey { get; set; }
        public string Name { get; set; }
        public string LanguageID { get; set; }
        public bool Active { get; set; }
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;
    }

}
