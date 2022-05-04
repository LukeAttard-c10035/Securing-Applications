using Data.Context;
using Domain.Interfaces;
using Domain.Models;
using System.Linq;

namespace Data.Repositories
{
    public class LogInDbRepository : ILogRepository
    {
        private FileTransferContext context;
        public LogInDbRepository(FileTransferContext _fileTransferContext)
        {
            context = _fileTransferContext;
        }

        public void AddLog(Log log)
        {
            context.Log.Add(log);
            context.SaveChanges();
        }
    }
}
