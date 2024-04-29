using FTPBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPBuddy.Data
{
    public class FtpServerDatabase
    {
        SQLiteAsyncConnection Database;
        
        public FtpServerDatabase()
        {
            Database = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "ftpbuddy.db"));
            Database.CreateTableAsync<FtpSavedServer>();
        }

        public async Task SaveFtpServer(FtpSavedServer server)
        {
            await Database.InsertAsync(server);
        }

        public async Task DeleteFtpServer(FtpSavedServer server)
        {
            await Database.DeleteAsync(server);
        }

        public async Task<List<FtpSavedServer>> GetAllFtpServers()
        {
            return await Database.Table<FtpSavedServer>().ToListAsync();
        }
    }
}
