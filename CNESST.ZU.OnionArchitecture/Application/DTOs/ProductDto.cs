using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Le nom du produit ne doit pas dépasser 100 caratères.")]
        [Required(ErrorMessage = "Le nom du produit est obligatoire.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, 1000, ErrorMessage = "Le prix du produit doit être compris entre 0 et 1000")]
        public decimal Price { get; set; }
    }
}
