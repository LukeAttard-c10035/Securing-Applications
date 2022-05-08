using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class CustomUser : IdentityUser
    {
        public string privateKey { get; set; }
        public string publicKey { get; set; }
    }
}
