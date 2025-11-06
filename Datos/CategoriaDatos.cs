using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class CategoriaDatos
    {
        #region LISTAR CATEGORIAS
        public static List<Categoria> getListCategorias(string filtro)
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (string.IsNullOrEmpty(filtro))
                    accesoDatos.setearConsulta("Select Id, Descripcion From CATEGORIAS ORDER BY Descripcion");
                else
                {
                    accesoDatos.setearConsulta(" Select Id, Descripcion From CATEGORIAS WHERE Descripcion LIKE '%' + @filtro + '%' ORDER BY Descripcion");
                    accesoDatos.agregarParametro("@filtro", filtro);
                }
                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    Categoria aux = new Categoria
                    {
                        // De esta manera evitaremos errores en la carga de la grilla
                        Id = accesoDatos.Lector["Id"] != DBNull.Value ? Convert.ToInt32(accesoDatos.Lector["Id"]) : 0,
                        Descripcion = accesoDatos.Lector["descripcion"] != DBNull.Value ? accesoDatos.Lector["Descripcion"].ToString() : null,
                    };
                    lista.Add(aux);

                }

                return lista;

            }
            catch (Exception ex)
            {
                // Propaga la excepción para que la maneje la capa superior.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        #endregion LISTAR CATEGORIAS

        #region ALTA CATEGORIA
        public static bool insertCategoria(string descripcion)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!existeCategoria(descripcion))
                {
                    accesoDatos.setearConsulta("INSERT into CATEGORIAS VALUES (@Descripcion)");
                    accesoDatos.agregarParametro("@Descripcion", descripcion);
                    accesoDatos.ejecutarLectura();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Propaga la excepción para que la maneje la capa superior.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        public static bool existeCategoria(string descripcion)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!string.IsNullOrEmpty(descripcion))
                {
                    accesoDatos.setearConsulta("Select Descripcion From CATEGORIAS WHERE Descripcion = @Descripcion ");
                    accesoDatos.agregarParametro("@Descripcion", descripcion);
                    accesoDatos.ejecutarLectura();
                    if (accesoDatos.Lector.Read())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                // Propaga la excepción para que la maneje la capa superior.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        #endregion ALTA CATEGORIA

        #region MODIFICAR CATEGORIA
        public static bool updateCategoria(Categoria seleccionada, string descripcion)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!string.IsNullOrEmpty(descripcion))
                {
                    if (!existeCategoria(descripcion))
                    {
                        accesoDatos.setearConsulta(" UPDATE CATEGORIAS " +
                        " SET Descripcion = @Descripcion " +
                        "WHERE Id = @Id");
                        accesoDatos.agregarParametro("@Descripcion", descripcion);
                        accesoDatos.agregarParametro("@Id", seleccionada.Id);
                        accesoDatos.ejecutarAccion();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                // Propaga la excepción para que la maneje la capa superior.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
        #endregion MODIFICAR CATEGORIA

        #region ELIMINAR CATEGORIA
        public static bool deleteCategoria(Categoria categoria)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!seUsaCategoria(categoria))//Verificamos si la categoria esta en uso.
                {
                    accesoDatos.setearConsulta(" DELETE CATEGORIAS WHERE Id = @Id ");
                    accesoDatos.agregarParametro("@Id", categoria.Id);
                    accesoDatos.ejecutarAccion();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }

        public static bool seUsaCategoria(Categoria categoria)//Verifica si se utiliza la categoria en al menos un artículo.
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta(" SELECT IdCategoria FROM ARTICULOS WHERE IdCategoria = @IdCategoria ");
                accesoDatos.agregarParametro("@IdCategoria", categoria.Id);
                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Propaga la excepción para que la maneje la capa superior.
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }
        }
    }
    #endregion ELIMINAR CATEGORIA
}

