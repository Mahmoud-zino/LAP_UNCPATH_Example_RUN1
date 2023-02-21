using UNCPath.AccessLayer.Models;
using UNCPath.BusinessLayer.ViewModels;

namespace UNCPath.BusinessLayer.Logic
{
    internal class UNCScanner
    {
        public Folder ScanPath(string path)
        {
            DirectoryInfo folderInfo = new(path);
            Folder mainFolder = new() 
            {
                Path = path,
                Name = folderInfo.Name,
                CreationDate = folderInfo.CreationTime,
                RootFolder = true
            };

            GetFolderDetails(mainFolder);

            mainFolder.Size = GetFolderSize(mainFolder);
            return mainFolder;
        }

        private void GetFolderDetails(Folder folder)
        {
            RegisterFiles(folder);

            string[] directories = Directory.GetDirectories(folder.Path, "", SearchOption.TopDirectoryOnly);
            foreach (string directory in directories)
            {
                DirectoryInfo directoryInfo = new(directory);
                Folder subFolder = new()
                { 
                    Path = directory,
                    Name = directoryInfo.Name,
                    CreationDate = directoryInfo.CreationTime
                };
                GetFolderDetails(subFolder);

                subFolder.Size = GetFolderSize(subFolder);

                folder.Folders.Add(subFolder);
            }
        }

        private static void RegisterFiles(Folder folder)
        {
            string[] filePaths = Directory.GetFiles(folder.Path, "", SearchOption.TopDirectoryOnly);

            foreach (string filePath in filePaths)
            {
                FileInfo fileInfo = new(filePath);
                folder.Files.Add(new AccessLayer.Models.File()
                {
                    Path = filePath,
                    Name = fileInfo.Name,
                    Size = fileInfo.Length,
                    CreationDate = fileInfo.CreationTime,
                });
            }
        }

        private static long GetFolderSize(Folder folder)
        {
            return folder.Folders.Sum(f => f.Size) + folder.Files.Sum(f => f.Size);
        }

        public string GetTopMonth(Folder folder)
        {
            List<UNCMonthDetails> monthsDetails = new();

            GetUNCMonthDetailsFromFolder(monthsDetails, folder);

            //order months by size then by file number and return the name of the first month
            return monthsDetails.OrderByDescending(m => m.FilesSize).OrderByDescending(m => m.NumberOfFilesCreated).First().Name;
        }

        private void GetUNCMonthDetailsFromFolder(List<UNCMonthDetails> monthsDetails, Folder folder)
        {
            foreach (Folder subFolder in folder.Folders)
            {
                GetUNCMonthDetailsFromFolder(monthsDetails, subFolder);
            }

            foreach (AccessLayer.Models.File file in folder.Files)
            {
                UNCMonthDetails? monthDetails = monthsDetails.FirstOrDefault(m => m.Month == file.CreationDate.Month);
                if (monthDetails == null)
                {
                    monthsDetails.Add(new UNCMonthDetails()
                    {
                        Month = file.CreationDate.Month,
                        Name = file.CreationDate.ToString("MMMM"),
                        NumberOfFilesCreated = 1,
                        FilesSize = file.Size
                    });
                }
                else
                {
                    monthDetails.NumberOfFilesCreated++;
                    monthDetails.FilesSize += file.Size;
                }
            }
            
        }
    }
}
