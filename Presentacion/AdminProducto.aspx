<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AdminProducto.aspx.cs" Inherits="Presentacion.AdminProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-1">
        </div>
        <%var usuario = Session["usuario"] as Entidades.Usuario;
            if (usuario != null && usuario.Admin)
            {%>
        <div class="col">
            <div class="text-center">
                <h3 class="mb-4 mt-4">LISTADO DE PRODUCTOS</h3>
            </div>
            <div class="row mt-4">
                <div class="col-6">
                    <div class="mb-3">
                        <asp:Panel runat="server" DefaultButton="btnBuscar">
                            <!-- Usamos Bootstrap input-group para alinear el TextBox y el botón en una misma fila -->
                            <div class="input-group">
                                <asp:TextBox ID="txtBusqueda" CssClass="form-control"
                                    placeholder="Escribe tu búsqueda aquí..." runat="server"></asp:TextBox>
                                <asp:Button ID="btnBuscar" CssClass="btn btn-outline-primary"
                                    runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-3">
                    <div class="input-group mb-3">
                        <asp:Label ID="LabelOrden" class="form-label fw-bold mt-2 me-3 mb-3" runat="server" Text="Ordenar por"></asp:Label>
                        <asp:DropDownList ID="DropDownOrden" OnSelectedIndexChanged="DropDownOrden_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-3">
                    <div class="input-group mb-3">
                        <asp:Label ID="LabelCategoria" class="form-label fw-bold mt-2 me-3 mb-3" runat="server" Text="Categoria"></asp:Label>
                        <asp:DropDownList ID="DropDownCategoria" OnSelectedIndexChanged="btnBuscar_Click" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <asp:GridView ID="dgvProductos" CssClass="table table-success" runat="server"
                DataKeyNames="Id" AutoGenerateColumns="false" OnSelectedIndexChanged="dgvProductos_SelectedIndexChanged"
                AllowPaging="true" PagerStyle-CssClass="d-none">
                <Columns>
                    <asp:BoundField HeaderText="NOMBRE" DataField="Nombre" />
                    <asp:BoundField HeaderText="CODIGO" DataField="Codigo" />
                    <asp:BoundField HeaderText="DESCRIPCIÓN" DataField="Descripcion" />
                    <asp:BoundField HeaderText="MARCA" DataField="Marca.Descripcion" />
                    <asp:BoundField HeaderText="CATEGORIA" DataField="Categoria.Descripcion" />
                    <asp:BoundField HeaderText="PRECIO" DataField="Precio" DataFormatString="${0:N0}" HtmlEncode="false" />
                    <asp:CommandField HeaderText="ACCIÓN" ShowSelectButton="true" SelectText="✍️" />
                </Columns>
            </asp:GridView>
            <div>
                <nav>
                    <ul class="pagination justify-content-center">
                        <li class="page-item">
                            <asp:Button ID="btnAnterior" runat="server" CssClass="page-link" Text="«" OnClick="btnAnterior_Click" />
                        </li>
                        <li class="page-item disabled">
                            <span class="page-link">
                                <asp:Label ID="lblPaginaActual" runat="server" /></span>
                        </li>
                        <li class="page-item">
                            <asp:Button ID="btnSiguiente" runat="server" CssClass="page-link" Text="»" OnClick="btnSiguiente_Click" />
                        </li>
                    </ul>
                </nav>
            </div>
            <asp:Button Text="AGREGAR" CssClass="btn btn-outline-primary px-4 py-2 rounded-pill shadow-sm" ID="btnAgregar" OnClick="btnAgregar_Click" runat="server" />
        </div>
        <%}
            else
            {
        %>
        <div class="col text-center mt-4">
            <h2>NO TIENES LOS PERMISOS PARA INGRESAR AQUÍ.</h2>
        </div>
        <%}%>
        <div class="col-1">
        </div>
    </div>
</asp:Content>
