using System.ComponentModel.DataAnnotations;

namespace bookStore.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Título")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required]
        [Range(0.01, 1000.00)]
        [Display(Name = "Precio")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
