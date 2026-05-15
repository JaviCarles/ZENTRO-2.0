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
    public partial class AdminMarca : System.Web.UI.Page
    {
        public Usuario usuario;
        int idMarca;
        public List<Marca> marcas = new List<Marca>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] != null)
            {
                usuario = (Usuario)Session["usuario"];
            }
            cargarGrilla();
        }

        #region CARGAR GRILLA
        public void cargarGrilla()
        {
            try
            {
                marcas = MarcaNegocio.listaMarcas();
                if (marcas != null && marcas.Count > 0)
                {
                    dgvMarcas.DataSource = marcas;
                    dgvMarcas.DataBind();
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
        protected void dgvMarcas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                idMarca = Convert.ToInt32(args[0]);
                string descripcion = args.Length > 1 ? args[1] : "";

                if (e.CommandName == "Editar")
                {
                    ViewState["IdMarcaEditar"] = idMarca; // guardar en ViewState
                    txtModificarMarca.Text = descripcion;
                    btnModificarMarca.Visible = true;
                    btnCerrar.Visible = true;
                    txtModificarMarca.Visible = true;
                }
                else if (e.CommandName == "Eliminar")
                {
                    eliminarMarca(idMarca);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        public void altaMarca()
        {
            try
            {
                string nuevaMarca = txtNuevaMarca.Text.Trim();
                if (!string.IsNullOrEmpty(nuevaMarca))
                {
                    MarcaNegocio.altaMarca(nuevaMarca);
                    cargarGrilla(); // refresca la grilla
                    txtNuevaMarca.Text = ""; // limpia el textbox
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }

        public void modificarMarca(int id)
        {
            try
            {
                string nuevaDescripcion = txtModificarMarca.Text.Trim();
                if (id != 0)
                {
                    if (!string.IsNullOrEmpty(nuevaDescripcion))
                    {
                        if (MarcaNegocio.modificarMarca(id, nuevaDescripcion))
                        {
                            cargarGrilla(); // refresca la grilla
                            txtModificarMarca.Text = ""; // limpia el textbox
                            btnModificarMarca.Visible = false;
                            btnCerrar.Visible = false;
                            txtModificarMarca.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(),
                            "alerta", "alert('Ya existe una marca con ese nombre!.');", true);
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

        public void eliminarMarca(int id)
        {
            try
            {
                if (!MarcaNegocio.eliminarMarca(id))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alerta", "alert('La marca no puede ser eliminada porque está asociada a productos.');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),
                        "alerta", "alert('La marca se ha eliminado correctamente.');", true);
                    cargarGrilla();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }
        protected void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            altaMarca();
        }

        protected void btnModificarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["IdMarcaEditar"] != null)
                {
                    int id = (int)ViewState["IdMarcaEditar"];
                    modificarMarca(id);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx");
            }
        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            btnCerrar.Visible = false;
            btnModificarMarca.Visible = false;
            txtModificarMarca.Visible = false;
        }
    }
}