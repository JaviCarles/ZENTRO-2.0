using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public static class ProductoNegocio
    {
        public static List<Producto> buscar(string filtro, string orden)
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                productos = ProductoDatos.getListProductos(filtro, orden); //llamamos al método getListMuebles() de la capa "MueblesDatos"
                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public static List<Categoria> listaCategorias()
        {
            try
            {
                return ProductoDatos.getListCategorias();
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al intentar listar categorias. Detalles: " + ex.Message, ex);
            }

        }

        public static List<Marca> listaMarcas()
        {
            try
            {
                return ProductoDatos.getListMarcas();
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al intentar listar marcas. Detalles: " + ex.Message, ex);
            }
        }

        public static void altaProducto(Producto producto)
        {
            try
            {
                ProductoDatos.insertProducto(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al intentar registrar un producto. Detalles: " + ex.Message, ex);
            }
        }

        public static void editarProducto(Producto producto)
        {
            try
            {
                ProductoDatos.updateProducto(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al intentar modificar un producto. Detalles: " + ex.Message, ex);
            }
        }

        public static void eliminarProducto(Producto producto)
        {
            try
            {
                ProductoDatos.deleteProducto(producto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
