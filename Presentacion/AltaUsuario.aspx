<%@ Page Title="Gestión de Usuario - RingoClothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AltaUsuario.aspx.cs" Inherits="Presentacion.AltaUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="container my-4 text-white" style="max-width: 900px;">
        <div class="text-center mb-4">
            <h3 class="text-uppercase tracking-wider" style="color: #cca97c;">
                <i class="bi bi-person-badge me-2"></i><asp:Label ID="lblTitulo" runat="server">NUEVO USUARIO</asp:Label>
            </h3>
            <hr style="border-color: rgba(243, 235, 224, 0.15);" />
        </div>

        <div class="row g-4 p-4 rounded shadow" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.1);">
            
            <div class="col-12 col-md-7">
                <% var usuario = (Entidades.Usuario)Session["usuario"];
                   if (usuario != null && usuario.Admin) { %>
                    <div class="mb-3" style="display:none;">
                        <label for="txtId" class="form-label text-black-50 small">Id</label>
                        <asp:TextBox TextMode="Number" ID="txtId" CssClass="form-control" runat="server" Visible="False" />
                    </div>
                <% } %>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label text-black-50 small fw-bold">CORREO ELECTRÓNICO</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="nombre@correo.com" runat="server" MaxLength="50" />
                    <%-- CORRECCIÓN: Quitamos 'd-block' para dejar que ASP.NET maneje el ocultamiento nativo --%>
                    <asp:RegularExpressionValidator
                        ID="revEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Ingrese un correo electrónico válido."
                        CssClass="text-danger small mt-1 fw-bold"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                        Display="Dynamic" />
                </div>

                <div class="row">
                    <div class="col-12 col-sm-6 mb-3">
                        <label for="txtPass" class="form-label text-black-50 small fw-bold">CONTRASEÑA</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtPass" TextMode="Password" CancelSubmit="false" CssClass="form-control" ClientIDMode="Static" runat="server" MaxLength="50" />
                            <button type="button" class="btn btn-outline-secondary border-opacity-25" onclick="togglePassword('txtPass', this)">
                                <i class="bi bi-eye text-black-50"></i>
                            </button>
                        </div>
                        <%-- CORRECCIÓN: Agregamos Display="Dynamic" y estilos de margen limpios --%>
                        <asp:RequiredFieldValidator 
                            ID="rfvPass" 
                            ControlToValidate="txtPass" 
                            ErrorMessage="La contraseña es obligatoria" 
                            CssClass="text-danger small fw-bold mt-1 shadow-none" 
                            runat="server" 
                            Display="Dynamic" />
                    </div>

                    <div class="col-12 col-sm-6 mb-3">
                        <label for="txtConfirmPass" class="form-label text-black-50 small fw-bold">CONFIRMAR CONTRASEÑA</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtConfirmPass" TextMode="Password" CancelSubmit="false" CssClass="form-control" ClientIDMode="Static" runat="server" MaxLength="50" />
                            <button type="button" class="btn btn-outline-secondary border-opacity-25" onclick="togglePassword('txtConfirmPass', this)">
                                <i class="bi bi-eye text-white-50"></i>
                            </button>
                        </div>
                        <%-- CORRECCIÓN: Agregamos Display="Dynamic" y estilos de margen limpios --%>
                        <asp:RequiredFieldValidator 
                            ID="rfvConfirmPass" 
                            ControlToValidate="txtConfirmPass" 
                            ErrorMessage="Debe confirmar la contraseña" 
                            CssClass="text-danger small fw-bold mt-1 shadow-none" 
                            runat="server" 
                            Display="Dynamic" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-6 mb-3">
                        <label for="txtNombre" class="form-label text-black-50 small fw-bold">NOMBRE</label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" placeholder="Ej: Juan" runat="server" MaxLength="50" />
                    </div>
                    <div class="col-6 mb-3">
                        <label for="txtApellido" class="form-label text-black-50 small fw-bold">APELLIDO</label>
                        <asp:TextBox ID="txtApellido" CssClass="form-control" placeholder="Ej: Pérez" runat="server" MaxLength="50" />
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-5 d-flex flex-column justify-content-center border-start border-secondary border-opacity-25 ps-md-4">
                <asp:UpdatePanel ID="updatePanelImagen" runat="server">
                    <ContentTemplate>
                        <div class="mb-3" style="display:none;">
                            <asp:TextBox ID="txtImagen" CssClass="form-control" runat="server"
                                Text="Content/Imagenes/Imagen_no_disponible.png"
                                AutoPostBack="true"
                                OnTextChanged="txtImagen_TextChanged"
                                ReadOnly="True" />
                        </div>
                        
                        <div class="text-center mb-4">
                            <label class="form-label d-block text-black-50 small fw-bold text-center mb-2">FOTO DE PERFIL</label>
                            <asp:Image ID="imgUsuario"
                                ImageUrl="Content/Imagenes/Imagen_no_disponible.png"
                                runat="server" CssClass="img-fluid rounded-circle img-thumbnail bg-transparent p-2 border-secondary" style="width: 140px; height: 140px; object-fit: cover; border-color: rgba(243, 235, 224, 0.2) !important;" />
                        </div>

                        <% var usrSession = (Entidades.Usuario)Session["usuario"];
                           if (usrSession != null && usrSession.Admin) { %>
                            <div class="mb-3">
                                <label for="dDLTipoUsuario" class="form-label text-black-50 small fw-bold">PERMISOS DE SISTEMA</label>
                                <asp:DropDownList ID="dDLTipoUsuario" CssClass="form-select" runat="server" />
                            </div>
                        <% } else { %>
                             <asp:DropDownList ID="DropDownList1" runat="server" Visible="false" />
                        <% } %>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="row mt-4 align-items-center p-2">
            <div class="col-12 text-center">
                <asp:Button Text="REGISTRAR" CssClass="btn btn-success px-5 rounded-pill fw-bold me-2 shadow-sm" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
                <asp:Button Text="CANCELAR" CssClass="btn btn-outline-light px-4 rounded-pill shadow-sm" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" CausesValidation="False" />
                
                <div class="mt-3">
                    <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-bold small d-block" />
                </div>
            </div>
        </div>
    </div>

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