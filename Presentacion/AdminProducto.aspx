<%@ Page Title="Administrar Productos - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AdminProducto.aspx.cs" Inherits="Presentacion.AdminProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-4">

        <% var usuario = Session["usuario"] as Entidades.Usuario;
            if (usuario != null && usuario.Admin)
            { %>

        <div class="text-center mb-4">
            <h2 class="titulo-admin text-uppercase">Listado de Productos</h2>
            <p class="text-black-50 small">Panel de control de stock y catálogo de Pampa Gaucha</p>
        </div>

        <div class="row g-3 mb-4 align-items-end">
            <div class="col-12 col-md-6 col-lg-4">
                <asp:Panel runat="server" DefaultButton="btnBuscar">
                    <div class="input-group shadow-sm text-black">
                   
                        <asp:TextBox ID="txtBusqueda" CssClass="form-control input-buscador-oscuro" placeholder="Buscar por nombre, código o marca..." runat="server"></asp:TextBox>
                        <asp:Button ID="btnBuscar" CssClass="btn btn-outline-light" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </asp:Panel>
            </div>

            <div class="col-6 col-md-3 col-lg-4">
                <div class="form-group">
                    <label class="form-label small fw-bold text-black-50 mb-1">Ordenar por</label>
                    <asp:DropDownList ID="DropDownOrden" OnSelectedIndexChanged="DropDownOrden_SelectedIndexChanged" AutoPostBack="true" CssClass="form-select filtroApp" runat="server"></asp:DropDownList>
                </div>
            </div>

            <div class="col-6 col-md-3 col-lg-4">
                <div class="form-group">
                    <label class="form-label small fw-bold text-black-50 mb-1">Categoría</label>
                    <asp:DropDownList ID="DropDownCategoria" OnSelectedIndexChanged="btnBuscar_Click" AutoPostBack="true" CssClass="form-select filtroApp" runat="server"></asp:DropDownList>
                </div>
            </div>
        </div>

        <div class="mb-3 text-end">
            <asp:LinkButton ID="btnAgregar" OnClick="btnAgregar_Click" CssClass="btn btn-pampa-primary rounded-pill px-4 shadow-sm" runat="server">
                    <i class="bi bi-plus-circle me-2"></i>Agregar Nuevo Producto
                </asp:LinkButton>
        </div>

        <div class="card bg-transparent border-0 shadow-lg mb-4">
            <div class="table-responsive rounded border border-secondary border-opacity-25">
                <asp:GridView ID="dgvProductos"
                    CssClass="table table-pampa-admin mb-0"
                    runat="server"
                    DataKeyNames="Id"
                    AutoGenerateColumns="false"
                    OnSelectedIndexChanged="dgvProductos_SelectedIndexChanged"
                    AllowPaging="true"
                    PagerStyle-CssClass="d-none">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" HeaderStyle-CssClass="text-nowrap" />
                        <asp:BoundField HeaderText="Código" DataField="Codigo" HeaderStyle-CssClass="text-nowrap" />

                        <asp:BoundField HeaderText="Descripción" DataField="Descripcion" ItemStyle-CssClass="col-descripcion-tabla" />

                        <asp:BoundField HeaderText="Marca" DataField="Marca.Descripcion" HeaderStyle-CssClass="text-nowrap" />
                        <asp:BoundField HeaderText="Categoría" DataField="Categoria.Descripcion" HeaderStyle-CssClass="text-nowrap" />
                        <asp:BoundField HeaderText="Precio" DataField="Precio" DataFormatString="${0:N0}" HtmlEncode="false" HeaderStyle-CssClass="text-nowrap" ItemStyle-CssClass="fw-bold text-end pe-3" />

                        <asp:TemplateField HeaderText="Acción" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="Select" CssClass="btn-tabla-editar" title="Editar Producto">✍️</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="mt-2 mb-4">
            <nav aria-label="Navegación de productos">
                <ul class="pagination justify-content-center mb-0">
                    <li class="page-item">
                        <asp:Button ID="btnAnterior" runat="server" CssClass="page-link" Text="«" OnClick="btnAnterior_Click" />
                    </li>
                    <li class="page-item disabled">
                        <span class="page-link px-4">
                            <asp:Label ID="lblPaginaActual" runat="server" CssClass="text-black-50 font-monospace" />
                        </span>
                    </li>
                    <li class="page-item">
                        <asp:Button ID="btnSiguiente" runat="server" CssClass="page-link" Text="»" OnClick="btnSiguiente_Click" />
                    </li>
                </ul>
            </nav>
        </div>

        <% }
        else
        { %>

        <div class="row my-5 py-5">
            <div class="col-12 text-center text-black-50">
                <i class="bi bi-shield-lock-fill text-danger display-1 mb-3"></i>
                <h2 class="fw-bold">Acceso Restringido</h2>
                <p class="lead">No tienes los permisos administrativos necesarios para ingresar a esta sección.</p>
                <a href="Default.aspx" class="btn btn-outline-light mt-3 rounded-pill px-4">Volver al Inicio</a>
            </div>
        </div>

        <% } %>
    </div>
</asp:Content>