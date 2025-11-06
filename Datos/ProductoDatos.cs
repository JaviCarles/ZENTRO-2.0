using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public static class ProductoDatos
    {
        #region Obtener listado de productos
        public static List<Producto> getListProductos(string filtro, string orden)
        {
            List<Producto> list = new List<Producto>();             
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                string columnaOrden = "ORDER BY Nombre"; // valor por defecto

                switch (orden)
                {
                    case "NOMBRE":
                        columnaOrden = "ORDER BY Nombre";
                        break;
                    case "CATEGORIA":
                        columnaOrden = "ORDER BY DescCat";
                        break;
                    case "MARCA":
                        columnaOrden = "ORDER BY DescMar";
                        break;
                    case "PRECIO":
                        columnaOrden = "ORDER BY Precio";
                        break;
                }
                // Si "filtro" es nulo o vacío, devolverá toda la lista de productos; en caso contrario, se filtrará según "filtro"
                if (string.IsNullOrEmpty(filtro))
                    accesoDatos.setearConsulta("SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion as DescPro, a.IdMarca, m.Descripcion as DescMar, a.IdCategoria, c.Descripcion as DescCat, a.ImagenUrl, a.Precio " +
                        " FROM Articulos a " +
                        " INNER JOIN Categorias c ON a.IdCategoria = c.Id " + 
                        " INNER JOIN Marcas m ON a.IdMarca = m.Id " +
                        columnaOrden);
                else
                {
                    accesoDatos.setearConsulta("SELECT a.Id, a.Codigo, a.Nombre, a.Descripcion as DescPro, a.IdMarca, m.Descripcion as DescMar, a.IdCategoria, c.Descripcion as DescCat, a.ImagenUrl, a.Precio " +
                         " FROM Articulos a " +
                         " INNER JOIN Categorias c ON a.IdCategoria = c.Id " +
                         " INNER JOIN Marcas m ON a.IdMarca = m.Id " +
                         " WHERE a.Nombre LIKE '%' + @filtro + '%' OR a.Codigo LIKE '%' + @filtro + '%' OR m.Descripcion LIKE '%' + @filtro + '%' OR c.Descripcion LIKE '%' + @filtro + '%' " +
                         columnaOrden);
                    accesoDatos.agregarParametro("@filtro", filtro); // Elimina espacios adicionales
                }

                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    Producto aux = new Producto
                    {
                        // De esta manera evitaremos errores en la carga de la grilla
                        Id = accesoDatos.Lector["Id"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["Id"]) : 0,
                        Nombre = accesoDatos.Lector["Nombre"] != DBNull.Value ? accesoDatos.Lector["Nombre"].ToString() : null,
                        Codigo = accesoDatos.Lector["Codigo"] != DBNull.Value ? accesoDatos.Lector["Codigo"].ToString() : null,
                        ImagenUrl = accesoDatos.Lector["ImagenUrl"] != DBNull.Value ? accesoDatos.Lector["ImagenUrl"].ToString() : null,
                        Precio = accesoDatos.Lector["Precio"] != DBNull.Value ? Convert.ToDecimal(accesoDatos.Lector["Precio"]) : 0,
                        IdMarca = accesoDatos.Lector["IdMarca"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["IdMarca"]) : 0,
                        IdCategoria = accesoDatos.Lector["IdCategoria"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["IdCategoria"]) : 0,
                        Categoria = accesoDatos.Lector["DescCat"] != DBNull.Value ? accesoDatos.Lector["DescCat"].ToString() : null,
                        Marca = accesoDatos.Lector["DescMar"] != DBNull.Value ? accesoDatos.Lector["DescMar"].ToString() : null,
                        Descripcion = accesoDatos.Lector["DescPro"] != DBNull.Value ? accesoDatos.Lector["DescPro"].ToString() : null
                    };
                    list.Add(aux);

                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

        }

        #endregion Obtener listado de productos

        #region Obtener listado de Categorias
        public static List<Categoria> getListCategorias()
        {
            List<Categoria> list = new List<Categoria>();

            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("SELECT Id, Descripcion FROM Categorias");
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    Categoria aux = new Categoria                  
                    {
                        // De esta manera evitaremos errores en la carga de la grilla
                        Id = accesoDatos.Lector["Id"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["Id"]) : 0,
                        Descripcion = accesoDatos.Lector["Descripcion"] != DBNull.Value ? accesoDatos.Lector["Descripcion"].ToString() : null,
                    };
                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return list;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        #endregion Obtener listado de Categorias

        #region Obtener listado de Marcas
        public static List<Marca> getListMarcas()
        {
            List<Marca> list = new List<Marca>();

            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("SELECT Id, Descripcion FROM Marcas");
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    Marca aux = new Marca
                    {
                        // De esta manera evitaremos errores en la carga de la grilla
                        Id = accesoDatos.Lector["Id"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["Id"]) : 0,
                        Descripcion = accesoDatos.Lector["Descripcion"] != DBNull.Value ? accesoDatos.Lector["Descripcion"].ToString() : null,
                    };
                    list.Add(aux);
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return list;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        #endregion Obetener listado de Marcas

        #region Insertar Producto
        public static void insertProducto(Producto producto)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (producto != null)
                {
                    accesoDatos.setearConsulta("Insert Into Articulos " +
                        "Values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Imagen, @Precio)");

                    accesoDatos.agregarParametro("@Nombre",producto.Nombre);
                    accesoDatos.agregarParametro("@Codigo",producto.Codigo);
                    accesoDatos.agregarParametro("@IdMarca",producto.IdMarca);
                    accesoDatos.agregarParametro("@IdCategoria",producto.IdCategoria);
                    accesoDatos.agregarParametro("@Descripcion",producto.Descripcion);
                    accesoDatos.agregarParametro("@Precio",producto.Precio);
                    accesoDatos.agregarParametro("@Imagen", producto.ImagenUrl);
                    accesoDatos.ejecutarAccion();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

        }
        #endregion Insertar Producto

        #region EDITAR PRODUCTO
        public static void updateProducto(Producto producto)
        {
            if (producto != null)
            {
                AccesoDatos accesoDatos = new AccesoDatos(); // Instanciamos un objeto accesoDatos.
                try
                {
                    // Consulta SQL para actualizar los datos del mueble en la base de datos.
                    accesoDatos.setearConsulta("UPDATE Articulos " +
                                                "SET Nombre = @Nombre, " +
                                                "Codigo = @Codigo, " +
                                                "IdMarca = @IdMarca, " +
                                                "IdCategoria = @IdCategoria, " +
                                                "Descripcion = @Descripcion, " +
                                                "ImagenUrl = @Imagen, " +
                                                "Precio = @Precio " +
                                                "WHERE Id = @Id");

                    // Asignamos los valores correspondientes a los parámetros.
                    accesoDatos.agregarParametro("@Nombre", producto.Nombre);
                    accesoDatos.agregarParametro("@Codigo", producto.Codigo);
                    accesoDatos.agregarParametro("@IdMarca", producto.IdMarca);
                    accesoDatos.agregarParametro("@IdCategoria", producto.IdCategoria);
                    accesoDatos.agregarParametro("@Descripcion", producto.Descripcion);
                    accesoDatos.agregarParametro("@Precio", producto.Precio);
                    accesoDatos.agregarParametro("@Imagen", producto.ImagenUrl);
                    accesoDatos.agregarParametro("@Id", producto.Id);

                    // Ejecutamos la acción en la base de datos.
                    accesoDatos.ejecutarAccion();
                }
                catch (Exception ex)
                {
                    // Propaga la excepción para que la maneje la capa superior.
                    throw new Exception("Error al actualizar el producto en la base de datos.", ex);
                }
                finally
                {
                    // Cierra la conexión a la base de datos en el bloque finally.
                    accesoDatos.cerrarConexion();
                }
            }
        }
        #endregion EDITAR PRODUCTO

        #region ELIMINAR PRODUCTO

        public static void deleteProducto(Producto producto) //La eliminación será física
        {
            if (producto != null)
            {
                AccesoDatos accesoDatos = new AccesoDatos(); //Instanciamos un objeto accesoDatos
                try
                {
                    accesoDatos.setearConsulta(" DELETE FROM ARTICULOS WHERE id = @Id ");

                    accesoDatos.agregarParametro("@Id", producto.Id); //método que asigna parámetro

                    accesoDatos.ejecutarAccion();
                }
                catch (Exception ex)
                {
                    // Propaga la excepción para que la maneje la capa superior.
                    throw new Exception(ex.Message,ex);
                }
                finally
                {
                    accesoDatos.cerrarConexion();
                }
            }
        }

        #endregion ELIMINAR PRODUCTO

    }
}
