
namespace WebApplication1.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string CUIT { get; set; }
        public string Telefono { get; set; }
        public string TelefonoAdicional { get; set; }
        public string Email { get; set; }
        public string Domicilio { get; set; }
        public string Provincia { get; set; }
        public string Pais { get; set; }
        public bool Activo { get; set; }
    }
}