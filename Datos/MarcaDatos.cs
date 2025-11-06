using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class MarcaDatos
    {
        #region LISTAR MARCAS
        public static List<Marca> getListMarcas(string filtro)
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (string.IsNullOrEmpty(filtro))
                    accesoDatos.setearConsulta("Select Id, Descripcion From MARCAS ORDER BY Descripcion");
                else
                {
                    accesoDatos.setearConsulta(" Select Id, Descripcion From MARCAS WHERE Descripcion LIKE '%' + @filtro + '%' ORDER BY Descripcion");
                    accesoDatos.agregarParametro("@filtro", filtro);
                }
                accesoDatos.ejecutarLectura();
                while (accesoDatos.Lector.Read()) //Si lector.Read() es true ingresará en el while mapeando hasta la última linea de la consulta
                {
                    Marca aux = new Marca
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
        #endregion LISTAR MARCAS

        #region ALTA MARCA
        public static bool insertMarca(string descripcion)// Si se inserta la marca, retorna true
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!existeMarca(descripcion))
                {
                    accesoDatos.setearConsulta("INSERT into MARCAS VALUES (@Descripcion)");
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

        public static bool existeMarca(string descripcion)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!string.IsNullOrEmpty(descripcion))
                {
                    accesoDatos.setearConsulta("Select Descripcion From MARCAS WHERE Descripcion = @Descripcion ");
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
        #endregion ALTA MARCA

        #region MODIFICAR MARCA
        public static bool updateMarca(Marca seleccionada, string descripcion)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!string.IsNullOrEmpty(descripcion))
                {
                    if (!existeMarca(descripcion))
                    {
                        accesoDatos.setearConsulta(" UPDATE MARCAS " +
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
        #endregion MODIFICAR MARCA

        #region ELIMINAR MARCA
        public static bool deleteMarca(Marca marca)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                if (!seUsaMarca(marca))//Verificamos si la categoria esta en uso.
                {
                    accesoDatos.setearConsulta(" DELETE MARCAS WHERE Id = @Id ");
                    accesoDatos.agregarParametro("@Id", marca.Id);
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

        public static bool seUsaMarca(Marca marca)//Verifica si se utiliza la categoria en al menos un artículo.
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta(" SELECT IdMarca FROM ARTICULOS WHERE IdMarca = @IdMarca ");
                accesoDatos.agregarParametro("@IdMarca", marca.Id);
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
    #endregion ELIMINAR MARCA
}

