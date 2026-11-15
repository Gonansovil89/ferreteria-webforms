using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CompraHistorial
    {
        public int CompraId { get; set; }
        public DateTime Fecha { get; set; }
        public string RazonSocial { get; set; }
        public string CodigoProducto { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
    }
}