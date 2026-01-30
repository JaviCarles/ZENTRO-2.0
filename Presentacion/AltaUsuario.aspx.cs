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
                dDLTipoUsuario.DataSource = new List<string> { "User", "Admin" };
                dDLTipoUsuario.DataBind();

                txtId.Enabled = false;
                if (Request.QueryString["id"] != null)//Si request recibe id es porque es modificación.
                {
                    Session["idUsuario"] = Convert.ToInt32(Request.QueryString["id"]);//Guardamos en session el id recibido.                   
                    Session["usuarioAModificar"] = obtenerUsuario(Convert.ToInt32(Session["idUsuario"]));// cargarUsuario devuelve un Usuario desde la db.
                    lblTitulo.Text = "MODIFICAR USUARIO";
                    cargarDesdeDb();//Carga los campos con el usuario traído desde la Db.
                    cargarImg();
                }
            }
        }

        protected void txtImagen_TextChanged(object sender, EventArgs e)
        {
            cargarImg();
        }
        public void cargarImg()
        {
            imgUsuario.ImageUrl = txtImagen.Text;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null && (int)Session["idUsuario"] != 0)
            {
                modificarUsuario((Usuario)Session["usuarioAModificar"]);//Aquí usamos el usuario guardado en session
            }
            else
                altaUsuario(); // Si request no recibe id, no entrará en el if porque se trata de un alta de producto.
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
                Session["idUsuario"] = null;
            Response.Redirect("Default.aspx", false);
        }
        public void altaUsuario()
        {
            if (validarCampos())
            {
                try
                {
                    // Lógica de negocio
                    if (UsuarioNegocio.altaUsuario(mapearDatos()))//mapearDatos() devuelve un usuario cargado con los datos del formulario.
                    {
                        Response.Redirect("Login.aspx", false);
                    }
                    else
                    {
                        lblError.Text = "Ya existe un usuario registrado con ese email.";
                        lblError.Focus();
                    }
                }
                catch (Exception ex)
                {
                    lblError.Focus();
                    lblError.Text = "Ocurrió un error al intentar dar de alta el usuario.";
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
            bool esAdminSeleccionado = dDLTipoUsuario.SelectedValue == "Admin";//necesario para comparar el dropDownList 
            if (usuario.Admin != esAdminSeleccionado) return true;
            //Si ninguno de los campos se modificó, devuelve false.
            return false;
        }
        public void modificarUsuario(Usuario usuario)
        {
            if (usuario != null) //Validamos existencia del usuario.
            {
                if (seModificoCampo(usuario))//Verificamos si se modificó al menos un campo.
                {
                    if (validarCampos())
                    {
                        try
                        {
                            UsuarioNegocio.editarUsuario(mapearDatos());//mapearDatos() devuelve un usuario.
                            Session.Abandon();
                            Response.Redirect("Login.aspx", false);
                        }
                        catch (Exception ex)
                        {

                        }
                        lblError.Text = null;
                    }
                }
                else
                {
                    lblError.Text = "No ha modificado ningún campo, presione 'Cancelar' si desea salir.";
                    lblError.Focus();
                }
            }
        }

        #region OBTENER USUARIO
        public Usuario obtenerUsuario(int id)
        {
            Usuario usuario = new Usuario();
            
            try
            {
               return usuario = UsuarioNegocio.obtenerUsuario(id);
            }
            catch (Exception ex)
            {
                return usuario;    
            }
        }
        #endregion OBTENER USUARIO

        #region CARGAR CAMPOS DESDE DB
        public void cargarDesdeDb()// carga los campos con el Usuario traído desde la DB
        {
            if (Session["usuarioAModificar"] != null)
            {
                Usuario usuario= (Usuario)Session["usuarioAModificar"];
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
        #endregion CARGAR CAMPOS DESDE DB

        #region MAPEAR DATOS
        public Usuario mapearDatos()//devuelve un usuario mapaedo desde el formulario.
        {
            // Construcción del objeto
            Usuario usuario = new Usuario();
            //Si es modificación, el id estará guardado es Session.
            if (Session["idUsuario"] != null && (int)Session["idUsuario"] > 0)
            {
                usuario.Id = int.Parse(txtId.Text);
            }
            usuario.Email = txtEmail.Text;
            usuario.Pass = txtPass.Text;
            usuario.Nombre = txtNombre.Text;
            usuario.Apellido = txtApellido.Text;
            usuario.Admin = dDLTipoUsuario.SelectedValue == "Admin";
            usuario.UrlImagen = txtImagen.Text;
            return usuario;
        }
        #endregion MAPEAR DATOS

        #region VALIDACIONES
        public bool validarCampos()
        {
            // Validación agrupada
            if (string.IsNullOrEmpty(txtEmail.Text) ||
                string.IsNullOrEmpty(txtPass.Text) ||
                string.IsNullOrEmpty(txtConfirmPass.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtConfirmPass.Text) ||
                string.IsNullOrEmpty(txtImagen.Text))
            {
                lblError.Focus();
                lblError.Text = "Todos los campos son obligatorios.";
                return false;
            }
            else if (txtPass.Text != txtConfirmPass.Text)
            {
                lblError.Text = "Las contraseñas no coinciden.";
                return false;
            }
            else
                return true;
        }
        #endregion VALIDACIONES
    }
}