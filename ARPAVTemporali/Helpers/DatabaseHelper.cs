using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ARPAVTemporali.Models;
using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Diagnostics;
using ARPAVTemporali.Persistance;

namespace ARPAVTemporali.Helpers
{
    public static class DatabaseHelper
    {
        private static SQLiteAsyncConnection Connection
        {
            get
            {
				//var app = Application.Current as App;
                //return app.DatabaseConnection;
                var db_path = GetLocalDBPath(Variables.DatabaseName);
                return new SQLiteAsyncConnection(db_path);
            }
        }

        public static async Task CheckUpdates()
        {
            var version = await GetDBVersion();
            bool overwrite = false;
            if (!(version).Equals(Variables.db_version)) 
            {
                // versions are different so overwrite the db
                overwrite = true;
				Debug.WriteLine("there is a new db version; overwriting");
            }
            else
            {
                Debug.WriteLine("the db is updated; no overwrite required");
            }
            var db_path = GetLocalDBPath(Variables.DatabaseName, overwrite); //overwrite db
        }

        private static string GetLocalDBPath(string dbName, bool overwrite = false)
        {
            IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
            var dbPath = fileHelper.GetLocalFilePath(dbName);
            fileHelper.CopyDatabaseIfNotExists(dbPath, overwrite);

            return dbPath;
        }

        /*
         * crea la tabella delle opzioni e inserisce una versione di default
         */
        private static async Task CreateOptionsTable()
        {
            await Connection
                    .CreateTableAsync<Option>()
                    .ContinueWith((results) =>
                    {
                        Debug.WriteLine("Table options created!");
                    });
            // add a dummy version for comparison
            var version = new Option { Key = "version", Value = "0" };
            await Connection.InsertAsync(version);
        }

        /*
         * ottiene la versione del database
         * crea la tabella opzioni se non esiste
         */
        private static async Task<string> GetDBVersion()
        {
            try
            {
                int total_options = await Connection.Table<Option>().CountAsync(); //check if the options table exists
                Debug.WriteLine("Options table exists");
            }
            catch (Exception ex)
            {
                //table does not exists: creating one to perform version comparison
                Debug.WriteLine("errore:" + ex.Message);
                await CreateOptionsTable();
            }
            Option version = await Connection.Table<Option>().Where(o => o.Key.Equals("version")).FirstAsync();

            return version.Value;
        }

        public async static Task<List<Comune>> GetComuni()
        {
            var results = await Connection.Table<Comune>()
                            .OrderBy(comune => comune.Provincia)
                            .OrderBy(comune => comune.Name)
                            .ToListAsync();
            //List<Comune> comuni = await Connection.QueryAsync<Comune>("SELECT * FROM Comuni ORDER BY Provincia ASC, Denominazione ASC");
            return results;
        }

        public async static Task<List<Notification>> GetNotifiche()
        {
            var results = await Connection.Table<Notification>()
                            .OrderByDescending(x => x.Data)
                            .Take(Variables.MaxNotificationNumber)
                            .ToListAsync();
            return results;
        }

        public static async Task RimuoviVecchieNotifiche()
        {

            List<Notification> notifications = await Connection.Table<Notification>()
                                                        .OrderByDescending(x => x.Data)
                                                        .Skip(Variables.MaxNotificationNumber)
                                                        .ToListAsync();
            var app = Application.Current as App;
            foreach (var notification in notifications)
            {
                await app.NotificationManager.DeleteNotificationAsync(notification.Key);
                await Connection.DeleteAsync(notification);
            }

        }

        public static async Task<int> GetTotalUnreadNotification()
        {
            return await Connection.Table<Notification>()
                                   .Where(n => n.IsRead == false).CountAsync();
        }

        /*
         * inserisci un oggetto nel database
         */
        public static async Task Insert<T>(T item)
        {
            await Connection.InsertAsync(item);
        }

        /*
         * aggiorna un oggetto nel database
         */
        public static async Task Update<T>(T item)
        {
            await Connection.UpdateAsync(item);
        }

        /*
         * elimina un oggetto nel database
         */
        public static async Task Delete<T>(T item)
        {
            await Connection.DeleteAsync(item);
        }

        public async static Task<List<Testo>> GetTesti()
        {
            string query = "SELECT parent.Id," +
                "parent.Ordine, " +
                "parent.Slug, " +
                "parent.Tipo, " +
                "IFNULL(translation.Titolo, def.Titolo) Titolo," +
                "IFNULL(translation.Descrizione, def.Descrizione) Descrizione " +
                "FROM TESTO as parent " +
                "LEFT OUTER JOIN TestoTranslation as translation " + //join requested translation, if available:
                "ON parent.Id = translation.Parent_Id " +
                "AND translation.Language=? " +
                "LEFT OUTER JOIN TestoTranslation as def " + //join default language of the item
                "ON parent.Id = def.Parent_Id " +
                "AND def.IsDefaultLanguage = 1 " +
                "WHERE parent.Tipo == 'info' " + //seleziona solo le info 
                "ORDER BY parent.Ordine ASC";
            string language = Helpers.Settings.Language;
            var results = await Connection.QueryAsync<Testo>(query, language);

            return results;
        }

        public async static Task<Testo> GetTestoBySlug(string slug)
        {
            
            // https://stackoverflow.com/a/27474681/1875109
            string query = "SELECT parent.Id," +
                "parent.Ordine, " +
                "parent.Slug, " +
                "parent.Tipo, " +
                "IFNULL(translation.Titolo, def.Titolo) Titolo," +
                "IFNULL(translation.Descrizione, def.Descrizione) Descrizione " +
                "FROM TESTO as parent " +
                "LEFT OUTER JOIN TestoTranslation as translation " + //join requested translation, if available:
                "ON parent.Id = translation.Parent_Id " +
                "AND translation.Language=? " +
                "LEFT OUTER JOIN TestoTranslation as def " + //join default language of the item
                "ON parent.Id = def.Parent_Id " +
                "AND def.IsDefaultLanguage = 1 " +
                "WHERE parent.Slug == ?";
            string language = Helpers.Settings.Language;

            var results = await Connection.QueryAsync<Testo>(query, language, slug);
            return results.FirstOrDefault<Testo>();
        }



    }
}
