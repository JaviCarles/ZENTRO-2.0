using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class AltaUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Inicialización segura del DropDownList
                dDLTipoUsuario.DataSource = new List<string> { "User", "Admin" };
                dDLTipoUsuario.DataBind();

                txtId.Enabled = false;

                if (Request.QueryString["id"] != null)
                {
                    Session["idUsuario"] = Convert.ToInt32(Request.QueryString["id"]);
                    Session["usuarioAModificar"] = obtenerUsuario(Convert.ToInt32(Session["idUsuario"]));
                    lblTitulo.Text = "MODIFICAR USUARIO";
                    btnAceptar.Text = "GUARDAR CAMBIOS";
                    cargarDesdeDb();
                    cargarImg();
                }
                else
                {
                    // Forzar valor por defecto si es una inscripción pública
                    dDLTipoUsuario.SelectedValue = "User";
                }
            }
        }

        protected void txtImagen_TextChanged(object sender, EventArgs e)
        {
            cargarImg();
        }

        public void cargarImg()
        {
            imgUsuario.ImageUrl = !string.IsNullOrEmpty(txtImagen.Text) ? txtImagen.Text : "https://i.imgur.com/1tVkLZm.jpeg";
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null && (int)Session["idUsuario"] != 0)
            {
                modificarUsuario((Usuario)Session["usuarioAModificar"]);
            }
            else
            {
                altaUsuario();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiarSesiones();
            Response.Redirect("Default.aspx", false);
        }

        private void limpiarSesiones()
        {
            Session["idUsuario"] = null;
            Session["usuarioAModificar"] = null;
        }

        public void altaUsuario()
        {
            if (validarCampos())
            {
                try
                {
                    Usuario nuevo = mapearDatos();
                    if (UsuarioNegocio.altaUsuario(nuevo))
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                    else
                    {
                        MostrarError("Ya existe un usuario registrado con ese email.");
                    }
                }
                catch (Exception)
                {
                    MostrarError("Ocurrió un error al intentar dar de alta el usuario.");
                }
            }
        }

        public bool seModificoCampo(Usuario usuario)
        {
            if (usuario.Email != txtEmail.Text) return true;
            if (usuario.Pass != txtPass.Text) return true;
            if (usuario.Nombre != txtNombre.Text) return true;
            if (usuario.Apellido != txtApellido.Text) return true;
            if (usuario.UrlImagen != txtImagen.Text) return true;

            bool esAdminSeleccionado = dDLTipoUsuario.SelectedValue == "Admin";
            if (usuario.Admin != esAdminSeleccionado) return true;

            return false;
        }

        public void modificarUsuario(Usuario usuario)
        {
            if (usuario != null)
            {
                if (seModificoCampo(usuario))
                {
                    if (validarCampos())
                    {
                        try
                        {
                            Usuario modificado = mapearDatos();
                            UsuarioNegocio.editarUsuario(modificado);

                            // Lógica segura de actualización de sesión
                            Usuario actualLogueado = (Usuario)Session["usuario"];
                            if (actualLogueado != null && actualLogueado.Id == modificado.Id)
                            {
                                // Si se editó a sí mismo, actualizamos su sesión activa
                                Session["usuario"] = modificado;
                            }

                            limpiarSesiones();
                            Response.Redirect("Default.aspx", false);
                        }
                        catch (Exception)
                        {
                            MostrarError("Error crítico al actualizar en base de datos.");
                        }
                    }
                }
                else
                {
                    MostrarError("No ha modificado ningún campo, presione 'Cancelar' si desea salir.");
                }
            }
        }

        public Usuario obtenerUsuario(int id)
        {
            try
            {
                return UsuarioNegocio.obtenerUsuario(id);
            }
            catch (Exception)
            {
                return new Usuario();
            }
        }

        public void cargarDesdeDb()
        {
            if (Session["usuarioAModificar"] != null)
            {
                Usuario usuario = (Usuario)Session["usuarioAModificar"];
                txtId.Text = usuario.Id.ToString();
                txtEmail.Text = usuario.Email;

                txtPass.TextMode = TextBoxMode.SingleLine;
                txtPass.Text = usuario.Pass;
                txtConfirmPass.TextMode = TextBoxMode.SingleLine;
                txtConfirmPass.Text = usuario.Pass;

                txtNombre.Text = usuario.Nombre;
                txtApellido.Text = usuario.Apellido;
                txtImagen.Text = usuario.UrlImagen;
                dDLTipoUsuario.SelectedValue = usuario.Admin ? "Admin" : "User";
            }
        }

        public Usuario mapearDatos()
        {
            Usuario usuario = new Usuario();
            if (Session["idUsuario"] != null && (int)Session["idUsuario"] > 0)
            {
                usuario.Id = int.Parse(txtId.Text);
            }
            usuario.Email = txtEmail.Text;
            usuario.Pass = txtPass.Text;
            usuario.Nombre = txtNombre.Text;
            usuario.Apellido = txtApellido.Text;
            usuario.Admin = dDLTipoUsuario.SelectedValue == "Admin";
            usuario.UrlImagen = !string.IsNullOrEmpty(txtImagen.Text) ? txtImagen.Text : "https://i.imgur.com/1tVkLZm.jpeg";
            return usuario;
        }

        public bool validarCampos()
        {
            if (string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPass.Text) ||
                string.IsNullOrEmpty(txtConfirmPass.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtApellido.Text))
            {
                MostrarError("Todos los campos a excepción de la foto son obligatorios.");
                return false;
            }

            if (txtPass.Text != txtConfirmPass.Text)
            {
                MostrarError("Las contraseñas ingresadas no coinciden.");
                return false;
            }

            lblError.Text = string.Empty;
            return true;
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            ScriptManager.RegisterStartupScript(this, GetType(), "scrollError",
                "var el = document.getElementById('" + lblError.ClientID + "'); if(el) el.scrollIntoView({behavior: 'smooth'});", true);
        }
    }
}