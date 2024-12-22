using System.ComponentModel.DataAnnotations;

namespace bookStore.Models
{
    public class CustomerDetails
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [Display(Name = "Nombre completo")]
        public string Name { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "La dirección debe tener entre 10 y 200 caracteres")]
        [Display(Name = "Dirección de envío")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "Por favor ingrese un número de teléfono válido")]
        [Display(Name = "Teléfono de contacto")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Por favor ingrese un email válido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Notas adicionales")]
        public string? Notes { get; set; }

        [Display(Name = "Código postal")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "El código postal debe tener 5 dígitos")]
        public string? PostalCode { get; set; }
    }
}