using Domain.Models;
using System;

namespace Application.ViewModels
{
    public class AddFileTransferViewModel
    {
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string Email { get; set; }
        [FutureDateValidator(ErrorMessage = "Date is in the past.")]
        public DateTime? ExpiryDate { get; set; }
    }
}
