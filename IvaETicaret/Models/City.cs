using System.ComponentModel.DataAnnotations;

namespace IvaETicaret.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<County>? Counties { get; set; }
    }
}
