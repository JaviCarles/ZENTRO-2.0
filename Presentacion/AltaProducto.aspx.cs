using Entidades;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion
{
    public partial class AltaProducto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtId.Enabled = false;
            if (!IsPostBack)
            {
                cargarCategorias();
                cargarMarcas();
                if (Request.QueryString["id"] != null)//Si request recibe id es porque es modificación.
                {
                    Session["idProducto"] = Convert.ToInt32(Request.QueryString["id"]);//Guardamos en session el id recibido.                   
                    Session["Producto"] = obtenerProducto((int)Session["idProducto"]);// cargarProducto devuelve un Producto desde la db.
                    lblTitulo.Text = "MOFIDICAR PRODUCTO";
                    cargarDesdeDb();//carga los campos con el Prodcuto traído desde la DB
                }
                else
                    btnEliminar.Visible = false;
            }
        }
        #region CARGAR DESPLEGABLE CATEGORIA
        public void cargarCategorias()
        {
            try
            {
                List<Categoria> categorias = new List<Categoria>();
                categorias = CategoriaNegocio.listaCategorias();
                if (categorias != null && categorias.Count > 0)
                {
                    DropDownListCategoria.DataSource = categorias;
                    DropDownListCategoria.DataTextField = "Descripcion";
                    DropDownListCategoria.DataValueField = "Id";
                    DropDownListCategoria.DataBind();
                }
                else
                {
                    // Manejo de error o mensaje al usuario
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion CARGAR DESPLEGABLE CATEGORIA

        #region CARGAR DESPLEGABLE MARCA
        public void cargarMarcas()
        {
            try
            {
                List<Marca> marcas = new List<Marca>();
                marcas = MarcaNegocio.listaMarcas();
                if (marcas != null && marcas.Count > 0)
                {
                    DropDownListMarca.DataSource = marcas;
                    DropDownListMarca.DataTextField = "Descripcion";
                    DropDownListMarca.DataValueField = "Id";
                    DropDownListMarca.DataBind();
                }
                else
                {
                    // Manejo de error o mensaje al usuario
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion CARGAR DESPLEGABLE MARCA

        protected void txtImagen_TextChanged(object sender, EventArgs e)
        {
            cargarImg();
        }
        public void cargarImg()
        {
            imgProducto.ImageUrl = txtImagen.Text;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Session["idProducto"] != null && (int)Session["idProducto"] != 0)
            {
                modificarProducto((Producto)Session["Producto"]);//Aquí usamos el producto guardado en session
            }
            else
                altaProducto(); // Si request no recibe id, no entrará en el if porque se trata de un alta de producto.
        }
        public Producto mapearDatos()//devuelve un producto mapaedo desde el formulario.
        {
            // Construcción del objeto
            Producto producto = new Producto();
            //Si es modificación, el id estará gusrdado es Session.
            if (Session["idProducto"] != null && (int)Session["idProducto"] != 0)
                producto.Id = int.Parse(txtId.Text);
            producto.Codigo = txtCodigo.Text;
            producto.Nombre = txtNombre.Text;
            producto.Descripcion = textAreaDescripcion.Text;
            producto.ImagenUrl = txtImagen.Text;
            producto.Marca = new Marca { Id = int.Parse(DropDownListMarca.SelectedValue) };
            producto.Categoria = new Categoria { Id = int.Parse(DropDownListCategoria.SelectedValue) };
            producto.Precio = decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture);

            return producto;
        }
        public void altaProducto()
        {
            if (validarCampos())
            {
                try
                {
                    // Lógica de negocio
                    ProductoNegocio.altaProducto(mapearDatos());//mapearDatos() devuelve un producto cargado con los datos del formulario.
                    Response.Redirect("AdminProducto.aspx", false);
                }
                catch (Exception ex)
                {
                    lblError.Text = "Ocurrió un error al guardar el producto.";
                }
            }
        }
        public Producto obtenerProducto(int id)//Devuelve el producto correspondiente al id.
        {
            Producto producto = new Producto();
            try
            {
                producto = ProductoNegocio.obtenerProducto(id);
                return producto;
            }
            catch (Exception ex)
            {
                return producto;
            }
        }
        public bool seModificoCampo(Producto producto)
        {
            if (producto.Codigo != txtCodigo.Text) return true;
            if (producto.Nombre != txtNombre.Text) return true;
            if (producto.Descripcion != textAreaDescripcion.Text) return true;
            if (producto.Marca.Id != int.Parse(DropDownListMarca.SelectedValue)) return true;
            if (producto.Categoria.Id != int.Parse(DropDownListCategoria.SelectedValue)) return true;
            if (producto.Precio != decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture)) return true;
            if (producto.ImagenUrl != txtImagen.Text) return true;
            //Si ninguno de los campos se modificó, devuelve false.
            return false;
        }
        public void modificarProducto(Producto producto)
        {
            if (producto != null) //Validamos existencia del producto.
            {
                if (seModificoCampo(producto))//Verificamos si se modificó al menos un campo.
                {
                    if (validarCampos())
                    {
                        try
                        {
                            ProductoNegocio.editarProducto(mapearDatos());//mapearDatos() devuelve un producto.
                            Session["idProducto"] = null;
                            Response.Redirect("AdminProducto.aspx", false);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                else
                    lblError.Text = "No se modificó ningún campo, presione 'Cancelar'.";
            }
        }
        #region VALIDACIONES
        public bool validarCampos()
        {
            // Validación agrupada
            if (string.IsNullOrEmpty(txtCodigo.Text) ||
                string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(txtImagen.Text) ||
                string.IsNullOrEmpty(DropDownListMarca.SelectedValue) ||
                string.IsNullOrEmpty(DropDownListCategoria.SelectedValue) ||
                string.IsNullOrEmpty(txtPrecio.Text))
            {
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.BorderStyle = BorderStyle.Solid;
                lblError.Text = "Todos los campos son obligatorios.";
                return false;
            }
            else
                return true;
        }
        #endregion VALIDACIONES
        /*public void buscarProducto(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }*/
        public void cargarDesdeDb()// carga los campos con el Prodcuto traído desde la DB
        {
            if ((Producto)Session["Producto"] != null)
            {
                Producto producto = (Producto)Session["Producto"];
                txtId.Text = producto.Id.ToString();
                txtCodigo.Text = producto.Codigo;
                txtNombre.Text = producto.Nombre;
                textAreaDescripcion.Text = producto.Descripcion;
                //Precio debe tener el siguiente formato para que el textBox html acepte decimal.
                txtPrecio.Text = producto.Precio.ToString(System.Globalization.CultureInfo.InvariantCulture);
                txtImagen.Text = producto.ImagenUrl;
                cargarImg();
                DropDownListCategoria.SelectedValue = producto.Categoria.Id.ToString();
                DropDownListMarca.SelectedValue = producto.Marca.Id.ToString();
            }
        }
        public void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Session["idProducto"] != null && (int)Session["idProducto"] != 0)
                Session["idProducto"] = null;
            Response.Redirect("AdminProducto.aspx", false);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                ProductoNegocio.eliminarProducto(mapearDatos());
                Session["idProducto"] = null;
                Response.Redirect("AdminProducto.aspx", false);
            }
            catch (Exception ex)
            {

            }
        }
    }
}