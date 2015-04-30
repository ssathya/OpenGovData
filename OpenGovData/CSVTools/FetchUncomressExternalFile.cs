using Anotar.NLog;
using SharpCompress.Common;
using SharpCompress.Reader;
using System;
using System.IO;
using System.Net;

namespace CSVTools
{
    public class FetchUncomressExternalFile
    {
        public FetchUncomressExternalFile()
        {
        }

        public bool GetExternalFile(string fileUrl, string destinationPath, string institutionsFile)
        {
            if (CheckCreateDestinationPath(destinationPath) == false)
                return false;
            if (NewFileExists(destinationPath, institutionsFile))
                return true;
            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.DownloadFile(fileUrl, institutionsFile);
                }
                catch (Exception exception)
                {
                    LogTo.Error("Could not get file from external site; application terminating");
                    return false;
                }
            }
            return true;
        }

        private static bool NewFileExists(string destinationPath, string institutionsFile)
        {
            if (CheckCreateDestinationPath(destinationPath) == false)
                return false;
            if (File.Exists(institutionsFile))
            {
                var lastSunday = GetLastSunday();
                var creationTime = File.GetCreationTime(institutionsFile);
                return creationTime > lastSunday;
            }
            return false;
        }

        private static DateTime GetLastSunday()
        {
            var lastSunday = DateTime.Now;
            lastSunday = lastSunday.AddDays(-(int)lastSunday.DayOfWeek);
            return lastSunday;
        }

        /// <summary>
        /// Checks and if not create destination path.
        /// </summary>
        /// <returns></returns>
        private static bool CheckCreateDestinationPath(string destinationPath)
        {
            if (!Directory.Exists(destinationPath))
            {
                try
                {
                    Directory.CreateDirectory("App_Data");
                }
                catch (Exception exception)
                {
                    LogTo.Error("Could not create App_Data folder: Details:{0}", exception.Message);
                    return false;
                }
            }
            return true;
        }

        public bool UncompressExternalFile(string dataFile, string destinationPath)
        {
            try
            {
                using (Stream stream = File.OpenRead(dataFile))
                {
                    var reader = ReaderFactory.Open(stream);
                    while (reader.MoveToNextEntry())
                    {
                        reader.WriteEntryToDirectory(destinationPath, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
            catch (Exception exception)
            {
                LogTo.Error("Could not extract files; Details:{0}", exception.Message);
                return false;
            }
            return true;
        }
    }
}