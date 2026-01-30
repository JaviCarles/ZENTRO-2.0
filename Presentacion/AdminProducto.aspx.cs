using Entidades;
using Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class AdminProducto : System.Web.UI.Page
    {
        List<Producto> lista = new List<Producto>();
        string orden, filtro = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarListadoOrden();
                orden = DropDownOrden.Text;
                PaginaActual = 0;
                listarProductos();
            }
        }
        private int PaginaActual//Property para la paginación
        {
            get { return ViewState["PaginaActual"] != null ? (int)ViewState["PaginaActual"] : 0; }
            set { ViewState["PaginaActual"] = value; }
        }

        public void listarProductos()
        {
            try
            {
                lista = ProductoNegocio.buscar(filtro, orden);
                dgvProductos.PageSize = 30;
                dgvProductos.PageIndex = PaginaActual;
                dgvProductos.DataSource = lista;
                dgvProductos.DataBind();

                lblPaginaActual.Text = $"Página {PaginaActual + 1} de {dgvProductos.PageCount}";
                btnAnterior.Enabled = PaginaActual > 0;
                btnSiguiente.Enabled = PaginaActual < dgvProductos.PageCount - 1;
            }
            catch (Exception ex)
            {
                Session.Add("Error", ex);
            }
        }
        public void cargarListadoOrden()
        {
            List<string> listaOrden = new List<string> { "NOMBRE", "CATEGORIA", "MARCA", "PRECIO" };
            DropDownOrden.DataSource = listaOrden;
            DropDownOrden.DataBind();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AltaProducto.aspx");
        }

        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id= dgvProductos.SelectedDataKey.Value.ToString();
            Response.Redirect("AltaProducto.aspx?id=" + id);
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (PaginaActual > 0)
            {
                PaginaActual--;
                listarProductos();
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (PaginaActual < dgvProductos.PageCount - 1)
            {
                PaginaActual++;
                listarProductos();
            }

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            orden = DropDownOrden.Text;
            filtro = txtBusqueda.Text;
            listarProductos();
        }
    }
}