﻿using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvaETicaret.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostaKodu { get; set; }
        [NotMapped]
        public string Role { get; set; }
        public Nullable<int> BranchId { get; set; }
      
        public ICollection<Adress>? Adresses { get; set; }


    }
}
