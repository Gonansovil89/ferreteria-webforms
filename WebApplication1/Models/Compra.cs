using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProveedorId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Observacion { get; set; }
        public List<CompraDetalle> Detalles { get; set; } = new List<CompraDetalle>();
    }
}