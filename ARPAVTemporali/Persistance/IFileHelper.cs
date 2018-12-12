using System;
using System.IO;

namespace ARPAVTemporali.Persistance
{
    public interface IFileHelper
    {
		string GetLocalFilePath(string filename);

		void CopyDatabaseIfNotExists(string dbPath, bool overwrite = false);

    }
}