using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class Zentro : System.Web.UI.MasterPage
    {
        public Usuario usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            usuario = Session["usuario"] as Entidades.Usuario;
            if (usuario != null && !String.IsNullOrEmpty(usuario.UrlImagen))
                imgUsuarioNav.ImageUrl = usuario.UrlImagen; 
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnModificarUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("AltaUsuario.aspx?id=" + usuario.Id, false);
        }
    }
}