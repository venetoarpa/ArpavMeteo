using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ARPAVTemporali.iOS.Persistance;
using ARPAVTemporali.Persistance;
using Foundation;
using System.Diagnostics;

[assembly: Dependency(typeof(FileHelper))]
namespace ARPAVTemporali.iOS.Persistance
{
    public class FileHelper : IFileHelper
    {
        public FileHelper()
        {
        }

        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }

		public void CopyDatabaseIfNotExists(string dbPath, bool overwrite = false)
		{
            /*if (File.Exists(dbPath) &&  overwrite == true)
            {
                Debug.WriteLine("deleting");
                File.Delete(dbPath);
            }*/
			if (overwrite || !File.Exists(dbPath))
			{
                Debug.WriteLine(overwrite ? "overwriting" : "not overwriting");
				var dbFileName = Path.GetFileName(dbPath); //get the filename
				var appdir = NSBundle.MainBundle.BundlePath; //get the path to the Resources folder
				string seedFile = Path.Combine(appdir, dbFileName); //get full path to db in Resource folder
				File.Copy(seedFile, dbPath, overwrite); //copy to environment folder of the iOS app
			}
		}
    }
}
