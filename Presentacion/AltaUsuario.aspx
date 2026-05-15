<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AltaUsuario.aspx.cs" Inherits="Presentacion.AltaUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- El SIGUIENTE ELEMENTO (SCRIPTMANAGER) ES REQUERIDO PARA USAR EL UPDATEPANEL --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="text-center mt-3 mb-3">
        <asp:Label ID="lblTitulo" CssClass="h4" runat="server" Text="Label">NUEVO USUARIO</asp:Label>
    </div>
    <div class="row">
        <!-- Columna izquierda -->
        <div class="col">
        </div>
        <div class="col-md-4 col-12">
            <% var usuario = (Entidades.Usuario)Session["usuario"];
                if (usuario != null && usuario.Admin)
                {  %>
            <div class="mb-3">
                <label for="txtId" class="form-label">Id</label>
                <asp:TextBox TextMode="Number" ID="txtId" CssClass="form-control" runat="server" Visible="False" />
            </div>
            <%} %>
            <div class="mb-3">
                <label for="txtEmail" class="form-label">Correo electrónico</label>
                <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" MaxLength="50" />
                <asp:RegularExpressionValidator
                    ID="revEmail"
                    runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Ingrese un correo electrónico válido."
                    CssClass="text-danger"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    Display="Dynamic" />
            </div>
            <div class="mb-3">
                <label for="txtPass" class="form-label">Contraseña</label>
                <div class="input-group">
                    <asp:TextBox ID="txtPass" TextMode="Password" CssClass="form-control" ClientIDMode="Static"
                        runat="server" MaxLength="50" />
                    <button type="button" class="btn btn-outline-secondary"  onclick="togglePassword('txtPass')">
                        <i class="bi bi-eye"></i>
                    </button>
                </div>
                <asp:RequiredFieldValidator
                    ID="rfvPass"
                    ControlToValidate="txtPass"
                    ErrorMessage="La contraseña es obligatoria"
                    CssClass="text-danger"
                    runat="server" />
            </div>
            <div class="mb-3">
                <label for="txtConfirmPass" class="form-label">Confirmar Contraseña</label>
                <div class="input-group">
                    <asp:TextBox ID="txtConfirmPass" TextMode="Password" CssClass="form-control" ClientIDMode="Static"
                        runat="server" MaxLength="50" />
                    <button type="button" class="btn btn-outline-secondary" onclick="togglePassword('txtConfirmPass')">
                        <i class="bi bi-eye"></i>
                    </button>
                </div>
                <asp:RequiredFieldValidator
                    ID="rfvConfirmPass"
                    ControlToValidate="txtConfirmPass"
                    ErrorMessage="Debe confirmar la contraseña"
                    CssClass="text-danger"
                    runat="server" />
            </div>
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" MaxLength="50" />
            </div>
            <div class="mb-3">
                <label for="txtApellido" class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server" MaxLength="50" />
            </div>
        </div>

        <!-- Columna derecha -->
        <div class="col-md-4 col-12">
            <asp:UpdatePanel ID="updatePanelImagen" runat="server">
                <ContentTemplate>
                    <div class="mb-3">
                        <label for="txtImagen" class="form-label">Cargar Imagen</label>
                        <asp:TextBox ID="txtImagen" CssClass="form-control" runat="server" AutoPostBack="true"
                            OnTextChanged="txtImagen_TextChanged"/>
                    </div>
                    <div class="text-center mb-3">
                        <asp:Image ID="imgUsuario"
                            ImageUrl="Content/Imagenes/Imagen no disponible.png"
                            runat="server" CssClass="img-fluid rounded mx-auto d-block" Width="50%" />
                    </div>
                    <%var usuario = (Entidades.Usuario)Session["usuario"];
                        if (usuario != null && usuario.Admin)
                        {  %>
                    <div class="mb-3">
                        <label for="dDLTipoUsuario" class="form-label">Permisos de usuario</label>
                        <asp:DropDownList ID="dDLTipoUsuario" CssClass="form-select" runat="server" />
                    </div>
                    <%} %>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col">
        </div>
    </div>

    <!-- Botones -->
    <div class="row mt-4 mb-3">
        <div class="col-2"></div>
        <div class="col-md-4 col-12">
            <asp:Button Text="ACEPTAR" CssClass="btn btn-outline-primary me-2" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
            <asp:Button Text="CANCELAR" CssClass="btn btn-outline-secondary" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" />
        </div>
    </div>

    <div class="row">
        <div class="col-2">
        </div>
        <div class="col-md-4 col-12">
            <asp:Label ID="lblError" runat="server" CssClass="text-danger w-100 text-center fw-bold mt-2" />
        </div>
    </div>
</asp:Content>
