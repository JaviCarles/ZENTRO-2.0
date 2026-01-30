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
    public partial class DetalleProducto : System.Web.UI.Page
    {
        public Producto producto;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)//Verificamos si se recibe un id por url.
            {
                int idProducto = Convert.ToInt32(Request.QueryString["id"]);//Guardamos el id recibido
                producto = new Producto();
                producto = ProductoNegocio.obtenerProducto(idProducto);
            }
        }
    }
}