using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["error"] != null)
                {
                    lblError.Text = Session["error"].ToString();
                }
                else
                {
                    // Mensaje de respaldo por si entran a la URL de forma directa sin un error real
                    lblError.Text = "Ocurrió un error inesperado en el sistema de gestión.";
                }
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // Limpiamos la variable para liberar memoria antes de redireccionar
            Session["error"] = null;
            Response.Redirect("Default.aspx", false);
        }
    }
}