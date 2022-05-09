using Application.Interfaces;
using Application.ViewModels;
using Domain.Interfaces;
using System.Collections.Generic;
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
                FilePath = model.FilePath,
                UserEmail = model.UserEmail,
                AuthorizedUsers = model.AuthorizedUsers,
                ExpiryDate = model.ExpiryDate,
                isExpired = false,
                DigitalSignature = model.DigitalSignature,
                FileName = model.FileName,
            }); ;
        }

        public FileTransferViewModel GetFileTransfer(int id)
        {
            var ft = ftRepo.GetFileTransfer(id);
            FileTransferViewModel model = new FileTransferViewModel()
            {
                Id = ft.Id,
                FilePath = ft.FilePath,
                AuthorizedUsers = ft.AuthorizedUsers,
                UserEmail = ft.UserEmail,
                ExpiryDate = ft.ExpiryDate,
                isExpired =  ft.isExpired,
                DigitalSignature = ft.DigitalSignature,
                FileName = ft.FileName,
            };
            return model;
        }

        public IQueryable<FileTransferViewModel> GetFileTransfers(string username, string web)
        {
            var list = from ft in ftRepo.GetFileTransfers()
                       where (ft.UserEmail == username)
                       orderby ft.Id descending
                       select new FileTransferViewModel()
                       {
                           Id = ft.Id,
                           //FilePath = $"https://{web}/{ft.FilePath}",
                           FilePath = $"https://{web}/{ft.FilePath}",
                           AuthorizedUsers = ft.AuthorizedUsers,
                           UserEmail = ft.UserEmail,
                           ExpiryDate = ft.ExpiryDate,
                           isExpired = ft.isExpired,
                           DigitalSignature = ft.DigitalSignature,
                           FileName = ft.FileName,
                       };
            return list;
        }
    }
}
