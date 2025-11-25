using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Modeles
{
    public class ShippingRate
    {
        [Key]
        public int Id { get; set; }

        public int BaladiyaId { get; set; }

        [ForeignKey("BaladiyaId")]
        public Baladiya Baladiya { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal HomePrice { get; set; } // Price for Home Delivery

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeskPrice { get; set; } // Price for Desk/Office Delivery
    }
}
