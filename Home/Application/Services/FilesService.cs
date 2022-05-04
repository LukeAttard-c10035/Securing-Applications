using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using System.Linq;

namespace Application.Services
{
    public class FilesService : IFilesService
    {
        private IFilesTransferRepository ftRepo;
        public FilesService(IFilesTransferRepository _ftRepo)
        {
            ftRepo = _ftRepo;
        }
        public void AddFileTransfer(AddFileTransferViewModel model)
        {
            ftRepo.AddFileTransfer(new Domain.Models.FileTransfer()
            {
                Title = model.Title,
                FilePath = model.FilePath,
                Message = model.Message,
                Password = model.Password,
                UserEmail = model.UserEmail,
                Email = model.Email
            });
        }

        public FileTransferViewModel GetFileTransfer(int id)
        {
            var ft = ftRepo.GetFileTransfer(id);
            FileTransferViewModel model = new FileTransferViewModel()
            {
                Id = ft.Id,
                Title = ft.Title,
                FilePath = ft.FilePath,
                Password = ft.Password,
                Email = ft.Email,
                UserEmail = ft.UserEmail,
                Message = ft.Message,
            };
            return model;
        }

        public IQueryable<FileTransferViewModel> GetFileTransfers(string username, string web)
        {
            var list = from ft in ftRepo.GetFileTransfers()
                       where (ft.UserEmail == username) || (ft.Email == username)
                       orderby ft.Id descending
                       select new FileTransferViewModel()
                       {
                           Id = ft.Id,
                           Title = ft.Title,
                           //FilePath = $"https://{web}/{ft.FilePath}",
                           FilePath = $"http://{web}/{ft.FilePath}",
                           Password = ft.Password,
                           Email = ft.Email,
                           UserEmail = ft.UserEmail,
                           Message = ft.Message,
                       };
            return list;
        }
    }
}
