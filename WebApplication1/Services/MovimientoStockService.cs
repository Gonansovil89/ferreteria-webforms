using System;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class MovimientoStockService
    {
        private ProductoRepository productoRepository =
        new ProductoRepository();

        private TipoMovimientoStockRepository tipoMovimientoRepository =
            new TipoMovimientoStockRepository();

        private MovimientoStockRepository movimientoStockRepository =
            new MovimientoStockRepository();

        public void RegistrarMovimiento(
            int productoId,
            int tipoMovimientoId,
            int motivoMovimientoId,
            int cantidad,
            string observacion)
        {
            Producto producto =
                productoRepository.ObtenerPorId(productoId);

            if (producto == null)
            {
                throw new Exception(
                    "El producto seleccionado no existe.");
            }

            if (!producto.Activo)
            {
                throw new Exception(
                    "El producto seleccionado está inactivo.");
            }

            TipoMovimientoStock tipo =
                tipoMovimientoRepository.ObtenerPorId(tipoMovimientoId);

            if (tipo == null)
            {
                throw new Exception(
                    "El tipo de movimiento no existe.");
            }

            if (!tipo.Activo)
            {
                throw new Exception(
                    "El tipo de movimiento está inactivo.");
            }

            if (cantidad <= 0)
            {
                throw new Exception(
                    "La cantidad debe ser mayor a cero.");
            }

            int stockAnterior = producto.Stock;
            int stockPosterior;

            if (tipo.Codigo == "ENTRADA")
            {
                stockPosterior =
                    stockAnterior + cantidad;
            }
            else if (tipo.Codigo == "SALIDA")
            {
                stockPosterior =
                    stockAnterior - cantidad;
            }
            else
            {
                throw new Exception(
                    "El tipo de movimiento no es válido.");
            }

            if (stockPosterior < 0)
            {
                throw new Exception(
                    "No hay stock suficiente para realizar la salida.");
            }

            MovimientoStock movimiento =
                new MovimientoStock();

            movimiento.ProductoId =
                productoId;

            movimiento.TipoMovimientoStockId =
                tipoMovimientoId;

            movimiento.MotivoMovimientoStockId =
                motivoMovimientoId;

            movimiento.Cantidad =
                cantidad;

            movimiento.StockAnterior =
                stockAnterior;

            movimiento.StockPosterior =
                stockPosterior;

            movimiento.Observacion =
                observacion;

            movimientoStockRepository
                .RegistrarMovimiento(movimiento);
        }

        public int CalcularStockPosterior(
            int productoId,
            int tipoMovimientoId,
            int cantidad)
        {
            Producto producto = productoRepository.ObtenerPorId(productoId);

            if (producto == null)
            {
                throw new Exception(
                    "El producto seleccionado no existe.");
            }

            if (!producto.Activo)
            {
                throw new Exception(
                    "El producto seleccionado está inactivo.");
            }

            TipoMovimientoStock tipo =
                tipoMovimientoRepository.ObtenerPorId(
                    tipoMovimientoId);

            if (tipo == null)
            {
                throw new Exception(
                    "El tipo de movimiento no existe.");
            }

            if (!tipo.Activo)
            {
                throw new Exception(
                    "El tipo de movimiento está inactivo.");
            }

            if (cantidad <= 0)
            {
                throw new Exception(
                    "La cantidad debe ser mayor a cero.");
            }

            int stockPosterior;

            if (tipo.Codigo == "ENTRADA")
            {
                stockPosterior =
                    producto.Stock + cantidad;
            }
            else if (tipo.Codigo == "SALIDA")
            {
                stockPosterior =
                    producto.Stock - cantidad;
            }
            else
            {
                throw new Exception(
                    "El tipo de movimiento no es válido.");
            }

            if (stockPosterior < 0)
            {
                throw new Exception(
                    "No hay stock suficiente para realizar la salida.");
            }

            return stockPosterior;
        }
    }
}