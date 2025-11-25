using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Modeles
{
    public class Baladiya
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int WilayaId { get; set; }
        
        [ForeignKey("WilayaId")]
        public Wilaya? Wilaya { get; set; }
    }
}
