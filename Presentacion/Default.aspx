<%@ Page Title="" Language="C#" MasterPageFile="~/Zentro.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentacion.ProductoHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="row mt-4">
        <div class="col-6">
            <div class="mb-3">
                <asp:Panel runat="server" DefaultButton="btnBuscar">
                    <!-- Usamos Bootstrap input-group para alinear el TextBox y el botón en una misma fila -->
                    <div class="input-group">
                        <asp:TextBox ID="txtBusqueda" CssClass="form-control"
                            placeholder="Escribe tu búsqueda aquí..." runat="server"></asp:TextBox>
                        <asp:Button ID="btnBuscar" CssClass="btn btn-primary"
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
    <div class="row row-cols-2 row-cols-lg-5 g-4">
    <asp:Repeater ID="rptProductos" runat="server" OnItemCommand="rptProductos_ItemCommand">
        <ItemTemplate>
            <div class="col">
                <div class="card h-100 shadow-sm border-0">
                    <h5 class="mt-1 mb-1"><%# Eval("Marca.Descripcion") %></h5>
                    <img src='<%# Eval("ImagenUrl") %>' 
                         class="card-img-top img-fluid" 
                         alt="Imagen no disponible"
                         onerror="this.onerror=null; this.src='Content/Imagenes/Imagen no disponible.png';" />
                    <%if (Session["usuario"] != null)
                        { %>
                    <asp:LinkButton ID="btnFavorito" runat="server"
                        CssClass="btn btn-light position-absolute top-0 end-0 m-2"
                        CommandName="Favorito"
                        CommandArgument='<%# Eval("Id") %>'>
                        <i class='<%# Negocio.ProductoNegocio.esFavorito((int)Eval("Id"),usuario != null ? usuario.Id : 0) 
                                    ? "bi bi-heart-fill" 
                                    : "bi bi-heart" %>'></i>
                    </asp:LinkButton>
                    <%} %>
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title text-truncate"><%# Eval("Nombre") %></h5>
                        <p class="card-text fw-bold text-primary mb-2">
                            $ <%# Eval("Precio", "{0:N0}") %>
                        </p>
                        <a href='DetalleProducto.aspx?id=<%# Eval("Id") %>' 
                           class="btn btn-outline-secondary mb-2 w-100">Ver detalles</a>
                        <a href="https://wa.me/5493446400000" target="_blank" 
                           class="btn btn-success w-100 mt-auto">
                            <i class="bi bi-whatsapp me-2"></i>Comprar
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

</asp:Content>

