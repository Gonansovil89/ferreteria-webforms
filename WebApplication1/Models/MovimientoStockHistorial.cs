using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MovimientoStockHistorial
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string ProductoCodigo { get; set; }

        public string ProductoNombre { get; set; }

        public string TipoMovimiento { get; set; }

        public string MotivoMovimiento { get; set; }

        public int Cantidad { get; set; }

        public int StockAnterior { get; set; }

        public int StockPosterior { get; set; }

        public string Observacion { get; set; }
    }
}