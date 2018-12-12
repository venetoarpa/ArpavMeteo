using System;
using System.Diagnostics;
using System.IO;
using ARPAVTemporali.Droid.Persistance;
using ARPAVTemporali.Persistance;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace ARPAVTemporali.Droid.Persistance
{
    public class FileHelper: IFileHelper
    {
        public FileHelper()
        {
        }

		/*
         * il DB deve essere nella cartella assets e la Build Action deve essere AndroidAsset
         */
		public string GetLocalFilePath(string filename)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			return Path.Combine(path, filename);
		}

        public void CopyDatabaseIfNotExists(string dbPath, bool overwrite)
        {
            if (overwrite || !File.Exists(dbPath))
            {
                var dbName = Path.GetFileName(dbPath);

                FileStream writeStream = new FileStream(dbPath, FileMode.OpenOrCreate, FileAccess.Write);
				Debug.WriteLine("Copying Database " + dbPath);
				Forms.Context.Assets.Open(dbName).CopyToAsync(writeStream);
			}
        }
    }
}
