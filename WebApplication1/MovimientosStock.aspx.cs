using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;

namespace WebApplication1
{
    public partial class MovimientosStock : System.Web.UI.Page
    {
        private ProductoRepository productoRepository = new ProductoRepository();

        private TipoMovimientoStockRepository tipoMovimientoRepository = new TipoMovimientoStockRepository();

        private MotivoMovimientoStockRepository motivoMovimientoRepository = new MotivoMovimientoStockRepository();

        private MovimientoStockRepository movimientoStockRepository = new MovimientoStockRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTiposMovimiento();
                CargarMotivos();
                CargarProductos();
                if (Request.QueryString["resultado"] == "ok")
                {
                    lblMensaje.Text =
                        "Movimiento registrado correctamente.";
                }
                CargarHistorial();
            }
        }
        private void CargarHistorial()
        {
            gvHistorial.DataSource =
                movimientoStockRepository.ObtenerHistorial();

            gvHistorial.DataBind();
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            int productoId;
            int tipoMovimientoId;
            int motivoMovimientoId;
            int cantidad;

            bool productoValido =
                int.TryParse(ddlProducto.SelectedValue, out productoId);

            bool tipoValido =
                int.TryParse(ddlTipoMovimiento.SelectedValue, out tipoMovimientoId);

            bool motivoValido =
                int.TryParse(ddlMotivo.SelectedValue, out motivoMovimientoId);

            bool cantidadValida =
                int.TryParse(txtCantidad.Text, out cantidad);

            string observacion =
                txtObservacion.Text.Trim();

            string error = "";

            if (!productoValido || productoId <= 0)
            {
                error += "Debe seleccionar un producto.<br />";
            }

            if (!tipoValido || tipoMovimientoId <= 0)
            {
                error += "Debe seleccionar un tipo de movimiento.<br />";
            }

            if (!motivoValido || motivoMovimientoId <= 0)
            {
                error += "Debe seleccionar un motivo.<br />";
            }

            if (!cantidadValida || cantidad <= 0)
            {
                error += "La cantidad debe ser mayor a cero.<br />";
            }

            if (!string.IsNullOrEmpty(error))
            {
                lblMensaje.Text = error;
                return;
            }

            try
            {
                MovimientoStockService movimientoStockService =
                    new MovimientoStockService();

                movimientoStockService.RegistrarMovimiento(
                    productoId,
                    tipoMovimientoId,
                    motivoMovimientoId,
                    cantidad,
                    observacion);

                Response.Redirect("~/MovimientosStock?resultado=ok");
            }
            catch (Exception ex)
            {
                lblMensaje.Text = ex.Message;
            }
        }
        private void CargarTiposMovimiento()
        {
            List<TipoMovimientoStock> tipos = tipoMovimientoRepository.ObtenerActivos();

            ddlTipoMovimiento.DataSource = tipos;
            ddlTipoMovimiento.DataTextField = "Descripcion";
            ddlTipoMovimiento.DataValueField = "Id";

            ddlTipoMovimiento.DataBind();
            ddlTipoMovimiento.Items.Insert(0,new ListItem("-- Seleccione --", "-1"));
        }
        private void CargarMotivos()
        {
            List<MotivoMovimientoStock> motivos =
                motivoMovimientoRepository.ObtenerActivos();

            ddlMotivo.DataSource = motivos;

            ddlMotivo.DataTextField = "Descripcion";
            ddlMotivo.DataValueField = "Id";

            ddlMotivo.DataBind();
            ddlMotivo.Items.Insert(0,new ListItem("-- Seleccione --", "-1"));
        }
        private void CargarProductos()
        {
            List<Producto> productos =
                productoRepository.ObtenerProductos(true);

            ddlProducto.DataSource = productos;

            ddlProducto.DataTextField = "DescripcionCompleta";
            ddlProducto.DataValueField = "Id";

            ddlProducto.DataBind();

            ddlProducto.Items.Insert(
                0,
                new ListItem("-- Seleccione --", "-1")
            );
        }
    }
}