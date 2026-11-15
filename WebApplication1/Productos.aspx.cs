using System;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public partial class Productos : System.Web.UI.Page
    {
        private ProductoRepository productoRepository = new ProductoRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrillasProductos();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string codigo = txtCodigo.Text.Trim().ToUpper();
            decimal precio;
            int stock;
            bool precioValido = decimal.TryParse(txtPrecio.Text, out precio);
            bool stockValido = int.TryParse(txtStock.Text, out stock);
            string error;

            bool productoValido = ValidarProducto(
                nombre,
                codigo,
                precioValido,
                stockValido,
                precio,
                stock,
                out error);

            if (!productoValido)
            {
                lblMensaje.Text = error;
                return;
            }

            bool codigoExistente = productoRepository.ExisteCodigo(codigo);

            if (codigoExistente)
            {
                bool codigoInactivo = productoRepository.ExisteCodigoInactivo(codigo);

                if (codigoInactivo)
                {
                    lblMensaje.Text = "El producto existe pero está inactivo.";
                    return;
                }

                lblMensaje.Text = "El código ya existe.";
                return;
            }

            Producto producto = new Producto();

            producto.Nombre = nombre;
            producto.Codigo = codigo;
            producto.Precio = precio;
            producto.Stock = stock;

            productoRepository.InsertarProducto(producto);

            Response.Redirect("~/Productos");
        }
        private void CargarProductos()
        {
            List<Producto> productos = productoRepository.ObtenerProductos(true);

            gvProductos.DataSource = productos;
            gvProductos.DataBind();
        }
        private bool ValidarProducto(
            string nombre,
            string codigo,
            bool precioValido,
            bool stockValido,
            decimal precio,
            int stock,
            out string error)
        {
            error = "";
            if (string.IsNullOrWhiteSpace(nombre))
            {
                error += "El nombre es obligatorio.<br />";
            }
            if (string.IsNullOrWhiteSpace(codigo))
            {
                error += "El codigo es obligatorio.<br />";
            }
            if (!stockValido || stock < 0)
            {
                error += "El stock es invalido.<br />";
            }
            if (!precioValido || precio <= 0)
            {
                error += "El precio invalido.<br />";
            }
            return string.IsNullOrEmpty(error);
        }
        private string ObtenerConnectionString()
        {
            return ConfigurationManager
                .ConnectionStrings["FerreteriaConnection"]
                .ConnectionString;
        }
        protected void gvProductos_RowEditing(
        object sender, GridViewEditEventArgs e)
        {
            gvProductos.EditIndex = e.NewEditIndex;

            CargarProductos();
        }

        protected void gvProductos_RowUpdating(
        object sender, GridViewUpdateEventArgs e)
        {
            int id = (int)gvProductos.DataKeys[e.RowIndex].Value;

            string nombre = e.NewValues["Nombre"].ToString().Trim();
            string precioTexto = e.NewValues["Precio"].ToString();
            decimal precio;
            int stock;
            bool precioValido = decimal.TryParse(precioTexto, out precio);

            if (string.IsNullOrWhiteSpace(nombre))
            {
                lblMensaje.Text = "El nombre es obligatorio.";
                return;
            }

            if (!precioValido || precio <= 0)
            {
                lblMensaje.Text = "El precio es inválido.";
                return;
            }
            productoRepository.ActualizarProducto(id, nombre, precio);
            gvProductos.EditIndex = -1;
            CargarProductos();
        }
        protected void gvProductos_RowCancelingEdit(
            object sender,
            GridViewCancelEditEventArgs e)
        {
            gvProductos.EditIndex = -1;

            CargarProductos();
        }
        protected void gvProductos_RowCommand(
            object sender,
            GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Desactivar")
            {
                // acá vamos a identificar el producto
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int id = (int)gvProductos.DataKeys[rowIndex].Value;

                productoRepository.DesactivarProducto(id);

                CargarGrillasProductos();
            }
        }
        
        private void CargarProductosInactivos()
        {
            List<Producto> productos = productoRepository.ObtenerProductos(false);

            gvProductosInactivos.DataSource = productos;
            gvProductosInactivos.DataBind();
        }
        protected void gvProductosInactivos_RowCommand(
                object sender,
                GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reactivar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                int id = (int)gvProductosInactivos.DataKeys[rowIndex].Value;

                productoRepository.ReactivarProducto(id);

                CargarGrillasProductos();
            }
        }
        private void CargarGrillasProductos()
        {
            CargarProductos();
            CargarProductosInactivos();
        }
    }
}