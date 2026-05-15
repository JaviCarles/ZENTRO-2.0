<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Presentacion.LoguinUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="row mt-3">
        <div class="col-8 offset-2 col-md-4 offset-md-4">
        <h3 class="ajusteTamañoDispositivo text-center  fw-bold mb-4 mt-4">INICIAR SESIÓN</h3>
            <div class="mb-3">
                 <asp:Label ID="lblValidacionUser" CssClass="form-label alert alert-danger fw-bold d-block" role="alert" runat="server" Text="Email o contraseña incorrectos."></asp:Label>
            </div>
            <div class="mb-3">
                <label for="txtEmail" class="form-label fw-bold">Correo electrónico </label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />
            </div>
            <div class="mb-3">
                <label for="txtPassword" class="form-label fw-bold">Contraseña</label>
                <asp:TextBox type="password" runat="server" ID="txtPassword" CssClass="form-control" />
            </div>
            <asp:Button Text="INGRESAR" CssClass="btn btn-outline-primary btn-lg w-100 mb-3" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
            <div class="row">
                <div class="col">
                    <a href="RecuperarPassUsuario.aspx">¿Olvidó su contraseña?</a>
                </div>
                <div class="col text-end">
                    <a href="AltaUsuario.aspx">Crear cuenta</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
