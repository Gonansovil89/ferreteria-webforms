
namespace WebApplication1.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public string DescripcionCompleta
        {
            get
            {
                return Codigo + " - " + Nombre;
            }
        }
    }
}