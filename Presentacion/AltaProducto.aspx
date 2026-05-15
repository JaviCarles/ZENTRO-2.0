<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AltaProducto.aspx.cs" Inherits="Presentacion.AltaProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%var usuario = Session["usuario"] as Entidades.Usuario;
    if (usuario != null && usuario.Admin)
    {%>
    <%-- El SIGUIENTE ELEMENTO (SCRIPTMANAGER) ES REQUERIDO PARA USAR EL UPDATEPANEL --%>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <asp:Label ID="lblTitulo" CssClass="h4" runat="server" Text="Label">AGREGAR PRODUCTO</asp:Label>
    <div class="row mt-4">
        <div class="col-6">
            <div class="mb-3">
                <label for="txtId" class="form-label">Id</label>
                <asp:TextBox TextMode="Number" ID="txtId" CssClass="form-control" runat="server" />
            </div>
            <div class="mb-3">
                <label for="txtCodigo" class="form-label">Código</label>
                <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" MaxLength="50" />
            </div>
            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" MaxLength="50" />
            </div>
            <div class="mb-3">
                <label for="textAreaDescripcion" class="form-label">Descripción</label>
                <asp:TextBox class="form-control" TextMode="MultiLine" ID="textAreaDescripcion" Rows="3" runat="server" MaxLength="149" />
            </div>
            <div class="mb-3">
                <label for="DropDownListMarca" cssclass="form-label">Marca</label>
                <asp:DropDownList ID="DropDownListMarca" CssClass="form-select" runat="server"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <label for="DropDownListCategoria" cssclass="form-label">Categoría</label>
                <asp:DropDownList ID="DropDownListCategoria" CssClass="form-select" runat="server"></asp:DropDownList>
            </div>
            <div class="mb-3">
                <label for="txtPrecio" class="form-label">Precio</label>
                <asp:TextBox TextMode="Number" ID="txtPrecio" CssClass="form-control" runat="server" min="0" step="1" />
            </div>
        </div>
        <div class="col-6">
            <asp:UpdatePanel ID="updatePanelImagen" runat="server">
                <ContentTemplate>
                    <div class="mb-3">
                        <label for="txtImagen" class="form-label">URL Imagen</label>
                        <asp:TextBox ID="txtImagen" CssClass="form-control" runat="server" AutoPostBack="true"
                            OnTextChanged="txtImagen_TextChanged" />
                    </div>
                    <div>
                        <asp:Image ID="imgProducto"
                            ImageUrl="Content/Imagenes/Imagen no disponible.png"
                            runat="server" Width="50%" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
    <div class="row mt-4">
        <div class="col-6">
            <asp:Button Text="ACEPTAR" CssClass="btn btn-primary" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" />
            <asp:Button Text="CANCELAR" CssClass="btn btn-primary" ID="btnCancelar" OnClick="btnCancelar_Click" runat="server" />
            <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        </div>
        <div class="col-2">
            <asp:UpdatePanel ID="UPBtnEliminar" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger"
                        OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este producto?');"
                        OnClick="btnEliminar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
<%}
    else
    {
%>
<div class="col text-center">
    <h2>NO TIENES LOS PERMISOS PARA INGRESAR AQUÍ.</h2>
</div>
<%}%>
</asp:Content>
