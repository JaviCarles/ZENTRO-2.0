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
            usuario = new Usuario();
            usuario = Session["usuario"] != null ? (Usuario)Session["usuario"] : null;
            if (!IsPostBack)
            {
                cargarListadoOrden();
                //orden = DropDownOrden.Text;
                listarProductos();
            }
        }

        public void listarProductos()
        {
            orden = DropDownOrden.Text;
            filtro = txtBusqueda.Text;
            try
            {
                if(usuario != null)
                    lista = ProductoNegocio.buscar(filtro, orden,null , usuario.Id);
                //else
                //    lista = ProductoNegocio.buscar(filtro, orden);
                rptProductos.DataSource = lista;
                rptProductos.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            listarProductos();
        }

        protected void rptProductos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Favorito")
            {
                //Capturamos el id del producto
                int idProducto = Convert.ToInt32(e.CommandArgument);

                // Alternar estado favorito en tu capa de negocio
                ProductoNegocio.insertarOEliminarFav(idProducto, usuario.Id);
                //Refrescamos el repetidor
                listarProductos();
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