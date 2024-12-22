namespace bookStore.Models
{
    public class ClassUsuarioModel
    {
        // Propiedades para el modelo de usuario
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }

        // Propiedades adicionales para manejar la respuesta del registro
        public bool Registrado { get; set; }
        public string Mensaje { get; set; }

        
        public string ConfirmPassword { get; set; }

        // Propiedades adicionales para manejar la respuesta del registro
        // public bool Registrado { get; set; }
        // public string Mensaje { get; set; }
    }
}
