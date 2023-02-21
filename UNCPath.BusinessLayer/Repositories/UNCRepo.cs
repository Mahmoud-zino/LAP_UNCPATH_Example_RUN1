using Microsoft.EntityFrameworkCore;
using UNCPath.AccessLayer.DataAccess;
using UNCPath.AccessLayer.Models;
using UNCPath.BusinessLayer.Logic;

namespace UNCPath.BusinessLayer.Repositories
{
    public class UNCRepo
    {
        private readonly UNCDbContext _dbContext = new();
        private readonly UNCScanner _scanner = new();

        public async Task ScanAndSavePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(nameof(path));

            Folder folder = _scanner.ScanPath(path);

            await _dbContext.Folders.AddAsync(folder);

            await _dbContext.SaveChangesAsync();
        }


        public async Task<List<Folder>> GetFolders()
        {
            return await _dbContext.Folders
                .Include(f => f.Folders)
                .Include(f => f.Files)
                .ToListAsync();
        }

        public string GetTopMonth(Folder folder)
        {
            return _scanner.GetTopMonth(folder);
        }

        public List<Folder> GetTop10FoldersAfterSize(Folder folder)
        {
            List<Folder> allFolders = new();
            foreach (Folder subFolder in folder.Folders)
            {
                allFolders.Add(subFolder);
            }
            return folder.Folders.OrderByDescending(f => f.Size).Take(10).ToList();
        }
    }
}
