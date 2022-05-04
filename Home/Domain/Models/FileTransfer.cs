using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FileTransfer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Message { get; set; } //not required
        public string Password { get; set; } // not requited

        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FilePath { get; set; }
    }
}
