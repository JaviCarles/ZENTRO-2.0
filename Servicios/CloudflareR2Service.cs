using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;              // Importa el SDK de Amazon S3 (compatible con Cloudflare R2)
using Amazon.S3.Model;        // Modelos de objetos S3 (PutObjectRequest, etc.)
using System.Configuration;
using System.Web; // Para usar métodos asincrónicos (async/await)

namespace Servicios
{
    public class CloudflareR2Service
    {
        // Variables privadas para almacenar las credenciales e información del bucket.
        // ConfigurationManager.AppSettings busca estas claves dentro de la sección <appSettings> del web.config.
        private readonly string customDomain = ConfigurationManager.AppSettings["R2CustomDomain"];
        private readonly string bucketName = ConfigurationManager.AppSettings["R2BucketName"];
        private readonly string serviceUrl = ConfigurationManager.AppSettings["R2ServiceUrl"];
        private readonly string accessKey = ConfigurationManager.AppSettings["R2AccessKey"];
        private readonly string secretKey = ConfigurationManager.AppSettings["R2SecretKey"];

        /// <summary>
        /// Crea e inicializa una nueva instancia del cliente de Amazon S3 apuntando a Cloudflare R2.
        /// </summary>
        private AmazonS3Client GetClient()
        {
            // Se configura el cliente de S3 para adaptarse a las especificaciones de Cloudflare
            var config = new AmazonS3Config
            {
                // Apunta a la URL de tu cuenta de Cloudflare (ej: https://<id-cuenta>.r2.cloudflarestorage.com)
                ServiceURL = serviceUrl,

                // Obligatorio para R2. Fuerza al SDK a usar rutas fijas (bucket/objeto) en vez de subdominios virtuales.
                ForcePathStyle = true,

            };

            // Retorna el cliente autenticado con tus llaves de API y la configuración especial
            return new AmazonS3Client(accessKey, secretKey, config);
        }

        /// <summary>
        /// Sube un flujo de datos (Stream) de una imagen a Cloudflare R2 de forma asíncrona.
        /// </summary>
        /// <param name="fileStream">El flujo de bytes del archivo a subir.</param>
        /// <param name="fileName">El nombre original del archivo (ej: "foto.jpg").</param>
        /// <returns>La URL pública completa para acceder a la imagen subida.</returns>
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            try
            {
                // Aseguramos que el puntero esté al inicio
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }

                // Copiamos el archivo a un MemoryStream. Esto evita que el SDK viejo intente
                // hacer "Chunked Streaming" (Subida en pedazos) que Cloudflare R2 rechaza con error 501.
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reseteamos puntero de la memoria
                    System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)3072 | (System.Net.SecurityProtocolType)768 | (System.Net.SecurityProtocolType)12288;
                    using (var client = GetClient())
                    {
                        string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(fileName)}";

                        var request = new PutObjectRequest
                        {
                            BucketName = bucketName,
                            Key = uniqueFileName,
                            InputStream = memoryStream, // Usamos la memoria intermedia
                            ContentType = MimeMapping.GetMimeMapping(fileName),

                            // Esta propiedad sí existe en la 3.7.100 y apaga la firma de streaming de AWS
                            DisablePayloadSigning = true
                        };

                        // Ejecuta la subida de forma limpia hacia Cloudflare
                        await client.PutObjectAsync(request);

                        string baseUrl = customDomain.TrimEnd('/');
                        return $"{baseUrl}/{uniqueFileName}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Esto va a hacer que la página explote, pero mostrándote el cartel con el error REAL
                throw new Exception("Error real al intentar subir a Cloudflare: " + ex.Message, ex);
            }            
        }

        /// <summary>
        /// Elimina un archivo físico de Cloudflare R2 a partir de su URL pública.
        /// </summary>
        /// <param name="imageUrl">La URL completa guardada en la base de datos.</param>
        public async Task DeleteImageAsync(string imageUrl)
        {
            // Si la URL está vacía o es nula, no hay nada que borrar en la nube
            if (string.IsNullOrEmpty(imageUrl)) return;

            try
            {
                // Extraemos solo el nombre del archivo de la URL (ej: "foto_20260603.jpg")
                string fileName = Path.GetFileName(imageUrl);

                using (var client = GetClient())
                {
                    var request = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = fileName
                    };

                    // Enviamos la orden de eliminación asíncrona a Cloudflare R2
                    await client.DeleteObjectAsync(request);
                }
            }
            catch (Exception)
            {
                // En producción puedes registrar el error en un log si lo deseas.
                // Se aconseja no frenar la app si falla el borrado de una imagen vieja.
            }
        }
    }
}
