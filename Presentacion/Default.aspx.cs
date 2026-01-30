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
        public List<Producto> lista {  get; set; }
        string orden,filtro = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = new Usuario();
            usuario = Session["usuario"] != null ? (Usuario)Session["usuario"] : null;
            if (!IsPostBack)
            {
                cargarListadoOrden();
                orden = DropDownOrden.Text;
              
                listarProductos();
            }
            
        }

        public void listarProductos()
        {
            try
            {
                lista = ProductoNegocio.buscar(filtro, orden);
                rptProductos.DataSource = lista;
                rptProductos.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        public void cargarListadoOrden()
        {
            List<string> listaOrden = new List<string> { "NOMBRE", "CATEGORIA", "MARCA", "PRECIO" };
            DropDownOrden.DataSource = listaOrden;
            DropDownOrden.DataBind();
        }

        protected void btnFavorito_Click(object sender, EventArgs e)
        {
            
            
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {   orden = DropDownOrden.Text;
            filtro = txtBusqueda.Text;
            listarProductos();
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
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
    }
}