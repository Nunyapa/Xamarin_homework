using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SQLite;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace GIBDD
{
    [Table("Profiles")]
    public class ProfilesTable
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        [NotNull]
        public bool TypeOfProfile { get; set; } // 1 - human, 0 - organization
        [MaxLength(40), NotNull]
        public string Sername { get; set; }
        [MaxLength(40), NotNull]
        public string Name { get; set; }
        [MaxLength(40), NotNull]
        public string MiddleName { get; set; } = "";
        [MaxLength(60), NotNull]
        public string Email { get; set; } = "";
        [MaxLength(15), NotNull]
        public string Phone { get; set; } = "";
        [MaxLength(255), NotNull]
        public string SelectedRegion { get; set; }
        [MaxLength(255), NotNull]
        public string SelectedDiv { get; set; }
        [MaxLength(255), NotNull]
        public string SelectedRegionOfIncident { get; set; }
        [MaxLength(255), NotNull]
        public string OrgName { get; set; } = "";
        [MaxLength(255), NotNull]
        public string OrgOptionalInformation { get; set; } = "";
        [MaxLength(40), NotNull]
        public string OutNumber { get; set; } = "";
        public DateTime RegistrOrgDate { get; set; }
        [MaxLength(40), NotNull]
        public string NumberLetter { get; set; } = "";
    }

    public class DatabaseData 
    {
        static private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
        private SQLiteAsyncConnection db;

        public DatabaseData() 
        {
            this.db = new SQLiteAsyncConnection(dbPath);
            db?.CreateTableAsync<ProfilesTable>();
        }

        public async void AddRecord(Profile profile)
        {
            var newRecord = new ProfilesTable
            {
                TypeOfProfile = profile.TypeOfProfile,
                Sername = profile.Sername,
                Name = profile.Name,
                MiddleName = profile.MiddleName,
                Email = profile.Email,
                Phone = profile.Phone,
                SelectedRegion = profile.SelectedRegion,
                SelectedDiv = profile.SelectedDiv,
                SelectedRegionOfIncident = profile.SelectedRegionOfIncident,
                OrgName = profile.OrgName,
                OrgOptionalInformation = profile.OrgOptionalInformation,
                OutNumber = profile.OrgName,
                RegistrOrgDate = profile.RegistrOrgDate,
                NumberLetter = profile.NumberLetter
            };
            await db?.InsertAsync(newRecord);
        }

        public async void DeleteRecord(int id)
        {
            await db?.DeleteAsync<ProfilesTable>(id);
        }

        public async void UpadateRecord(Profile profile, int id)
        {
            var record = new ProfilesTable
            {
                Id = id,
                TypeOfProfile = profile.TypeOfProfile,
                Sername = profile.Sername,
                Name = profile.Name,
                MiddleName = profile.MiddleName,
                Email = profile.Email,
                Phone = profile.Phone,
                SelectedRegion = profile.SelectedRegion,
                SelectedDiv = profile.SelectedDiv,
                SelectedRegionOfIncident = profile.SelectedRegionOfIncident
            };
            await db?.UpdateAsync(record);
        }

        public async Task<ProfilesTable> GetRecord(int id)
        {
            var response = await db?.GetAsync<ProfilesTable>(id);
            return response;
        }

        public async Task<List<ProfilesTable>> GetAllRecords()
        {
            var list = await db?.QueryAsync<ProfilesTable>("SELECT * FROM Profiles");
            return list;
        }

        static private Profile toProfile (ProfilesTable record)
        {
            var profile = new Profile 
            {
                
                TypeOfProfile = record.TypeOfProfile,
                Sername = record.Sername,
                Name = record.Name,
                MiddleName = record.MiddleName,
                Email = record.Email,
                Phone = record.Phone,
                SelectedRegion = record.SelectedRegion,
                SelectedDiv = record.SelectedDiv,
                SelectedRegionOfIncident = record.SelectedRegionOfIncident,
                OrgName = record.OrgName,
                OrgOptionalInformation = record.OrgOptionalInformation,
                OutNumber = record.OrgName,
                RegistrOrgDate = record.RegistrOrgDate,
                NumberLetter = record.NumberLetter
            };
            return profile;
        }


    }


    public class FilesData 
    {
        public Dictionary<int, string> Regions { get { return RegionsDataGetter(); } }
        public Dictionary<int, List<string>> Divisions { get { return DivisionsDataGetter(); } }

        private Dictionary<int, string> RegionsDataGetter()
        {
            Dictionary<int, string> reg = new Dictionary<int, string>();
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(SubsectionPickerModelView));
            Stream stream = assembly.Assembly.GetManifestResourceStream("GIBDD.GIBDDResources.index_value.json");
            using (var reader = new System.IO.StreamReader(stream, Encoding.Default))
            {
                var data = reader.ReadToEnd();
                var jsonObj = JObject.Parse(data);
                List<string> listOfKeys = jsonObj.Properties().Select(p => p.Name).ToList();
                foreach (var i in listOfKeys)
                {
                    reg.Add(int.Parse(i), jsonObj[i].ToString());
                }
            }
            return reg;
        }

        private Dictionary<int, List<string>> DivisionsDataGetter()
        {
            Dictionary<int, List<string>> divisions = new Dictionary<int, List<string>>();
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(SubsectionPickerModelView));
            Stream stream = assembly.Assembly.GetManifestResourceStream("GIBDD.GIBDDResources.region_number_to_divisions.json");
            using (var reader = new System.IO.StreamReader(stream, Encoding.Default))
            {
                var data = reader.ReadToEnd();
                var jsonObj = JObject.Parse(data);
                List<string> listOfKeys = jsonObj.Properties().Select(p => p.Name).ToList();
                foreach (var i in listOfKeys)
                {
                    JArray temp = (JArray)jsonObj[i];
                    List<string> valueList = temp.ToObject<List<string>>();
                    divisions.Add(int.Parse(i), valueList);
                }
            }
            return divisions;
        }

    }
}
