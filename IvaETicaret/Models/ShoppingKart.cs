﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvaETicaret.Models
{
    public class ShoppingKart
    {
        public ShoppingKart()
        {
            Count= 1;
        }
        [Key]
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? ApplicationUser { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        //public int BranchId { get; set; }
        //[ForeignKey("BranchId")]
        //public Branch Branch { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public double Price { get; set; }

    }
}
