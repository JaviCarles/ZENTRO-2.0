using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MarcaNegocio
    {
        public static List<Marca> buscar(string filtro)
        {
            List<Marca> lista = new List<Marca>();
            try
            {
                lista = MarcaDatos.getListMarcas(filtro);
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool altaMarca(string descripcion)
        {
            try
            {
               return MarcaDatos.insertMarca(descripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool modificarMarca(Marca seleccionada, string descripcion)
        {
            try
            {
                return MarcaDatos.updateMarca(seleccionada, descripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool eliminarMarca(Marca marca)
        {
            try
            {
                return MarcaDatos.deleteMarca(marca);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
