using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

namespace Presentacion
{
    public partial class ProductoHome : System.Web.UI.Page
    {
        public Usuario usuario;
        public List<Producto> lista { get; set; }
        string orden, filtro = "";
        int idCategoria;
        protected void Page_Load(object sender, EventArgs e)
        {

            usuario = new Usuario();
            usuario = Session["usuario"] != null ? (Usuario)Session["usuario"] : null;
            if (!IsPostBack)
            {
                cargarListadoOrden();
                orden = DropDownOrden.Text;
                cargarListadoCategoria();
                buscar();
            }

        }

        public void listarProductos()
        {
            try
            {
                if (idCategoria != 0)
                    lista = ProductoNegocio.buscar(filtro, orden, idCategoria);
                else
                    lista = ProductoNegocio.buscar(filtro, orden);
                rptProductos.DataSource = lista;
                rptProductos.DataBind();
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        public void cargarListadoOrden()
        {
            List<string> listaOrden = new List<string> { "Nombre", "Marca", "Precio" };
            DropDownOrden.DataSource = listaOrden;
            DropDownOrden.DataBind();
        }

        public void cargarListadoCategoria()
        {
            try
            {
                List<Categoria> listaCategoria = CategoriaNegocio.listaCategorias();
                Categoria cat = new Categoria { Id = 0, Descripcion = "Todas" };
                listaCategoria.Add(cat);
                DropDownCategoria.DataSource = listaCategoria;
                DropDownCategoria.DataTextField = "Descripcion";
                DropDownCategoria.DataValueField = "Id";
                DropDownCategoria.SelectedValue = "0";
                DropDownCategoria.DataBind();
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            buscar();
        }

        public void buscar()
        {
            try
            {
                orden = DropDownOrden.Text;
                filtro = txtBusqueda.Text;
                idCategoria = int.Parse(DropDownCategoria.SelectedValue);
                listarProductos();
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }
        protected void DropDownOrden_SelectedIndexChanged(object sender, EventArgs e)
        {
            buscar();
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Favorito")
                {
                    int idProducto = Convert.ToInt32(e.CommandArgument);

                    // Alternar estado favorito en tu capa de negocio
                    ProductoNegocio.insertarOEliminarFav(idProducto, usuario.Id);
                    //Refrescamos el repetidor
                    listarProductos();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }
    }
}