using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entidades;

namespace Presentacion
{
    public partial class RecuperarPassUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMensaje.Text = string.Empty;
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            string emailIngresado = txtEmail.Text.Trim();

            if (!string.IsNullOrEmpty(emailIngresado))
            {
                // Verificamos si el usuario existe en la Base de Datos
                if (UsuarioNegocio.existeUsuario(emailIngresado))
                {
                    enviarCorreoRecuperacion(emailIngresado);
                }
                else
                {
                    lblMensaje.Text = "El correo ingresado no pertenece a una cuenta existente.";
                    lblMensaje.CssClass = "text-danger small fw-bold mt-2 d-inline-block";
                }
            }
            else
            {
                lblMensaje.Text = "Por favor, escriba su correo electrónico.";
                lblMensaje.CssClass = "text-danger small fw-bold mt-2 d-inline-block";
            }
        }

        private void enviarCorreoRecuperacion(string usuarioEmail)
        {
            // 1. Creamos la contraseña provisoria y actualizamos la DB
            string nuevaPassword = Utilidades.GenerarPasswordAleatoria();

            Usuario usuario = new Usuario();
            usuario.Email = usuarioEmail;
            usuario.Pass = nuevaPassword; // Si usás hash/encriptación en DB, aplicalo acá

            try
            {
                // Impactamos el cambio en la base de datos
                UsuarioNegocio.recuperarContraseña(usuario);

                // 2. Estructuramos el MailMessage
                MailMessage mail = new MailMessage();

                // IMPORTANTE: Poné acá el mail real que vas a usar para despachar los correos del sistema
                string mailSoporte = "javiercarles1206@gmail.com";

                mail.From = new MailAddress(mailSoporte, "Ringo Clothes");
                mail.To.Add(usuarioEmail); // Se le envía al mail dinámico que cargó el cliente
                mail.Subject = "Nueva contraseña temporal - Ringo Clothes";

                // Diseño del cuerpo del mail en HTML
                mail.Body = $"<div style='font-family: sans-serif; color: #333;'>" +
                            $"<h2>Hola,</h2>" +
                            $"<p>Recibimos una solicitud para restablecer la contraseña de tu cuenta.</p>" +
                            $"<p>Tu clave temporal de acceso es: <span style='font-size: 1.2rem; font-weight: bold; color: #cca97c; background: #fafdff; padding: 4px 8px; border: 1px solid #ddd; rounded: 4px;'>{nuevaPassword}</span></p>" +
                            $"<p>Por cuestiones de seguridad, te recomendamos cambiarla desde tu perfil ni bien ingreses al sistema.</p>" +
                            $"<hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />" +
                            $"<p style='font-size: 0.9rem; color: #777;'>Soporte Técnico - Ringo Clothes</p>" +
                            $"</div>";
                mail.IsBodyHtml = true;

                // 3. Configuración del Servidor SMTP de Gmail Real
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;

                // ⚠️ REEMPLAZAR: "tu_cuenta_sistema@gmail.com" por tu mail y las "xxxx..." por la clave de 16 dígitos de Google
                smtp.Credentials = new NetworkCredential(mailSoporte, "jpsr mccl mpuz njky");

                // 4. Envío definitivo
                smtp.Send(mail);

                // 5. Limpieza y aviso de éxito en pantalla
                txtEmail.Text = string.Empty;
                lblMensaje.Text = "Se ha enviado la nueva clave temporal a tu casilla de correo.";
                lblMensaje.CssClass = "text-success small fw-bold mt-2 d-inline-block";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al procesar el envío: " + ex.Message;
                lblMensaje.CssClass = "text-danger small fw-bold mt-2 d-inline-block";
            }
        }
    }
}