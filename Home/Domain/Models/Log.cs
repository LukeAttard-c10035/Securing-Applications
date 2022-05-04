using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
