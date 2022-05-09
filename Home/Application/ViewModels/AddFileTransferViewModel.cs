using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class AddFileTransferViewModel
    {
        public string FilePath { get; set; }
        public string UserEmail { get; set; }
        public string AuthorizedUsers { get; set; }
        [DataType(DataType.Date)]
        [FutureDateValidator(ErrorMessage = "Date is in the past.")]
        public DateTime? ExpiryDate { get; set; }
        public string DigitalSignature { get; set; }
        public string FileName { get; set; }
    }
}
