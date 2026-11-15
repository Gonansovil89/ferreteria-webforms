using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public partial class Compras : System.Web.UI.Page
    {
        private ProveedorRepository proveedorRepository = new ProveedorRepository();
        private ProductoRepository productoRepository = new ProductoRepository();
        private CompraRepository compraRepository = new CompraRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["resultado"] == "ok")
            {
                lblMensaje.Text = "Compra registrada correctamente.";
                lblMensaje.CssClass = "message text-success";
            }

            if (!IsPostBack)
            {
                CargarProveedores();
                CargarProductos();
                CargarHistorial();
            }
        }
        private void CargarProveedores()
        {
            ddlProveedor.DataSource = proveedorRepository.ObtenerActivos();

            ddlProveedor.DataTextField = "RazonSocial";
            ddlProveedor.DataValueField = "Id";

            ddlProveedor.DataBind();

            ddlProveedor.Items.Insert(0, new ListItem("-- Seleccionar proveedor --", ""));
        }
        private void CargarProductos()
        {
            ddlProducto.DataSource = productoRepository.ObtenerProductos(true);
            ddlProducto.DataTextField = "DescripcionCompleta";
            ddlProducto.DataValueField = "Id";
            ddlProducto.DataBind();
            ddlProducto.Items.Insert(0, new ListItem("-- Seleccionar Producto --", ""));
        }
        protected void btnRegistrarCompra_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            lblMensaje.CssClass = "message";

            // valido proveedor
            if (ddlProveedor.SelectedValue == "")
            {
                ddlProveedor.Focus();
                lblMensaje.Text = "Error - Debe seleccionar un proveedor";
                lblMensaje.CssClass = "message text-danger";
                return;
            }
            // valido producto
            if (ddlProducto.SelectedValue == "" || ddlProducto.SelectedValue == "-1")
            {
                ddlProducto.Focus();
                lblMensaje.Text = "Error - Debe seleccionar un producto";
                lblMensaje.CssClass = "message text-danger";
                return;
            }

            // valido cantidad
            int cantidad;

            bool esvalido = int.TryParse(txtCantidad.Text, out cantidad);

            if (esvalido == false || cantidad <= 0)
            {
                txtCantidad.Focus();
                lblMensaje.Text = "Error - Ingrese una cantidad valida";
                lblMensaje.CssClass = "message text-danger";
                return;
            }

            // valido precio
            decimal precio;

            bool precio_esvalido = decimal.TryParse(txtPrecioUnitario.Text, out precio);

            if (precio_esvalido == false || precio <= 0)
            {
                txtPrecioUnitario.Focus();
                lblMensaje.Text = "Error - Ingrese un precio valido";
                lblMensaje.CssClass = "message text-danger";
                return;
            }

            //lblMensaje.Text = "Pasaron las validaciones";
            //lblMensaje.CssClass = "message text-success";

            int proveedorId = int.Parse(ddlProveedor.SelectedValue);
            int productoId = int.Parse(ddlProducto.SelectedValue);

            Compra compra = new Compra();

            compra.ProveedorId = proveedorId;
            compra.Observacion = txtObservacion.Text.Trim();

            CompraDetalle detalle = new CompraDetalle();

            detalle.ProductoId = productoId;
            //detalle.ProductoId = int.Parse(ddlProveedor.SelectedValue);
            detalle.Cantidad = cantidad;
            detalle.PrecioUnitario = precio;

            compra.Detalles.Add(detalle);

            try
            {
                compraRepository.InsertarCompra(compra);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al registrar la compra: " + ex.Message;
                lblMensaje.CssClass = "message text-danger";
                return;
            }

            Response.Redirect("~/Compras?resultado=ok");
        }

        private void CargarHistorial()
        {
            List<CompraHistorial> compras = compraRepository.ObtenerHistorial();

            gvHistorialCompras.DataSource = compras;
            gvHistorialCompras.DataBind();
        }
    }
}