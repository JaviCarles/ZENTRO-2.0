using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlDataReader lector;
        private SqlCommand comando;

        public SqlDataReader Lector //Solo leemos el lector sin posibilidad de escribirlo
        {
            get { return lector; }
        }

        public AccesoDatos() // Constructor
        {
            conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            // conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            comando = new SqlCommand();
        }

        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }

        }
        public void agregarParametro(string nombreParametro, object valor)
        {
            try
            {
                if (comando == null)
                {
                    comando = new SqlCommand();
                }
                comando.Parameters.AddWithValue(nombreParametro, valor ?? DBNull.Value); // Manejo de valores nulos
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public int ejecutarEscalar() // este metodo nos devolverá el Id del objeto para luego poder insertarlo en los detalles en la misma consulta.
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                object resultado = comando.ExecuteScalar();
                return Convert.ToInt32(resultado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }

        public void limpiarParametros()
        {
            comando.Parameters.Clear();
        }


        public void cerrarConexion()
        {
            if (lector != null) // si existe lector procedemos a cerrarlo.
                lector.Close();
            conexion.Close(); // cerramos conexión.
        }
    }
}
