using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNESST.ZU.AppDemo.Models
{
    [Serializable]
    public partial class ProductDetailsModel
    {
        public int Id { get; set; }
        public int StockAvailable { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, 9999.99, ErrorMessage = "Le prix n'est pas valide.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
