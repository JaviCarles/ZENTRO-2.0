using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public static List<Categoria> buscar(string filtro)
        {
            List<Categoria> lista = new List<Categoria>();  
            try
            {
                lista = CategoriaDatos.getListCategorias(filtro);
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool altaCategoria(string descripcion)
        {
            try
            {
              return  CategoriaDatos.insertCategoria(descripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool modificarCategoria(Categoria seleccionada, string descripcion) 
        {
            try
            {
               return CategoriaDatos.updateCategoria(seleccionada, descripcion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool eliminarCategoria(Categoria categoria)
        {
            try
            {
               return CategoriaDatos.deleteCategoria(categoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
