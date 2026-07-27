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
using Amazon.S3;
using Amazon.S3.Model;
using Servicios;
using System.IO;
using Microsoft.SqlServer.Server;
using System.Threading.Tasks;
using System.Configuration;


namespace Presentacion
{
    public partial class AltaProducto : System.Web.UI.Page
    {
        #region LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Form != null)
            {
                Page.Form.Enctype = "multipart/form-data";
            }

            txtId.Enabled = false;

            if (!IsPostBack)
            {
                AppSettingsExiste();
                ScriptManager1.RegisterPostBackControl(btnAceptar);

                cargarCategorias();
                cargarMarcas();

                // CAPTURA ESTABLE DESDE EL QUERYSTRING
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int id))
                {
                    // Buscamos el producto real con el ID de la URL
                    Producto prodDb = obtenerProducto(id);

                    if (prodDb != null)
                    {
                        // Guardamos el objeto en el ViewState local de ESTA pestaña/página
                        ViewState["ProductoOriginal"] = prodDb;

                        lblTitulo.Text = "MODIFICAR PRODUCTO";
                        cargarDesdeDb(); // Va a leer del ViewState ahora
                    }
                }
                else
                {
                    // Es un Alta: aseguramos que no haya basura en el ViewState
                    ViewState["ProductoOriginal"] = null;
                    btnEliminar.Visible = false;
                }
            }
        }
        #endregion LOAD

        #region APPSETTINGS EXISTE?

        private void AppSettingsExiste()
        {
            //  Mapeamos la ruta del archivo AppSettings.config en el servidor
            string rutaConfig = Server.MapPath("~/AppSettings.config");

            //  Si NO existe el archivo, bloqueamos el control
            if (!File.Exists(rutaConfig))
            {
                // Deshabilitamos el control FileUpload
                CargarImagen.Enabled = false;

                // Le añadimos un mensaje
                lblCargarImagen.Text = "⚠️ La opción de subir imágenes a la nube no está configurada en este entorno..";

               
            }
        }

        #endregion APPSETTINGS EXISTE?

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

        #region Url Imagen Asincrona

        // Cambiamos la firma para que sea un método asíncrono nativo
        //Ahora la firma recibe el Stream y el nombre capturados en el hilo principal
        public async Task<string> UrlImagenCloudflareAsync(Stream fileStream, string fileName)
        {
            // Evaluamos el Stream que entra por parámetro, ya no miramos el control de la pantalla
            if (fileStream != null && fileStream.Length > 0)
            {
                // Instanciamos el servicio que está en tu biblioteca de servicios
                var r2Service = new CloudflareR2Service();

                // EJECUCIÓN SEGURA: Le pasamos el flujo aislado directamente al SDK
                string imageUrl = await r2Service.UploadImageAsync(fileStream, fileName);

                // Retornamos la URL final entregada por Cloudflare R2
                return imageUrl;
            }

            // Si no vino ningún archivo válido en el parámetro, retornamos null
            return null;
        }
        #endregion Url Imagen Asincrona

        #region BOTON ACEPTAR

        // El botón ACEPTAR ahora es asíncrono para poder procesar la subida a la nube
        protected async void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                // Chequeamos el ViewState para decidir si es Modificación o Alta
                Producto original = (Producto)ViewState["ProductoOriginal"];

                if (original != null && original.Id != 0)
                {
                    await modificarProductoAsync(original);
                }
                else
                {
                    await altaProductoAsync();
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString(); // Esto está bien para la página de error global
                Response.Redirect("Error.aspx", false);
            }
        }
        #endregion BOTON ACEPTAR

        #region MAPEAR DATOS
        // Devuelve un producto mapeado desde el formulario (excluyendo la imagen que es asíncrona)
        public Producto mapearDatos()
        {
            Producto producto = new Producto();

            // Evaluamos si es Modificación chequeando si guardamos algo en el ViewState
            Producto original = (Producto)ViewState["ProductoOriginal"];
            if (original != null && original.Id != 0)
            {
                producto.Id = original.Id;
            }

            producto.Codigo = txtCodigo.Text;
            producto.Nombre = txtNombre.Text;
            producto.Descripcion = textAreaDescripcion.Text;
            producto.ImagenUrl = string.Empty;
            producto.Marca = new Marca { Id = int.Parse(DropDownListMarca.SelectedValue) };
            producto.Categoria = new Categoria { Id = int.Parse(DropDownListCategoria.SelectedValue) };
            producto.Precio = decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture);

            return producto;
        }
        #endregion MAPEAR DATOS

        #region Alta Producto Async
        public async Task altaProductoAsync()
        {
            // 1. Cláusula de guarda corregida (una sola y limpia)
            if (!validarCampos()) return;

            string urlImagen = null;

            try
            {
                // 2. Resolvemos de dónde viene la imagen
                if (!string.IsNullOrWhiteSpace(txtImagen.Text))
                {
                    urlImagen = txtImagen.Text.Trim();
                }
                else if (CargarImagen.HasFile)
                {
                    // 🔥 CAPTURA EN CALIENTE: Guardamos los bytes y el nombre en el hilo principal
                    // antes de que el 'await' congele la ejecución.
                    Stream flujoArchivo = CargarImagen.FileContent;
                    string nombreArchivo = Path.GetFileName(CargarImagen.FileName);

                    // Le pasamos las variables locales seguras al método asíncrono
                    urlImagen = await UrlImagenCloudflareAsync(flujoArchivo, nombreArchivo);
                }

                // 3. Mapeamos el producto e inyectamos la URL que devolvió Cloudflare
                Producto nuevoProducto = mapearDatos();
                nuevoProducto.ImagenUrl = urlImagen;

                // 4. Guardamos en la Base de Datos
                await ProductoNegocio.altaProductoAsync(nuevoProducto);
                //ProductoNegocio.altaProducto(nuevoProducto);
            }
            catch (Exception ex)
            {
                // Si Cloudflare falla o la BD explota, cae acá garantizado
                Session["error"] = "Error en el alta del producto: " + ex.Message + " | Detalle interno: " + ex.InnerException?.Message;
                Response.Redirect("Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // 5. Redirección de éxito si todo salió perfecto
            Response.Redirect("AdminProducto.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        //ESTA LOGICA ANDA SOLO CON CLOUDFLARE
        //// Lógica de Alta transformada a Asíncrona
        //public async Task altaProductoAsync()
        //{
        //    if (validarCampos())
        //    {
        //        try
        //        {
        //            Producto nuevoProducto = mapearDatos();

        //            // Si el usuario seleccionó una imagen, la subimos a Cloudflare antes de guardar
        //            if (CargarImagen.HasFile)
        //            {
        //                nuevoProducto.ImagenUrl = await UrlImagenCloudflareAsync();
        //            }

        //            // Guardamos en Base de Datos
        //            ProductoNegocio.altaProducto(nuevoProducto);
        //            Response.Redirect("AdminProducto.aspx", false);
        //        }
        //        catch (Exception ex)
        //        {
        //            Session["error"] = ex.ToString();
        //            Response.Redirect("Error.aspx", false);
        //        }
        //    }
        //}

        #endregion Alta Producto Async

        #region OBTENER PRODUCTO
        public Producto obtenerProducto(int id)//Devuelve el producto correspondiente al id.
        {
            Producto producto;
            try
            {
                producto = new Producto();
                producto = ProductoNegocio.obtenerProducto(id);
                return producto;
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
                return null;
            }
        }
        #endregion OBTENER PRODUCTO

        #region Se modifico Campo??
        public bool seModificoCampo(Producto producto)
        {
            if (producto.Codigo != txtCodigo.Text) return true;
            if (producto.Nombre != txtNombre.Text) return true;
            if (producto.Descripcion != textAreaDescripcion.Text) return true;
            if (producto.Marca.Id != int.Parse(DropDownListMarca.SelectedValue)) return true;
            if (producto.Categoria.Id != int.Parse(DropDownListCategoria.SelectedValue)) return true;
            if (producto.Precio != decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture)) return true;
            if (CargarImagen.HasFile) return true;

            // NUEVO: Si la URL manual es distinta a la que tenía el producto, se considera una modificación
            if (!string.IsNullOrWhiteSpace(txtImagen.Text) && producto.ImagenUrl != txtImagen.Text.Trim()) return true;

            //Si ninguno de los campos se modificó, devuelve false.
            return false;
        }
        //ESTA VERSIÓN ES LA QUE FUNCIONA SOLO CON CLOUDFLARE
        //public bool seModificoCampo(Producto producto)
        //{
        //    if (producto.Codigo != txtCodigo.Text) return true;
        //    if (producto.Nombre != txtNombre.Text) return true;
        //    if (producto.Descripcion != textAreaDescripcion.Text) return true;
        //    if (producto.Marca.Id != int.Parse(DropDownListMarca.SelectedValue)) return true;
        //    if (producto.Categoria.Id != int.Parse(DropDownListCategoria.SelectedValue)) return true;
        //    if (producto.Precio != decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture)) return true;
        //    if (CargarImagen.HasFile) return true;

        //    //Si ninguno de los campos se modificó, devuelve false.
        //    return false;
        //}
        // Lógica de Modificación transformada a Asíncrona
        // Lógica de Modificación Asíncrona con limpieza de imágenes viejas

        #endregion Se modifico campo??

        #region Modificar Producto Async

        // Lógica de Modificación Asíncrona con limpieza de imágenes viejas
        public async Task modificarProductoAsync(Producto productoOriginal)
        {
            if (productoOriginal != null)
            {
                if (seModificoCampo(productoOriginal))
                {
                    if (validarCampos())
                    {
                        try
                        {
                            Producto productoModificado = mapearDatos();

                            // CASE 1: Si se ingresó o cambió una URL manual de texto
                            if (!string.IsNullOrWhiteSpace(txtImagen.Text) && txtImagen.Text.Trim() != productoOriginal.ImagenUrl)
                            {
                                string urlImagenVieja = productoOriginal.ImagenUrl;
                                productoModificado.ImagenUrl = txtImagen.Text.Trim();

                                // Si pones un link manual nuevo y había una foto en tu R2, la borramos para no acumular basura
                                if (!string.IsNullOrEmpty(urlImagenVieja) && urlImagenVieja.Contains("cloudflarestorage.com"))
                                {
                                    var r2Service = new CloudflareR2Service();
                                    await r2Service.DeleteImageAsync(urlImagenVieja);
                                }
                            }
                            // CASE 2: Si el usuario seleccionó un archivo nuevo desde el FileUpload
                            else if (CargarImagen.HasFile)
                            {
                                string urlImagenVieja = productoOriginal.ImagenUrl;

                                // 🔥 CAPTURA CRÍTICA EN CALIENTE: Aseguramos el archivo antes de liberar el hilo con el await
                                Stream flujoArchivo = CargarImagen.FileContent;
                                string nombreArchivo = Path.GetFileName(CargarImagen.FileName);

                                // Llamamos al método pasándole las variables locales aisladas
                                productoModificado.ImagenUrl = await UrlImagenCloudflareAsync(flujoArchivo, nombreArchivo);

                                // Si la subida fue exitosa y existía una imagen previa en R2, la limpiamos
                                if (!string.IsNullOrEmpty(urlImagenVieja) && productoModificado.ImagenUrl != null && urlImagenVieja.Contains("cloudflarestorage.com"))
                                {
                                    var r2Service = new CloudflareR2Service();
                                    await r2Service.DeleteImageAsync(urlImagenVieja);
                                }
                            }
                            // CASE 3: Si no se tocó la imagen, mantenemos la que ya tenía el producto original
                            else
                            {
                                productoModificado.ImagenUrl = productoOriginal.ImagenUrl;
                            }

                            // Salvaguarda: si por algún motivo la subida falló (devolvió null) pero había un archivo, 
                            // no pisamos con null la base de datos, conservamos la que estaba.
                            if (productoModificado.ImagenUrl == null && CargarImagen.HasFile)
                            {
                                productoModificado.ImagenUrl = productoOriginal.ImagenUrl;
                            }

                            // Guardamos los cambios definitivos en SQL Server
                            ProductoNegocio.editarProducto(productoModificado);

                            // Limpieza de sesión y redirección segura para WebForms
                            Session["idProducto"] = null;
                            Session["Producto"] = null;
                            Response.Redirect("AdminProducto.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                        catch (Exception ex)
                        {
                            Session["error"] = "Error al intentar modificar el producto: " + ex.Message;
                            Response.Redirect("Error.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                        }
                    }
                }
                else
                {
                    lblError.ForeColor = System.Drawing.Color.DarkOrange;
                    lblError.Text = "No se modificó ningún campo, presione 'Cancelar'.";
                }
            }
        }

        //ESTA LOGICA ANDA SOLO CON CLOUDFLARE
        //public async Task modificarProductoAsync(Producto productoOriginal)
        //{
        //    if (productoOriginal != null)
        //    {
        //        if (seModificoCampo(productoOriginal))
        //        {
        //            if (validarCampos())
        //            {
        //                try
        //                {
        //                    Producto productoModificado = mapearDatos();

        //                    // Si el usuario seleccionó un archivo nuevo...
        //                    if (CargarImagen.HasFile)
        //                    {
        //                        // 1. OBLIGATORIO: Guardamos la URL de la imagen vieja antes de pisar el objeto
        //                        string urlImagenVieja = productoOriginal.ImagenUrl;

        //                        // 2. Subimos la nueva imagen a Cloudflare R2
        //                        productoModificado.ImagenUrl = await UrlImagenCloudflareAsync();

        //                        // 3. LIMPIEZA: Si existía una imagen anterior en la nube, la borramos para no acumular basura
        //                        if (!string.IsNullOrEmpty(urlImagenVieja))
        //                        {
        //                            var r2Service = new CloudflareR2Service();
        //                            await r2Service.DeleteImageAsync(urlImagenVieja);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Si NO subió un archivo nuevo, conservamos la URL de la imagen existente
        //                        productoModificado.ImagenUrl = productoOriginal.ImagenUrl;
        //                    }

        //                    // Guardamos los cambios en la Base de Datos SQL Server
        //                    ProductoNegocio.editarProducto(productoModificado);

        //                    // Limpieza de sesión y redirección
        //                    Session["idProducto"] = null;
        //                    Session["Producto"] = null;
        //                    Response.Redirect("AdminProducto.aspx", false);
        //                }
        //                catch (Exception ex)
        //                {
        //                    Session["error"] = ex.ToString();
        //                    Response.Redirect("Error.aspx", false);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            lblError.Text = "No se modificó ningún campo, presione 'Cancelar'.";
        //        }
        //    }
        //}

        #endregion Modificar Producto Async

        #region VALIDACIONES

        public bool validarCampos()
        {
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) ||
                string.IsNullOrEmpty(DropDownListMarca.SelectedValue) || string.IsNullOrEmpty(DropDownListCategoria.SelectedValue) ||
                string.IsNullOrEmpty(txtPrecio.Text))
            {
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = "Todos los campos de texto son obligatorios.";
                return false;
            }

            // Ya no miramos la Session. Miramos el ViewState.
            bool esAlta = (ViewState["ProductoOriginal"] == null);

            if (esAlta && !CargarImagen.HasFile && string.IsNullOrWhiteSpace(txtImagen.Text))
            {
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = "Debes seleccionar una imagen o ingresar una URL para el nuevo producto.";
                return false;
            }

            return true;
        }

        // ESTA VERSIÓN ES LA QUE ANDA SOLO CON CLOUDFLARE

        //public bool validarCampos()
        //{
        //    // 1. Validamos primero los campos de texto comunes que SIEMPRE son obligatorios
        //    if (string.IsNullOrEmpty(txtCodigo.Text) ||
        //        string.IsNullOrEmpty(txtNombre.Text) ||
        //        string.IsNullOrEmpty(DropDownListMarca.SelectedValue) ||
        //        string.IsNullOrEmpty(DropDownListCategoria.SelectedValue) ||
        //        string.IsNullOrEmpty(txtPrecio.Text))
        //    {
        //        lblError.ForeColor = System.Drawing.Color.Red;
        //        lblError.BorderStyle = BorderStyle.Solid;
        //        lblError.Text = "Todos los campos de texto son obligatorios.";
        //        return false;
        //    }

        //    //La imagen solo es obligatoria si es un ALTA (nuevo producto)
        //    bool esAlta = (Session["idProducto"] == null || (int)Session["idProducto"] == 0);
        //    if (esAlta && !CargarImagen.HasFile)
        //    {
        //        lblError.ForeColor = System.Drawing.Color.Red;
        //        lblError.BorderStyle = BorderStyle.Solid;
        //        lblError.Text = "Debes seleccionar una imagen para el nuevo producto.";
        //        return false;
        //    }

        //    return true;
        //}
        #endregion VALIDACIONES

        #region CARGAR DESDE DB
        public void cargarDesdeDb()
        {
            // Leemos del ViewState local
            Producto producto = (Producto)ViewState["ProductoOriginal"];

            if (producto != null)
            {
                txtId.Text = producto.Id.ToString();
                txtCodigo.Text = producto.Codigo;
                txtNombre.Text = producto.Nombre;
                textAreaDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.Precio.ToString("F0", new System.Globalization.CultureInfo("es-AR"));
                imgProducto.ImageUrl = producto.ImagenUrl;
                txtImagen.Text = producto.ImagenUrl;
                DropDownListCategoria.SelectedValue = producto.Categoria.Id.ToString();
                DropDownListMarca.SelectedValue = producto.Marca.Id.ToString();
            }
        }

        #endregion CARGAR DESDE DB

        #region BOTON CANCELAR
        public void btnCancelar_Click(object sender, EventArgs e)
        {
            //Al irse de la página, el ViewState muere solo, a diferencia de SESSION que hay que limpiarlo.
            Response.Redirect("AdminProducto.aspx", false);
        }

        #endregion BOTON CANCELAR

        #region BOTON ELIMINAR
        protected async void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                // Recuperamos el producto desde el ViewState
                Producto productoAEliminar = (Producto)ViewState["ProductoOriginal"];

                if (productoAEliminar != null)
                {
                    var r2Service = new CloudflareR2Service();

                    if (!string.IsNullOrEmpty(productoAEliminar.ImagenUrl))
                    {
                        await r2Service.DeleteImageAsync(productoAEliminar.ImagenUrl);
                    }

                    ProductoNegocio.eliminarProducto(productoAEliminar);
                }

                Response.Redirect("AdminProducto.aspx", false);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

        #endregion BOTON ELIMINAR
    }
}