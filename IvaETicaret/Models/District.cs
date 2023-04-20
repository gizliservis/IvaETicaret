using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvaETicaret.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int CountyId { get; set; }
        [ForeignKey(nameof( CountyId))]
        public virtual County? County { get; set; }
        public ICollection<Branch>? Branches { get; set; }
    }
}
