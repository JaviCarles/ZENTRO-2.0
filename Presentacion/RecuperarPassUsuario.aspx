<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="RecuperarPassUsuario.aspx.cs" Inherits="Presentacion.RecuperarPassUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h2 class="text-center fw-bold mb-4 mt-4">¿Olvidaste tu contraseña?</h2>
        
        <div class="row mt-3">
            <div class="col-4">
            </div>
            <div class="col mb-3">
                <p class="fw-bold">Ingresá tu correo y te enviaremos una nueva clave.</p>
                <label for="txtEmail" class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
                <%--<asp:RequiredFieldValidator ControlToValidate="txtEmail" ErrorMessage="Este campo es obligatorio" runat="server" CssClass="text-danger" />--%>
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary w-100 mt-3" OnClick="btnEnviar_Click" />
                <asp:Label ID="lblMensaje" runat="server" CssClass="text-success mt-3 d-block fw-bold" />
            </div>
            <div class="col-4">
            </div>
        </div>
    </div>
</asp:Content>
