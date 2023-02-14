using System.ComponentModel.DataAnnotations;

namespace IvaETicaret.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Category>? Categories { get; set; }
        public virtual ICollection<Branch>? Branches { get; set; }
    }
}
