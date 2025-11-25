using System.ComponentModel.DataAnnotations;

namespace API.Modeles
{
    public class Wilaya
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
