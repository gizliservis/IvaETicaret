using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvaETicaret.Models
{
    public class County
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        public ICollection<District>? Districts { get; set; }
    }
}
