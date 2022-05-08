using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FileTransfer
    {
        [Key]
        public int Id { get; set; }

        public string Password { get; set; } // not requited

        [Required]
        public string UserEmail { get; set; }
        
        public string AuthorizedUsers { get; set; }
        [Required]
        public string FilePath { get; set; }

        [FutureDateValidator(ErrorMessage = "Date is in the past.")]
        public DateTime? ExpiryDate { get; set; }
        [Required]
        public bool isExpired { get; set; }
    }
}
