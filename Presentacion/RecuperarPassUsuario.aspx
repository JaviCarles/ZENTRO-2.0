<%@ Page Title="Recuperar Contraseña - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="RecuperarPassUsuario.aspx.cs" Inherits="Presentacion.RecuperarPassUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-5 text-black" style="max-width: 500px;">
        
        <div class="p-4 rounded shadow" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.1);">
            
            <div class="text-center mb-4">
                <h3 class="text-uppercase tracking-wider fw-bold" style="color: #cca97c;">
                    <i class="bi bi-key-fill me-2"></i>RECUPERAR CLAVE
                </h3>
                <p class="text-black-50 small">Ingresá tu correo y te enviaremos una contraseña temporal de acceso.</p>
                <hr style="border-color: rgba(243, 235, 224, 0.15);" />
            </div>

            <div class="mb-3">
                <label for="txtEmail" class="form-label text-black-50 small fw-bold">CORREO ELECTRÓNICO</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="nombre@correo.com" MaxLength="50" />
                
                <asp:RegularExpressionValidator
                    ID="revEmail"
                    runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Ingrese un correo electrónico válido."
                    CssClass="text-danger small mt-1 fw-bold"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    Display="Dynamic" />
            </div>

            <asp:Button Text="ENVIAR NUEVA CLAVE" CssClass="btn btn-success btn-lg w-100 mb-3 rounded-pill fw-bold shadow-sm" ID="btnEnviar" OnClick="btnEnviar_Click" runat="server" />
            
            <div class="text-center mb-3">
                <asp:Label ID="lblMensaje" runat="server" />
            </div>

            <div class="text-center pt-2 border-top border-secondary border-opacity-25">
                <a href="Login.aspx" class="text-black-50 text-decoration-none small"><i class="bi bi-arrow-left me-1"></i>Volver al Inicio de Sesión</a>
            </div>

        </div>
    </div>
</asp:Content>