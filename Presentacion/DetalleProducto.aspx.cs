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
            // Verificamos si es la primera carga de la página (buena práctica en WebForms)
            if (!IsPostBack)
            {
                // 1. Verificamos si se recibe un id por url
                if (Request.QueryString["id"] != null)
                {
                    try
                    {
                        int idProducto = Convert.ToInt32(Request.QueryString["id"]); // Guardamos el id recibido
                        producto = ProductoNegocio.obtenerProducto(idProducto);

                        // 2. PROTECCIÓN EXTRA: Si mandaron un ID pero no existe en la BD (producto vuelve null)
                        if (producto == null)
                        {
                            RedireccionarAlInicio();
                        }
                    }
                    catch (Exception)
                    {
                        // Si ponen letras en el ID o rompe la conversión, al inicio por seguridad
                        RedireccionarAlInicio();
                    }
                }
                else
                {
                    // 3. SI ENTRAS DESDE VISUAL STUDIO (ID es null): Redirecciona directo y no explota
                    RedireccionarAlInicio();
                }
            }
        }

        // Método auxiliar para no repetir código de redirección segura
        private void RedireccionarAlInicio()
        {
            // Cambiá "Default.aspx" por "Catalogos.aspx" o el nombre de tu página principal si es distinto
            Response.Redirect("Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}