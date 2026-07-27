<%@ Page Title="Administrar Marcas - Ringo Clothes" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AdminMarca.aspx.cs" Inherits="Presentacion.AdminMarca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container my-4">

        <% var usuario = Session["usuario"] as Entidades.Usuario;
            if (usuario != null && usuario.Admin)
            { %>

        <div class="text-center mb-4">
            <h2 class="titulo-admin text-uppercase">Gestión de Marcas</h2>
            <p class="text-black-50 small">Panel de configuración para los fabricantes y marcas del catálogo</p>
        </div>

        <div class="row g-4">

            <div class="col-12 col-lg-7">
                <div class="card bg-transparent border-0 shadow-lg">
                    <div class="table-responsive rounded border border-secondary border-opacity-25">
                        <asp:GridView ID="dgvMarcas"
                            runat="server"
                            DataKeyNames="Id"
                            AutoGenerateColumns="false"
                            CssClass="table table-pampa-admin mb-0"
                            OnRowCommand="dgvMarcas_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Nombre de la Marca" DataField="Descripcion" HeaderStyle-CssClass="w-75" />

                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-center" HeaderStyle-CssClass="text-center text-nowrap">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditar" runat="server"
                                            CommandName="Editar"
                                            CommandArgument='<%# Eval("Id") + "|" + Eval("Descripcion")%>'
                                            CssClass="btn btn-sm text-warning me-2 fs-5"
                                            title="Editar Marca">
            <i class="bi bi-pencil-square"></i>
        </asp:LinkButton>

                                        <asp:LinkButton ID="btnEliminar" runat="server"
                                            CommandName="Eliminar"
                                            CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-sm text-danger fs-5"
                                            title="Eliminar Marca"
                                            OnClientClick="return confirm('¿Estás seguro de que deseas eliminar esta marca?');">
            <i class="bi bi-trash3-fill"></i>
        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="alert alert-dark border-secondary border-opacity-25 text-center my-3 text-black-50">
                                    <i class="bi bi-info-circle-fill me-2"></i>No hay marcas registradas actualmente.
                                   
                                </div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-12 col-lg-5">

                <div class="card p-3 mb-4" style="background-color: rgba(43, 30, 22, 0.4); border: 1px solid rgba(243, 235, 224, 0.15);">
                    <h5 class="text-uppercase mb-3" style="color: #cca97c; font-size: 0.9rem; letter-spacing: 1px;"><i class="bi bi-plus-circle me-2"></i>Agregar Nueva Marca</h5>
                    <div class="input-group shadow-sm">
                        <asp:TextBox ID="txtNuevaMarca" runat="server" CssClass="form-control" Placeholder="Ej: Pampa Hierros..."></asp:TextBox>
                        <asp:Button ID="btnAgregarMarca" runat="server" Text="Agregar" CssClass="btn btn-pampa-primary" OnClick="btnAgregarMarca_Click" />
                    </div>
                </div>

                <% if (txtModificarMarca.Visible)
                    { %>
                <div class="card p-3 border-warning border-opacity-25" style="background-color: rgba(43, 30, 22, 0.6);">
                    <h5 class="text-warning text-uppercase mb-3" style="font-size: 0.9rem; letter-spacing: 1px;"><i class="bi bi-pencil-fill me-2"></i>Modificar Seleccionada</h5>
                    <div class="mb-3">
                        <asp:TextBox ID="txtModificarMarca" runat="server" CssClass="form-control" Placeholder="Modifique la marca aquí"></asp:TextBox>
                    </div>
                    <div class="d-flex gap-2">
                        <asp:Button ID="btnModificarMarca" runat="server" Text="Guardar Cambios" CssClass="btn btn-success flex-grow-1" OnClick="btnModificarMarca_Click" />
                        <asp:Button ID="btnCerrar" runat="server" Text="Cancelar" CssClass="btn btn-outline-light" OnClick="btnCerrar_Click" />
                    </div>
                </div>
                <% } %>
            </div>
        </div>

        <div class="modal fade" id="modalAviso" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-black border-secondary border-opacity-50" style="background-color: #2b1e16; backdrop-filter: blur(10px);">
                    <div class="modal-header border-secondary border-opacity-25">
                        <h5 class="modal-title fw-bold" style="color: #cca97c;"><i class="bi bi-exclamation-triangle-fill me-2 text-warning"></i>Acción Bloqueada</h5>
                        <button type="button" class="btn-close btn-close-black" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body py-4 text-center">
                        <p class="mb-0 fs-5" style="color: #e2dacb;">La marca seleccionada no puede ser eliminada porque contiene productos vinculados en el catálogo.</p>
                    </div>
                    <div class="modal-footer border-secondary border-opacity-25">
                        <button type="button" class="btn btn-pampa-primary px-4 rounded-pill" data-bs-dismiss="modal">Entendido</button>
                    </div>
                </div>
            </div>
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
