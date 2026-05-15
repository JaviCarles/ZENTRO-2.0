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
    public partial class LoguinUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblValidacionUser.Visible = false;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblValidacionUser.Text = "Por favor, complete ambos campos";
                lblValidacionUser.Visible = true;
            }
            else
                loguear();
        }

        public void loguear()
        {
            Usuario usuario;
            try
            {
                usuario = UsuarioNegocio.loguear(txtEmail.Text, txtPassword.Text);
                if (usuario != null)
                {
                    Session["usuario"] = usuario; // guardamos en Session el user.
                    Response.Redirect("Default.aspx", false);
                }
                else
                {
                    lblValidacionUser.Text = "Email o contraseña incorrectos.";
                    lblValidacionUser.Visible = true;
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