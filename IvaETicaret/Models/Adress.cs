using System.ComponentModel.DataAnnotations;

namespace IvaETicaret.Models
{
    public class Adress
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TamAdres { get; set; }
        public string Il { get; set; }
        public string Ilce { get; set; }
        public string Semt { get; set; }
        public int BayiId { get; set; }
    }
}
