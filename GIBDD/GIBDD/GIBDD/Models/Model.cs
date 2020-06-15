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
using System.Diagnostics;

namespace GIBDD
{
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
        [MaxLength(400), NotNull]
        public string OrgOptionalInformation { get; set; } = "";
        [MaxLength(40), NotNull]
        public string OutNumber { get; set; } = "";
        public DateTime RegistrOrgDate { get; set; }
        [MaxLength(40), NotNull]
        public string NumberLetter { get; set; } = "";
    }

    public class AppealsTable
    {
        [PrimaryKey, AutoIncrement, Unique, NotNull]
        public int Id { get; set; }

        [NotNull]
        public string AppealText { get; set; }
        
    }

    public class DatabaseData 
    {

        static private string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db");
        private SQLiteAsyncConnection db;

        public DatabaseData() 
        {
            DbInit();
        }

        private void DbInit()
        {
            if (db == null)
                db = new SQLiteAsyncConnection(dbPath);
            db?.CreateTableAsync<ProfilesTable>();
            db?.CreateTableAsync<AppealsTable>();
        }


        //APPEALS TABLE

        //public event EventHandler OnChangeAppealsTableRecord;

        //private void OnChangeRecordAppealsTableReaction()
        //{
        //    OnChangeAppealsTableRecord?.Invoke(this, EventArgs.Empty);
        //}

        public async Task<List<AppealsTable>> GetAllRecordFromApppealsTable()
        {
            List<AppealsTable> response;
            try
            {
                response = await db?.QueryAsync<AppealsTable>("SELECT * FROM AppealsTable ORDER BY Id DESC");
                
            }
            catch (SQLite.SQLiteException e)
            {
                response = new List<AppealsTable>();
                Debug.WriteLine(e);
            }
            return response;
        }


        public async void AddRecordToAppealsTable(AppealsTable appeal)
        {
            try
            {
                await db?.InsertAsync(appeal);
                //OnChangeRecordAppealsTableReaction();
            }
            catch (SQLite.SQLiteException e)
            {
                Debug.WriteLine(e);
            }
        }


        // PROFILES TABLE
        public event EventHandler OnChangeProfilesTableRecord;

        private void OnChangeRecordProfilesTableReaction()
        {
            OnChangeProfilesTableRecord?.Invoke(this, EventArgs.Empty);
        }


        public async void AddRecordToProfilesTable(ProfilesTable profile)
        {
            try
            {
                await db?.InsertAsync(profile);
                OnChangeRecordProfilesTableReaction();
            }
            catch (SQLite.SQLiteException e)
            {
                Debug.WriteLine(e);
            }
        }

        public async void DeleteRecordFromProfilesTable(int id)
        {
            try
            {
                await db?.DeleteAsync<ProfilesTable>(id);
                OnChangeRecordProfilesTableReaction();
            }
            catch (SQLite.SQLiteException e)
            {
                Debug.WriteLine(e);
            }
        }

        public async void UpdateRecord(ProfilesTable profile)
        {
            try
            {
                await db?.UpdateAsync(profile);
                OnChangeRecordProfilesTableReaction();
            }
            catch (SQLite.SQLiteException e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task<ProfilesTable> GetRecordFromProfilesTable(int id)
        {
            ProfilesTable response;
            try
            {
                response = await db?.GetAsync<ProfilesTable>(id);
            }
            catch (SQLite.SQLiteException e)
            {
                response = new ProfilesTable();
                Debug.WriteLine(e);
            }
            return response;
        }

        public async Task<List<ProfilesTable>> GetAllRecordsFromProfilesTable()
        {
            List<ProfilesTable> response;
            try
            {
                response = await db?.QueryAsync<ProfilesTable>("SELECT * FROM ProfilesTable ORDER BY Id DESC");
            }
            catch (SQLite.SQLiteException e)
            {
                response = new List<ProfilesTable>();
                Debug.WriteLine(e);
            }
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
