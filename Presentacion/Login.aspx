<%@ Page Title="Iniciar Sesión - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Presentacion.LoguinUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="container my-5 text-black" style="max-width: 500px;">
        
        <div class="p-4 rounded shadow" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.1);">
            
            <div class="text-center mb-4">
                <h3 class="text-uppercase tracking-wider fw-bold" style="color: #cca97c;">
                    <i class="bi bi-box-arrow-in-right me-2"></i>INICIAR SESIÓN
                </h3>
                <hr style="border-color: rgba(243, 235, 224, 0.15);" />
            </div>

            <% if (lblValidacionUser.Visible) { %>
                <div class="mb-3">
                     <asp:Label ID="lblValidacionUser" CssClass="form-label alert alert-danger fw-bold d-block small py-2" role="alert" runat="server" Text="Email o contraseña incorrectos."></asp:Label>
                </div>
            <% } %>

            <div class="mb-3">
                <label for="txtEmail" class="form-label text-black-50 small fw-bold">CORREO ELECTRÓNICO</label>
                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" placeholder="nombre@correo.com" MaxLength="50" />
            </div>

            <div class="mb-4">
                <label for="txtPassword" class="form-label text-black-50 small fw-bold">CONTRASEÑA</label>
                <div class="input-group">
                    <asp:TextBox TextMode="Password" runat="server" ID="txtPassword" CssClass="form-control" ClientIDMode="Static" placeholder="Ingrese su contraseña" MaxLength="50" />
                    <button type="button" class="btn btn-outline-secondary border-opacity-25" onclick="togglePassword('txtPassword', this)">
                        <i class="bi bi-eye text-white-50"></i>
                    </button>
                </div>
            </div>

            <asp:Button Text="INGRESAR" CssClass="btn btn-success btn-lg w-100 mb-4 rounded-pill fw-bold shadow-sm" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
            
            <div class="row small pt-2 border-top border-secondary border-opacity-25">
                <div class="col-12 col-sm-6 mb-2 mb-sm-0">
                    <a href="RecuperarPassUsuario.aspx" class="text-black-50 text-decoration-none hover-gold"><i class="bi bi-question-circle me-1"></i>¿Olvidó su contraseña?</a>
                </div>
                <div class="col-12 col-sm-6 text-sm-end">
                    <a href="AltaUsuario.aspx" class="fw-bold text-decoration-none" style="color: #cca97c;"><i class="bi bi-person-plus me-1"></i>Crear cuenta</a>
                </div>
            </div>

        </div>
    </div>

    <%-- Script nativo para el ojo de la contraseña --%>
    <script type="text/javascript">
        function togglePassword(inputId, button) {
            var input = document.getElementById(inputId);
            var icon = button.querySelector('i');
            if (input.type === "password") {
                input.type = "text";
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                input.type = "password";
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        }
    </script>
</asp:Content>
