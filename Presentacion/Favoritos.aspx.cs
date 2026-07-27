using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Favoritos : System.Web.UI.Page
    {
        public Usuario usuario;
        public List<Producto> lista { get; set; }
        string orden, filtro = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Session["usuario"] != null ? (Usuario)Session["usuario"] : null;

            if (!IsPostBack)
            {
                cargarListadoOrden();
                listarProductos();
            }
        }

        public void listarProductos()
        {
            orden = DropDownOrden.Text;
            filtro = txtBusqueda.Text;
            try
            {
                if (usuario != null)
                {
                    lista = ProductoNegocio.buscar(filtro, orden, null, usuario.Id);
                    rptProductos.DataSource = lista;
                    rptProductos.DataBind();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            listarProductos();
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Favorito")
                {
                    int idProducto = Convert.ToInt32(e.CommandArgument);

                    // Alterna el estado (lo borra de la tabla de favoritos si ya existía)
                    ProductoNegocio.insertarOEliminarFav(idProducto, usuario.Id);

                    // Refrescamos el repetidor para que desaparezca de la pantalla al instante
                    listarProductos();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        public void cargarListadoOrden()
        {
            List<string> listaOrden = new List<string> { "Nombre", "Categoria", "Marca", "Precio" };
            DropDownOrden.DataSource = listaOrden;
            DropDownOrden.DataBind();
        }
    }
}