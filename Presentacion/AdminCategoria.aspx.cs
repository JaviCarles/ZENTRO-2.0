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
    public partial class AdminCategoria : System.Web.UI.Page
    {
        public Usuario usuario;
        int idCategoria;
        public List<Categoria> categorias = new List<Categoria>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                usuario = (Usuario)Session["usuario"];
            }

            // ¡AGREGÁ ESTO AQUÍ!
            if (!IsPostBack)
            {
                // Forzamos que arranquen ocultos en el primer ingreso
                txtModificarCategoria.Visible = false;
                btnModificarCategoria.Visible = false;
                btnCerrar.Visible = false;
            }

            cargarGrilla();
        }

        #region CARGAR GRILLA
        public void cargarGrilla()
        {
            try
            {
                categorias = CategoriaNegocio.listaCategorias();
                if (categorias != null && categorias.Count > 0)
                {
                    dgvCategorias.DataSource = categorias;
                    dgvCategorias.DataBind();
                }
                else
                {
                    // Manejo de error o mensaje al usuario
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }
        #endregion CARGAR GRILLA

        protected void dgvCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                idCategoria = Convert.ToInt32(args[0]);
                string descripcion = args.Length > 1 ? args[1] : "";

                if (e.CommandName == "Editar")
                {
                    ViewState["IdCategoriaEditar"] = idCategoria; // guardar en ViewState
                    txtModificarCategoria.Text = descripcion;
                    btnModificarCategoria.Visible = true;
                    btnCerrar.Visible = true;
                    txtModificarCategoria.Visible = true;
                }
                else if (e.CommandName == "Eliminar")
                {
                    eliminarCategoria(idCategoria);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        public void altaCategoria()
        {
            try
            {
                string nuevaCategoria = txtNuevaCategoria.Text.Trim();
                if (!string.IsNullOrEmpty(nuevaCategoria))
                {
                    CategoriaNegocio.altaCategoria(nuevaCategoria);
                    cargarGrilla(); // refresca la grilla
                    txtNuevaCategoria.Text = ""; // limpia el textbox
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }
        public void modificarCategoria(int id)
        {
            try
            {
                string nuevaDescripcion = txtModificarCategoria.Text.Trim();
                if (id != 0)
                {
                    if (!string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        if (CategoriaNegocio.modificarCategoria(id, nuevaDescripcion))
                        {
                            cargarGrilla(); // refresca la grilla
                            txtModificarCategoria.Text = ""; // limpia el textbox
                            btnModificarCategoria.Visible = false;
                            btnCerrar.Visible = false;
                            txtModificarCategoria.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                            "alerta", "alert('Ya existe una categoria con ese nombre!.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(),
                        "alerta", "alert('Debe completar el campo con el nuevo nombre y luego presionar aceptar!.');", true);
                    }

                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }
        public void eliminarCategoria(int id)
        {
            try
            {
                if (!CategoriaNegocio.eliminarCategoria(id))
                {
                    // ¡Invocamos el modal premium que armamos en el HTML en lugar del alert clásico!
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "$('#modalAviso').modal('show');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alerta", "alert('La categoría se ha eliminado correctamente.');", true);
                    cargarGrilla();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }
        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            altaCategoria();
        }

        protected void btnModificarCategoria_Click(object sender, EventArgs e)
        {
            if (ViewState["IdCategoriaEditar"] != null)
            {
                int id = (int)ViewState["IdCategoriaEditar"];
                modificarCategoria(id);
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            btnCerrar.Visible = false;
            btnModificarCategoria.Visible = false;
            txtModificarCategoria.Visible = false;
        }
    }
}