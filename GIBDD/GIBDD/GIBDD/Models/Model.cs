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
using Xamarin.Forms;

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
        public event EventHandler OnChangeRecord;

        private void OnChangeRecordReaction()
        {
            OnChangeRecord?.Invoke(this, EventArgs.Empty);
        }

        public DatabaseData() 
        {
            DbInit();
        }

        private void DbInit()
        {
            db = new SQLiteAsyncConnection(dbPath);
            db?.CreateTableAsync<ProfilesTable>();
        }

        public async void AddRecord(ProfilesTable profile)
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
            OnChangeRecordReaction();
        }

        public async void DeleteRecord(int id)
        {
            if (db == null)
                DbInit();
            await db?.DeleteAsync<ProfilesTable>(id);
            OnChangeRecordReaction();
        }

        public async void UpdateRecord(ProfilesTable profile)
        {
            if (db == null)
                DbInit();
            await db?.UpdateAsync(profile);
            OnChangeRecordReaction();
        }

        public async Task<ProfilesTable> GetRecord(int id)
        {
            if (db == null)
                DbInit();
            var response = await db?.GetAsync<ProfilesTable>(id);
            //var response = await db?.GetAsync<ProfilesTable>(id);
            return response;
        }

        public async Task<List<ProfilesTable>> GetAllRecords()
        {
            if (db == null)
                DbInit();
            var response = await db?.QueryAsync<ProfilesTable>("SELECT * FROM Profiles");
            return response;
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
