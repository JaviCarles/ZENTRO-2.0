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

        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (existeUsuario(txtEmail.Text))
                {
                    recuperarContraseña(txtEmail.Text);
                    txtEmail.Text = string.Empty;
                    lblMensaje.Text = "Se ha enviado una nueva clave a su correo.";
                    lblMensaje.CssClass = "text-success mt-3 d-block";
                }
                else
                {
                    lblMensaje.Text = "El correo ingresado no pertenece a una cuenta existente.";
                    lblMensaje.CssClass = "text-danger mt-3 d-block";
                }
            }
            else
            {
                lblMensaje.Text = "Escriba su correo electrónico.";
                lblMensaje.CssClass = "text-danger mt-3 d-block";
            }
        }

        public static bool existeUsuario(string email)
        {
            return UsuarioNegocio.existeUsuario(email);
        }

        public void recuperarContraseña(string usuarioEmail)
        {
            Usuario usuario = new Usuario();
            // Generar una contraseña 
            string nuevaPassword = Utilidades.GenerarPasswordAleatoria();
            //Enviamos nueva Pass a la DB.
            usuario.Email = usuarioEmail;
            usuario.Pass = nuevaPassword;

            try
            {
                UsuarioNegocio.recuperarContraseña(usuario);
                // Crear el mensaje
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("no-reply@tusistema.com");
                mail.To.Add(usuarioEmail);
                mail.Subject = "Recuperación de contraseña";
                mail.Body = "Hola, recibimos tu solicitud de recuperación de contraseña. Tu nueva clave temporal es: " + nuevaPassword + ". Por favor cámbiala al ingresar.";


                // Configurar SMTP para Hotmail/Outlook
                SmtpClient smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 587);
                smtp.Credentials = new NetworkCredential("b7eb1147c08ebd", "5c0767fa1b0f84");
                smtp.EnableSsl = true;

                // Enviar
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al enviar correo: " + ex.Message;
            }

        }
    }
}