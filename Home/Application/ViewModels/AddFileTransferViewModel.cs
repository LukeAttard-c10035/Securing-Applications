using Domain.Models;
using System;

namespace Application.ViewModels
{
    public class AddFileTransferViewModel
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string AuthorizedUsers { get; set; }
        [FutureDateValidator(ErrorMessage = "Date is in the past.")]
        public DateTime? ExpiryDate { get; set; }
    }
}
