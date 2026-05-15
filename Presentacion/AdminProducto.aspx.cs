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
        int idCategoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarCategorias();
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
            orden = DropDownOrden.Text;
            filtro = txtBusqueda.Text;
            idCategoria = Convert.ToInt32(DropDownCategoria.SelectedValue);
            try
            {
                if (idCategoria != 0)
                    lista = ProductoNegocio.buscar(filtro, orden, idCategoria);
                else
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
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        #region CARGAR DESPLEGABLE CATEGORIA
        public void cargarCategorias()
        {
            try
            {
                List<Categoria> categorias = new List<Categoria>();
                categorias = CategoriaNegocio.listaCategorias();
                Categoria cat = new Categoria { Id = 0, Descripcion = "Todas" };
                categorias.Add(cat);
                if (categorias != null && categorias.Count > 0)
                {
                    DropDownCategoria.DataSource = categorias;
                    DropDownCategoria.DataTextField = "Descripcion";
                    DropDownCategoria.DataValueField = "Id";
                    DropDownCategoria.SelectedValue = "0";
                    DropDownCategoria.DataBind();
                }
                else
                {
                    // Manejo de error o mensaje al usuario
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }
        #endregion CARGAR DESPLEGABLE CATEGORIA

        #region CARGAR DESPLEGABLE ORDEN
        public void cargarListadoOrden()
        {
            List<string> listaOrden = new List<string> { "Nombre", "Marca", "Precio" };
            DropDownOrden.DataSource = listaOrden;
            DropDownOrden.DataBind();
        }
        #endregion CARGAR DESPLEGABLE ORDEN
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("AltaProducto.aspx", false);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }

        protected void dgvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string id = dgvProductos.SelectedDataKey.Value.ToString();
                Response.Redirect("AltaProducto.aspx?id=" + id, false);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
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

        protected void DropDownOrden_SelectedIndexChanged(object sender, EventArgs e)
        {
            listarProductos();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            listarProductos();
        }
    }
}