using System.ComponentModel.DataAnnotations;

namespace IvaETicaret.Models
{
    public class OdemeTur
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
