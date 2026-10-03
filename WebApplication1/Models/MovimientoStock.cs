using System;

namespace WebApplication1.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int TipoMovimientoStockId { get; set; }

        public int MotivoMovimientoStockId { get; set; }

        public int Cantidad { get; set; }

        public int StockAnterior { get; set; }

        public int StockPosterior { get; set; }

        public string Observacion { get; set; }

        public DateTime Fecha { get; set; }
    }
}