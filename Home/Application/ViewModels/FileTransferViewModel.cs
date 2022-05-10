using Domain.Models;
using System;

namespace Application.ViewModels
{
    public class FileTransferViewModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string AuthorizedUsers { get; set; }
        public string FilePath { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool isExpired { get; set; }
        public string DigitalSignature { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
