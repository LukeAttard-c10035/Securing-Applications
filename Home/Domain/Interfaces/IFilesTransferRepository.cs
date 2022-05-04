using Domain.Models;
using System.Linq;

namespace Domain.Interfaces
{
    public interface IFilesTransferRepository
    {
        public void AddFileTransfer(FileTransfer file);
        public FileTransfer GetFileTransfer(int id);
        public IQueryable<FileTransfer> GetFileTransfers();
    }
}
