<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="AdminMarca.aspx.cs" Inherits="Presentacion.AdminMarca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%if (usuario != null && usuario.Admin)
        { %>
    <h2 class="mt-1 md-2">MARCAS</h2>

    <div class="row">
        <div class="col-md-6 text-center">
            <asp:GridView ID="dgvMarcas" runat="server"
                DataKeyNames="Id" AutoGenerateColumns="false" CssClass="table table-striped"
                OnRowCommand="dgvMarcas_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="NOMBRE" DataField="Descripcion" />
                    <asp:TemplateField HeaderText="ACCIONES">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEditar" runat="server"
                                CommandName="Editar"
                                CommandArgument='<%# Eval("Id") + "|" + Eval("Descripcion")%>'
                                CssClass="btn btn-sm btn-outline-primary me-2">
                                    Editar
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEliminar" runat="server"
                                CommandName="Eliminar"
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-sm btn-outline-danger"
                                OnClientClick="return confirm('¿Estás seguro de que deseas eliminar esta marca?');">
                                    Eliminar
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info text-center">
                        No hay marcas registradas.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
            <div class="row mb-3">
                <div class="col-md-6">
                    <div class="input-group">
                        <asp:TextBox ID="txtNuevaMarca" runat="server"
                            CssClass="form-control"
                            Placeholder="Ingrese la nueva marca aquí">
                        </asp:TextBox>
                        <asp:Button ID="btnAgregarMarca" runat="server"
                            Text="Agregar"
                            CssClass="btn btn-primary"
                            OnClick="btnAgregarMarca_Click" />
                    </div>
                    <%-- modal que se mostrará si no puede ser eliminada una marca --%>
                    <div class="modal fade" id="modalAviso" tabindex="-1" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Aviso</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                </div>
                                <div class="modal-body">
                                    La marca no puede ser eliminada porque está asociada a productos.
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtModificarMarca" runat="server"
                        CssClass="form-control"
                        Placeholder="Modifique la marca aquí" Visible="False">
                    </asp:TextBox>
                    <asp:Button ID="btnModificarMarca" runat="server"
                        Text="Modificar"
                        CssClass="btn btn-primary"
                        OnClick="btnModificarMarca_Click" Visible="False" />
                    <asp:Button ID="btnCerrar" runat="server"
                        Text="Cerrar"
                        CssClass="btn btn-danger"
                        OnClick="btnCerrar_Click" Visible="False" />
                </div>
            </div>
        </div>
    </div>
    <%}
    else
    { %>
    <div class="col text-center">
        <h2>NO TIENES LOS PERMISOS PARA INGRESAR AQUÍ.</h2>
    </div>
    <%}%>
</asp:Content>
