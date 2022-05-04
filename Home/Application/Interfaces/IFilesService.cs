using Application.ViewModels;
using System.Linq;

namespace Application.Interfaces
{
    public interface IFilesService
    {
        public IQueryable<FileTransferViewModel> GetFileTransfers(string username, string web);
        public FileTransferViewModel GetFileTransfer(int id);
        public void AddFileTransfer(AddFileTransferViewModel model);
    }
}
