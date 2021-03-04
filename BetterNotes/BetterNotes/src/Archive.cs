using System;
using System.IO;
using System.IO.Compression;

namespace BetterNotes {
    public static class Archive {
        public static void ArchiveFile(string folder, string savePath)
        {
            if (File.Exists(savePath))
            {
                if (OverwriteFile()) File.Delete(savePath);
            }
            ZipFile.CreateFromDirectory(folder, savePath);
        }

        public static void UnarchiveFile(string archivePath, string extractPath)
        {
            if (Directory.Exists(extractPath))
            {
                if (ErrorFolderExists(extractPath)) Directory.Delete(extractPath, true);
            }
            ZipFile.ExtractToDirectory(archivePath, extractPath);
        }

        public static bool OverwriteFile()
        {
            bool overwrite = false;
            //TODO: CALL A UI ELEMENT ASKING THE USER IF THEY WANT TO OVERWRITE THE CURRENT FILE
            return overwrite;
        }

        public static bool ErrorFolderExists(string notePath)
        {
            bool errorCorrected = false;
            //TODO: CHECK IF A NOTE OBJECT WITH A PATH MATCHING THE BNOT FILE IS OPEN IN EDITOR (if no, errorCorrected = true)
            //TODO: CALL A UI ELEMENT TELLING THE USER THE FOLDER EXISTS, ALLOWS USER TO CONTINUE ANYWAY TO OVERWRITE THE CURRENT ACTIVE FOLDER, OR SET A NEW NAME FOR THE NOTE
            return errorCorrected;
        }
     
    }
}